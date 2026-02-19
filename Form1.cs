using OmeZarr;
using OmeZarr.Core.OmeZarr;
using OmeZarr.Core.OmeZarr.Helpers;
using OmeZarr.Core.OmeZarr.Nodes;
using System.Runtime.InteropServices;

namespace Zarr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackBar1.ValueChanged += TrackBar1_ValueChanged;
        }

        private async void TrackBar1_ValueChanged(object? sender, EventArgs e)
        {
            var imagef = reader.AsMultiscaleImage();
            var levelf = await imagef.OpenResolutionLevelAsync(0);
            var planef = await levelf.ReadPlaneAsync(t: trackBar3.Value, c: trackBar2.Value, z: trackBar1.Value);
            byte[] rawBytes = planef.Data;
            byte[] bgra32 = planef.ToBytes<ushort>(PixelFormat.Bgra32);
            Bitmap bmp = FromBgra32(bgra32, planef.Width, planef.Height);
            pictureBox.Image = bmp;
        }

        public static Bitmap FromBgra32(byte[] pixelData, int width, int height)
        {
            if (pixelData == null)
                throw new ArgumentNullException(nameof(pixelData));

            if (pixelData.Length != width * height * 4)
                throw new ArgumentException("Invalid buffer size for 32bpp image.");

            var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var rect = new Rectangle(0, 0, width, height);
            var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            try
            {
                Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }
        MultiscaleNode multiscaleNode;
        OmeZarrReader reader;
        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        }

        private async void trackBar2_Scroll(object sender, EventArgs e)
        {
            var imagef = reader.AsMultiscaleImage();
            var levelf = await imagef.OpenResolutionLevelAsync(0);
            var planef = await levelf.ReadPlaneAsync(t: trackBar3.Value, c: trackBar2.Value, z: trackBar1.Value);
            byte[] rawBytes = planef.Data;
            byte[] bgra32 = planef.ToBytes<ushort>(PixelFormat.Bgra32);
            Bitmap bmp = FromBgra32(bgra32, planef.Width, planef.Height);
            pictureBox.Image = bmp;
        }

        private async void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            OmeZarrReader omeZarrReader = null;
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = false;
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            filePath = openFileDialog.FileName.ToString();
            */
            string path = "https://uk1s3.embassy.ebi.ac.uk/idr/zarr/v0.4/idr0073A/9798462.zarr";
            reader = await OmeZarrReader.OpenAsync(path);
            var image = reader.AsMultiscaleImage();
            var level = await image.OpenResolutionLevelAsync(image.Multiscales.Count()-1);
            var plane = await level.ReadTilePixelsAsync(0,0, image.Multiscales.,256,trackBar3.Value, trackBar2.Value, trackBar1.Value);

            Console.WriteLine($"Downloaded plane: {plane.Width}x{plane.Height}, {plane.Data.Length:N0} bytes");
            try
            {
                byte[] rawBytes = plane.Data;
                if (plane.DataType == "uint8")
                {
                    byte[] bgra32 = plane.ToBytes<byte>(PixelFormat.Bgra32);
                    Bitmap bmp = FromBgra32(bgra32, plane.Width, plane.Height);
                    pictureBox.Image = bmp;
                }
                else if (plane.DataType == "uint18")
                {
                    byte[] bgra32 = plane.ToBytes<ushort>(PixelFormat.Bgra32);
                    Bitmap bmp = FromBgra32(bgra32, plane.Width, plane.Height);
                    pictureBox.Image = bmp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void trackBar1_Scroll(object sender, EventArgs e)
        {
            var imagef = reader.AsMultiscaleImage();
            var levelf = await imagef.OpenResolutionLevelAsync(0);
            var planef = await levelf.ReadPlaneAsync(t: trackBar3.Value, c: trackBar2.Value, z: trackBar1.Value);
            byte[] rawBytes = planef.Data;
            byte[] bgra32 = planef.ToBytes<ushort>(PixelFormat.Bgra32);
            Bitmap bmp = FromBgra32(bgra32, planef.Width, planef.Height);
            pictureBox.Image = bmp;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
