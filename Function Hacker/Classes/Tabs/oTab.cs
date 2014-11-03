using System.Windows.Forms;

namespace FunctionHacker.Classes.Tabs
{
    public class oTab
    {
        public bool activated;
        protected ToolStrip toolStrip;
        protected oTabManager parent;
        protected Panel panelMain;
        protected ToolStrip toolStrip1;
        protected string tabTitle;
        protected TabPage workingPage;

        public oTab(oTabManager parent, ToolStrip toolStrip, Panel panelMain, ToolStrip mainToolStrip, string tabTitle)
        {
            this.toolStrip = toolStrip;
            this.parent = parent;
            this.panelMain = panelMain;
            toolStrip1 = mainToolStrip;
            this.tabTitle = tabTitle;
            activated = false;
            workingPage = new TabPage(tabTitle);
        }

        ~oTab()
        {
            // Release the tab lock if we were active
            if( activated )
                deactivate();
        }

        /// <summary>
        /// Container for our visible components
        /// </summary>
        public TabPage WorkingPage
        {
            get { return workingPage; }
        }

        /// <summary>
        /// Container for our visible components
        /// </summary>
        public oTabManager Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// This activates the tab and initializes all the GUI elements. This function is designed to be
        /// overriden by the specialized tab elements.
        /// </summary>
        public virtual void activate()
        {
            activated = true;
        }

        /// <summary>
        /// Returns if this tab is process specific. Process specific tabs will be closed when attaching to
        /// a new process.
        /// </summary>
        public virtual bool isProcessSpecific()
        {
            return false;
        }

        /// <summary>
        /// This deactivates the tab and hides/cleans up all the GUI elements. This function is designed to be
        /// overriden by the specialized tab elements.
        /// </summary>
        public virtual void deactivate()
        {
            if (activated)
            {
                parent.tabLock(false);
                activated = false;

                // Clean up the toolstrip
                toolStrip1.Items.Clear();
            }
        }

        /// <summary>
        /// Get tab Title
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return tabTitle;
        }
    }
}
