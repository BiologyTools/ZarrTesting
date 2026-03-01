using AForge;
using BioImager;
using BioLib;
using CSScripting;
using java.awt;
using OmeZarr;
using OmeZarr.Core.OmeZarr;
using OmeZarr.Core.OmeZarr.Helpers;
using OmeZarr.Core.OmeZarr.Nodes;
using System;
using System.Collections.Generic;
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
        ImageView view = new ImageView();
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
            viewpanel.Controls.Add(view);
            view.Dock = DockStyle.Fill;
            view.BringToFront();
            //App.Initialize();
        }
        public static unsafe nint GetPointer(byte[] data)
        {
            fixed (byte* p = data)
                return (nint)p;
        }
        OmeZarrReader omeZarrReader = null;
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
        public static BioImage SelectedImage
        {
            get { return ImageView.SelectedImage; }
            set { ImageView.SelectedImage = value; }
        }
        public List<System.Drawing.Bitmap> Bitmaps = new List<System.Drawing.Bitmap>();
        private async void butSelectPath_Click(object sender, EventArgs e)
        {
            //view.Images.Clear();
            if (view.Images.Count > 0)
            {
                SelectedImage = view.Images[0];
                SelectedImage.Coordinate = view.GetCoordinate();
            }
            else
            {
                view.Images.Clear();
                BioImage bm = await BioImage.OpenURL(pathBox.Text, new ZCT(0, 0, 0), (int)view.PyramidalOrigin.X, (int)view.PyramidalOrigin.Y, view.Width, view.Height);
                ImageView.SelectedImage = bm;
                SelectedImage.Coordinate = view.GetCoordinate();
                view.AddImage(bm);
            }
            view.Invalidate();
        }
    }
}
