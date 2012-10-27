namespace KarateGeek.reports
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.athinaViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // athinaViewer
            // 
            this.athinaViewer.ActiveViewIndex = -1;
            this.athinaViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.athinaViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.athinaViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.athinaViewer.Location = new System.Drawing.Point(0, 0);
            this.athinaViewer.Name = "athinaViewer";
            this.athinaViewer.Size = new System.Drawing.Size(788, 472);
            this.athinaViewer.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(270, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 472);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.athinaViewer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer athinaViewer;
        private System.Windows.Forms.Button button1;

    }
}