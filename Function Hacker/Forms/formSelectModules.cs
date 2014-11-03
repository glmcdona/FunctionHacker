using System;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes;
using FunctionHacker.Properties;

namespace FunctionHacker.Forms
{
    public partial class formSelectModules : Form
    {
        public formSelectModules()
        {
            InitializeComponent();
        }

        private void formSelectModules_Load(object sender, EventArgs e)
        {
            Text = @"Process: > " + oProcess.activeProcess.ProcessName + @" < pid: " + oProcess.activeProcess.Id +
                   @" - Select modules for injection";
            refreshModuleList();
            string windowsDir = Environment.GetEnvironmentVariable("SystemRoot");
            foreach (ListViewItem item in listModules.Items)
            {
                if (Settings.Default.RBExeOnly || Settings.Default.RBNonWindowsDll)
                    item.Checked = (item.Text == oProcess.activeProcess.MainModule.ModuleName);
                if (Settings.Default.RBNonWindowsDll)
                    if (windowsDir != null)
                        item.Checked |= !item.SubItems[2].Text.Contains(windowsDir);
                if (Settings.Default.RBAllDll)
                    item.Checked = true;
            }
        }

        private void refreshModuleList()
        {
            try
            {
                // Clear the module list
                listModules.Items.Clear();

                // Load the module list
                string[,] moduleList = oProcess.getModuleList();
                if (moduleList == null)
                {
                    oConsole.printMessage("oProcess.getModuleList() returned null");
                }
                else
                {
                    // Add the modules to the module list
                    for (int i = 0; i < moduleList.GetLength(0); i++)
                    {
                        oConsole.printMessage("Module " + moduleList[i, 0] + ", " + moduleList[i, 1] + ", " +
                                              moduleList[i, 2]);
                        listModules.Items.Add(
                            new ListViewItem(new[] {moduleList[i, 0], moduleList[i, 1], moduleList[i, 2]}));
                    }
                }

                // Sort the list
                listModules.Sort();
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Set the disassembly mode
            oProcess.setDisassemblyMode( new DISASSEMBLY_MODE(checkInjectCallbacks.Checked,
                !checkOnlyIntermodular.Checked, checkForceAllIntermodular.Checked, false, checkAggressiveDereferencing.Checked) );

            // Add all the selected modules to the active module list.
            foreach (ListViewItem item in listModules.Items)
            {
                if (item.Checked)
                {
                    if (
                        oProcess.addActiveModule(new[]
                                                     {
                                                         item.SubItems[0].Text, item.SubItems[1].Text,
                                                         item.SubItems[2].Text
                                                     }))
                    {
                        oConsole.printMessage("Activated module " + item.SubItems[0].Text + ", " + item.SubItems[1].Text +
                                              ", " + item.SubItems[2].Text);
                    }
                    else
                    {
                        oConsole.printMessage("Failed to activate module " + item.SubItems[0].Text + ", " +
                                              item.SubItems[1].Text + ", " + item.SubItems[2].Text);
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            oConsole.printMessage("User cancelled selection of modules.");
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listModules.Items)
                item.Checked = true;
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listModules.Items)
                item.Checked = false;
        }

        private void checkAggressiveDereferencing_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkOnlyIntermodularCheckedChanged(object sender, EventArgs e)
        {
            if( checkOnlyIntermodular.Checked == true )
            {
                // Disable callback recording
                checkInjectCallbacks.Checked = false;
                checkInjectCallbacks.Enabled = false;
            }else
            {
                // Enable callback recording
                checkInjectCallbacks.Enabled = true;
            }
                
        }
    }
}