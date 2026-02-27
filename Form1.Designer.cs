using System.Drawing;
using System.Windows.Forms;
namespace Zarr
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            folderBrowserDialog = new FolderBrowserDialog();
            pictureBox = new PictureBox();
            openFileDialog = new OpenFileDialog();
            trackBar1 = new TrackBar();
            trackBar2 = new TrackBar();
            trackBar3 = new TrackBar();
            panel1 = new Panel();
            butGo = new Button();
            label1 = new Label();
            pathBox = new TextBox();
            hScrollBar = new HScrollBar();
            vScrollBar = new VScrollBar();
            statuslabel = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(11, 4, 0, 4);
            menuStrip1.Size = new Size(1007, 44);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 36);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(250, 44);
            openToolStripMenuItem.Text = "Open File";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.Location = new Point(0, 160);
            pictureBox.Margin = new Padding(6);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(975, 531);
            pictureBox.TabIndex = 1;
            pictureBox.TabStop = false;
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar1.Location = new Point(2, 188);
            trackBar1.Margin = new Padding(6);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(990, 90);
            trackBar1.TabIndex = 2;
            trackBar1.TickStyle = TickStyle.None;
            // 
            // trackBar2
            // 
            trackBar2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar2.Location = new Point(0, 0);
            trackBar2.Margin = new Padding(6);
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(984, 90);
            trackBar2.TabIndex = 3;
            trackBar2.TickStyle = TickStyle.None;
            // 
            // trackBar3
            // 
            trackBar3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar3.Location = new Point(0, 96);
            trackBar3.Margin = new Padding(6);
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(984, 90);
            trackBar3.TabIndex = 4;
            trackBar3.TickStyle = TickStyle.None;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(trackBar2);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(trackBar3);
            panel1.Location = new Point(0, 742);
            panel1.Margin = new Padding(6);
            panel1.Name = "panel1";
            panel1.Size = new Size(997, 269);
            panel1.TabIndex = 5;
            // 
            // butGo
            // 
            butGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            butGo.Location = new Point(910, 62);
            butGo.Margin = new Padding(6);
            butGo.Name = "butGo";
            butGo.Size = new Size(65, 49);
            butGo.TabIndex = 7;
            butGo.Text = "Go";
            butGo.UseVisualStyleBackColor = true;
            butGo.Click += butSelectPath_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 70);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(65, 32);
            label1.TabIndex = 6;
            label1.Text = "Path:";
            // 
            // pathBox
            // 
            pathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pathBox.Location = new Point(87, 58);
            pathBox.Margin = new Padding(6);
            pathBox.Name = "pathBox";
            pathBox.Size = new Size(801, 39);
            pathBox.TabIndex = 5;
            pathBox.Text = "https://uk1s3.embassy.ebi.ac.uk/idr/zarr/v0.3/idr0075A/9528933.zarr";
            // 
            // hScrollBar
            // 
            hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            hScrollBar.Location = new Point(2, 698);
            hScrollBar.Name = "hScrollBar";
            hScrollBar.Size = new Size(1005, 18);
            hScrollBar.TabIndex = 8;
            hScrollBar.Scroll += hScrollBar_Scroll;
            // 
            // vScrollBar
            // 
            vScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            vScrollBar.Location = new Point(973, 126);
            vScrollBar.Name = "vScrollBar";
            vScrollBar.Size = new Size(18, 567);
            vScrollBar.TabIndex = 9;
            vScrollBar.Scroll += vScrollBar_Scroll;
            // 
            // statuslabel
            // 
            statuslabel.AutoSize = true;
            statuslabel.Location = new Point(13, 122);
            statuslabel.Margin = new Padding(6, 0, 6, 0);
            statuslabel.Name = "statuslabel";
            statuslabel.Size = new Size(0, 32);
            statuslabel.TabIndex = 10;
            // 
            // Form1
            // 
            AcceptButton = butGo;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1007, 1015);
            Controls.Add(statuslabel);
            Controls.Add(vScrollBar);
            Controls.Add(hScrollBar);
            Controls.Add(butGo);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(pictureBox);
            Controls.Add(pathBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(6);
            Name = "Form1";
            Text = "Mainform";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog;
        private OpenFileDialog openFileDialog;
        private PictureBox pictureBox;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private TrackBar trackBar3;
        private Panel panel1;
        private Label label1;
        private TextBox pathBox;
        private Button butGo;
        private HScrollBar hScrollBar;
        private VScrollBar vScrollBar;
        private Label statuslabel;
    }
}
