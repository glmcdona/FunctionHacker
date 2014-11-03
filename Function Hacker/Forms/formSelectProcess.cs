using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BufferOverflowProtection;
using FunctionHacker.Classes;

namespace FunctionHacker.Forms
{
    public partial class FormSelectProcess : Form
    {
        public FormSelectProcess()
        {
            InitializeComponent();
        }

        private void FormSelectProcessLoad(object sender, EventArgs e)
        {
            // Refresh the process list
            RefreshProcessList();
            dataProcessList.Sort(dataProcessList.Columns[1], ListSortDirection.Descending);
        }

        private void RefreshTimerTick(object sender, EventArgs e)
        {
            // Refresh the process list
            RefreshProcessList();
        }

        private void RefreshProcessList()
        {
            try
            {
                SortOrder sortOrder = dataProcessList.SortOrder;
                DataGridViewColumn sortedColumn = dataProcessList.SortedColumn;

                int index = 0;
                if (dataProcessList.SelectedRows.Count > 0)
                {
                    index = dataProcessList.SelectedRows[0].Index;
                }

                // Clear the process list
                dataProcessList.Rows.Clear();
                //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataProcessList, new object[] { true });
                // Load the process list
                string[,] processList = oProcess.getProcessList();
                if (processList == null)
                {
                    oConsole.printMessage("oProcess.getProcessList() returned null");
                }
                else
                {
                    // Add the processes to the process list
                    for (int i = 0; i < processList.GetLength(0); i++)
                    {
                        oConsole.printMessage("Process " + processList[i, 0] + ", " + processList[i, 1] + ", " +
                                              processList[i, 2] + ", " + processList[i, 3] + ", " + processList[i, 4]);
                        dataProcessList.Rows.Add(new[]
                                                     {
                                                         processList[i, 0], processList[i, 1], processList[i, 2],
                                                         processList[i, 3], processList[i, 4]
                                                     });
                        if (processList[i, 3].CompareTo("Higher Privilege Required") == 0)
                            dataProcessList.Rows[dataProcessList.Rows.Count - 1].Cells[3].Style.BackColor = Color.Red;
                    }
                }
                if (sortedColumn != null)
                    dataProcessList.Sort(sortedColumn, sortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
                dataProcessList.Rows[index].Selected = true;
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
            }
        }

        private void dataProcessList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataProcessList.Rows.Count)
                {
                    // Load the active process
                    if (oProcess.setActiveProcess(new[]
                                                      {
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[0].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[1].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[2].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[3].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[4].Value
                                                      }))
                    {
                        // Load the module list for this process
                        oConsole.printMessage("Set '" + dataProcessList.Rows[e.RowIndex].Cells[0].Value +
                                              "' to be the active process.");
                        Close();
                    }
                    else
                    {
                        oConsole.printMessageShow("Failed to set active process to " +
                                              dataProcessList.Rows[e.RowIndex].Cells[0].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[1].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[2].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[3].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[4].Value + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
            }
        }

        private void DataProcessListCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DataProcessListCellDoubleClick1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataProcessList.Rows.Count)
                {
                    // Load the active process
                    if (oProcess.setActiveProcess(new[]
                                                      {
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[0].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[1].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[2].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[3].Value,
                                                          (string) dataProcessList.Rows[e.RowIndex].Cells[4].Value
                                                      }))
                    {
                        // Load the module list for this process
                        oConsole.printMessage("Set '" + dataProcessList.Rows[e.RowIndex].Cells[0].Value +
                                              "' to be the active process.");
                        Close();
                    }
                    else
                    {
                        oConsole.printMessage("Failed to set active process to " +
                                              dataProcessList.Rows[e.RowIndex].Cells[0].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[1].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[2].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[3].Value + ", " +
                                              dataProcessList.Rows[e.RowIndex].Cells[4].Value + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                oConsole.printException(ex);
            }
        }

        private void DataProcessListCellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataProcessList_CellDoubleClick(sender, e);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
        }
    }
}