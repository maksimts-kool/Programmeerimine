namespace Tund8
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ToodeBox = new System.Windows.Forms.TextBox();
            this.KogusBox = new System.Windows.Forms.TextBox();
            this.HindBox = new System.Windows.Forms.TextBox();
            this.KategooriadBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LisaKat = new System.Windows.Forms.Button();
            this.KustutaKat = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.Lisa = new System.Windows.Forms.Button();
            this.Uuenda = new System.Windows.Forms.Button();
            this.Kustuta = new System.Windows.Forms.Button();
            this.Puhasta = new System.Windows.Forms.Button();
            this.Otsi = new System.Windows.Forms.Button();
            this.Label35 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.SuspendLayout();
            //
            // ToodeBox
            //
            this.ToodeBox.Text =  "ToodeBox";
            this.ToodeBox.Modified =  true;
            this.ToodeBox.SelectedText =  "ToodeBox";
            this.ToodeBox.SelectionLength = 8;
            this.ToodeBox.Location = new System.Drawing.Point(95,22);
            this.ToodeBox.Size = new System.Drawing.Size(130,23);
            //
            // KogusBox
            //
            this.KogusBox.Text =  "KogusBox";
            this.KogusBox.Modified =  true;
            this.KogusBox.Location = new System.Drawing.Point(95,57);
            this.KogusBox.Size = new System.Drawing.Size(130,23);
            this.KogusBox.TabIndex = 1;
            //
            // HindBox
            //
            this.HindBox.Text =  "HindBox";
            this.HindBox.Modified =  true;
            this.HindBox.Location = new System.Drawing.Point(95,92);
            this.HindBox.Size = new System.Drawing.Size(130,23);
            this.HindBox.TabIndex = 2;
            //
            // KategooriadBox
            //
            this.KategooriadBox.DropDownWidth = 130;
            this.KategooriadBox.ItemHeight = 15;
            this.KategooriadBox.Text =  "KategooriadBox";
            this.KategooriadBox.Location = new System.Drawing.Point(95,127);
            this.KategooriadBox.Size = new System.Drawing.Size(130,23);
            this.KategooriadBox.TabIndex = 3;
            //
            // label1
            //
            this.label1.AutoSize =  true;
            this.label1.Text =  "Toode:";
            this.label1.Location = new System.Drawing.Point(20,25);
            this.label1.Size = new System.Drawing.Size(43,15);
            this.label1.TabIndex = 4;
            //
            // label2
            //
            this.label2.AutoSize =  true;
            this.label2.Text =  "Kogus:";
            this.label2.Location = new System.Drawing.Point(20,60);
            this.label2.Size = new System.Drawing.Size(43,15);
            this.label2.TabIndex = 5;
            //
            // label3
            //
            this.label3.AutoSize =  true;
            this.label3.Text =  "Hind:";
            this.label3.Location = new System.Drawing.Point(20,95);
            this.label3.Size = new System.Drawing.Size(36,15);
            this.label3.TabIndex = 6;
            //
            // LisaKat
            //
            this.LisaKat.Text =  "Lisa kategooriat";
            this.LisaKat.Location = new System.Drawing.Point(20,174);
            this.LisaKat.Size = new System.Drawing.Size(110,34);
            this.LisaKat.TabIndex = 8;
            this.LisaKat.Click += new System.EventHandler(LisaKat_Click);
            //
            // KustutaKat
            //
            this.KustutaKat.Text =  "Kustuta kategooriat";
            this.KustutaKat.Location = new System.Drawing.Point(136,176);
            this.KustutaKat.Size = new System.Drawing.Size(120,32);
            this.KustutaKat.TabIndex = 9;
            this.KustutaKat.Click += new System.EventHandler(KustutaKat_Click);
            //
            // PictureBox
            //
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 21;
            this.PictureBox.Text =  "PictureBox25";
            this.PictureBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PictureBox.Location = new System.Drawing.Point(340,20);
            this.PictureBox.Size = new System.Drawing.Size(328,216);
            //
            // DataGridView
            //
            this.DataGridView.AllowUserToAddRows =  false;
            this.DataGridView.AllowUserToDeleteRows =  false;
            this.DataGridView.AllowUserToResizeColumns =  false;
            this.DataGridView.AllowUserToResizeRows =  false;
            this.DataGridView.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.DataGridView.ReadOnly =  true;
            this.DataGridView.Text =  "DataGridView27";
            this.DataGridView.Location = new System.Drawing.Point(20,260);
            this.DataGridView.Size = new System.Drawing.Size(650,150);
            this.DataGridView.TabIndex = 22;
            this.DataGridView.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView_CellMouseLeave);
            this.DataGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridView_CellMouseEnter);
            //
            // Lisa
            //
            this.Lisa.Text =  "Lisa";
            this.Lisa.Location = new System.Drawing.Point(20,212);
            this.Lisa.TabIndex = 23;
            this.Lisa.Click += new System.EventHandler(Lisa_Click);
            //
            // Uuenda
            //
            this.Uuenda.Text =  "Uuenda";
            this.Uuenda.Location = new System.Drawing.Point(100,212);
            this.Uuenda.TabIndex = 24;
            this.Uuenda.Click += new System.EventHandler(Uuenda_Click);
            //
            // Kustuta
            //
            this.Kustuta.Text =  "Kustuta";
            this.Kustuta.Location = new System.Drawing.Point(180,212);
            this.Kustuta.TabIndex = 25;
            this.Kustuta.Click += new System.EventHandler(Kustuta_Click);
            //
            // Puhasta
            //
            this.Puhasta.Text =  "Puhasta";
            this.Puhasta.Location = new System.Drawing.Point(276,152);
            this.Puhasta.Size = new System.Drawing.Size(40,84);
            this.Puhasta.TabIndex = 26;
            this.Puhasta.Click += new System.EventHandler(Puhasta_Click);
            //
            // Otsi
            //
            this.Otsi.Text =  "Otsi fail";
            this.Otsi.Location = new System.Drawing.Point(276,56);
            this.Otsi.Size = new System.Drawing.Size(40,88);
            this.Otsi.TabIndex = 32;
            this.Otsi.Click += new System.EventHandler(Otsi_Click);
            //
            // Label35
            //
            this.Label35.AutoSize =  true;
            this.Label35.Text =  "Kategooria:";
            this.Label35.Location = new System.Drawing.Point(20,130);
            this.Label35.Size = new System.Drawing.Size(67,15);
            this.Label35.TabIndex = 35;
         //
         // form
         //
            this.Size = new System.Drawing.Size(712,472);
            this.Text =  "Pood";
            this.Controls.Add(this.ToodeBox);
            this.Controls.Add(this.KogusBox);
            this.Controls.Add(this.HindBox);
            this.Controls.Add(this.KategooriadBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LisaKat);
            this.Controls.Add(this.KustutaKat);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.DataGridView);
            this.Controls.Add(this.Lisa);
            this.Controls.Add(this.Uuenda);
            this.Controls.Add(this.Kustuta);
            this.Controls.Add(this.Puhasta);
            this.Controls.Add(this.Otsi);
            this.Controls.Add(this.Label35);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.ResumeLayout(false);
        } 

        #endregion 

        private System.Windows.Forms.TextBox ToodeBox;
        private System.Windows.Forms.TextBox KogusBox;
        private System.Windows.Forms.TextBox HindBox;
        private System.Windows.Forms.ComboBox KategooriadBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button LisaKat;
        private System.Windows.Forms.Button KustutaKat;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.Button Lisa;
        private System.Windows.Forms.Button Uuenda;
        private System.Windows.Forms.Button Kustuta;
        private System.Windows.Forms.Button Puhasta;
        private System.Windows.Forms.Button Otsi;
        private System.Windows.Forms.Label Label35;
    }
}

// private void LisaKat_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void KustutaKat_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void DataGridView_CellMouseLeave(System.Object? sender, System.Windows.Forms.DataGridViewCellEventArgs e)
// {
// 
// }

// private void DataGridView_CellMouseEnter(System.Object? sender, System.Windows.Forms.DataGridViewCellEventArgs e)
// {
// 
// }

// private void Lisa_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Uuenda_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Kustuta_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Puhasta_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Otsi_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

