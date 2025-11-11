using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging.Effects;
using System.Runtime.CompilerServices;
using System.Windows.Forms;


namespace Tund8;

public partial class Form1 : Form
{
    SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=C:\Users\opilane\source\repos\ProgrammeerimineTARpv24\Tund8\ShopDB.mdf;
      Integrated Security=True;");
    SqlCommand command;
    SqlDataAdapter adapter_kat;
    SaveFileDialog open, save;
    string extension;
    public Form1()
    {
        InitializeComponent();
    }
    private void KategooriadBox_Click(object sender, EventArgs e)
    {
        bool on = false;
        foreach (var item in KategooriadBox.Items)
        {
            if (item.ToString() == KategooriadBox.Text)
            {
                on = true;
            }
        }
        if (on == false)
        {
            command = new SqlCommand("INSERT INTO KatTabel(Kategooria_nim,Kirjeldus) VALUES (@kat)", connect);
            connect.Open();
        }
    }
    private void OtsiFail_Click(object sender, EventArgs e)
    {
        open = new SaveFileDialog();
        open.InitialDirectory = @"C:\Users\opilane\Pildid";
        open.Multiselect = true;
        open.Filter = "Images Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
        FileInfo open_info = new FileInfo(@"C:\Users\opilane\Pildid\" + open.FileName);
        if (open.ShowDialog() == DialogResult.OK && ToodeBox.Text != null)
        {
            save = new SaveFileDialog();
            save.InitialDirectory = Path.GetFullPath(@"..\..\Pildid");
            extension = Path.GetExtension(open.FileName);
            save.FileName = ToodeBox
        }
    }
    private void NaitaKategooriad()
    {
        connect.Open();
        adapter_kat = new SqlDataAdapter("SELECT Id,Kategooria_nim FROM KatTabel", connect);
        DataTable dt_kat = new DataTable();
        adapter_kat.Fill(dt_kat);
        foreach (DataRow item in dt_kat.Rows)
        {
            if (!KategooriadBox.Items.Contains(item["Kategooria_nim"]))
            {
                KategooriadBox.Items.Add(item["Kategooria_nim"]);
            }
            else
            {
                MessageBox.Show("Selline ");
            }
        }
    }
}
