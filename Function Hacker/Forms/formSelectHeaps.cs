using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionHacker.Classes;

namespace FunctionHacker.Forms
{
    public struct DISASSEMBLY_MODE
    {
        public bool recordCallbacks;
        public bool recordIntramodular;
        public bool forceRecordingAllIntermodular;
        public bool recordNewLibraries;
        public bool aggressiveDereferencing;

        public DISASSEMBLY_MODE(bool recordCallbacks, bool recordIntramodular, bool forceRecordingAllIntermodular, bool recordNewLibraries, bool aggressiveDereferencing)
        {
            this.recordCallbacks = recordCallbacks;
            this.recordIntramodular = recordIntramodular;
            this.forceRecordingAllIntermodular = forceRecordingAllIntermodular;
            this.recordNewLibraries = recordNewLibraries;
            this.aggressiveDereferencing = aggressiveDereferencing;
        }

    }

    public partial class formSelectHeaps : Form
    {
        private List<HEAP_INFO> map;
        private List<HEAP_INFO> selectedHeaps;
        private List<HEAP_INFO> invalidSourceHeaps;
        private List<ProcessModule> selectedModules;
        private Hashtable listItemLookup;
        private DISASSEMBLY_MODE disassemblyMode;

        public formSelectHeaps(List<HEAP_INFO> map, List<ProcessModule> selectedModules, DISASSEMBLY_MODE disassemblyMode, Form parent)
        {
            this.map = map;
            this.selectedModules = selectedModules;
            this.disassemblyMode = disassemblyMode;
            selectedHeaps = new List<HEAP_INFO>(0);
            invalidSourceHeaps = new List<HEAP_INFO>(0);
            listItemLookup = new Hashtable();

            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(parent.Location.X + parent.Size.Width / 2 - 200, parent.Location.Y + parent.Size.Height / 2 - 275);
        }

        private void formSelectHeaps_Load(object sender, EventArgs e)
        {
            // Generate the list of selected heaps
            List<HEAP_INFO> selectedHeaps = determineDefaultSelectedHeaps();

            // Creat the list of items
            ListViewItem[] newItems = new ListViewItem[map.Count];

            // Generate the list of heaps
            for (int i = 0; i < map.Count; i++ )
            {
                newItems[i] =
                    new ListViewItem(new[]
                                     {
                                         oMemoryFunctions.MakeFixedLengthHexString(map[i].heapAddress.ToString("X"), 8),
                                         map[i].heapProtection,
                                         (map[i].associatedModule != null ? map[i].associatedModule.ModuleName : "none"),
                                         map[i].extra
                                     });

                // Highlight this row if it is executable
                if (map[i].heapProtection.Contains("EXECUTE"))
                    newItems[i].BackColor = Color.LightGreen;

                // Check this row if it is selected
                if (selectedHeaps.Contains(map[i]))
                    newItems[i].Checked = true;

                // Add the new item to the hash table lookup, so we can figure out which heap is associated with a list item.
                listItemLookup.Add(newItems[i], map[i]);
            }

            // Add the new item to the list
            listHeaps.Items.AddRange(newItems);

        }

        private List<HEAP_INFO> determineDefaultSelectedHeaps()
        {
            // This function returns a list of heaps that will be selected by default according to the disassembly method;
            List<HEAP_INFO> result = new List<HEAP_INFO>();

            for (int i = 0; i < map.Count; i++ )
            {
                // Check if this heap is included
                bool included = false;
                if (map[i].extra.Contains("PE") ||
                    (map[i].extra.Contains("PE") && map[i].associatedModule != null && selectedModules.Contains(map[i].associatedModule)) ||
                    (map[i].associatedModule != null && map[i].heapAddress <= (ulong)map[i].associatedModule.EntryPointAddress && map[i].heapAddress + map[i].heapLength >= (ulong)map[i].associatedModule.EntryPointAddress && selectedModules.Contains(map[i].associatedModule)))

                {
                    result.Add(map[i]);
                }
            }

            return result;
        }

        private List<HEAP_INFO> determineInvalidCallSourceHeaps(List<HEAP_INFO> selectedHeaps)
        {
            // This function generates the list of heaps that will be used to exclude inter-modular calls from not-selected heaps.
            List<HEAP_INFO> result = new List<HEAP_INFO>(10);

            if (!disassemblyMode.forceRecordingAllIntermodular)
            {
                // Build a list of heap .code sections that are not selected, these will be excluded.
                for (int i = 0; i < map.Count; i++)
                {
                    // Check if this heap is an invalid call source
                    if (map[i].extra.Contains("entry_point") && !selectedHeaps.Contains(map[i]))
                    {
                        // This is a code region that was not selected, mark it as invalid for inter-modular call recording.
                        result.Add(map[i]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the list of selected heaps to reverse-engineer
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getSelectedHeaps()
        {
            return selectedHeaps;
        }

        /// <summary>
        /// Get the list of selected invalid heaps for inter-modular call recording.
        /// </summary>
        /// <returns></returns>
        public List<HEAP_INFO> getInvalidSourceHeaps()
        {
            return invalidSourceHeaps;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            // Generate the list of selected heaps
            foreach(ListViewItem item in listHeaps.Items)
            {
                if( item.Checked )
                {
                    // This item is included
                    selectedHeaps.Add( (HEAP_INFO) listItemLookup[item] );
                }
            }

            // Update the invalid heaps
            invalidSourceHeaps = determineInvalidCallSourceHeaps(selectedHeaps);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHeaps.Items)
                item.Checked = false;
        }
    }
}
