using System.Reflection;
using System.Windows.Forms;

namespace FunctionHacker.Controls
{
    /// <summary>
    /// Speeded up data grid for fast filtering
    /// </summary>
    public class DataGridViewEx : DataGridView
    {
        readonly FieldInfo fieldInfo = typeof(DataGridViewElement).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
        private bool m_Suspended;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataGridViewEx"/> is suspended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if suspended; otherwise, <c>false</c>.
        /// </value>
        public bool Suspended
        {
            get { return m_Suspended; }
            set
            {
                if (m_Suspended != value)
                {
                    m_Suspended = value;
                    if (!m_Suspended && Rows.Count > 0)
                    {
                        bool visible = Rows[0].Visible;
                        Rows[0].Visible = !visible;
                        Rows[0].Visible = visible;
                    }
                }
            }
        }

        /// <summary>
        /// Find number of visible records;
        /// </summary>
        public int VisibleRowsCount
        {
            get { return GetVisibleRowsCount(); }
        }

        private int GetVisibleRowsCount()
        {
            int count = 0;
            foreach (DataGridViewRow dataGridViewRow in Rows)
            {
                if (dataGridViewRow.Visible)
                    count++;
            }
            return count;
        }

        protected override void OnRowStateChanged(int rowIndex, DataGridViewRowStateChangedEventArgs e)
        {
            if (!m_Suspended) 
                base.OnRowStateChanged(rowIndex, e);
        }

        protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
        {
            if (!m_Suspended) 
                base.OnCellValueNeeded(e);
        }
        
        /// <summary>
        /// In combination with suspended mode - quick setting of internal record state
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="visible"></param>
        public void SetRowVisibleStateQuick(int rowIndex, bool visible)
        {
            DataGridViewRow dataGridViewRow = Rows[rowIndex];
            //zb: set row state over reflection to avoid to fire whole event chain (a little bit dirty, but work)
            DataGridViewElementStates state = dataGridViewRow.State;
            if (visible)
                state |= DataGridViewElementStates.Visible;
            else
                state &= ~DataGridViewElementStates.Visible;
            fieldInfo.SetValue(dataGridViewRow, state);
        }
    }
}