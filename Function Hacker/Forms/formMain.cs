using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BufferOverflowProtection;
using FunctionHacker.Classes;
using FunctionHacker.Classes.Tabs;
using FunctionHacker.Properties;

namespace FunctionHacker.Forms
{
    public partial class formMain : Form
    {
        private readonly oTabManager tabManager;
        private readonly SystemHotKey hotKey;

        public formMain()
        {
            InitializeComponent();

            // Create the tab manager
            tabControlMain.TabPages.Clear();
            tabManager = new oTabManager(tabControlMain, toolStrip2, panelVisualization, panelPlay, panelMain);

            // Add the basic tabs
            tabManager.addIntroTab("Introduction Page", true);
            //tabManager.addFunctionListTab("Function List: Empty", new oFunctionList( new List<oFunction>()), true );
            //tabManager.addAssemblyViewerTab("Assembly Viewer 1",false);
            //tabManager.addAssemblyViewerTab("Assembly Viewer 2",false);
            //tabManager.addMemoryViewerTab("Memory Viewer", false);

            // Main lock the tab control
            tabManager.mainLock(true);

            //Hotkey handler (we can register more hotkeys if we need them)
            hotKey = new SystemHotKey(Keys.F5, SystemHotKey.KeyModifiers.Control, OnHotKey);
        }

        private void OnHotKey(object sender, EventArgs eventArgs)
        {
            oTab activeTab = tabManager.ActiveTab;
            if (activeTab is oTabFunctionList)
                (activeTab as oTabFunctionList).InvokeStartStopRecording();
        }

        private void processAttach_Click(object sender, EventArgs e)
        {
            AttachProcess(null);
        }

        /// <summary>
        /// Attach process for injection
        /// </summary>
        /// <param name="process"></param>
        private void AttachProcess(Process process)
        {
            try
            {
                oProcess.clearActiveProcess(tabManager);
                if (process == null)
                {
                    FormSelectProcess selectProcess = new FormSelectProcess();
                    oConsole.printMessage("Prompting user for process to attach to...");
                    selectProcess.ShowDialog(this);
                }
                else
                {
                    // Set the active process
                    oProcess.activeProcess = process;
                }
                // If we have an active process then make our code injections
                if (oProcess.activeProcess != null)
                {
                    // Let the user select the modules
                    formSelectModules selectModule = new formSelectModules();
                    oConsole.printMessage("Prompting user for modules to analyze...");

                    if (selectModule.ShowDialog(this) == DialogResult.OK)
                    {
                        oConsole.printMessage(oProcess.activeModules.Count + " modules were selected by user.");

                        // Initialize the process information
                        oProcess.generateMemoryMap();
                        initProcessInformation();

                        // Let the user modify the selected heaps if desired
                        formSelectHeaps selectHeaps = new formSelectHeaps(oProcess.map, oProcess.activeModules,
                                                                          oProcess.disassemblyMode, this);
                        selectHeaps.ShowDialog(this);

                        if (selectHeaps.DialogResult == DialogResult.OK)
                        {
                            // Begin disassembly process
                            oConsole.printMessage("Disassemblying active modules...");
                            oProcess.disassembleProcess(this, selectHeaps.getSelectedHeaps());

                            // Begin the injection process
                            if (oProcess.inject(this, selectHeaps.getInvalidSourceHeaps()))
                            {
                                tabManager.addFunctionListTab("Function List: Full",
                                                              new oFunctionList(oFunctionMaster.functions), true,
                                                              false);
                                tabManager.mainLock(false); // release our lock
                            }
                            else
                            {
                                tabManager.mainLock(true); // lock the tab bar
                                oConsole.printMessageShow("ERROR: Failed to inject code.");
                            }
                        }
                        else
                        {
                            tabManager.mainLock(true); // lock the tab bar
                            oConsole.printMessage("The user cancelled at the heap selection menu.");
                        }
                    }
                    else
                    {
                        tabManager.mainLock(true); // lock the tab bar
                        oConsole.printMessage("No modules were selected by user.");
                    }
                }
                else
                {
                    tabManager.mainLock(true); // lock the tab bar
                }
            }
            catch (Exception ex)
            {
                tabManager.mainLock(true); // lock the tab bar
                oConsole.printException(ex);
            }
        }

        private void initProcessInformation()
        {
            // Extact the information about the process
            lblMainModulePath.Text = oProcess.activeProcess.MainModule.FileName;
            lblTitle.Text = oProcess.activeProcess.MainWindowTitle;

            try
            {
                // Extract the icon
                IntPtr[] hDummy = new IntPtr[1] {IntPtr.Zero};
                IntPtr[] hIconEx = new IntPtr[1] {IntPtr.Zero};
                int readIconCount =
                    (int) ExtractIconEx(oProcess.activeProcess.MainModule.FileName, 0, hIconEx, hDummy, (uint) 1);
                if (readIconCount > 0 && hIconEx[0] != IntPtr.Zero)
                {
                    Icon extractedIcon = (Icon) Icon.FromHandle(hIconEx[0]).Clone();
                    iconBox.Image = extractedIcon.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                // Extracting the icon failed
                oConsole.printMessage("Failed to extract the icon from the process.");
                oConsole.printException(ex);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex,
                                                 IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);


        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
                                                 [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        /// <summary>
        /// Check if process is 64 bit - untill now not supported 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static bool Is64BitProcess(Process process)
        {
            bool isWow64Process;
            if (!IsWow64Process(process.Handle, out isWow64Process))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return !isWow64Process;
        }

        /// <summary>
        /// Search process over window handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowFinder_MouseUp(object sender, MouseEventArgs e)
        {
            //bring GUI back
            BringToFront();
            //get a rid of ugly red frame 
            windowFinder.RefreshWindow();
            if (windowFinder.SelectedHandle != IntPtr.Zero)
            {
                int targetProcessId;
                GetWindowThreadProcessId(windowFinder.SelectedHandle, out targetProcessId);
                Process process = Process.GetProcessById(targetProcessId);
                //if (Is64BitProcess(process))
                //{
                //    oConsole.printMessageShow("ERROR: 64 Bit processes are not supported in moment!");
                //    return;
                //}
                try
                {
                    if (process.MainModule.FileName != string.Empty)
                    {
                        AttachProcess(process);
                    }
                }
                catch (Exception)
                {
                    oConsole.printMessageShow("ERROR: Not enough rights to attach to process!");
                }
            }
        }

        private void windowFinder_MouseDown(object sender, MouseEventArgs e)
        {
            //hide GUI if process runnign in background
            SendToBack();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            // Create the assembly generator to trigger ASM code errors for the developer.
            //oAssemblyGenerator.buildInjection(1000, oAssemblyCode.mainRecordFunction, null, 1000, 10000);
        }

        private void tabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brushText = new SolidBrush(Color.Black);
            SolidBrush brushBackground = new SolidBrush(tabControlMain.BackColor);

            TabPage tabPage = tabControlMain.TabPages[e.Index];
            Rectangle myTabRect = e.Bounds;
            Rectangle innerBounds = new Rectangle(e.Bounds.X + 25, e.Bounds.Y + 4, e.Bounds.Width - 20, e.Bounds.Height);
            g.FillRectangle(brushBackground, myTabRect);

            string textOut = tabPage.Text;
            tabPage.ToolTipText = textOut;
            if (tabPage.ToolTipText.Length > 20)
                textOut = tabPage.ToolTipText.Substring(0, 19) + "...";
            else
                textOut = tabPage.ToolTipText;
            if (tabPage.ImageIndex != -1)
                g.DrawImage(imageListTabs.Images[tabPage.ImageIndex], myTabRect.X + 4, myTabRect.Y + 3);
            g.DrawString(textOut, DefaultFont, brushText, innerBounds);
            g.DrawImage(imageListTabs.Images[0], myTabRect.X + myTabRect.Width - 20, myTabRect.Y + 2);
        }

        private void tabControlMain_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControlMain.TabPages.Count; i++)
            {
                Rectangle rect = tabControlMain.GetTabRect(i);
                if (rect.Contains(e.X, e.Y))
                {
                    double positionX = rect.X + rect.Width - 20;
                    //if (e.Button == MouseButtons.Left && e.X - rect.X <= 18 && e.Y - rect.Y <= 18)
                    if (e.Button == MouseButtons.Left && e.X > positionX)
                    {
                        //if (i > 1)
                        //    if (tabControlMain.SelectedIndex == i)
                        //        tabControlMain.SelectedIndex = i - 1;
                        //tabControlMain.TabPages.RemoveAt(i);
                        tabManager.closeTab(i, false);
                    }
                    else
                        tabControlMain.SelectedIndex = i;
                    break;
                }
            }
        }

        private void panel3_DoubleClick(object sender, EventArgs e)
        {
            tabManager.mainLock(false);
            //components test area :)
            //tabManager.addMemoryViewerTab("Memory view", true, 0, 200);
            tabManager.addSettingsTab("Application settings", true);
        }


        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblMainModulePath_Click(object sender, EventArgs e)
        {

        }



        private void toolStripSettings_Click(object sender, EventArgs e)
        {
            tabManager.mainLock(false);
            tabManager.addSettingsTab("Application settings", true);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}