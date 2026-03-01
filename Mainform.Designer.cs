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
            openFileDialog = new OpenFileDialog();
            butGo = new Button();
            label1 = new Label();
            pathBox = new TextBox();
            statuslabel = new Label();
            viewpanel = new Panel();
            menuStrip1.SuspendLayout();
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
            pathBox.Location = new Point(47, 27);
            pathBox.Name = "pathBox";
            pathBox.Size = new Size(433, 23);
            pathBox.TabIndex = 5;
            pathBox.Text = "https://uk1s3.embassy.ebi.ac.uk/idr/zarr/v0.4/idr0048A/9846152.zarr/";
            // 
            // statuslabel
            // 
            statuslabel.AutoSize = true;
            statuslabel.Location = new Point(7, 57);
            statuslabel.Name = "statuslabel";
            statuslabel.Size = new Size(42, 15);
            statuslabel.TabIndex = 10;
            statuslabel.Text = "Status:";
            // 
            // viewpanel
            // 
            viewpanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            viewpanel.Location = new Point(0, 75);
            viewpanel.Name = "viewpanel";
            viewpanel.Size = new Size(542, 188);
            viewpanel.TabIndex = 11;
            // 
            // Form1
            // 
            AcceptButton = butGo;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 264);
            Controls.Add(statuslabel);
            Controls.Add(butGo);
            Controls.Add(label1);
            Controls.Add(pathBox);
            Controls.Add(menuStrip1);
            Controls.Add(viewpanel);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Mainform";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog;
        private OpenFileDialog openFileDialog;
        private Label label1;
        private TextBox pathBox;
        private Button butGo;
        private Label statuslabel;
        private Panel viewpanel;
    }
}
