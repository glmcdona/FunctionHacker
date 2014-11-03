using FunctionHacker.Classes.Visualization;
using FunctionHacker.Forms;
using FunctionHacker.Controls;

namespace FunctionHacker.Forms
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
          this.contextMenuStripMemory = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.toolStripMenuItemEditMemory = new System.Windows.Forms.ToolStripMenuItem();
          this.imageListTabs = new System.Windows.Forms.ImageList(this.components);
          this.tabControlMain = new System.Windows.Forms.TabControl();
          this.tabPage1 = new System.Windows.Forms.TabPage();
          this.panelMain = new System.Windows.Forms.Panel();
          this.panelPlayBarContainer = new System.Windows.Forms.Panel();
          this.label20 = new System.Windows.Forms.Label();
          this.tabPage2 = new System.Windows.Forms.TabPage();
          this.tabPage3 = new System.Windows.Forms.TabPage();
          this.tabPage4 = new System.Windows.Forms.TabPage();
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.panel2 = new System.Windows.Forms.Panel();
          this.panel3 = new System.Windows.Forms.Panel();
          this.windowFinder = new FunctionHacker.Controls.WindowFinder();
          this.toolStrip2 = new System.Windows.Forms.ToolStrip();
          this.processAttach = new System.Windows.Forms.ToolStripButton();
          this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
          this.toolStripSettings = new System.Windows.Forms.ToolStripButton();
          this.lblMainModulePath = new System.Windows.Forms.Label();
          this.lblTitle = new System.Windows.Forms.Label();
          this.iconBox = new System.Windows.Forms.PictureBox();
          this.label19 = new System.Windows.Forms.Label();
          this.contextMenuStripMemory.SuspendLayout();
          this.tabControlMain.SuspendLayout();
          this.tabPage1.SuspendLayout();
          this.panelMain.SuspendLayout();
          this.panelPlayBarContainer.SuspendLayout();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          this.panel2.SuspendLayout();
          this.panel3.SuspendLayout();
          this.toolStrip2.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
          this.SuspendLayout();
          // 
          // contextMenuStripMemory
          // 
          this.contextMenuStripMemory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEditMemory});
          this.contextMenuStripMemory.Name = "contextMenuStripMemory";
          this.contextMenuStripMemory.Size = new System.Drawing.Size(68, 26);
          // 
          // toolStripMenuItemEditMemory
          // 
          this.toolStripMenuItemEditMemory.Name = "toolStripMenuItemEditMemory";
          this.toolStripMenuItemEditMemory.Size = new System.Drawing.Size(67, 22);
          // 
          // imageListTabs
          // 
          this.imageListTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabs.ImageStream")));
          this.imageListTabs.TransparentColor = System.Drawing.Color.Transparent;
          this.imageListTabs.Images.SetKeyName(0, "Close.ico");
          this.imageListTabs.Images.SetKeyName(1, "Info.ico");
          this.imageListTabs.Images.SetKeyName(2, "FunctionFilter.ico");
          this.imageListTabs.Images.SetKeyName(3, "FunctionFull.ico");
          this.imageListTabs.Images.SetKeyName(4, "MemoryView.ico");
          this.imageListTabs.Images.SetKeyName(5, "Settings.ico");
          // 
          // tabControlMain
          // 
          this.tabControlMain.Controls.Add(this.tabPage1);
          this.tabControlMain.Controls.Add(this.tabPage2);
          this.tabControlMain.Controls.Add(this.tabPage3);
          this.tabControlMain.Controls.Add(this.tabPage4);
          this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
          this.tabControlMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
          this.tabControlMain.HotTrack = true;
          this.tabControlMain.ImageList = this.imageListTabs;
          this.tabControlMain.ItemSize = new System.Drawing.Size(150, 18);
          this.tabControlMain.Location = new System.Drawing.Point(5, 10);
          this.tabControlMain.Name = "tabControlMain";
          this.tabControlMain.SelectedIndex = 0;
          this.tabControlMain.ShowToolTips = true;
          this.tabControlMain.Size = new System.Drawing.Size(1074, 704);
          this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
          this.tabControlMain.TabIndex = 67;
          this.tabControlMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlMain_DrawItem);
          this.tabControlMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlMain_MouseDown);
          // 
          // tabPage1
          // 
          this.tabPage1.BackColor = System.Drawing.Color.White;
          this.tabPage1.Controls.Add(this.panelMain);
          this.tabPage1.ImageIndex = 0;
          this.tabPage1.Location = new System.Drawing.Point(4, 22);
          this.tabPage1.Name = "tabPage1";
          this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
          this.tabPage1.Size = new System.Drawing.Size(1066, 678);
          this.tabPage1.TabIndex = 0;
          this.tabPage1.Text = "Functions: Complete List";
          this.tabPage1.UseVisualStyleBackColor = true;
          // 
          // panelMain
          // 
          this.panelMain.BackColor = System.Drawing.Color.White;
          this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.panelMain.Controls.Add(this.panelPlayBarContainer);
          this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panelMain.Location = new System.Drawing.Point(3, 3);
          this.panelMain.Name = "panelMain";
          this.panelMain.Size = new System.Drawing.Size(1060, 672);
          this.panelMain.TabIndex = 68;
          // 
          // panelPlayBarContainer
          // 
          this.panelPlayBarContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.panelPlayBarContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.panelPlayBarContainer.Controls.Add(this.label20);
          this.panelPlayBarContainer.Location = new System.Drawing.Point(3, 618);
          this.panelPlayBarContainer.Name = "panelPlayBarContainer";
          this.panelPlayBarContainer.Size = new System.Drawing.Size(1052, 53);
          this.panelPlayBarContainer.TabIndex = 65;
          // 
          // label20
          // 
          this.label20.Image = ((System.Drawing.Image)(resources.GetObject("label20.Image")));
          this.label20.Location = new System.Drawing.Point(1, 19);
          this.label20.Name = "label20";
          this.label20.Size = new System.Drawing.Size(22, 19);
          this.label20.TabIndex = 74;
          // 
          // tabPage2
          // 
          this.tabPage2.ImageKey = "warning.png";
          this.tabPage2.Location = new System.Drawing.Point(4, 22);
          this.tabPage2.Name = "tabPage2";
          this.tabPage2.Size = new System.Drawing.Size(1066, 678);
          this.tabPage2.TabIndex = 1;
          this.tabPage2.Text = "Functions: Unnamed";
          this.tabPage2.UseVisualStyleBackColor = true;
          // 
          // tabPage3
          // 
          this.tabPage3.ImageKey = "download.png";
          this.tabPage3.Location = new System.Drawing.Point(4, 22);
          this.tabPage3.Name = "tabPage3";
          this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
          this.tabPage3.Size = new System.Drawing.Size(1066, 678);
          this.tabPage3.TabIndex = 2;
          this.tabPage3.Text = "Memory Viewer";
          this.tabPage3.UseVisualStyleBackColor = true;
          // 
          // tabPage4
          // 
          this.tabPage4.ImageKey = "ok.png";
          this.tabPage4.Location = new System.Drawing.Point(4, 22);
          this.tabPage4.Name = "tabPage4";
          this.tabPage4.Size = new System.Drawing.Size(1066, 678);
          this.tabPage4.TabIndex = 3;
          this.tabPage4.Text = "Assembly Viewer";
          this.tabPage4.UseVisualStyleBackColor = true;
          // 
          // splitContainer1
          // 
          this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
          this.splitContainer1.IsSplitterFixed = true;
          this.splitContainer1.Location = new System.Drawing.Point(0, 0);
          this.splitContainer1.Name = "splitContainer1";
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add(this.panel2);
          this.splitContainer1.Panel1.Controls.Add(this.label19);
          this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 5, 10);
          this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add(this.tabControlMain);
          this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5, 10, 10, 10);
          this.splitContainer1.Size = new System.Drawing.Size(1188, 724);
          this.splitContainer1.SplitterDistance = 95;
          this.splitContainer1.TabIndex = 66;
          this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
          // 
          // panel2
          // 
          this.panel2.Controls.Add(this.panel3);
          this.panel2.Location = new System.Drawing.Point(2, 24);
          this.panel2.Name = "panel2";
          this.panel2.Size = new System.Drawing.Size(90, 199);
          this.panel2.TabIndex = 70;
          // 
          // panel3
          // 
          this.panel3.BackColor = System.Drawing.Color.White;
          this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.panel3.Controls.Add(this.windowFinder);
          this.panel3.Controls.Add(this.toolStrip2);
          this.panel3.Controls.Add(this.lblMainModulePath);
          this.panel3.Controls.Add(this.lblTitle);
          this.panel3.Controls.Add(this.iconBox);
          this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
          this.panel3.Location = new System.Drawing.Point(0, 0);
          this.panel3.Name = "panel3";
          this.panel3.Size = new System.Drawing.Size(90, 196);
          this.panel3.TabIndex = 62;
          this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
          // 
          // windowFinder
          // 
          this.windowFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("windowFinder.BackgroundImage")));
          this.windowFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
          this.windowFinder.Location = new System.Drawing.Point(0, 0);
          this.windowFinder.Name = "windowFinder";
          // TODO: Code generation for 'this.windowFinder.SelectedHandle' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
          this.windowFinder.Size = new System.Drawing.Size(20, 21);
          this.windowFinder.TabIndex = 25;
          this.windowFinder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.windowFinder_MouseDown);
          this.windowFinder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.windowFinder_MouseUp);
          // 
          // toolStrip2
          // 
          this.toolStrip2.BackColor = System.Drawing.Color.Linen;
          this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
          this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processAttach,
            this.toolStripButton6,
            this.toolStripSettings});
          this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
          this.toolStrip2.Location = new System.Drawing.Point(0, 0);
          this.toolStrip2.Name = "toolStrip2";
          this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
          this.toolStrip2.Size = new System.Drawing.Size(88, 68);
          this.toolStrip2.TabIndex = 24;
          this.toolStrip2.Text = "toolStrip2";
          // 
          // processAttach
          // 
          this.processAttach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.processAttach.Image = ((System.Drawing.Image)(resources.GetObject("processAttach.Image")));
          this.processAttach.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.processAttach.Name = "processAttach";
          this.processAttach.Size = new System.Drawing.Size(86, 19);
          this.processAttach.Text = "Attach";
          this.processAttach.ToolTipText = "Hint: You can click and drag the target icon bolow onto a process to attach.";
          this.processAttach.Click += new System.EventHandler(this.processAttach_Click);
          // 
          // toolStripButton6
          // 
          this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolStripButton6.Enabled = false;
          this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
          this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolStripButton6.Name = "toolStripButton6";
          this.toolStripButton6.Size = new System.Drawing.Size(86, 19);
          this.toolStripButton6.Text = "Open";
          // 
          // toolStripSettings
          // 
          this.toolStripSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolStripSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSettings.Image")));
          this.toolStripSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
          this.toolStripSettings.Name = "toolStripSettings";
          this.toolStripSettings.Size = new System.Drawing.Size(86, 19);
          this.toolStripSettings.Text = "Settings";
          this.toolStripSettings.Click += new System.EventHandler(this.toolStripSettings_Click);
          // 
          // lblMainModulePath
          // 
          this.lblMainModulePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.lblMainModulePath.Location = new System.Drawing.Point(0, 179);
          this.lblMainModulePath.Name = "lblMainModulePath";
          this.lblMainModulePath.Size = new System.Drawing.Size(96, 16);
          this.lblMainModulePath.TabIndex = 23;
          this.lblMainModulePath.Text = "n/a";
          this.lblMainModulePath.Click += new System.EventHandler(this.lblMainModulePath_Click);
          // 
          // lblTitle
          // 
          this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.lblTitle.Location = new System.Drawing.Point(0, 163);
          this.lblTitle.Name = "lblTitle";
          this.lblTitle.Size = new System.Drawing.Size(96, 16);
          this.lblTitle.TabIndex = 22;
          this.lblTitle.Text = "n/a";
          this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
          // 
          // iconBox
          // 
          this.iconBox.BackColor = System.Drawing.Color.Silver;
          this.iconBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.iconBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("iconBox.InitialImage")));
          this.iconBox.Location = new System.Drawing.Point(-1, 68);
          this.iconBox.Name = "iconBox";
          this.iconBox.Size = new System.Drawing.Size(90, 92);
          this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
          this.iconBox.TabIndex = 21;
          this.iconBox.TabStop = false;
          // 
          // label19
          // 
          this.label19.AutoSize = true;
          this.label19.BackColor = System.Drawing.Color.White;
          this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label19.Location = new System.Drawing.Point(2, 6);
          this.label19.Name = "label19";
          this.label19.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
          this.label19.Size = new System.Drawing.Size(54, 19);
          this.label19.TabIndex = 62;
          this.label19.Text = "Process";
          this.label19.Click += new System.EventHandler(this.label19_Click);
          // 
          // formMain
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(1188, 724);
          this.Controls.Add(this.splitContainer1);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Name = "formMain";
          this.Text = "Function Hacker";
          this.Load += new System.EventHandler(this.formMain_Load);
          this.contextMenuStripMemory.ResumeLayout(false);
          this.tabControlMain.ResumeLayout(false);
          this.tabPage1.ResumeLayout(false);
          this.panelMain.ResumeLayout(false);
          this.panelPlayBarContainer.ResumeLayout(false);
          this.splitContainer1.Panel1.ResumeLayout(false);
          this.splitContainer1.Panel1.PerformLayout();
          this.splitContainer1.Panel2.ResumeLayout(false);
          this.splitContainer1.ResumeLayout(false);
          this.panel2.ResumeLayout(false);
          this.panel3.ResumeLayout(false);
          this.panel3.PerformLayout();
          this.toolStrip2.ResumeLayout(false);
          this.toolStrip2.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelPlayBarContainer;
        private System.Windows.Forms.Label label20;
        private oVisPlayBar panelPlay;
        private oVisMain panelVisualization;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ImageList imageListTabs;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMemory;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditMemory;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private WindowFinder windowFinder;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton processAttach;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.Label lblMainModulePath;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.ToolStripButton toolStripSettings;
        private System.Windows.Forms.Label label19;
    }
}

