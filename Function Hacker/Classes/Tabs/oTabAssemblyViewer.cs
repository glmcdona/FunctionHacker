using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionHacker.Classes.Tabs;

namespace FunctionHacker.Classes
{
    class oTabAssemblyViewer : oTab
    {
        private ToolStripButton toolGotoAddress;
        private ToolStripButton toolSearchForExpression;
        private ToolStripButton toolClose;
        private TextBox assemblyViewControl;

        public oTabAssemblyViewer(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip, string tabTitle)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            InitializeComponents();
            panelMain.Controls.Add(this.assemblyViewControl);
            assemblyViewControl.Visible = false;
        }

        public override void activate()
        {
            if (this.activated)
                return;
            // This code runs when the tab is activated.
            base.activate();

            InitializeComponentsOnActive();
            assemblyViewControl.Visible = true;
        }

        public override void deactivate()
        {
            // This code runs when the tab is deactivated
            base.deactivate();
            toolStrip1.Items.Clear();

            assemblyViewControl.Visible = false;
        }

        public override bool isProcessSpecific()
        {
            return true;
        }

        private void InitializeComponentsOnActive()
        {
            this.toolGotoAddress = new System.Windows.Forms.ToolStripButton();
            this.toolSearchForExpression = new System.Windows.Forms.ToolStripButton();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightCyan;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolGotoAddress,
            this.toolSearchForExpression,
            this.toolClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(986, 25);
            this.toolStrip1.TabIndex = 62;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolGotoAddress
            // 
            this.toolGotoAddress.BackColor = System.Drawing.Color.LightCyan;
            this.toolGotoAddress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolGotoAddress.Enabled = true;
            this.toolGotoAddress.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolGotoAddress.Name = "toolGotoAddress";
            this.toolGotoAddress.Size = new System.Drawing.Size(92, 22);
            this.toolGotoAddress.Text = "Goto Address";
            this.toolGotoAddress.Click += new System.EventHandler(this.toolGotoAddress_Click);
            // 
            // toolFilterReplace
            // 
            this.toolSearchForExpression.BackColor = System.Drawing.Color.LightCyan;
            this.toolSearchForExpression.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSearchForExpression.Enabled = true;
            this.toolSearchForExpression.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSearchForExpression.Name = "toolSearchForExpression";
            this.toolSearchForExpression.Size = new System.Drawing.Size(37, 22);
            this.toolSearchForExpression.Text = "Search For Expression";
            this.toolSearchForExpression.Click += new System.EventHandler(this.toolSearchForExpression_Click);
            // 
            // toolClose
            // 
            this.toolClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolClose.Enabled = true;
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(40, 22);
            this.toolClose.Text = "Close";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);

            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
        }

        private void InitializeComponents()
        {
            // 
            // assemblyViewControl
            // 
            this.assemblyViewControl = new TextBox();
            this.assemblyViewControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            //this.assemblyViewControl.BackColor = System.Drawing.Color.White;
            this.assemblyViewControl.Location = new System.Drawing.Point(3, 28);
            this.assemblyViewControl.Name = "assemblyViewControl";
            this.assemblyViewControl.Size = new System.Drawing.Size(panelMain.Width - 15, panelMain.Height - 80);
            this.assemblyViewControl.Text = "TODO: Change this textbox control to an assembly viewer control.";
            this.assemblyViewControl.Multiline = true;
        }

        private void toolGotoAddress_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Clicked on the goto address toolbar button in the assembly viewer.");
        }

        private void toolSearchForExpression_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Search for expression.");
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Closing.");
        }

        
    }
}
