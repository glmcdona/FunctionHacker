using System.Windows.Forms;
using FunctionHacker.Controls;

namespace FunctionHacker.Classes.Tabs
{
    internal class oTabExportFunction : oTab
    {
        private oFunction function;

        public oTabExportFunction(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip,
                             string tabTitle, oFunction function)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            ExportFunction exportFunction = new ExportFunction(function) { Dock = DockStyle.Fill };
            workingPage.Controls.Add(exportFunction);
        }

        public override void activate()
        {
            if (activated)
                return;
            // This code runs when the tab is activated.
            base.activate();
            InitializeComponentsOnActive();
        }

        private static void InitializeComponentsOnActive()
        {
        }
    }
}