using System;
using System.Windows.Forms;

namespace FunctionHacker.Controls
{
    /// <summary>
    /// Persists user settings
    /// </summary>
    public partial class userSettings : UserControl
    {
        public userSettings()
        {
            InitializeComponent();
        }

        private void save()
        {
            //use of .net application setting property data binding mechanism for save and load
            Properties.Settings.Default.Save();
        }

        private void queueSave()
        {
            timerSave.Enabled = false;
            timerSave.Enabled = true;
        }

        private void customChangedEvent(object sender, EventArgs e)
        {
            queueSave();
        }

        ~userSettings()
        {
            // Commit changes and save
            numericUpDownRefreshRate.Focus();
            numericUpDownMaxCalls.Focus();
            circularBufferSize.Focus();
            numBytesDereference.Focus();
            save();
        }

        private void timerSave_Tick(object sender, EventArgs e)
        {
            save();
            timerSave.Enabled = false;
        }

        private void buttonSignaturePath_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxSignaturePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSigDumpPath_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxSigDumpPath.Text = openFileDialog1.FileName;
            }

        }



    }
}
