using System.Windows.Forms;
using FunctionHacker.Controls;

namespace FunctionHacker.Classes.Tabs
{
    internal class oTabSettingsPage : oTab
    {
        public oTabSettingsPage(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip,
                             string tabTitle)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            userSettings about = new userSettings { Dock = DockStyle.Fill };
            workingPage.Name = tabTitle;
            workingPage.Controls.Add(about);
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