using OmeZarr.Core.OmeZarr;
using OmeZarr.Core.OmeZarr.Coordinates;
using OmeZarr.Core.OmeZarr.Metadata;
using OmeZarr.Core.OmeZarr.Nodes;

// =============================================================================
// Sample 1 — Multiscale image (most common case)
// =============================================================================
public static class ZarrTests
{
    public static async Task ReadMultiscaleImage(string zarrPath)
{
    await using var reader = await OmeZarrReader.OpenAsync(zarrPath);

    Console.WriteLine($"Root node type: {reader.RootNodeType}");

    var image = reader.AsMultiscaleImage();

    // Inspect the multiscale metadata before reading anything
    var multiscale = image.Multiscales[0];

    Console.WriteLine($"Image name:  {multiscale.Name ?? "(unnamed)"}");
    Console.WriteLine($"Axes:        {string.Join(", ", multiscale.Axes.Select(a => $"{a.Name} [{a.Unit}]"))}");
    Console.WriteLine($"Resolutions: {multiscale.Datasets.Length}");

    // Open all resolution levels so we can inspect pixel sizes and pick the right one
    var levels = await image.OpenAllResolutionLevelsAsync();

    foreach (var (level, index) in levels.Select((l, i) => (l, i)))
    {
        var pixelSize = level.GetPixelSize();
        var pixelSizeStr = string.Join(" x ", pixelSize.Select(p => p.ToString("G4")));

        Console.WriteLine($"  Level {index}: shape [{string.Join(", ", level.Shape)}]  pixel size: {pixelSizeStr}");
    }

    // -------------------------------------------------------------------------
    // Read a specific region in physical coordinates
    // Axes for a typical 5D image are: t, c, z, y, x
    // -------------------------------------------------------------------------

    // Physical ROI: t=0s, c=0, z=0, y=0..100µm, x=0..100µm
    // Origin and Size must match the number of axes declared in the multiscale
    var roi = new PhysicalROI(
        origin: [0, 0, 0, 0, 0],       // t, c, z, y, x  in declared units
        size: [1, 1, 1, 100, 100]    // read 1 timepoint, 1 channel, 1 z-slice, 100µm × 100µm
    );

    // Read at full resolution (level 0)
    var fullRes = levels[0];
    var result = await fullRes.ReadRegionAsync(roi);

    Console.WriteLine($"\nRead result: {result}");
    // e.g. "uint16 [1, 1, 1, 512, 512] (t, c, z, y, x) — 524288 bytes"

    // Cast bytes to ushort for uint16 data
    if (result.DataType == "uint16")
    {
        var pixels = new ushort[result.ElementCount];
        Buffer.BlockCopy(result.Data, 0, pixels, 0, result.Data.Length);

        Console.WriteLine($"First pixel value: {pixels[0]}");
        Console.WriteLine($"Max pixel value:   {pixels.Max()}");
    }

    // -------------------------------------------------------------------------
    // Find the best resolution level for a given physical pixel size target
    // e.g. you want roughly 0.5µm/px for display — pick the closest level
    // -------------------------------------------------------------------------

    double targetMicronsPerPixel = 0.5;
    int spatialAxisIndex = 3;   // y axis in t,c,z,y,x layout

    var bestLevel = levels
        .Select((l, i) => (level: l, index: i, xyPixelSize: l.GetPixelSize()[spatialAxisIndex]))
        .OrderBy(l => Math.Abs(l.xyPixelSize - targetMicronsPerPixel))
        .First();

    Console.WriteLine($"\nBest level for {targetMicronsPerPixel}µm/px: level {bestLevel.index} ({bestLevel.xyPixelSize:G4} µm/px)");
}

    // =============================================================================
    // Sample 2 — Reading labels alongside the image
    // =============================================================================

    public static async Task ReadWithLabels(string zarrPath)
{
    await using var reader = await OmeZarrReader.OpenAsync(zarrPath);

    var image = reader.AsMultiscaleImage();

    if (!await image.HasLabelsAsync())
    {
        Console.WriteLine("No labels found.");
        return;
    }

    var labelGroup = await image.OpenLabelsAsync();

    Console.WriteLine($"Available labels: {string.Join(", ", labelGroup.LabelNames)}");

    // Open a specific label (e.g. cell segmentation masks)
    var cellLabel = await labelGroup.OpenLabelAsync("cells");

    Console.WriteLine($"Label colors defined: {cellLabel.ImageLabelMetadata.Colors.Length}");

    // Labels have the same multiscale structure as images
    var labelLevel = await cellLabel.OpenResolutionLevelAsync(datasetIndex: 0);
    var extent = labelLevel.GetPhysicalExtent();

    Console.WriteLine($"Label physical extent: {extent}");

    // Read the full label extent as pixel region (label values, not intensities)
    var roi = extent; // full extent
    var result = await labelLevel.ReadRegionAsync(roi);

    // Label data is typically uint32 (label IDs)
    if (result.DataType == "uint32")
    {
        var labelIds = new uint[result.ElementCount];
        Buffer.BlockCopy(result.Data, 0, labelIds, 0, result.Data.Length);

        var uniqueLabels = labelIds.Distinct().Where(id => id != 0).Count();
        Console.WriteLine($"Unique cell IDs in region: {uniqueLabels}");
    }
}

    // =============================================================================
    // Sample 3 — HCS Plate navigation
    // =============================================================================

    public static async Task ReadHcsPlate(string zarrPath)
{
    await using var reader = await OmeZarrReader.OpenAsync(zarrPath);

    var plate = reader.AsPlate();

    Console.WriteLine($"Plate: {plate.PlateMetadata.Name}");
    Console.WriteLine($"Rows:    {string.Join(", ", plate.Rows.Select(r => r.Name))}");
    Console.WriteLine($"Columns: {string.Join(", ", plate.Columns.Select(c => c.Name))}");
    Console.WriteLine($"Wells:   {plate.Wells.Count}");

    // Open a specific well
    var well = await plate.OpenWellAsync(rowName: "B", columnName: "3");

    Console.WriteLine($"\nWell B3 has {well.Fields.Count} field(s)");

    // Open the first field (acquisition) in the well
    var field = await well.OpenFieldAsync(fieldIndex: 0);
    var level = await field.OpenResolutionLevelAsync(datasetIndex: 0);
    var pixelSize = level.GetPixelSize();
    var axes = field.Multiscales[0].Axes;

    Console.WriteLine($"Field 0 axes: {string.Join(", ", axes.Select(a => $"{a.Name}({a.Unit})"))}");
    Console.WriteLine($"Field 0 shape: [{string.Join(", ", level.Shape)}]");

    // Read a central 200µm × 200µm region from the first z-plane of each channel
    // Typical HCS layout: c, z, y, x  (no time)
    var physicalCenter = level.GetPhysicalExtent();
    var centerY = physicalCenter.Origin[2] + physicalCenter.Size[2] / 2;
    var centerX = physicalCenter.Origin[3] + physicalCenter.Size[3] / 2;

    var roiSize = 200.0;  // µm

    var centerRoi = new PhysicalROI(
        origin: [0, 0, centerY - roiSize / 2, centerX - roiSize / 2],
        size: [axes.Length, 1, roiSize, roiSize]
    );

    var result = await level.ReadRegionAsync(centerRoi);
    Console.WriteLine($"\nCenter ROI result: {result}");

    // -------------------------------------------------------------------------
    // Iterate all wells programmatically
    // -------------------------------------------------------------------------

    Console.WriteLine("\n--- All wells summary ---");

    foreach (var wellRef in plate.Wells)
    {
        var w = await plate.OpenWellAsync(wellRef.Path);
        var f = await w.OpenFieldAsync(fieldIndex: 0);
        var l = await f.OpenResolutionLevelAsync(datasetIndex: 0);
        var extent = l.GetPhysicalExtent();
        var pxSize = l.GetPixelSize();

        Console.WriteLine(
            $"  {wellRef.Path,-6}  shape [{string.Join(", ", l.Shape)}]  " +
            $"xy size: {pxSize[^1]:G3}µm/px");
    }
}

    // =============================================================================
    // Sample 4 — Writing back a modified ROI (e.g. after processing)
    // =============================================================================

    public static async Task WriteBackRegion(string zarrPath)
{
    await using var reader = await OmeZarrReader.OpenAsync(zarrPath);

    var image = reader.AsMultiscaleImage();
    var level = await image.OpenResolutionLevelAsync(datasetIndex: 0);

    // Read a region
    var roi = new PhysicalROI(
        origin: [0, 0, 0, 50, 50],
        size: [1, 1, 1, 64, 64]
    );

    var readResult = await level.ReadRegionAsync(roi);

    // Modify the data (example: zero out a region as a mask)
    var modifiedData = (byte[])readResult.Data.Clone();
    Array.Clear(modifiedData, 0, modifiedData.Length);

    // Write back via pixel region
    // (We need the pixel region — ReadRegionAsync computed it internally,
    //  so we re-derive it here using the same ROI + transform)
    var multiscale = image.Multiscales[0];
    var transformSvc = new CoordinateTransformService();
    var pixelRegion = transformSvc.PhysicalToPixel(roi, multiscale.Datasets[0], multiscale, level.Shape);

    await level.WriteRegionAsync(pixelRegion, modifiedData);

    Console.WriteLine($"Wrote back modified region: {pixelRegion}");
}

    // =============================================================================
    // Sample 5 — Unknown root type (auto-dispatch)
    // =============================================================================

    public static async Task OpenUnknownRoot(string zarrPath)
{
    await using var reader = await OmeZarrReader.OpenAsync(zarrPath);

    Console.WriteLine($"Detected: {reader.RootNodeType}");

    var root = reader.OpenRoot();  // returns OmeZarrNode — cast as needed

    switch (root)
    {
        case PlateNode plate:
            Console.WriteLine($"Plate with {plate.Wells.Count} wells");
            break;

        case MultiscaleNode image:
            Console.WriteLine($"Image with {image.Multiscales[0].Datasets.Length} resolution levels");
                await ReadMultiscaleImage(zarrPath);
                break;
        case WellNode well:
            Console.WriteLine($"Well with {well.Fields.Count} fields");
            break;

        default:
            Console.WriteLine($"Other node type: {root.GetType().Name}");
            break;
    }
}

    public static async void Run(string[] args)
    {
        // =============================================================================
        // Entry point — point at your .zarr folder
        // =============================================================================

        var zarrPath = args.Length > 0 ? args[0] : @"C:\data\my_dataset.zarr";

        Console.WriteLine($"Opening: {zarrPath}\n");

        await OpenUnknownRoot(zarrPath);
    }
}