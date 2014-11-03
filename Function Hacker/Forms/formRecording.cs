using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Function_Debugger.Classes;

namespace Function_Debugger.Forms
{
    public partial class formRecording : Form
    {
        private oFunctionList list;
        public formRecording(oFunctionList referenceList)
        {
            this.list = referenceList;

            // Begin recording data
            list.startRecording();

            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void formRecording_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Stop recording data
            list.stopRecording();
        }
    }
}
