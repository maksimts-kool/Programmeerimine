namespace Tund9;

partial class Pood
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Label0 = new System.Windows.Forms.Label();
        this.Label1 = new System.Windows.Forms.Label();
        this.Label2 = new System.Windows.Forms.Label();
        this.Label3 = new System.Windows.Forms.Label();
        this.TextBox4 = new System.Windows.Forms.TextBox();
        this.TextBox5 = new System.Windows.Forms.TextBox();
        this.TextBox6 = new System.Windows.Forms.TextBox();
        this.ComboBox7 = new System.Windows.Forms.ComboBox();
        this.Button8 = new System.Windows.Forms.Button();
        this.Button9 = new System.Windows.Forms.Button();
        this.Button10 = new System.Windows.Forms.Button();
        this.DataGridView11 = new System.Windows.Forms.DataGridView();
        this.PictureBox13 = new System.Windows.Forms.PictureBox();
        this.Button13 = new System.Windows.Forms.Button();
        this.ComboLang = new System.Windows.Forms.ComboBox();
        ((System.ComponentModel.ISupportInitialize)(this.DataGridView11)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox13)).BeginInit();
        this.SuspendLayout();
        //
        // Label0
        //
        this.Label0.AutoSize = true;
        this.Label0.TextAlign = System.Drawing.ContentAlignment.TopRight;
        this.Label0.Text = "Toode nimetus";
        this.Label0.Location = new System.Drawing.Point(20, 36);
        this.Label0.Size = new System.Drawing.Size(86, 15);
        //
        // Label1
        //
        this.Label1.AutoSize = true;
        this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
        this.Label1.Text = "Toode kogus";
        this.Label1.Location = new System.Drawing.Point(32, 80);
        this.Label1.Size = new System.Drawing.Size(75, 15);
        this.Label1.TabIndex = 1;
        //
        // Label2
        //
        this.Label2.AutoSize = true;
        this.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
        this.Label2.Text = "Toode hind";
        this.Label2.Location = new System.Drawing.Point(40, 124);
        this.Label2.Size = new System.Drawing.Size(67, 15);
        this.Label2.TabIndex = 2;
        //
        // Label3
        //
        this.Label3.AutoSize = true;
        this.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
        this.Label3.Text = "Kategooria";
        this.Label3.Location = new System.Drawing.Point(44, 168);
        this.Label3.Size = new System.Drawing.Size(64, 15);
        this.Label3.TabIndex = 3;
        //
        // TextBox4
        //
        this.TextBox4.Location = new System.Drawing.Point(128, 32);
        this.TextBox4.Size = new System.Drawing.Size(120, 23);
        this.TextBox4.TabIndex = 4;
        //
        // TextBox5
        //
        this.TextBox5.Location = new System.Drawing.Point(128, 76);
        this.TextBox5.Size = new System.Drawing.Size(120, 23);
        this.TextBox5.TabIndex = 5;
        //
        // TextBox6
        //
        this.TextBox6.Location = new System.Drawing.Point(128, 120);
        this.TextBox6.Size = new System.Drawing.Size(120, 23);
        this.TextBox6.TabIndex = 6;
        //
        // ComboBox7
        //
        this.ComboBox7.ItemHeight = 15;
        this.ComboBox7.Text = "Vali";
        this.ComboBox7.Location = new System.Drawing.Point(128, 164);
        this.ComboBox7.TabIndex = 7;
        //
        // Button8
        //
        this.Button8.BackColor = System.Drawing.Color.MediumSeaGreen;
        this.Button8.Text = "Lisa";
        this.Button8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.Button8.ForeColor = System.Drawing.Color.White;
        this.Button8.Location = new System.Drawing.Point(16, 208);
        this.Button8.Size = new System.Drawing.Size(112, 30);
        this.Button8.TabIndex = 8;
        this.Button8.Click += new System.EventHandler(Button8_Click);
        //
        // Button9
        //
        this.Button9.BackColor = System.Drawing.Color.RoyalBlue;
        this.Button9.Text = "Uuenda";
        this.Button9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.Button9.ForeColor = System.Drawing.Color.White;
        this.Button9.Location = new System.Drawing.Point(136, 208);
        this.Button9.Size = new System.Drawing.Size(112, 30);
        this.Button9.TabIndex = 9;
        this.Button9.Click += new System.EventHandler(Button9_Click);
        //
        // Button10
        //
        this.Button10.BackColor = System.Drawing.Color.Crimson;
        this.Button10.Text = "Kustuta";
        this.Button10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.Button10.ForeColor = System.Drawing.Color.White;
        this.Button10.Location = new System.Drawing.Point(256, 208);
        this.Button10.Size = new System.Drawing.Size(112, 30);
        this.Button10.TabIndex = 10;
        this.Button10.Click += new System.EventHandler(Button10_Click);
        //
        // DataGridView11
        //
        this.DataGridView11.AllowUserToAddRows = false;
        this.DataGridView11.AllowUserToDeleteRows = false;
        this.DataGridView11.AllowUserToResizeColumns = true;
        this.DataGridView11.AllowUserToResizeRows = false;
        this.DataGridView11.ReadOnly = true;
        this.DataGridView11.Text = "DataGridView11";
        this.DataGridView11.Location = new System.Drawing.Point(16, 248);
        this.DataGridView11.Size = new System.Drawing.Size(484, 164);
        this.DataGridView11.TabIndex = 11;
        this.DataGridView11.AutoGenerateColumns = false;
        this.DataGridView11.RowTemplate.Height = 60;
        LooVeerud();
        this.DataGridView11.Columns["Lisatud"].Width = 120;
        DataGridView11.Columns["PiltPreview"].Width = 80;
        DataGridView11.Columns["Kogus"].Width = 50;
        DataGridView11.Columns["Hind"].Width = 50;
        DataGridView11.Columns["Id"].Width = 50;

        //
        // PictureBox13
        //
        this.PictureBox13.TabIndex = 13;
        this.PictureBox13.Text = "PictureBox13";
        this.PictureBox13.Location = new System.Drawing.Point(260, 32);
        this.PictureBox13.Size = new System.Drawing.Size(240, 156);
        //
        // Button13
        //
        this.Button13.BackColor = System.Drawing.Color.LightSalmon;
        this.Button13.Text = "Vali pilt";
        this.Button13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.Button13.ForeColor = System.Drawing.Color.White;
        this.Button13.Location = new System.Drawing.Point(376, 208);
        this.Button13.Size = new System.Drawing.Size(112, 30);
        this.Button13.TabIndex = 13;
        this.Button13.Click += new System.EventHandler(Button13_Click);
        //
        // form
        //
        this.Size = new System.Drawing.Size(540, 468);
        this.Text = "Form1";
        this.Controls.Add(this.Label0);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.Label2);
        this.Controls.Add(this.Label3);
        this.Controls.Add(this.TextBox4);
        this.Controls.Add(this.TextBox5);
        this.Controls.Add(this.TextBox6);
        this.Controls.Add(this.ComboBox7);
        this.Controls.Add(this.Button8);
        this.Controls.Add(this.Button9);
        this.Controls.Add(this.Button10);
        this.Controls.Add(this.DataGridView11);
        this.Controls.Add(this.PictureBox13);
        this.Controls.Add(this.Button13);
        ((System.ComponentModel.ISupportInitialize)(this.DataGridView11)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox13)).EndInit();
        this.ResumeLayout(false);
        //
        // ComboLang
        //
        this.ComboLang.DropDownStyle = ComboBoxStyle.DropDownList;
        this.ComboLang.Items.AddRange(new object[] { "ET", "EN" });
        this.ComboLang.SelectedIndex = 0; // default: Estonian
        this.ComboLang.Location = new System.Drawing.Point(430, 5);
        this.ComboLang.Size = new System.Drawing.Size(60, 23);
        this.ComboLang.SelectedIndexChanged += new System.EventHandler(ComboLang_SelectedIndexChanged);

        this.Controls.Add(this.ComboLang);
    }



    #endregion

    private void LooVeerud()
    {
        DataGridView11.Columns.Clear();

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Id",
            HeaderText = "ID"
        });

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "ToodeNimetus",
            HeaderText = "Nimetus"
        });

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Kogus",
            HeaderText = "Kogus"
        });

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Hind",
            HeaderText = "Hind"
        });

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Kategooria",
            HeaderText = "Kategooria"
        });

        DataGridViewImageColumn imgCol = new DataGridViewImageColumn()
        {
            Name = "PiltPreview",
            HeaderText = "Pilt",
            ImageLayout = DataGridViewImageCellLayout.Zoom
        };
        DataGridView11.Columns.Add(imgCol);

        DataGridView11.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Lisatud",
            HeaderText = "Lisatud"
        });

        foreach (DataGridViewColumn col in DataGridView11.Columns)
        {
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }

    private System.Windows.Forms.Label Label0;
    private System.Windows.Forms.Label Label1;
    private System.Windows.Forms.Label Label2;
    private System.Windows.Forms.Label Label3;
    private System.Windows.Forms.TextBox TextBox4;
    private System.Windows.Forms.TextBox TextBox5;
    private System.Windows.Forms.TextBox TextBox6;
    private System.Windows.Forms.ComboBox ComboBox7;
    private System.Windows.Forms.Button Button8;
    private System.Windows.Forms.Button Button9;
    private System.Windows.Forms.Button Button10;
    private System.Windows.Forms.DataGridView DataGridView11;
    private System.Windows.Forms.PictureBox PictureBox13;
    private System.Windows.Forms.Button Button13;
    private System.Windows.Forms.ComboBox ComboLang;
}

// private void Button8_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Button9_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Button10_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

// private void Button13_Click(System.Object? sender, System.EventArgs e)
// {
// 
// }

