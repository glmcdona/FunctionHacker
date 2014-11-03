using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionHacker.Classes.Tabs;
using FunctionHacker.Classes.Visualization;
using FunctionHacker.Controls;
using FunctionHacker.Forms;

namespace FunctionHacker.Classes
{
    
    public class oTabCallSender : oTab
    {
        private callSender callSenderControl;


        public oTabCallSender(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip, string tabTitle, oFunctionList recordedData)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            

            // Initialize the controls we need
            InitializeComponents(recordedData);
        }


        private void InitializeComponents(oFunctionList recordedData)
        {
            this.callSenderControl = new callSender(recordedData);
            this.workingPage.Controls.Add(callSenderControl);
            this.callSenderControl.Location = new System.Drawing.Point(0, 0);
            this.callSenderControl.Dock = DockStyle.Fill;
        }


        public override bool isProcessSpecific()
        {
            return true;
        }
        

        public override void deactivate()
        {
            base.deactivate();
        }

        public override void activate()
        {
            if( this.activated )
                return;

            // Perform common activate function
            base.activate();
        }
    }

}
