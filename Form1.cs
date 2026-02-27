using AForge;
using BioLib;
using OmeZarr;
using OmeZarr.Core.OmeZarr;
using OmeZarr.Core.OmeZarr.Helpers;
using OmeZarr.Core.OmeZarr.Nodes;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Bitmap = System.Drawing.Bitmap;

namespace Zarr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MultiscaleNode multiscaleNode;
        OmeZarrReader reader;
        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            OmeZarrReader omeZarrReader = reader;
            string filePath = null;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog.SelectedPath;
                reader = await OmeZarrReader.OpenAsync(filePath);
                var imagef = reader.AsMultiscaleImage();
                var levelf = await imagef.OpenResolutionLevelAsync(0);
                var planef = await levelf.ReadPlaneAsync(t: 0, c: 0, z: 0);
                try
                {
                    byte[] rawBytes = planef.Data;
                    byte[] bgra32 = planef.ToBytes<ushort>(PixelFormat.Bgra32);
                    Bitmap bmp = FromBgra32(bgra32, planef.Width, planef.Height);
                    pictureBox.Image = bmp;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            */
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static unsafe nint GetPointer(byte[] data)
        {
            fixed (byte* p = data)
                return (nint)p;
        }
        OmeZarrReader omeZarrReader = null;
        public PointD PyramidalOrigin = new PointD(0, 0);
        public static void EnsureLittleEndian(byte[] data, int elementSize)
        {
            if (!BitConverter.IsLittleEndian)
                SwapEndianness(data, elementSize);
        }
        public static void SwapEndianness(byte[] data, int elementSize)
        {
            if (data.Length % elementSize != 0)
                throw new ArgumentException("Invalid element size");

            for (int i = 0; i < data.Length; i += elementSize)
            {
                Array.Reverse(data, i, elementSize);
            }
        }
        public static BioImage SelectedImage;
        private async void butSelectPath_Click(object sender, EventArgs e)
        {
            // -------------------------------------------------------
            // Snapshot ALL control values before the first await
            // Everything below this block is potentially off-thread
            // -------------------------------------------------------
            string path = pathBox.Text;
            int picWidth = pictureBox.Width;
            int picHeight = pictureBox.Height;
            int scrollX = hScrollBar.Value;
            int scrollY = vScrollBar.Value;
            int tVal = trackBar1.Value;
            int cVal = trackBar2.Value;
            int zVal = trackBar3.Value;

            try
            {
                // -------------------------------------------------------
                // Async work — all off UI thread
                // -------------------------------------------------------
                if (SelectedImage == null)
                {
                    BioImage image = await BioImage.OpenURL(
                        path,
                        new AForge.ZCT(tVal, cVal, zVal),
                        0, 0, picWidth, picHeight)
                        .ConfigureAwait(false);

                    SelectedImage = image;
                }

                if (omeZarrReader == null)
                {
                    omeZarrReader = await OmeZarrReader.OpenAsync(path)
                        .ConfigureAwait(false);
                    reader = omeZarrReader;
                }

                var imagef = reader.AsMultiscaleImage();
                var levelf = await imagef.OpenResolutionLevelAsync(0).ConfigureAwait(false);
                var ress = await imagef.OpenAllResolutionLevelsAsync().ConfigureAwait(false);

                // -------------------------------------------------------
                // Determine trackbar maximums from shape
                // -------------------------------------------------------
                int maxZ = 0, maxC = 0, maxT = 0;

                if (levelf.Shape.Length == 3)
                {
                    maxZ = (int)levelf.Shape[0];
                }
                else if (levelf.Shape.Length == 4)
                {
                    maxC = (int)levelf.Shape[0];
                    maxZ = (int)levelf.Shape[1];
                }
                else if (levelf.Shape.Length == 5)
                {
                    maxT = (int)levelf.Shape[0];
                    maxC = (int)levelf.Shape[1];
                    maxZ = (int)levelf.Shape[2];
                }

                // -------------------------------------------------------
                // Read tile (async, off UI thread)
                // -------------------------------------------------------
                string dataType = levelf.DataType;

                System.Drawing.Bitmap renderedBitmap = null;

                if (levelf.Rank > 0)
                {
                    var tileResult = await levelf.ReadTileAsync(
                        scrollX, scrollY,
                        picWidth, picHeight,
                        t: tVal, c: cVal, z: zVal)
                        .ConfigureAwait(false);

                    renderedBitmap = RenderTile(tileResult, dataType, tVal, cVal, zVal);
                }

                // -------------------------------------------------------
                // UI updates — back on UI thread via Invoke
                // -------------------------------------------------------
                Invoke(() =>
                {
                    statuslabel.Text = $"Opened: {reader.RootNodeType}, NGFF {reader.NgffVersion}";
                    vScrollBar.Maximum = (int)SelectedImage.Resolutions[0].SizeX;
                    hScrollBar.Maximum = (int)SelectedImage.Resolutions[0].SizeY;
                    trackBar1.Maximum = maxZ;
                    trackBar2.Maximum = maxC;
                    trackBar3.Maximum = maxT;

                    SelectedImage.Coordinate = new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value);

                    if (renderedBitmap != null)
                        pictureBox.Image = renderedBitmap;
                });
            }
            catch (Exception ex)
            {
                Invoke(() =>
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        // ---------------------------------------------------------------
        // Helper — pure data → bitmap conversion, no async, no UI access
        // ---------------------------------------------------------------
        private System.Drawing.Bitmap RenderTile(
            PlaneResult tileResult,
            string dataType,
            int t, int c, int z)
        {
            var coord = new ZCT(t, c, z);
            int width = tileResult.Width;
            int height = tileResult.Height;
            byte[] data = tileResult.Data;

            AForge.PixelFormat pixelFormat = dataType switch
            {
                "uint16" => AForge.PixelFormat.Format16bppGrayScale,
                "uint8" => AForge.PixelFormat.Format8bppIndexed,
                _ => throw new NotSupportedException($"Unsupported datatype: {dataType}")
            };

            AForge.Bitmap bm = new AForge.Bitmap("", width, height, pixelFormat, data, coord, 0);
            AForge.Bitmap bmp = bm.GetImageRGBA(true);

            return new System.Drawing.Bitmap(
                width, height, width * 4,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                bmp.Data);
        }
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            butGo.PerformClick();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            butGo.PerformClick();
        }
    }
}
