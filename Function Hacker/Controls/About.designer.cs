namespace FunctionHacker.Controls
{
    partial class About
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
          this.label1 = new System.Windows.Forms.Label();
          this.label3 = new System.Windows.Forms.Label();
          this.label4 = new System.Windows.Forms.Label();
          this.label5 = new System.Windows.Forms.Label();
          this.linkLabel1 = new System.Windows.Forms.LinkLabel();
          this.label2 = new System.Windows.Forms.Label();
          this.label6 = new System.Windows.Forms.Label();
          this.SuspendLayout();
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Underline);
          this.label1.Location = new System.Drawing.Point(3, 0);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(172, 20);
          this.label1.TabIndex = 68;
          this.label1.Text = "Function Hacker v3.0:";
          // 
          // label3
          // 
          this.label3.AutoSize = true;
          this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
          this.label3.Location = new System.Drawing.Point(24, 45);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(0, 13);
          this.label3.TabIndex = 70;
          // 
          // label4
          // 
          this.label4.AutoSize = true;
          this.label4.Location = new System.Drawing.Point(3, 78);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(205, 117);
          this.label4.TabIndex = 71;
          this.label4.Text = resources.GetString("label4.Text");
          // 
          // label5
          // 
          this.label5.Location = new System.Drawing.Point(3, 216);
          this.label5.Name = "label5";
          this.label5.Size = new System.Drawing.Size(812, 125);
          this.label5.TabIndex = 72;
          this.label5.Text = resources.GetString("label5.Text");
          // 
          // linkLabel1
          // 
          this.linkLabel1.AutoSize = true;
          this.linkLabel1.Location = new System.Drawing.Point(134, 36);
          this.linkLabel1.Name = "linkLabel1";
          this.linkLabel1.Size = new System.Drawing.Size(138, 13);
          this.linkLabel1.TabIndex = 73;
          this.linkLabel1.TabStop = true;
          this.linkLabel1.Text = "http://www.split-code.com/";
          this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(24, 36);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(113, 13);
          this.label2.TabIndex = 75;
          this.label2.Text = "by Split-Code Analysis,";
          // 
          // label6
          // 
          this.label6.AutoSize = true;
          this.label6.Location = new System.Drawing.Point(14, 20);
          this.label6.Name = "label6";
          this.label6.Size = new System.Drawing.Size(202, 13);
          this.label6.TabIndex = 76;
          this.label6.Text = "Game hacking and malware analysis tool.\r\n";
          // 
          // About
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.Controls.Add(this.label6);
          this.Controls.Add(this.linkLabel1);
          this.Controls.Add(this.label5);
          this.Controls.Add(this.label4);
          this.Controls.Add(this.label3);
          this.Controls.Add(this.label1);
          this.Controls.Add(this.label2);
          this.Name = "About";
          this.Size = new System.Drawing.Size(908, 413);
          this.Load += new System.EventHandler(this.About_Load);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
    }
}
