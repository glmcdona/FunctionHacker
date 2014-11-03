using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using FunctionHacker.Classes;

namespace FunctionHacker.Controls
{
    /// <summary>
    /// Speeded up data grid for fast filtering
    /// </summary>
    public class DataGridViewCall : DataGridView
    {
        private oFunction functionBase;
        private oSingleData measuredData;
        private List<oArgument> argumentClasses;
        private DataGridView mainGrid = null;

        public DataGridViewCall()
        {
            this.EditMode = DataGridViewEditMode.EditOnEnter;
            this.AllowUserToAddRows = false;

            // Create the event callbacks
            this.CellValueChanged += new DataGridViewCellEventHandler(DataGridViewCall_CellValueChanged);
            this.CellContentClick += new DataGridViewCellEventHandler(DataGridViewCall_CellClick);
            this.CurrentCellDirtyStateChanged += new EventHandler(DataGridViewCall_CurrentCellDirtyStateChanged);
            this.CellClick += new DataGridViewCellEventHandler(DataGridViewCall_CellClick);
            this.CellBeginEdit += new DataGridViewCellCancelEventHandler(DataGridViewCall_CellBeginEdit);
        }


        void DataGridViewCall_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
                return;

            // If this is column 0, we need to change the value to only the strong name
            if( e.ColumnIndex == 0 )
            {
                // Set this cell text to the strong name
                Rows[e.RowIndex].Cells[e.ColumnIndex].Value = argumentClasses[e.RowIndex].getStrongName();
            }
        }

        void DataGridViewCall_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (IsCurrentCellDirty && CurrentCell.ColumnIndex == 1)
            {
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        

        void DataGridViewCall_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
            {
                return;
            }

            // Get the row, and then the cell, of the clicked cell
            var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];

            // If the cell is a combobox, make it drop down
            if (cell is DataGridViewComboBoxCell)
            {
                DataGridViewComboBoxEditingControl comboboxEdit = (DataGridViewComboBoxEditingControl)this.EditingControl;
                if (comboboxEdit != null)
                {
                    comboboxEdit.DroppedDown = true;
                }
                DataGridViewCall_CellValueChanged(sender, e);
            }
        }

        void DataGridViewCall_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // A value has changed
            if (this.Rows.Count > 0)
            {

                if (e.ColumnIndex == 0)
                {
                    // Changed the name of the argument
                    argumentClasses[e.RowIndex].setStrongName((string)this.Rows[e.RowIndex].Cells[0].Value);
                    this.Rows[e.RowIndex].Cells[0].Value = argumentClasses[e.RowIndex].getName();

                    // Tell the main grid to redraw the argument column
                    if (mainGrid != null)
                    {
                        mainGrid.InvalidateColumn(3);
                    }

                }
                else if (e.ColumnIndex == 1)
                {
                    // Changed the type of the argument

                    // Update the type
                    argumentClasses[e.RowIndex].setType(
                        (DISPLAY_TYPE)Enum.Parse(typeof(DISPLAY_TYPE), (string)this.Rows[e.RowIndex].Cells[1].EditedFormattedValue));

                    // Update the cell data interpretation
                    ARGUMENT_STRING_COLLECTION stringData = functionBase.getArgumentString(measuredData);
                    this.Rows[e.RowIndex].Cells[2].Value = stringData.values[e.RowIndex];

                    // Tell the main grid to redraw the argument column
                    if (mainGrid != null)
                    {
                        mainGrid.InvalidateColumn(3);
                    }
                }
            }
        }

        public void setData(oFunction functionBase, oSingleData measuredData)
        {

            this.functionBase = functionBase;
            this.measuredData = measuredData;
            if (functionBase == null)
            {
                this.Rows.Clear();
                return;
            }
            this.argumentClasses = functionBase.getArgumentList();
            
             

            // Cleanup this datagridview)
            this.Rows.Clear();

            // Build the combobox selection options
            string[] options = Enum.GetNames(typeof(DISPLAY_TYPE));

            // Fill out this datagridview based on the supplied data
            ARGUMENT_STRING_COLLECTION stringData = functionBase.getArgumentString(measuredData);
            DataGridViewRow[] newRows = new DataGridViewRow[argumentClasses.Count];
            for( int i = 0; i < argumentClasses.Count; i++ )
            {
                // Add this row
                newRows[i] = new DataGridViewRow();

                // Add the name cell
                newRows[i].Cells[newRows[i].Cells.Add(new DataGridViewTextBoxCell())].Value = stringData.names[i];

                // Add the type cell combobox
                DataGridViewComboBoxCell comboBox = new DataGridViewComboBoxCell();
                comboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                comboBox.Value = Enum.GetName(typeof(DISPLAY_TYPE), argumentClasses[i].displayMethod);
                comboBox.Items.AddRange(options);
                newRows[i].Cells.Add(comboBox);

                // Add the value cell
                newRows[i].Cells[newRows[i].Cells.Add(new DataGridViewTextBoxCell())].Value = stringData.values[i];
            }

            // Add the new rows
            this.Rows.AddRange(newRows);

        }

        public void setParent(DataGridView dataGridCalls)
        {
            this.mainGrid = dataGridCalls;
        }
    }
}