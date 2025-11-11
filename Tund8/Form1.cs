using System.Data;
using Microsoft.Data.SqlClient;

namespace Tund8;

public partial class Form1 : Form
{
    private SqlCommand? _command;
    private SqlConnection _connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=C:\Users\maksi\Documents\GitHub\Programmeerimine\Tund8\ShopDB.mdf;
      Integrated Security=True;");
    private SqlDataAdapter? _adapterProduct;
    public Form1()
    {
        InitializeComponent();
        UpdateCategories();
    }
    private void UpdateCategories()
    {
        _connect.Open();
        _adapterProduct = new SqlDataAdapter("SELECT Id, Kategooria_nim FROM KatTabel", _connect);
        DataTable dt = new();
        _adapterProduct.Fill(dt);

        foreach (DataRow item in dt.Rows)
        {
            if (!KategooriadBox.Items.Contains(item["Kategooria_nim"]))
                KategooriadBox.Items.Add(item["Kategooria_nim"]);
            else
            {
                _command = new SqlCommand("DELETE FROM KatTabel WHERE Id=@id", _connect);
                _command.Parameters.AddWithValue("@id", item["Id"]);
                _command.ExecuteNonQuery();
            }
        }

        _connect.Close();
    }
    private void LisaKat_Click(System.Object? sender, System.EventArgs e)
    {
        bool on = false;
        foreach (var item in KategooriadBox.Items)
        {
            if (item.ToString() == KategooriadBox.Text)
                on = true;
        }

        if (!on)
        {
            _command = new SqlCommand("INSERT INTO KatTabel (Kategooria_nim) VALUES (@cat)", _connect);
            _connect.Open();
            _command.Parameters.AddWithValue("@cat", KategooriadBox.Text);
            _command.ExecuteNonQuery();
            _connect.Close();
            KategooriadBox.Items.Clear();
            UpdateCategories();
            MessageBox.Show($"Kategooria {KategooriadBox.Text} on lisatud!");
        }
        else
            MessageBox.Show("Selline kategooriat on juba olemas!");
    }

    private void KustutaKat_Click(System.Object? sender, System.EventArgs e)
    {
        if (KategooriadBox.SelectedItem != null)
        {
            _connect.Open();
            string value = KategooriadBox.SelectedItem.ToString() ?? string.Empty;
            _command = new SqlCommand("DELETE FROM KatTabel WHERE Kategooria_nim=@cat", _connect);
            _command.Parameters.AddWithValue("@cat", value);
            _command.ExecuteNonQuery();
            _connect.Close();
            KategooriadBox.Items.Clear();
            UpdateCategories();
        }
    }

    private SaveFileDialog? _saveFileDialog;
    private OpenFileDialog? _openFileDialog;

    private void Otsi_Click(System.Object? sender, System.EventArgs e)
    {
        _openFileDialog = new OpenFileDialog();
        _openFileDialog.InitialDirectory = @"C:\Users\maksi\Pictures";
        _openFileDialog.Multiselect = true;
        _openFileDialog.Filter = "Pictures Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
        string product = ToodeBox.Text;

        FileInfo openInfo = new(@"C:\Users\opilane\Pictures" + _openFileDialog.FileName);
        if (_openFileDialog.ShowDialog() == DialogResult.OK && product != null)
        {
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.InitialDirectory = Path.GetFullPath(@"..\..\Pictures");

            string ext = Path.GetExtension(_openFileDialog.FileName);
            _saveFileDialog.FileName = product + ext;
            _saveFileDialog.Filter = "Pictures" + ext + "|" + ext;

            if (_saveFileDialog.ShowDialog() == DialogResult.OK && product != null)
            {
                File.Copy(_openFileDialog.FileName, _saveFileDialog.FileName);
                PictureBox.Image = Image.FromFile(_saveFileDialog.FileName);
            }
        }
        else
            MessageBox.Show("Puudub toode nimetus või oli vajatud Cancel");
    }
}
