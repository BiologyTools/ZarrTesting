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
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(542, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(124, 22);
            openToolStripMenuItem.Text = "Open File";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.Location = new Point(0, 59);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(525, 265);
            pictureBox.TabIndex = 1;
            pictureBox.TabStop = false;
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar1.Location = new Point(1, 88);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(533, 45);
            trackBar1.TabIndex = 2;
            trackBar1.TickStyle = TickStyle.None;
            // 
            // trackBar2
            // 
            trackBar2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar2.Location = new Point(0, 0);
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(530, 45);
            trackBar2.TabIndex = 3;
            trackBar2.TickStyle = TickStyle.None;
            // 
            // trackBar3
            // 
            trackBar3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBar3.Location = new Point(0, 45);
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(530, 45);
            trackBar3.TabIndex = 4;
            trackBar3.TickStyle = TickStyle.None;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(trackBar2);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(trackBar3);
            panel1.Location = new Point(0, 348);
            panel1.Name = "panel1";
            panel1.Size = new Size(537, 126);
            panel1.TabIndex = 5;
            // 
            // butGo
            // 
            butGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            butGo.Location = new Point(490, 29);
            butGo.Name = "butGo";
            butGo.Size = new Size(35, 23);
            butGo.TabIndex = 7;
            butGo.Text = "Go";
            butGo.UseVisualStyleBackColor = true;
            butGo.Click += butSelectPath_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 33);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 6;
            label1.Text = "Path:";
            // 
            // pathBox
            // 
            pathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pathBox.Location = new Point(51, 30);
            pathBox.Name = "pathBox";
            pathBox.Size = new Size(433, 23);
            pathBox.TabIndex = 5;
            pathBox.Text = "https://uk1s3.embassy.ebi.ac.uk/idr/zarr/v0.3/idr0109A/12922361.zarr";
            // 
            // hScrollBar
            // 
            hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            hScrollBar.Location = new Point(1, 327);
            hScrollBar.Name = "hScrollBar";
            hScrollBar.Size = new Size(541, 18);
            hScrollBar.TabIndex = 8;
            hScrollBar.Scroll += hScrollBar_Scroll;
            // 
            // vScrollBar
            // 
            vScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            vScrollBar.Location = new Point(524, 59);
            vScrollBar.Name = "vScrollBar";
            vScrollBar.Size = new Size(18, 266);
            vScrollBar.TabIndex = 9;
            vScrollBar.Scroll += vScrollBar_Scroll;
            // 
            // Form1
            // 
            AcceptButton = butGo;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 476);
            Controls.Add(vScrollBar);
            Controls.Add(hScrollBar);
            Controls.Add(butGo);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(pictureBox);
            Controls.Add(pathBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
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
    }
}
