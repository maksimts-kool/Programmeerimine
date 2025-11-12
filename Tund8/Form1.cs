using System.Data;
using Microsoft.Data.SqlClient;

namespace Tund8;

public partial class Form1 : Form
{
    private SqlCommand? _command;
    private SqlConnection _connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=C:\Users\opilane\source\repos\ProgrammeerimineTARpv24\Tund8\ShopDB.mdf;
      Integrated Security=True;");
    private SqlDataAdapter? _adapterProduct;
    public Form1()
    {
        InitializeComponent();
        UpdateCategories();
        NaitaAndmed();
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
        _openFileDialog.InitialDirectory = @"C:\Users\opilane\Pictures";
        _openFileDialog.Multiselect = true;
        _openFileDialog.Filter = "Pictures Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
        string product = ToodeBox.Text;

        if (_openFileDialog.ShowDialog() == DialogResult.OK && product != null)
        {
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.InitialDirectory = Path.GetFullPath(@"..\..\Pildid");

            string ext = Path.GetExtension(_openFileDialog.FileName);
            _saveFileDialog.FileName = product + ext;
            _saveFileDialog.Filter = "Pildid" + ext + "|" + ext;

            if (_saveFileDialog.ShowDialog() == DialogResult.OK && product != null)
            {
                File.Copy(_openFileDialog.FileName, _saveFileDialog.FileName);
                PictureBox.Image = Image.FromFile(_saveFileDialog.FileName);
            }
        }
        else
            MessageBox.Show("Puudub toode nimetus või oli vajatud Cancel");
    }
    private SqlDataAdapter? _adapter_toode;
    public void NaitaAndmed()
    {
        _connect.Open();
        DataTable _dt_toode = new DataTable();
        _adapter_toode = new SqlDataAdapter("SELECT ToodeTabel.Id, ToodeTabel.Toode_nim, ToodeTabel.Kogus, ToodeTabel.Hind, ToodeTabel.Pilt, ToodeTabel.Bpilt, KatTabel.Kategooria_nim AS Kategooria_nim FROM ToodeTabel INNER JOIN KatTabel ON ToodeTabel.Kategooriad = KatTabel.Id", _connect);
        _adapter_toode.Fill(_dt_toode);
        DataGridView.Columns.Clear();
        DataGridView.DataSource = _dt_toode;
        DataGridViewComboBoxColumn combo_kat = new DataGridViewComboBoxColumn();
        combo_kat.DataPropertyName = "Kategooria_nim";
        HashSet<string> keys = new HashSet<string>();
        foreach (DataRow item in _dt_toode.Rows)
        {
            string kat_n = item["Kategooria_nim"]?.ToString() ?? string.Empty;
            if (!keys.Contains(kat_n))
            {
                keys.Add(kat_n);
                combo_kat.Items.Add(kat_n);
            }
        }
        DataGridView.Columns.Add(combo_kat);
        PictureBox.Image = Image.FromFile(Path.Combine(Path.GetFullPath(@"..\..\..\Pildid"), "epood.png"));
        _connect.Close();
    }
    private Form? popupForm;
    private void LooPilt(Image image, int r)
    {
        popupForm = new Form();
        popupForm.FormBorderStyle = FormBorderStyle.None;
        popupForm.StartPosition = FormStartPosition.Manual;
        popupForm.Size = new Size(200, 200);
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = image;
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        popupForm.Controls.Add(pictureBox);
        System.Drawing.Rectangle cellRectangle = DataGridView.GetCellDisplayRectangle(4, r, true);
        System.Drawing.Point popupLoc = DataGridView.PointToScreen(cellRectangle.Location);
        popupForm.Location = new System.Drawing.Point(popupLoc.X + cellRectangle.Width, popupLoc.Y);
        popupForm.Show();
    }
    byte[]? imageData;
    private void DataGridView_CellMouseEnter(System.Object? sender, System.Windows.Forms.DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == 4 && e.RowIndex >= 0)
        {
            imageData = DataGridView.Rows[e.RowIndex].Cells["Bpilt"].Value as byte[];
            if (imageData != null)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image img = Image.FromStream(ms);
                    LooPilt(img, e.RowIndex);
                }
            }
        }
    }
    private void DataGridView_CellMouseLeave(System.Object? sender, System.Windows.Forms.DataGridViewCellEventArgs e)
    {
        if (popupForm != null && !popupForm.IsDisposed)
        {
            popupForm.Close();
        }
    }
    private string? extension;
    private void Lisa_Click(System.Object? sender, System.EventArgs e)
    {
        if (ToodeBox.Text.Trim() != string.Empty && KogusBox.Text.Trim() != string.Empty && HindBox.Text.Trim() != string.Empty && KategooriadBox.Text.Trim() != string.Empty)
        {
            try
            {
                _connect.Open();
                _command = new SqlCommand("SELECT Id FROM KatTabel WHERE Kategooria_nim=@kat", _connect);
                _command.Parameters.AddWithValue("@kat", KategooriadBox.Text);
                int Id = Convert.ToInt32(_command.ExecuteScalar());
                _command = new SqlCommand("INSERT INTO ToodeTabel (Toode_nim, Kogus, Hind, Pilt, Bpilt, Kategooriad) VALUES (@toode, @kogus, @hind, @pilt, @bpilt, @kat)", _connect);
                _command.Parameters.AddWithValue("@toode", ToodeBox.Text);
                _command.Parameters.AddWithValue("@kogus", KogusBox.Text);
                _command.Parameters.AddWithValue("@hind", HindBox.Text);
                extension = Path.GetExtension(_openFileDialog?.FileName ?? string.Empty);
                _command.Parameters.AddWithValue("@pilt", ToodeBox.Text + extension);
                imageData = File.ReadAllBytes(_openFileDialog?.FileName ?? string.Empty);
                _command.Parameters.AddWithValue("@bpilt", imageData);
                _command.Parameters.AddWithValue("@kat", Id);
                _command.ExecuteNonQuery();
                _connect.Close();
                NaitaAndmed();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga andmebaasiga ühendamisel: " + ex.Message);
            }
        }
    }
}
