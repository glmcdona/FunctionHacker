using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionHacker.Classes;

namespace FunctionHacker.Controls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            linkLabel1.Links.Add(0, linkLabel1.Text.Length,"http://www.split-code.com");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelIntro_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Links[0].LinkData.ToString());
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

    }
}
