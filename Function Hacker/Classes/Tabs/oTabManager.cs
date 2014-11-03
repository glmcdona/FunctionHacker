using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FunctionHacker.Classes.Visualization;

namespace FunctionHacker.Classes.Tabs
{
    public class oTabManager
    {
        private List<oTab> tabs;
        private TabControl tabController;
        private ToolStrip toolStrip;
        private oVisMain visMain;
        private oVisPlayBar visPlayBar;
        private int activeTab;
        private bool lockedFromMain;
        private bool lockedFromTab;
        private ToolStrip mainToolStrip;
        private Panel panelMain;

        public oTabManager( TabControl tabController, ToolStrip toolStrip, oVisMain visMain, oVisPlayBar visPlayBar, Panel panelMain )
        {
            this.tabController = tabController;
            this.toolStrip = toolStrip;
            this.visMain = visMain;
            this.visPlayBar = visPlayBar;
            this.panelMain = panelMain;
            tabs = new List<oTab>(0);
            activeTab = tabController.SelectedIndex;

            // Create the toolstrip
            mainToolStrip = new ToolStrip();
            panelMain.Controls.Add(this.mainToolStrip);

            // Let's create callbacks from the tab controller.
            tabController.SelectedIndexChanged += tabController_SelectedIndexChanged;
        }

        private void tabController_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Change the selected tab information

            // Deactivate previous tab
            if (activeTab >= 0 && activeTab < tabs.Count && tabs[activeTab].activated)
                tabs[activeTab].deactivate();
            
            // Activate current tab
            if (activeTab >= 0 && activeTab < tabs.Count && tabs[activeTab].activated == false)
            {
                // Activate this tab
                tabs[activeTab].activate();
            }
            activeTab = tabController.SelectedIndex;
        }

        /// <summary>
        /// This creates a new tab from a list of functions with specified name.
        /// </summary>
        /// <param name="name">Display name of the tab.</param>
        /// <param name="functionList">The list of functions to use.</param>
        /// <param name="select">Whether or not to select this new tab.</param>
        /// <param name="filtered">Whether or not is function subset - just for presentation.</param>
        public void addFunctionListTab(string name, oFunctionList functionList, bool select, bool filtered)
        {
            // Create the new oTab class and tab page
            oTabFunctionList tabFunctionList = new oTabFunctionList(this, toolStrip, panelMain, mainToolStrip, name, functionList, visMain, visPlayBar);
            tabs.Add(tabFunctionList);
            tabController.TabPages.Add(tabFunctionList.WorkingPage);
            tabFunctionList.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(filtered ? @"FunctionFilter.ico" : @"FunctionFull.ico");

            if( select )
            {
                // Deactivate the selected tab
                if( tabController.SelectedIndex >= 0 && tabController.SelectedIndex < tabController.TabPages.Count )
                    tabs[tabController.SelectedIndex].deactivate();

                // Select the new tab page
                tabController.SelectedIndex = tabs.Count - 1;
                tabs[tabs.Count - 1].activate();
            }
        }


        /// <summary>
        /// Creates a new export function tab for the specified function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="select"></param>
        /// <param name="function"></param>
        public void addExportFunctionTab(string name, bool select, oFunction function)
        {
            // Create the new oTab class and tab page
            oTabExportFunction tabExportFunction = new oTabExportFunction(this, toolStrip, panelMain, mainToolStrip, name, function);
            tabs.Add(tabExportFunction);
            tabController.TabPages.Add(tabExportFunction.WorkingPage);
            tabExportFunction.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(@"Export.ico");

            if (select)
            {
                // Deactivate the selected tab
                if (tabController.SelectedIndex >= 0 && tabController.SelectedIndex < tabController.TabPages.Count)
                    tabs[tabController.SelectedIndex].deactivate();

                // Select the new tab page
                tabController.SelectedIndex = tabs.Count - 1;
                tabs[tabs.Count - 1].activate();
            }
        }

        /// <summary>
        /// This creates a new tab to view introduction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="select"></param>
        public void addIntroTab(string name, bool select)
        {
            // Create the new oTab class and tab page
            oTabIntroPage tabIntroPage = new oTabIntroPage(this, toolStrip, panelMain, mainToolStrip, name);
            tabs.Add(tabIntroPage);
            
            tabController.TabPages.Add(tabIntroPage.WorkingPage);
            tabIntroPage.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(@"info.ico");

            if (select)
            {
                tabs[tabController.SelectedIndex].deactivate();
                tabController.SelectedTab = tabIntroPage.WorkingPage;
                tabs[tabIntroPage.WorkingPage.TabIndex].activate();
            }
            activeTab = tabController.SelectedIndex;
        }

        /// <summary>
        /// This creates a new tab to view assembly.
        /// </summary>
        /// <param name="name">Display name of the tab.</param>
        /// <param name="select">Whether or not to select this new tab.</param>
        public void addAssemblyViewerTab(string name, bool select)
        {
            // Create the new oTab class and tab page
            tabs.Add(new oTabAssemblyViewer(this, toolStrip, panelMain, mainToolStrip, name));
            tabController.TabPages.Add(name);
            if (select)
            {
                // Deactivate the selected tab
                if (tabController.SelectedIndex >= 0 && tabController.SelectedIndex < tabController.TabPages.Count)
                    tabs[tabController.SelectedIndex].deactivate();

                // Select the new tab page
                tabController.SelectedIndex = tabs.Count - 1;
                tabs[tabs.Count - 1].activate();
            }
        }

        /// <summary>
        /// This creates a new tab to view memory.
        /// </summary>
        /// <param name="name">Display name of the tab.</param>
        /// <param name="select">Whether or not to select this new tab.</param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        public void addMemoryViewerTab(string name, bool @select, ulong address, uint length)
        {
            // Create the new oTab class and tab page
            oTabMemoryViewer tabMemoryViewer = new oTabMemoryViewer(this, toolStrip, panelMain, mainToolStrip, name, address, length);
            tabs.Add(tabMemoryViewer);
            tabController.TabPages.Add(tabMemoryViewer.WorkingPage);
            tabMemoryViewer.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(@"memoryview.ico");

            if (select)
                tabController.SelectedTab = tabMemoryViewer.WorkingPage;
        }

        /// <summary>
        /// This creates a new tab for sending funciton calls.
        /// </summary>
        /// <param name="name">Display name of the tab.</param>
        /// <param name="select">Whether or not to select this new tab.</param>
        public void addCallSenderTab(string name, bool @select, oFunctionList data)
        {
            // Create the new oTab class and tab page
            oTabCallSender tabCallSender = new oTabCallSender(this, toolStrip, panelMain, mainToolStrip, name, data);
            tabs.Add(tabCallSender);
            tabController.TabPages.Add(tabCallSender.WorkingPage);
            tabCallSender.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(@"memoryview.ico");

            if (select)
                tabController.SelectedTab = tabCallSender.WorkingPage;
        }

        /// <summary>
        /// This creates a new tab to view memory.
        /// </summary>
        /// <param name="name">Display name of the tab.</param>
        /// <param name="select">Whether or not to select this new tab.</param>
        public void addSettingsTab(string name, bool @select)
        {
            if (tabController.TabPages[name] == null)
            {
                // Create the new oTab class and tab page
                oTab oTabSettingsPage = new oTabSettingsPage(this, toolStrip, panelMain, mainToolStrip, name);
                tabs.Add(oTabSettingsPage);
                tabController.TabPages.Add(oTabSettingsPage.WorkingPage);
                oTabSettingsPage.WorkingPage.ImageIndex = tabController.ImageList.Images.IndexOfKey(@"settings.ico");
            }
            if (select)
                tabController.SelectedTab = tabController.TabPages[name];
        }

        /// <summary>
        /// Locks the tab control the main interface. This should be set True when not attached to a process, and things like that.
        /// </summary>
        /// <param name="locked"></param>
        public void mainLock( bool locked )
        {
            lockedFromMain = locked;
            updateControlLock();
        }

        /// <summary>
        /// This locks the tab control from the tab page. This should be done when doing something like making a data recording.
        /// </summary>
        /// <param name="locked"></param>
        public void tabLock( bool locked )
        {
            lockedFromTab = locked;
            updateControlLock();
        }

        private void updateControlLock()
        {
            if( lockedFromMain || lockedFromTab )
            {
                // Lock the tab control
                //tabController.Enabled = false;
            }else
            {
                // Unlock the tab
                //tabController.Enabled = true;
            }
        }

        /// <summary>
        /// close tab
        /// </summary>
        /// <param name="tab"></param>
        public void closeTab(oTab tab, bool overrideLock)
        {
            int index = tabs.IndexOf(tab);
            if( index < tabs.Count && index >= 0 )
            {
                // Close the tab only if it is not the complete function list; this cannot be closed.
                if (overrideLock || tab.GetName() != "Function List: Full")
                {
                    if (tab.activated)
                        tab.deactivate();
                    if (index == tabController.SelectedIndex && index - 1 > 0)
                    {
                        tabs[index - 1].activate();
                        tabController.SelectedIndex = index - 1;
                    }
                    else if (index == tabController.SelectedIndex && index + 1 < tabs.Count)
                    {
                        tabs[index + 1].activate();
                        tabController.SelectedIndex = index + 1;
                    }
                    tabs.Remove(tab);
                    tabController.TabPages.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// close tab at position
        /// </summary>
        public void closeTab(int tabIndex, bool overrideLock)
        {
            closeTab(tabs[tabIndex], overrideLock);
        }

        /// <summary>
        /// Return currently active tab
        /// </summary>
        public oTab ActiveTab
        {
            get { return tabs[activeTab]; }
        }

        /// <summary>
        /// Cleans up tabs that are specific to the process.
        /// </summary>
        public void cleanupProcessSpecificTabs()
        {
            for (int i = 0; i < tabs.Count; i++ )
            {
                if (tabs[i].isProcessSpecific())
                {
                    this.closeTab(tabs[i], true);
                    i--;
                }
            }
        }

        
    }
}
