using AForge;
using BioLib;
using CSScripting;
using java.awt;
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
        public static void SwapEndianness(byte[] data, int elementSize)
        {
            if (elementSize <= 1)
                return;

            if (data.Length % elementSize != 0)
                throw new ArgumentException(
                    "Array length must be divisible by element size.");

            int half = elementSize / 2;

            for (int i = 0; i < data.Length; i += elementSize)
            {
                int left = i;
                int right = i + elementSize - 1;

                for (int j = 0; j < half; j++)
                {
                    byte tmp = data[left];
                    data[left] = data[right];
                    data[right] = tmp;

                    left++;
                    right--;
                }
            }
        }
        public static BioImage SelectedImage;
        private void butSelectPath_Click(object sender, EventArgs e)
        {
            if (SelectedImage == null)
            {
                BioImage image = BioImage.OpenURL(
                    pathBox.Text,
                    new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value),
                    0, 0, pictureBox.Width, pictureBox.Height).Result;

                SelectedImage = image;
                if (omeZarrReader == null)
                {
                    omeZarrReader = OmeZarrReader.OpenAsync(pathBox.Text).Result;
                    reader = omeZarrReader;
                }
            }
            statuslabel.Text = $"Opened: {reader.RootNodeType}, NGFF {reader.NgffVersion}";
            vScrollBar.Maximum = (int)SelectedImage.Resolutions[0].SizeX;
            hScrollBar.Maximum = (int)SelectedImage.Resolutions[0].SizeY;
            var imagef = reader.AsMultiscaleImage();
            var levelf = imagef.OpenResolutionLevelAsync(0).Result;
            SelectedImage.levelf = levelf;
            var ress = imagef.OpenAllResolutionLevelsAsync().Result;
            if (levelf.Shape.Length == 3)
            {
                trackBar1.Maximum = (int)levelf.Shape[0];
                trackBar2.Maximum = 0;
                trackBar3.Maximum = 0;
                SelectedImage.Coordinate = new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value);
            }
            else
            if (levelf.Shape.Length == 4)
            {
                trackBar1.Maximum = (int)levelf.Shape[1];
                trackBar2.Maximum = (int)levelf.Shape[0];
                trackBar3.Maximum = 0;
                SelectedImage.Coordinate = new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value);
            }
            else
            if (levelf.Shape.Length == 5)
            {
                trackBar1.Maximum = (int)levelf.Shape[2];
                trackBar2.Maximum = (int)levelf.Shape[1];
                trackBar3.Maximum = (int)levelf.Shape[0];
                SelectedImage.Coordinate = new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value);
            }

            if (SelectedImage.Resolutions.Count > 1)
            {
                try
                {
                    if (SelectedImage.levels[0].Rank > 0)
                    {
                        var tileResult = SelectedImage.levelf.ReadTileAsync(
                            hScrollBar.Value, vScrollBar.Value,
                            pictureBox.Width, pictureBox.Height,
                            t: trackBar1.Value,
                            c: trackBar2.Value,
                            z: trackBar3.Value).Result;

                        var tileWidth = tileResult.Width;
                        var tileHeight = tileResult.Height;
                        var bts = tileResult.Data;

                        if (SelectedImage.levelf.DataType == "uint16")
                        {
                            AForge.Bitmap bm = new AForge.Bitmap(
                                "", tileWidth, tileHeight,
                                AForge.PixelFormat.Format16bppGrayScale,
                                bts,
                                new ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value),
                                0, null, false);
                            //SwapEndianness(bm.Bytes, 2);
                            System.Drawing.Bitmap sb = new System.Drawing.Bitmap(
                                tileWidth, tileHeight, tileWidth * 4,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                                 bm.GetRGBData(BitConverter.IsLittleEndian));
                            pictureBox.Image = sb;
                        }
                        else if (SelectedImage.levelf.DataType == "uint8")
                        {
                            AForge.Bitmap bm = new AForge.Bitmap(
                                "", tileWidth, tileHeight,
                                AForge.PixelFormat.Format8bppIndexed,
                                bts,
                                new ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value),
                                0);
                            AForge.Bitmap bmp = bm.GetImageRGBA(true);
                            System.Drawing.Bitmap sb = new System.Drawing.Bitmap(
                                tileWidth, tileHeight, tileWidth * 4,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                                bmp.Data);
                            pictureBox.Image = sb;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var planef = SelectedImage.levelf.ReadPlaneAsync(
                    t: trackBar1.Value, c: trackBar2.Value, z: trackBar3.Value);
                try
                {
                    if (SelectedImage.levelf.Rank > 0)
                    {
                        if (SelectedImage.levelf.DataType == "uint16")
                        {

                            var tileResult = SelectedImage.levelf.ReadTileAsync(
                                hScrollBar.Value, vScrollBar.Value,
                                pictureBox.Width, pictureBox.Height,
                                t: trackBar1.Value, c: trackBar2.Value, z: trackBar3.Value).Result;

                            var tileWidth = tileResult.Width;
                            var tileHeight = tileResult.Height;

                            AForge.Bitmap bm = new AForge.Bitmap("", tileWidth, tileHeight,
                                AForge.PixelFormat.Format16bppGrayScale, tileResult.Data,
                                new ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), 0);
                            AForge.Bitmap bmp = bm.GetImageRGBA(true);
                            var sb = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmp.Data);
                            pictureBox.Image = sb;

                        }
                        else if (SelectedImage.levelf.DataType == "uint8")
                        {
                            var bm = new AForge.Bitmap(
                                (int)SelectedImage.levelf.Shape[SelectedImage.levelf.Rank - 1],
                                (int)SelectedImage.levelf.Shape[SelectedImage.levelf.Rank - 2],
                                AForge.PixelFormat.Format8bppIndexed,
                                planef.Result.Data,
                                new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), "");
                            var bmp = bm.GetImageRGBA(true);      // was skipping GetImageRGBA — now consistent
                            var sb = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmp.Data);
                            pictureBox.Image = sb;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
