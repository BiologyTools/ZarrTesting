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
            if(SelectedImage == null)
            {
                BioImage image = await BioImage.OpenURL(pathBox.Text, new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value));
            }
            if (omeZarrReader == null)
            {
                omeZarrReader = await OmeZarrReader.OpenAsync(pathBox.Text);
                reader = omeZarrReader;
            }
            string filePath = pathBox.Text;
            var imagef = reader.AsMultiscaleImage();
            var levelf = await imagef.OpenResolutionLevelAsync(0);
            var ress = await imagef.OpenAllResolutionLevelsAsync();
            if(ress.Count > 1)
            {
                var planef = await levelf.ReadPlaneAsync(t: trackBar1.Value, c: trackBar2.Value, z: trackBar3.Value);
                try
                {
                    if (levelf.Rank > 0)
                    {
                        if (levelf.DataType == "uint16")
                        {
                            //AForge.Bitmap bm = new AForge.Bitmap("",(int)levelf.Shape[levelf.Rank - 1], (int)levelf.Shape[levelf.Rank - 2], AForge.PixelFormat.Format16bppGrayScale,
                            AForge.Bitmap bm = new AForge.Bitmap("", SelectedImage.PyramidalSize.Width, SelectedImage.PyramidalSize.Height, AForge.PixelFormat.Format16bppGrayScale,
                                planef.Data, new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), 0, null, false, true);
                            System.Drawing.Bitmap sb = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm.Data);
                            pictureBox.Image = sb;
                        }
                        else if (levelf.DataType == "uint8")
                        {
                            AForge.Bitmap bm = new AForge.Bitmap("", (int)levelf.Shape[levelf.Rank - 1], (int)levelf.Shape[levelf.Rank - 2], AForge.PixelFormat.Format8bppIndexed,
                                planef.Data, new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), 0, null, false, true);
                            System.Drawing.Bitmap sb = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm.Data);
                            pictureBox.Image = sb;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var planef = await levelf.ReadPlaneAsync(t: trackBar1.Value, c: trackBar2.Value, z: trackBar3.Value);
                try
                {
                    if (levelf.Rank > 0)
                    {
                        if (levelf.DataType == "uint16")
                        {
                            AForge.Bitmap bm = new AForge.Bitmap((int)levelf.Shape[levelf.Rank - 1], (int)levelf.Shape[levelf.Rank - 2], AForge.PixelFormat.Format16bppGrayScale,
                                planef.Data, new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), "");
                            AForge.Bitmap bmp = bm.GetImageRGBA(true);
                            System.Drawing.Bitmap sb = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmp.Data);
                            pictureBox.Image = sb;
                        }
                        else if (levelf.DataType == "uint8")
                        {
                            AForge.Bitmap bm = new AForge.Bitmap((int)levelf.Shape[levelf.Rank - 1], (int)levelf.Shape[levelf.Rank - 2], AForge.PixelFormat.Format8bppIndexed,
                                planef.Data, new AForge.ZCT(trackBar1.Value, trackBar2.Value, trackBar3.Value), "");
                            Bitmap bmp = new System.Drawing.Bitmap(bm.Width, bm.Height, bm.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm.Data);
                            pictureBox.Image = bmp;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Zarr file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            

        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
