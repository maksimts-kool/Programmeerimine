namespace Tund7
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnOpenForm1;
        private Button btnOpenForm2;
        private Button btnOpenForm3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnOpenForm1 = new System.Windows.Forms.Button();
            this.btnOpenForm2 = new System.Windows.Forms.Button();
            this.btnOpenForm3 = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // btnOpenForm1
            this.btnOpenForm1.Location = new System.Drawing.Point(30, 30);
            this.btnOpenForm1.Name = "btnOpenForm1";
            this.btnOpenForm1.Size = new System.Drawing.Size(120, 40);
            this.btnOpenForm1.Text = "Ava Form 1";
            this.btnOpenForm1.Click += new System.EventHandler(this.btnOpenForm1_Click);

            // btnOpenForm2
            this.btnOpenForm2.Location = new System.Drawing.Point(30, 80);
            this.btnOpenForm2.Name = "btnOpenForm2";
            this.btnOpenForm2.Size = new System.Drawing.Size(120, 40);
            this.btnOpenForm2.Text = "Ava Form 2";
            this.btnOpenForm2.Click += new System.EventHandler(this.btnOpenForm2_Click);

            // btnOpenForm3
            this.btnOpenForm3.Location = new System.Drawing.Point(30, 130);
            this.btnOpenForm3.Name = "btnOpenForm3";
            this.btnOpenForm3.Size = new System.Drawing.Size(120, 40);
            this.btnOpenForm3.Text = "Ava Form 3";
            this.btnOpenForm3.Click += new System.EventHandler(this.btnOpenForm3_Click);

            // FormMain
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Controls.Add(this.btnOpenForm1);
            this.Controls.Add(this.btnOpenForm2);
            this.Controls.Add(this.btnOpenForm3);
            this.Name = "FormMain";
            this.Text = "Select a Form";
            this.ResumeLayout(false);
        }
    }
}