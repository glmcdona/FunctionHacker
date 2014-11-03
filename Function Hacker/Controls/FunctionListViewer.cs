using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FunctionHacker.Classes;
using FunctionHacker.Classes.Tabs;
using FunctionHacker.Forms;
using FunctionHacker.Properties;

namespace FunctionHacker.Controls
{
    public partial class FunctionListViewer : UserControl
    {
        private readonly oTab tab;
        private oFunctionList functionList;
        private bool callListResizeRows = true;
        //private oVisPlayBar controlVisPlayBar = null;

        public FunctionListViewer()
        {
            InitializeComponent();
            //controlVisPlayBar = new oVisPlayBar(100);

            // Set the DataGridViewCall parent
            dataGridCallArguments.setParent(dataGridCalls);

            // Set the main panel parent to the play bar
            controlVisMain.setPlayBar(controlVisPlayBar);
            controlVisPlayBar.setMainVisualization(controlVisMain);
        }

        public FunctionListViewer(oTab tab)
        {
            InitializeComponent();
            //controlVisPlayBar = new oVisPlayBar(100);
            this.tab = tab;
            splitContainerFunctionsPlaybar.Panel2.Controls.Add(controlVisPlayBar);
            controlVisPlayBar.Dock = DockStyle.Fill;
            controlVisPlayBar.UpdateRate = (int) Settings.Default.TimelineRefreshRate;
            //controlVisPlayBar.Location = new Point(2, Height - controlVisPlayBar.Height);

            // Set the DataGridViewCall parent
            dataGridCallArguments.setParent(dataGridCalls);

            // Set the main panel parent to the play bar
            controlVisMain.setPlayBar(controlVisPlayBar);
            controlVisPlayBar.setMainVisualization(controlVisMain);

            // Initialize the main visualization lookup tables
            controlVisMain.initializeFunctionList(oProcess.map, oFunctionMaster.functions);
        }


        /// <summary>
        /// Used to set and get the function list associated with this control.
        /// </summary>
        public oFunctionList FunctionList
        {
            get { return functionList; }
            set
            {
                functionList = value;
                controlVisPlayBar.setData(functionList);
                InvalidateData();
            }
        }

        /// <summary>
        /// Invalidates all the data of the data grid view controls.
        /// </summary>
        public void InvalidateData()
        {
            invalidateFunctionsData(0);
            invalidateCallsData();
            InvalidateCallCountDataOnly();
        }

        private void invalidateCallsData()
        {
            // Invalidate the call list information
            int newCount = (functionList != null && functionList.dataVis != null ? functionList.getCallCount() : 0);
            if (newCount != dataGridCalls.RowCount)
            {
                // Set the new call data information
                dataGridCalls.Rows.Clear();
                dataGridCalls.RowCount = newCount;

                // Pre-process row sizes if the row count is less than 2,000
                if (newCount < 2000 && functionList.dataVis != null && functionList != null)
                {
                    // Measure the size of a single line
                    int height =
                        TextRenderer.MeasureText("a" + Environment.NewLine + "a", dataGridCalls.DefaultCellStyle.Font).
                            Height/2;
                    dataGridCalls.SuspendLayout();
                    dataGridCalls.Visible = false;

                    // Resize all the rows, so that it is continuous scrolling
                    for (int i = 0; i < dataGridCalls.Rows.Count; i++)
                    {
                        // Decide on the size of this row
                        int numLineBreaks = functionList.getFunctionCall(i).arguments.Count() + 3;
                        dataGridCalls.Rows[i].Height = height*numLineBreaks;
                    }

                    // Resume layout
                    dataGridCalls.Visible = true;
                    dataGridCalls.ResumeLayout();

                    // Do not use the row prepaint handler to resize rows
                    callListResizeRows = false;
                }else
                {
                    // Use the row prepaint handler to resize rows
                    callListResizeRows = true;
                }
            }
        }

        private void dataGridCalls_RowPrepaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if( callListResizeRows && functionList != null && functionList.dataVis != null )
            {
                // Resize this row
                int numLineBreaks = functionList.getFunctionCall(e.RowIndex).arguments.Count() + 3;
                dataGridCalls.Rows[e.RowIndex].Height = (TextRenderer.MeasureText("a" + Environment.NewLine + "a", dataGridCalls.DefaultCellStyle.Font).Height * numLineBreaks) / 2;
            }
        }

        private void invalidateFunctionsData(int selectRow)
        {
            // Invalidate the function list information
            int newCount = (functionList != null ? functionList.getCount() : 0);
            if (newCount != dataGridFunctions.RowCount)
            {
                dataGridFunctions.Rows.Clear();
                dataGridFunctions.RowCount = newCount;
                if (newCount == 0)
                    return;
                if (selectRow >= newCount)
                    selectRow = newCount - 1;
                dataGridFunctions.CurrentCell = dataGridFunctions.Rows[selectRow].Cells[1];
            }
        }

        /// <summary>
        /// Invalidates only the call count information of the data.
        /// </summary>
        public void InvalidateCallCountDataOnly()
        {
            // Invalidate the call count data
            dataGridFunctions.InvalidateColumn(2); // Call count column
            dataGridCalls.Invalidate();
        }


        /// <summary>
        /// This updates whether the toolstrip buttons are pressable or not.
        /// </summary>
        private void updateToolBarEnabled()
        {
            // Update the status of the toolbar buttons
            if (functionList == null)
            {
                toolButtonFilter.Enabled = false;
                toolButtonFilterComparison.Enabled = false;
                toolButtonRename.Enabled = false;
                toolButtonSave.Enabled = false;
                toolButtonRecord.Enabled = false;
            }
            else if (toolButtonRecord.Text == @"Stop Recording")
            {
                toolButtonFilter.Enabled = false;
                toolButtonFilterComparison.Enabled = false;
                toolButtonRename.Enabled = false;
                toolButtonSave.Enabled = false;
                toolButtonRecord.Enabled = true;
            }
            else if (!oProcess.processStillRunning())
            {
                toolButtonFilter.Enabled = true;
                toolButtonFilterComparison.Enabled = true;
                toolButtonRename.Enabled = true;
                toolButtonSave.Enabled = true;
                toolButtonRecord.Enabled = false;
            }
            else
            {
                toolButtonFilter.Enabled = true;
                toolButtonFilterComparison.Enabled = true;
                toolButtonRename.Enabled = true;
                toolButtonSave.Enabled = true;
                toolButtonRecord.Enabled = true;
            }
        }


        /// <summary>
        /// We are starting or stopping data recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolButtonRecord_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            if (toolButtonRecord.Text == @"Start Recording")
            {
                if (!oProcess.processStillRunning())
                {
                    MessageBox.Show(
                        @"Attached process already terminated. Try to restart process and attach to it again.");
                    return;
                }

                // Update the settings incease they have been changed
                oFunctionMaster.applySettings();

                // Start recording data
                if (functionList.startRecording())
                {
                    toolButtonRecord.Text = @"Stop Recording";

                    // Cleanup the breakdown plot
                    controlVisPlayBar.removeAllExtraTimeSeries();
                    this.autosizePlaybar();

                    // Reset the zoom on the playbar
                    controlVisPlayBar.resetZoom();

                    // Disable the breakdown plot button
                    buttonBreakdown.Enabled = false;

                    // Disable toolbar buttons
                    updateToolBarEnabled();

                    // Invalidate the call count data
                    InvalidateData();

                    // Lock the tab menu
                    //tab.Parent.tabLock(true);
                }
            }
            else
            {
                // Stop recording
                functionList.stopRecording();
                toolButtonRecord.Text = @"Start Recording";

                // Reset the zoom on the playbar
                controlVisPlayBar.resetZoom();

                // Enable the breakdown plot button
                buttonBreakdown.Enabled = true;

                // Enable toolbar buttons
                updateToolBarEnabled();

                // Invalidate the call counts
                InvalidateData();

                // Disable this from being clicked on for 2 seconds to prevent
                // accidental double-clicking on this.
                toolButtonRecord.Enabled = false;
                enableStartRecording.Enabled = true;

                // Unlock the tab menu
                //tab.Parent.tabLock(false);
            }
        }

        /// <summary>
        /// Present the filter data dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolButtonFilter_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;

            // Generate the filter type
            FILTER_TYPE type = FILTER_TYPE.FUNCTION_LIST_FILTERED;
            if (tab.GetName() == "Function List: Full")
                type = FILTER_TYPE.FUNCTION_LIST_FULL;

            formFilter filterDialog = new formFilter(functionList, tab.Parent, (oTabFunctionList) tab,
                                              type,
                                              controlVisPlayBar);
            filterDialog.ShowDialog(TopLevelControl);
        }

        private void toolButtonFilterComparison_Click(object sender, EventArgs e)
        {
        }

        private void toolButtonRename_Click(object sender, EventArgs e)
        {
        }

        private void toolButtonSave_Click(object sender, EventArgs e)
        {
        }

        private void dataGridCalls_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Request this cell value fromt he current function list
            if (controlVisPlayBar != null && functionList != null)
            {
                // Determine if this call is selected
                bool selected = controlVisPlayBar.isCallSelected(e.RowIndex);
                dataGridCalls.Rows[e.RowIndex].DefaultCellStyle.BackColor = selected ? Color.LightGray : Color.White;

                // Request the call info at row and column
                e.Value = functionList.getCallListCell(e.RowIndex, e.ColumnIndex, dataGridCalls.Columns[e.ColumnIndex].Width, dataGridCalls.DefaultCellStyle.Font);
            }
        }

        private void dataGridFunctions_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Request this cell value from the current function list
            if (functionList != null)
            {
                e.Value = functionList.getFunctionListCell(e.RowIndex, e.ColumnIndex, controlVisPlayBar);
            }
        }

        /// <summary>
        /// Disables the selected functions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disableSelectedFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList != null && oProcess.processStillRunning())
            {
                List<int> rowIndices = new List<int>(dataGridFunctions.SelectedRows.Count);
                for (int i = 0; i < dataGridFunctions.SelectedRows.Count; i++)
                {
                    rowIndices.Add(dataGridFunctions.Rows.IndexOf(dataGridFunctions.SelectedRows[i]));
                }
                if (rowIndices.Count > 0)
                {
                    functionList.disableFunctions(rowIndices);

                    // Refresh the display column
                    dataGridFunctions.InvalidateColumn(0);
                }
            }
        }


        /// <summary>
        /// Enables the selected functions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableSelectedFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList != null && oProcess.processStillRunning())
            {
                List<int> rowIndices = new List<int>(dataGridFunctions.SelectedRows.Count);
                for (int i = 0; i < dataGridFunctions.SelectedRows.Count; i++)
                {
                    rowIndices.Add(dataGridFunctions.Rows.IndexOf(dataGridFunctions.SelectedRows[i]));
                }
                if (rowIndices.Count > 0)
                {
                    functionList.enableFunctions(rowIndices);

                    // Refresh the display column
                    dataGridFunctions.InvalidateColumn(0);
                }
            }
        }

        /// <summary>
        /// Deletes the selected functions as well as the corresponding calls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSelectedFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList == null)
                return;

            if (tab.GetName() == "Function List: Full")
            {
                // Can't delete the functions
                MessageBox.Show(
                    @"Cannot remove functions from the full function list. Instead, create a copy of the list first by pressing the Duplicate button.");
                return;
            }
            // Delete the selected functions
            // Generate a list of function addresses
            List<int> rows = new List<int>(dataGridFunctions.SelectedRows.Count);
            rows.AddRange(from DataGridViewRow selectedRow in dataGridFunctions.SelectedRows select selectedRow.Index);
            if (rows.Count > 0)
            {
                int lastDeletedRow = rows.Last();
                functionList.removeFunctions(rows);
                invalidateFunctionsData(lastDeletedRow);
            }
        }

        /// <summary>
        /// Creates a new funciton list from the selected functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newListSelectedFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList != null)
            {
                // Get the selected rows
                List<int> rowIndices = new List<int>(dataGridFunctions.SelectedRows.Count);
                for (int i = 0; i < dataGridFunctions.SelectedRows.Count; i++)
                {
                    rowIndices.Add(dataGridFunctions.Rows.IndexOf(dataGridFunctions.SelectedRows[i]));
                }
                if (rowIndices.Count > 0)
                {
                    // Duplicate the current list
                    oFunctionList newList = functionList.Clone();

                    // Clip the functions to the selected functions
                    newList.clipFunctions_selected(rowIndices);

                    // Create the new function list tab
                    tab.Parent.addFunctionListTab("Selected Functions", newList, true, true);
                }
            }
        }


        /// <summary>
        /// Activates this instance.
        /// </summary>
        public void Activate()
        {
            // Refresh the data
            InvalidateData();

            // Update the button enabled status
            updateToolBarEnabled();
        }

        public void deactivate()
        {
            // TODO: Clear the directx drawing stuff to save memory?
        }

        private void contextMenuFunctions_Opening(object sender, CancelEventArgs e)
        {
            // Disable the "Send calls to selection function" option if more than 3 rows are selected.
            sendCallsToSelectedFunctionToolStripMenuItem.Enabled = dataGridFunctions.SelectedRows.Count <= 3;

            //Disable rename if more rows are selected
            renameSelectedFunctionToolStripMenuItem.Enabled = dataGridFunctions.SelectedRows.Count == 1;
            
            //Hide IDA import menu if dumpsig.exe is not found
            importNamesFromIDASignaturesToolStripMenuItem.Visible = File.Exists(Settings.Default.DumpSigPath);
        }

        private void sendCallsToSelectedFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a sendCalls tab for each of the selected functions
            DataGridViewSelectedRowCollection rows = dataGridFunctions.SelectedRows;
            for (int i = 0; i < rows.Count; i++)
            {
                // Create a filtered copy of the data with only the calls of interest
                oFunctionList singleFunctionList = functionList.Clone();

                // Filter the calls
                singleFunctionList.clipCalls_addressDest(
                    new RANGE_PARSE_4BYTE(functionList.functions[rows[i].Index].address));

                // Filter the functions
                List<uint> function = new List<uint>(1) {functionList.functions[rows[i].Index].address};
                singleFunctionList.clipFunctions_address(function);

                tab.Parent.addCallSenderTab(functionList.functions[rows[i].Index].address.ToString("X"), true,
                                            singleFunctionList);
            }
        }

        private void toolStripCallSend_Click(object sender, EventArgs e)
        {
            // Create a sendCalls tab for each of the selected call function destinations
            DataGridViewSelectedRowCollection rows = dataGridCalls.SelectedRows;
            for (int i = 0; i < rows.Count; i++)
            {
                // Create a filtered copy of the data with only the calls of interest
                oFunctionList singleFunctionList = functionList.Clone();

                // Filter the calls
                singleFunctionList.clipCalls_addressDest(
                    new RANGE_PARSE_4BYTE(functionList.getData()[rows[i].Index].destination));

                // Filter the functions
                List<uint> function = new List<uint>(1) {functionList.getData()[rows[i].Index].destination};
                singleFunctionList.clipFunctions_address(function);

                tab.Parent.addCallSenderTab(functionList.getData()[rows[i].Index].destination.ToString("X"), true,
                                            singleFunctionList);
            }
        }

        private void contextMenuStripCalls_Opening(object sender, CancelEventArgs e)
        {
            // Disable the "Send calls to selection function" option if more than 10 rows are selected.
            toolStripCallSend.Enabled = dataGridCalls.SelectedRows.Count <= 10;

            // Only enable the "Goto Function" opetion if a single call is selected
            toolStripCallGoto.Enabled = dataGridCalls.SelectedRows.Count == 1;
        }

        private void toolStripCallGoto_Click(object sender, EventArgs e)
        {
            if (dataGridCalls.SelectedRows.Count == 1)
            {
                // Select the function corresponding to this function call destination
                oFunction function = functionList.getFunctionFromCallIndex(dataGridCalls.SelectedRows[0].Index);

                if (function != null)
                {
                    // Get the function row index
                    int row = functionList.functions.IndexOf(function);

                    if (row >= 0)
                    {
                        // Select the tab
                        tabControlFunctionsCalls.SelectedIndex = 0;

                        // Select the row
                        dataGridFunctions.Select();
                        dataGridFunctions.CurrentCell = dataGridFunctions.Rows[row].Cells[0];
                    }
                }
            }
        }

        private void toolStripCallDisable_Click(object sender, EventArgs e)
        {
            // Disable the functions corresponding to the calls
            SetFunctionState(false);
        }

        private void toolStripCallEnable_Click(object sender, EventArgs e)
        {
            // Enable the functions corresponding to the calls
            SetFunctionState(true);
        }

        /// <summary>
        /// Enable or disable selected functions
        /// </summary>
        /// <param name="state"></param>
        private void SetFunctionState(bool state)
        {
            if (functionList != null && oProcess.processStillRunning())
                foreach (DataGridViewRow selectedRow in dataGridCalls.SelectedRows)
                {
                    int rowIndex = selectedRow.Index;
                    if (rowIndex >= 0)
                    {
                        oSingleData call = functionList.dataVis.getData(rowIndex);
                        if (call != null)
                        {
                            if (oFunctionMaster.destinationToFunction.ContainsKey(call.destination))
                            {
                                oFunction function = (oFunction) oFunctionMaster.destinationToFunction[call.destination];
                                if (state)
                                    function.enable();
                                else
                                {
                                    function.disable();
                                }
                            }
                        }
                    }
                }
            DataGridViewColumn dataGridViewColumn = dataGridCalls.Columns["colDestination"];
            if (dataGridViewColumn != null)
                dataGridCalls.InvalidateColumn(dataGridViewColumn.Index);
        }

        private void renameSelectedFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridFunctions.Focus();
            dataGridFunctions.BeginEdit(true);
        }


        private void dataGridFunctions_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (functionList != null && oProcess.processStillRunning())
            {
                DataGridViewRow row = dataGridFunctions.Rows[e.RowIndex];
                if (row.Index >= 0)
                {
                    oFunction function = functionList.functions[e.RowIndex];
                    if (function != null)
                    {
                        function.name = e.Value.ToString();
                    }
                }
            }
        }

        private void dataGridFunctions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridFunctions.CurrentCell = dataGridFunctions[0, dataGridFunctions.CurrentCell.RowIndex];
        }

        private void renameSelectedFunctionsOrdinalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList != null && oProcess.processStillRunning())
                foreach (DataGridViewRow selectedRow in dataGridFunctions.SelectedRows)
                {
                    int rowIndex = selectedRow.Index;
                    if (rowIndex >= 0)
                    {
                        oFunction function = functionList.functions[rowIndex];
                        if (function != null)
                        {
                            if (function.name == string.Empty)
                                function.name = rowIndex.ToString();
                        }
                    }
                    DataGridViewColumn gridViewColumn = dataGridFunctions.Columns["Description"];
                    if (gridViewColumn != null)
                        dataGridFunctions.InvalidateColumn(gridViewColumn.Index);
                }
        }

        private void LoadNamesFromMapFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (functionList != null && oProcess.processStillRunning())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"MAP Files (*.map)|*.MAP| All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Path.GetDirectoryName(oProcess.activeProcess.MainModule.FileName);
                Dictionary<uint, string> mappedFunctions = new Dictionary<uint, string>();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    uint codeBase = 0x1000 + (uint) oProcess.activeProcess.MainModule.BaseAddress;
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith(@" 0001:") && line.Substring(14, 7) == @"       ")
                            {
                                uint key = codeBase +
                                           uint.Parse(line.Substring(6, 8), NumberStyles.HexNumber);
                                if (!mappedFunctions.ContainsKey(key))
                                    mappedFunctions.Add(key, line.Substring(21));
                            }
                        }
                    }
                    foreach (oFunction function in functionList.functions)
                    {
                        if (mappedFunctions.ContainsKey(function.address))
                            function.name = mappedFunctions[function.address];
                    }
                }
                DataGridViewColumn gridViewColumn = dataGridFunctions.Columns["Description"];
                if (gridViewColumn != null)
                    dataGridFunctions.InvalidateColumn(gridViewColumn.Index);
            }
        }

        private void ToolStripTextBoxFastFilterTextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            string searchFor = toolStripTextBoxFastFilter.Text.ToUpper();
            if (tabControlFunctionsCalls.SelectedTab == tabPageFunctionList)
            {
                dataGridFunctions.Suspended = true;
                foreach (DataGridViewRow selectedRow in dataGridFunctions.Rows)
                {
                    dataGridFunctions.SetRowVisibleStateQuick(selectedRow.Index,
                                                              functionList.functions[selectedRow.Index].name.ToUpper().
                                                                  Contains(searchFor));
                }
                dataGridFunctions.Suspended = false;
            }
            SetCallsCount();
        }

        private void DataGridFunctionsRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SetCallsCount();
        }

        private void DataGridFunctionsRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetCallsCount();
        }

        private void TabControlFunctionsCallsSelectedIndexChanged(object sender, EventArgs e)
        {
            // Initialize the tab as necessary
            bool functionPageVisible = tabControlFunctionsCalls.SelectedTab == tabPageFunctionList;
            toolStripTextBoxFastFilter.Visible = functionPageVisible;
            toolStripLabelFastFilter.Visible = functionPageVisible;
            SetCallsCount();
        }

        private void SetCallsCount()
        {
            if (tabControlFunctionsCalls.SelectedTab == tabPageFunctionList)
                toolStripLabelNumberOfRecords.Text = @" Functions: " + dataGridFunctions.VisibleRowsCount;
            else
                toolStripLabelNumberOfRecords.Text = @" Calls: " + dataGridCalls.Rows.Count;
        }

        /// <summary>
        /// Start / stop recording from outside
        /// </summary>
        public void StartStopRecording()
        {
            toolButtonRecord.PerformClick();
        }

        private void toolStripButtonTakeFastFilter_Click(object sender, EventArgs e)
        {
            oFunctionList functionListOutput = functionList.Clone();
            for (int index = dataGridFunctions.Rows.Count - 1; index >= 0; index--)
            {
                DataGridViewRow row = dataGridFunctions.Rows[index];
                if (!row.Visible)
                {
                    functionListOutput.functions.RemoveAt(row.Index);
                }
            }
            tab.Parent.addFunctionListTab("Filtered: " + toolStripTextBoxFastFilter.Text, functionListOutput, true, true);
        }

        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard(1);
        }

        private void nameToolStripMenu_Click(object sender, EventArgs e)
        {
            CopyToClipboard(2);
        }

        private void addressesNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard(0);
        }

        /// <summary>
        /// Copies address/name to clipboard.
        /// </summary>
        /// <param name="what">What to copy</param>
        private void CopyToClipboard(int what)
        {
            if (functionList != null )
            {
                string text = string.Empty;
                for (int i = dataGridFunctions.SelectedRows.Count - 1; i >= 0; i--)
                {
                    int row = dataGridFunctions.Rows.IndexOf(dataGridFunctions.SelectedRows[i]);
                    switch (what)
                    {
                        case 1:
                            text += functionList.functions[row].address.ToString("X") + Environment.NewLine;
                            break;
                        case 2:
                            text += functionList.functions[row].name + Environment.NewLine;
                            break;
                        default:
                            text += functionList.functions[row].address.ToString("X") +
                                    "\t" + functionList.functions[row].name + Environment.NewLine;
                            break;
                    }
                }
                if (text != string.Empty)
                {
                    Clipboard.SetText(text);
                }
            }
        }

        

        private void buttonBreakdown_Click(object sender, EventArgs e)
        {
            contextMenuPlaybar.Show(buttonBreakdown, new Point(0, buttonBreakdown.Height));
        }

        private void menuBreakdownModule_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Break the function list down by the module of the call destination.

            // First generate a list of unique heaps
            List<HEAP_INFO> heaps = functionList.getModuleList();

            if (heaps.Count > 1)
            {
                // Now create a function list for each heap, filtered to each heap range
                foreach (HEAP_INFO heap in heaps)
                {
                    // Create the filter range object
                    RANGE_PARSE_4BYTE range = new RANGE_PARSE_4BYTE((uint)heap.heapAddress,
                                                      (uint) (heap.heapAddress + heap.heapLength));

                    // Create a cloned and filtered copy of the function list
                    oFunctionList newList = functionList.Clone();
                    newList.clipCalls_addressDest(range);

                    // Add this filtered function list to the breakdown
                    controlVisPlayBar.addTimeSeriesFromFunctionList(newList,
                                                                    (heap.associatedModule != null
                                                                         ? heap.associatedModule.ModuleName
                                                                         : "unknown, " + heap.heapAddress.ToString("X")));
                }

                // Resize the playbar panel
                autosizePlaybar();
            }
        }

        private void menuBreakdownCallType_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Add plots for intermodular and intramodular

            // Add the intermodular plot
            oFunctionList newList = functionList.Clone();
            newList.clipCalls_type(1);
            controlVisPlayBar.addTimeSeriesFromFunctionList(newList, "intermodular calls");

            // Add the intramodular plot
            newList = functionList.Clone();
            newList.clipCalls_type(2);
            controlVisPlayBar.addTimeSeriesFromFunctionList(newList, "intramodular calls");

            // Resize the playbar panel
            autosizePlaybar();
        }

        private void menuRemoveAllPlots_Click(object sender, EventArgs e)
        {
            controlVisPlayBar.removeAllExtraTimeSeries();

            // Resize the playbar panel
            autosizePlaybar();
        }

        private void autosizePlaybar()
        {
            if (functionList == null) return;
            // Resizes the playbar panel horizontal splitter to be able to display the plots
            int numPlots = controlVisPlayBar.getNumPlots();

            // A couple settings that can be adjusted:
            const double maxSize = 0.75; // 75 percent of screen as max size
            int preferredSize = 80; // 80 pixels per plot

            // Calculate the preferred size
            if (numPlots >= 12)
                preferredSize = preferredSize/3;
            else if (numPlots >= 8)
                preferredSize = preferredSize/2;
            else if (numPlots >= 4)
                preferredSize = (int) (preferredSize/1.5);


            // Calculate the room requested
            int requiredSize = preferredSize*numPlots;

            // Compare the requested room versus the maximum available room
            if (Height*maxSize < requiredSize)
            {
                // Adjust the size of the plots to fit in the space we are allowed
                preferredSize = (int) ((Height*maxSize)/numPlots);
            }

            // Set the size of the panel
            splitContainerFunctionsPlaybar.SplitterDistance = Height - (preferredSize*numPlots + 4);
        }

        private void breakdownByNumCalls(int numBins)
        {
            if (functionList == null) return;
            // Get the maximimum number of recordable calls
            int maxCalls = (int)Properties.Settings.Default.MaxRecordedCalls;
            if (numBins > maxCalls)
                numBins = maxCalls;

            // Generate the bins for the breakdown
            double stepCount = ((maxCalls - 1))/((double) (numBins - 1)); // 1 bin is reserved for maxCalls+
            oFunctionList newList;
            for (int i = 0; i < numBins - 1; i++)
            {
                // Add this bin as a plot
                newList = functionList.Clone();
                newList.clipFunctions_CallCount((int) (i*stepCount), (int) ((i + 1)*stepCount) - 1);

                // Only add this bin if it has at least 1 call
                if (newList.getCallCount() > 0)
                    controlVisPlayBar.addTimeSeriesFromFunctionList(newList,
                                                                    "called between " + ((int) (i*stepCount)).ToString() +
                                                                    " and " + ((int) ((i + 1)*stepCount)).ToString() +
                                                                    " times.");
            }

            // Add the last bin
            newList = functionList.Clone();
            newList.clipFunctions_CallCount(maxCalls, maxCalls);
            if (newList.getCallCount() > 0)
                controlVisPlayBar.addTimeSeriesFromFunctionList(newList,
                                                                "called " + maxCalls.ToString() + " or more times.");


            // Resize the playbar panel
            autosizePlaybar();
        }

        private void menuBreakdownThread_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Breakdown the function list based on the thread

            // First generate a list of unique stack heaps
            List<HEAP_INFO> heaps = functionList.getStackHeapList();

            if (heaps.Count > 1)
            {
                // Now create a function list for each heap, filtered to each heap range
                foreach (HEAP_INFO heap in heaps)
                {
                    // Create the filter range object
                    RANGE_PARSE_4BYTE range = new RANGE_PARSE_4BYTE((uint)heap.heapAddress,
                                                      (uint) (heap.heapAddress + heap.heapLength));

                    // Create a cloned and filtered copy of the function list
                    oFunctionList newList = functionList.Clone();
                    newList.clipCalls_addressStack(range);

                    // Add this filtered function list to the breakdown
                    controlVisPlayBar.addTimeSeriesFromFunctionList(newList,
                                                                    "thread stack " + heap.heapAddress.ToString("X"));
                }

                // Resize the playbar panel
                autosizePlaybar();
            }
            else
            {
                MessageBox.Show("Only one thread was found, so this recording cannot be broken down by thread.","Unable to break down by thread",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void binsToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(6);
        }

        private void binsToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(11);
        }

        private void binsToolStripMenuItem21_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(21);
        }

        private void binsToolStripMenuItem31_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(31);
        }

        private void binsToolStripMenuItem41_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(41);
        }

        private void binsToolStripMenuItem51_Click(object sender, EventArgs e)
        {
            breakdownByNumCalls(51);
        }

        private void enableStartRecording_Tick(object sender, EventArgs e)
        {
            // Enable the record button again
            toolButtonRecord.Enabled = true;
            enableStartRecording.Enabled = false;
        }

        private void menuAddPlotIntermodular_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Add a plot for intermodular

            // Add the intermodular plot
            oFunctionList newList = functionList.Clone();
            newList.clipCalls_type(1);
            controlVisPlayBar.addTimeSeriesFromFunctionList(newList, "intermodular calls");

            // Resize the playbar panel
            autosizePlaybar();
        }

        private void menuAddPlotIntramodular_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Add a plot for intramodular

            // Add the intramodular plot
            oFunctionList newList = functionList.Clone();
            newList.clipCalls_type(2);
            controlVisPlayBar.addTimeSeriesFromFunctionList(newList, "intramodular calls");

            // Resize the playbar panel
            autosizePlaybar();
        }

        private void menuAddPlotHasString_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;
            // Add a plot for calls that have a string argument

            // Create the filtered list
            oFunctionList newList = functionList.Clone();
            newList.clipCalls_hasStringArgument();

            // Add the list
            controlVisPlayBar.addTimeSeriesFromFunctionList(newList, "has string argument");

            // Resize the playbar panel
            autosizePlaybar();
        }

        private void menuAddPlotFromFilter_Click(object sender, EventArgs e)
        {
            if (functionList == null) return;

            // Generate the filtered list
            formFilter filterDialog = new formFilter(functionList, tab.Parent, (oTabFunctionList)tab,
                                              FILTER_TYPE.GENERAL,
                                              controlVisPlayBar);
            if( filterDialog.ShowDialog(TopLevelControl) == DialogResult.OK )
            {
                // Add the filtered result as a plot
                controlVisPlayBar.addTimeSeriesFromFunctionList(filterDialog.functionListOutput, "custom filter");

                // Resize the playbar panel
                autosizePlaybar();
            }
        }

        private void moduleToolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
        {
            // The user is opening the module list menu item, we need to fill out the list of modules
            
            // Clear the existing dropdown items
            moduleToolStripMenuItem1.DropDownItems.Clear();

            if (functionList == null)
                return;

            
            
            // First generate a list of unique heaps
            List<HEAP_INFO> heaps = functionList.getModuleList();
            if (heaps.Count > 1)
            {
                for (int i = 0; i < heaps.Count; i++ )
                    if (heaps[i].associatedModule != null)
                        moduleToolStripMenuItem1.DropDownItems.Add(heaps[i].associatedModule.ModuleName, null, menuAddPlotModule_Click);
            }
        }

        private void menuAddPlotModule_Click(object sender, EventArgs e)
        {
            // The user would like to add a plot for a specific module

            // First generate the list of heaps
            List<HEAP_INFO> heaps = functionList.getModuleList();

            // Find the name of the module we are looking for
            int i = 0;
            for(i = 0; i < moduleToolStripMenuItem1.DropDownItems.Count; i++)
                if(moduleToolStripMenuItem1.DropDownItems[i] == sender)
                    break;

            if( moduleToolStripMenuItem1.DropDownItems[i] == sender )
            {
                // We found the module we are looking for

                // Associate the module with a heap
                int j = 0;
                for (j = 0; j < heaps.Count; j++)
                    if (moduleToolStripMenuItem1.DropDownItems[i].Text.CompareTo(heaps[j].associatedModule.ModuleName) == 0)
                        break;

                if (moduleToolStripMenuItem1.DropDownItems[i].Text.CompareTo(heaps[j].associatedModule.ModuleName) == 0)
                {
                    // We found the heap

                    // Create the filter range object
                    RANGE_PARSE_4BYTE range = new RANGE_PARSE_4BYTE((uint)heaps[j].heapAddress,
                                                      (uint)(heaps[j].heapAddress + heaps[j].heapLength));

                    // Create a cloned and filtered copy of the function list
                    oFunctionList newList = functionList.Clone();
                    newList.clipCalls_addressDest(range);

                    // Add this filtered function list to the breakdown
                    controlVisPlayBar.addTimeSeriesFromFunctionList(newList,
                                                                    (heaps[j].associatedModule != null
                                                                         ? heaps[j].associatedModule.ModuleName
                                                                         : "unknown, " + heaps[j].heapAddress.ToString("X")));

                    // Resize the playbar panel
                    autosizePlaybar();
                }
            }
        }


        public readonly string[] blueHighlights = { "eax", "ecx", "edx", "[ebp+8]", "[ebp+C]", "[ebp+10]", "[ebp+14]", "[ebp+18]", "[ebp+1C]", "[ebp+20]", "arg1", "arg2", "arg3", "arg4", "arg5", "arg6", "arg7" };
        public readonly string[] blackBoldHighlights = { "Arguments for Selected Calls:" };

        private void dataGridCalls_SelectionChanged(object sender, EventArgs e)
        {
            // We need to update the argument display
           
            // Get the selected rows
            List<int> rowIndices = new List<int>(dataGridCalls.SelectedRows.Count);
            for (int i = 0; i < dataGridCalls.SelectedRows.Count && i < 50; i++) // Will only process the first 50 selected functions
                rowIndices.Add(dataGridCalls.Rows.IndexOf(dataGridCalls.SelectedRows[i]));

            // View the argument details of only the first selected row
            if( rowIndices.Count > 0 )
            {
                // Link the call display to this row index
                dataGridCallArguments.setData(functionList.getFunctionFromCallIndex(rowIndices[0]), functionList.getFunctionCall(rowIndices[0]));
            }

            /*
            // First get a list of the selected rows
            List<int> rowIndices = new List<int>(dataGridCalls.SelectedRows.Count);
            for (int i = 0; i < dataGridCalls.SelectedRows.Count && i < 50; i++) // Will only process the first 50 selected functions
                rowIndices.Add(dataGridCalls.Rows.IndexOf(dataGridCalls.SelectedRows[i]));


            // Set the argument text box
            if (functionList != null)
                this.richTextCallArgs.Text = "Arguments for Selected Calls:" + Environment.NewLine + functionList.getCallArgumentString(rowIndices);
            else
                this.richTextCallArgs.Text = "Arguments for Selected Calls:";

            // Perform syntax highlighting

            // Highlight all the text green
            richTextCallArgs.Select(0, richTextCallArgs.Text.Length);
            richTextCallArgs.SelectionColor = Color.Green;

            // Blue highlighting
            foreach (string blueHighlight in blueHighlights)
            {
                int index = 0;
                index = richTextCallArgs.Find(blueHighlight, index, RichTextBoxFinds.WholeWord);
                while( index >= 0 )
                {
                    // Highlight this find result
                    richTextCallArgs.Select(index, blueHighlight.Length);
                    richTextCallArgs.SelectionColor = Color.Blue;

                    // Find the next highlight
                    index = richTextCallArgs.Find(blueHighlight, index+1, RichTextBoxFinds.WholeWord);
                }
            }

            // blackBoldHighlights highlighting
            foreach (string blackBoldHighlight in blackBoldHighlights)
            {
                int index = 0;
                index = richTextCallArgs.Find(blackBoldHighlight, index, RichTextBoxFinds.WholeWord);
                while (index >= 0)
                {
                    // Highlight this find result
                    richTextCallArgs.Select(index, blackBoldHighlight.Length);
                    richTextCallArgs.SelectionColor = Color.Black;

                    // Find the next highlight
                    index = richTextCallArgs.Find(blackBoldHighlight, index + 1, RichTextBoxFinds.WholeWord);
                }
            }
             */
        }

        private void toolStripExportFunction_Click(object sender, EventArgs e)
        {
            if (functionList != null && dataGridFunctions.SelectedRows.Count <= 5 )
            {
                // Open the export function menu for each function
                foreach (DataGridViewRow selectedRow in dataGridFunctions.SelectedRows)
                {
                    oFunction function = functionList.getFunction(dataGridFunctions.Rows.IndexOf(selectedRow));
                    tab.Parent.addExportFunctionTab("Export " + function.address.ToString("X"), true, function);
                }
            }
                
        }

        private void dataGridCalls_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void importNamesFromIDASignaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (functionList != null)
            {
                using (formSignatures signatureForm = new formSignatures())
                {
                    signatureForm.TryToApplySignature(dataGridFunctions.SelectedRows, functionList);
                }
                DataGridViewColumn gridViewColumn = dataGridFunctions.Columns["Description"];
                if (gridViewColumn != null)
                    dataGridFunctions.InvalidateColumn(gridViewColumn.Index);
            }
        }

    }
}