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
        string categoryName = KategooriadBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(categoryName))
        {
            MessageBox.Show("Kategooria nimi ei tohi olla tühi!",
                             "Viga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (categoryName.Length < 3)
        {
            MessageBox.Show("Kategooria nimi peab sisaldama vähemalt 3 tähemärki!",
                             "Viga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        bool exists = false;
        foreach (var item in KategooriadBox.Items)
        {
            if (string.Equals(item.ToString(), categoryName, StringComparison.OrdinalIgnoreCase))
            {
                exists = true;
                break;
            }
        }

        if (exists)
        {
            MessageBox.Show("Selline kategooria on juba olemas!",
                             "Hoiatus", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            using (_connect)
            {
                _connect.Open();
                _command = new SqlCommand("INSERT INTO KatTabel (Kategooria_nim) VALUES (@cat)", _connect);
                _command.Parameters.AddWithValue("@cat", categoryName);
                _command.ExecuteNonQuery();
            }

            KategooriadBox.Items.Clear();
            UpdateCategories();

            MessageBox.Show($"Kategooria '{categoryName}' on lisatud!",
                             "Õnnestus", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga kategooria lisamisel: " + ex.Message,
                            "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            _saveFileDialog.InitialDirectory = Path.GetFullPath(@"..\..\..\Pildid");

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
    private string? extension = string.Empty;
    private void Lisa_Click(System.Object? sender, System.EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ToodeBox.Text) ||
            string.IsNullOrWhiteSpace(KogusBox.Text) ||
            string.IsNullOrWhiteSpace(HindBox.Text) ||
            string.IsNullOrWhiteSpace(KategooriadBox.Text))
        {
            MessageBox.Show("Palun täida kõik väljad!", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(KogusBox.Text, out int kogus) || kogus < 0)
        {
            MessageBox.Show("Kogus peab olema positiivne täisarv!", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!decimal.TryParse(HindBox.Text.Replace(',', '.'),
        System.Globalization.NumberStyles.Number,
        System.Globalization.CultureInfo.InvariantCulture, out decimal hind) || hind < 0)
        {
            MessageBox.Show("Hind peab olema positiivne arv!",
                "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (_openFileDialog == null || string.IsNullOrEmpty(_openFileDialog.FileName) || !File.Exists(_openFileDialog.FileName))
        {
            MessageBox.Show("Palun vali pilt enne lisamist!", "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            _connect.Open();

            _command = new SqlCommand("SELECT Id FROM KatTabel WHERE Kategooria_nim=@kat", _connect);
            _command.Parameters.AddWithValue("@kat", KategooriadBox.Text);
            int Id = Convert.ToInt32(_command.ExecuteScalar());

            _command = new SqlCommand("INSERT INTO ToodeTabel (Toode_nim, Kogus, Hind, Pilt, Bpilt, Kategooriad) VALUES (@toode, @kogus, @hind, @pilt, @bpilt, @kat)", _connect);
            _command.Parameters.AddWithValue("@toode", ToodeBox.Text.Trim());
            _command.Parameters.AddWithValue("@kogus", kogus);
            _command.Parameters.AddWithValue("@hind", hind);

            string ext = Path.GetExtension(_openFileDialog.FileName);
            _command.Parameters.AddWithValue("@pilt", ToodeBox.Text.Trim() + ext);

            byte[] imageData = File.ReadAllBytes(_openFileDialog.FileName);
            _command.Parameters.AddWithValue("@bpilt", imageData);
            _command.Parameters.AddWithValue("@kat", Id);

            _command.ExecuteNonQuery();
            _connect.Close();

            NaitaAndmed();
            MessageBox.Show($"Toode '{ToodeBox.Text}' on lisatud!", "Õnnestus", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga andmebaasiga ühendamisel: " + ex.Message);
            if (_connect.State == ConnectionState.Open)
                _connect.Close();
        }
    }
    private void Puhasta_Click(System.Object? sender, System.EventArgs e)
    {
        ToodeBox.Text = "";
        KogusBox.Text = "";
        HindBox.Text = "";
        KategooriadBox.SelectedItem = null;
        using (FileStream fs = new FileStream(Path.Combine(Path.GetFullPath(@"..\..\..\Pildid"), "epood.png"), FileMode.Open, FileAccess.Read))
        {
            PictureBox.Image = Image.FromStream(fs);
        }
    }
    private int Id;
    private void Kustuta_Click(System.Object? sender, System.EventArgs e)
    {
        Id = Convert.ToInt32(DataGridView.CurrentRow?.Cells["Id"].Value);
        MessageBox.Show(Id.ToString());
        if (Id != 0)
        {
            _command = new SqlCommand("DELETE FROM ToodeTabel WHERE Id=@id", _connect);
            _connect.Open();
            _command.Parameters.AddWithValue("@id", Id);
            _command.ExecuteNonQuery();
            _connect.Close();
            NaitaAndmed();
            MessageBox.Show("Toode on kustutatud!");
        }
        else
        {
            MessageBox.Show("Vali toode, mida kustutada soovid!");
        }
    }
    private void Uuenda_Click(System.Object? sender, System.EventArgs e)
    {
        // Identify selected product (based on clicked row)
        if (DataGridView.CurrentRow == null)
        {
            MessageBox.Show("Vali vähemalt üks tooterida, mida uuendada!");
            return;
        }

        int id = Convert.ToInt32(DataGridView.CurrentRow.Cells["Id"].Value);
        if (id <= 0)
        {
            MessageBox.Show("Vale ID väärtus!");
            return;
        }

        try
        {
            if (_connect.State != ConnectionState.Open)
                _connect.Open();

            // --- Build dynamic UPDATE statement ---
            List<string> updateFields = new List<string>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connect;

            // Update only fields the user provided
            if (!string.IsNullOrWhiteSpace(ToodeBox.Text))
            {
                updateFields.Add("Toode_nim=@toode");
                cmd.Parameters.AddWithValue("@toode", ToodeBox.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(KogusBox.Text))
            {
                if (int.TryParse(KogusBox.Text, out int kogus))
                {
                    updateFields.Add("Kogus=@kogus");
                    cmd.Parameters.AddWithValue("@kogus", kogus);
                }
                else
                {
                    MessageBox.Show("Kogus peab olema arv!");
                    _connect.Close();
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(HindBox.Text))
            {
                if (decimal.TryParse(
                        HindBox.Text.Replace(',', '.'),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out decimal hind))
                {
                    updateFields.Add("Hind=@hind");
                    cmd.Parameters.AddWithValue("@hind", hind);
                }
                else
                {
                    MessageBox.Show("Hind peab olema number!");
                    _connect.Close();
                    return;
                }
            }

            if (PictureBox.Image != null && _openFileDialog != null && !string.IsNullOrEmpty(_openFileDialog.FileName))
            {
                if (File.Exists(_openFileDialog.FileName))
                {
                    string ext = Path.GetExtension(_openFileDialog.FileName);
                    string fileName = ToodeBox.Text.Trim() + ext;

                    updateFields.Add("Pilt=@pilt, Bpilt=@bpilt");

                    byte[] imageBytes = File.ReadAllBytes(_openFileDialog.FileName);
                    cmd.Parameters.AddWithValue("@pilt", fileName);
                    cmd.Parameters.AddWithValue("@bpilt", imageBytes);
                }
            }

            // Update category if selected
            if (KategooriadBox.SelectedItem != null)
            {
                updateFields.Add("Kategooriad=@kat");
                SqlCommand getCatCmd = new SqlCommand("SELECT Id FROM KatTabel WHERE Kategooria_nim=@nim", _connect);
                getCatCmd.Parameters.AddWithValue("@nim", KategooriadBox.Text);
                int katId = Convert.ToInt32(getCatCmd.ExecuteScalar());
                cmd.Parameters.AddWithValue("@kat", katId);
            }

            // --- Execute update if there are any fields to modify ---
            if (updateFields.Count == 0)
            {
                MessageBox.Show("Pole midagi uuendada! Täida vähemalt üks väli.");
                _connect.Close();
                return;
            }

            string updateQuery =
                "UPDATE ToodeTabel SET " +
                string.Join(", ", updateFields) +
                " WHERE Id=@id";

            cmd.CommandText = updateQuery;
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            _connect.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Toote andmed on uuendatud!");
                NaitaAndmed();
            }
            else
            {
                MessageBox.Show("Uuendus ebaõnnestus — kontrolli andmeid!");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga andmete uuendamisel: " + ex.Message);
            if (_connect.State == ConnectionState.Open)
                _connect.Close();
        }
    }
    private void Pood_Click(System.Object? sender, System.EventArgs e)
    {
        Form storeForm = new Form
        {
            Text = "Pood — Kliendivaade",
            Size = new Size(1000, 700),
            StartPosition = FormStartPosition.CenterScreen
        };

        SplitContainer split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            IsSplitterFixed = false,
            FixedPanel = FixedPanel.Panel1,
            SplitterWidth = 6
        };

        split.SplitterDistance = (int)(storeForm.Width * 0.2);

        ListBox listBoxCategories = new ListBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11, FontStyle.Regular),
            BorderStyle = BorderStyle.None
        };

        FlowLayoutPanel panelProducts = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            Padding = new Padding(10)
        };

        split.Panel1.Controls.Add(listBoxCategories);
        split.Panel2.Controls.Add(panelProducts);
        storeForm.Controls.Add(split);

        try
        {
            if (_connect.State != ConnectionState.Open)
                _connect.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT Id, Kategooria_nim FROM KatTabel", _connect);
            DataTable categories = new DataTable();
            adapter.Fill(categories);

            listBoxCategories.DataSource = categories;
            listBoxCategories.DisplayMember = "Kategooria_nim";
            listBoxCategories.ValueMember = "Id";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga kategooriate laadimisel: " + ex.Message);
        }
        finally
        {
            if (_connect.State == ConnectionState.Open)
                _connect.Close();
        }

        listBoxCategories.SelectedIndexChanged += (s, ev) =>
        {
            if (listBoxCategories.SelectedValue == null)
                return;

            panelProducts.Controls.Clear();
            int catId = Convert.ToInt32(listBoxCategories.SelectedValue);

            try
            {
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SELECT Toode_nim, Hind, Kogus, Pilt " +
                    "FROM ToodeTabel WHERE Kategooriad = @id", _connect);
                cmd.Parameters.AddWithValue("@id", catId);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable products = new DataTable();
                adp.Fill(products);

                foreach (DataRow row in products.Rows)
                {
                    Panel prodPanel = new Panel
                    {
                        Width = 200,
                        Height = 250,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10)
                    };

                    string imagePath = Path.Combine(
                        Path.GetFullPath(@"..\..\..\Pildid"),
                        row["Pilt"].ToString() ?? "epood.png");

                    PictureBox pb = new PictureBox
                    {
                        Width = 180,
                        Height = 150,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = File.Exists(imagePath)
                            ? Image.FromFile(imagePath)
                            : Image.FromFile(Path.Combine(Path.GetFullPath(@"..\..\..\Pildid"), "epood.png")),
                        Dock = DockStyle.Top
                    };

                    Label lblName = new Label
                    {
                        Text = row["Toode_nim"].ToString(),
                        Dock = DockStyle.Bottom,
                        Height = 30,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };
                    Label lblPrice = new Label
                    {
                        Text = $"{Convert.ToDecimal(row["Hind"]):N2} €",
                        Dock = DockStyle.Bottom,
                        Height = 20,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    Label lblQty = new Label
                    {
                        Text = $"Laos: {row["Kogus"]}",
                        Dock = DockStyle.Bottom,
                        Height = 20,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    prodPanel.Controls.Add(lblQty);
                    prodPanel.Controls.Add(lblPrice);
                    prodPanel.Controls.Add(lblName);
                    prodPanel.Controls.Add(pb);

                    panelProducts.Controls.Add(prodPanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga toodete laadimisel: " + ex.Message);
            }
            finally
            {
                if (_connect.State == ConnectionState.Open)
                    _connect.Close();
            }
        };

        storeForm.Show();
    }
}
