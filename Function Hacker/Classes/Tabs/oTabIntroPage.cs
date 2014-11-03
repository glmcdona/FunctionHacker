using System.Windows.Forms;
using FunctionHacker.Controls;

namespace FunctionHacker.Classes.Tabs
{
    internal class oTabIntroPage : oTab
    {
        public oTabIntroPage(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip,
                             string tabTitle)
            : base(parent, toolStrip, panelMain, mainToolStrip, tabTitle)
        {
            About about = new About {Dock = DockStyle.Fill};
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