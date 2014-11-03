using System;
using System.Windows.Forms;
using FunctionHacker.Controls;

namespace FunctionHacker.Classes.Tabs
{
    class oTabMemoryViewer : oTab
    {
        public oTabMemoryViewer(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip, String tabTitle, ulong address, uint length)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            MemoryViewer memoryViewer = new MemoryViewer(address, length) {Dock = DockStyle.Fill};
            WorkingPage.Controls.Add(memoryViewer);
        }

        public override bool isProcessSpecific()
        {
            return true;
        }

        public override void deactivate()
        {
            
        }

        public override void activate()
        {
            
        }
    }
}
