using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FunctionHacker.Forms
{
    public partial class formProgress : Form
    {
        public formProgress()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            Application.DoEvents();
        }

        public formProgress(Form parent)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(parent.Location.X + parent.Size.Width / 2 - 200, parent.Location.Y + parent.Size.Height / 2 - 75);
            Application.DoEvents();
        }

        public formProgress(Panel parent)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(parent.Location.X + parent.Size.Width / 2 - 200, parent.Location.Y + parent.Size.Height / 2 - 75);
            Application.DoEvents();
        }

        private void formProgress_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
        }

        public void setMax(int max)
        {
            this.progressBar.Maximum = max;
            this.progressBar.Value = this.progressBar.Minimum;
            this.Invalidate();
            Application.DoEvents();
            //progressBar.Update();
            //progressBar.Refresh();
        }

        public void setMin(int min)
        {
            this.progressBar.Minimum = min;
            this.progressBar.Value = this.progressBar.Minimum;
            this.Invalidate();
            Application.DoEvents();
            //progressBar.Update();
            //progressBar.Refresh();
        }

        public void setValue(int value)
        {
            if (value < this.progressBar.Minimum) { setMin(value); }
            if (value > this.progressBar.Maximum) { setMax(value); }
            this.progressBar.Value = value;
            this.Invalidate();
            Application.DoEvents();
            //progressBar.Update();
            //progressBar.Refresh();
        }

        public void increment()
        {
            setValue(this.progressBar.Value + 1);
        }

        public void increment(int change)
        {
            setValue(this.progressBar.Value + change);
        }

        public void setLabel1(String text)
        {
            this.label1.Text = text;
            this.Invalidate();
            Application.DoEvents();
            //label1.Update();
            //label1.Refresh();
        }

        public void setLabel2(String text)
        {
            this.label2.Text = text;
            this.Invalidate();
            Application.DoEvents();
            //label2.Update();
            //label2.Refresh();
        }

        public void setTitle(String text)
        {
            this.Text = text;
            this.Invalidate();
            Application.DoEvents();
            //this.Update();
            //this.Refresh();
        }
    }
}

