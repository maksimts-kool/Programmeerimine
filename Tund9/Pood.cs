using Microsoft.EntityFrameworkCore;

namespace Tund9;

public partial class Pood : Form
{
    private readonly TootedContext _db;
    private readonly string configPath = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory), "config.txt");
    public Pood()
    {
        InitializeComponent();
        if (File.Exists(configPath))
        {
            string lang = File.ReadAllText(configPath).Trim();
            if (lang == "EN" || lang == "ET")
                ComboLang.SelectedItem = lang;
        }
        else
        {
            ComboLang.SelectedItem = "ET";
        }
        SetLanguage(ComboLang.SelectedItem.ToString());

        TextBox4.TextChanged += (_, __) => UuendaNupud();
        TextBox5.TextChanged += (_, __) => UuendaNupud();
        TextBox6.TextChanged += (_, __) => UuendaNupud();
        ComboBox7.SelectedIndexChanged += (_, __) => UuendaNupud();
        DataGridView11.SelectionChanged += (_, __) => UuendaNupud();

        _db = new TootedContext();
        LaeKategoorariad();
        LaeTooted();
        using (var context = new TootedContext())
        {
            context.EnsureCreated();
        }
        UuendaNupud();
    }
    private void LaeTooted()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        string imagesFolder = Path.Combine(projectDir, "Images");

        var list = _db.Tooted.Include(t => t.Kategooria).ToList();

        DataGridView11.Rows.Clear();

        foreach (var t in list)
        {
            var imgPath = Path.Combine(imagesFolder, t.Pilt ?? "default.png");

            Image img = null;
            if (File.Exists(imgPath))
            {
                using (var temp = Image.FromFile(imgPath))
                {
                    img = new Bitmap(temp);
                }
            }

            DataGridView11.Rows.Add(
                t.Id,
                t.ToodeNimetus,
                t.Kogus,
                t.Hind,
                t.Kategooria.KategooriaNimetus,
                img,
                t.Lisatud
            );
        }
    }
    private void LaeKategoorariad()
    {
        ComboBox7.DataSource = _db.Kategooriad.ToList();
        ComboBox7.DisplayMember = "KategooriaNimetus";
        ComboBox7.ValueMember = "Id";
    }
    string valitudPilt = string.Empty;
    private void Button8_Click(System.Object? sender, System.EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TextBox4.Text) ||
            string.IsNullOrWhiteSpace(TextBox5.Text) ||
            string.IsNullOrWhiteSpace(TextBox6.Text) ||
            ComboBox7.SelectedItem == null)
        {
            MessageBox.Show("Palun täida kõik väljad.");
            return;
        }
        var uus = new Toode
        {
            ToodeNimetus = TextBox4.Text,
            Kogus = int.Parse(TextBox5.Text),
            Hind = double.Parse(TextBox6.Text),
            KategooriaId = (int)ComboBox7.SelectedValue,
            Lisatud = DateTime.Now
        };
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));

        string imagesFolder = Path.Combine(projectDir, "Images");

        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        string fileName;

        if (string.IsNullOrEmpty(valitudPilt))
        {
            fileName = "default.png";
        }
        else
        {
            fileName = Path.GetFileName(valitudPilt);
            string destPath = Path.Combine(imagesFolder, fileName);
            File.Copy(valitudPilt, destPath, true);
        }

        uus.Pilt = fileName;
        _db.Tooted.Add(uus);
        _db.SaveChanges();
        LaeTooted();
        PuhastaVorm();
    }
    private void Button13_Click(System.Object? sender, System.EventArgs e)
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Filter = "Pildi failid|*.jpg;*.png;*.jpeg;*.bmp";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            valitudPilt = dlg.FileName;
            PictureBox13.Image = Image.FromFile(valitudPilt);
            PictureBox13.SizeMode = PictureBoxSizeMode.Zoom;
            UuendaNupud();
        }
    }
    private void PuhastaVorm()
    {
        TextBox4.Clear();
        TextBox5.Clear();
        TextBox6.Clear();
        ComboBox7.SelectedIndex = -1;
        PictureBox13.Image = null;
        valitudPilt = string.Empty;
    }
    private void Button9_Click(object? sender, EventArgs e)
    {
        if (DataGridView11.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vali rida.");
            return;
        }

        int id = (int)DataGridView11.SelectedRows[0].Cells["Id"].Value;
        var toode = _db.Tooted.Find(id);

        if (TextBox4.Text != "" && TextBox4.Text != toode.ToodeNimetus)
            toode.ToodeNimetus = TextBox4.Text;

        if (TextBox5.Text != "" && int.TryParse(TextBox5.Text, out int kogus)
            && kogus != toode.Kogus)
            toode.Kogus = kogus;

        if (TextBox6.Text != "" && double.TryParse(TextBox6.Text, out double hind)
            && hind != toode.Hind)
            toode.Hind = hind;

        if (ComboBox7.SelectedItem != null &&
            (int)ComboBox7.SelectedValue != toode.KategooriaId)
            toode.KategooriaId = (int)ComboBox7.SelectedValue;

        if (!string.IsNullOrEmpty(valitudPilt))
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
            string imagesFolder = Path.Combine(projectDir, "Images");

            Directory.CreateDirectory(imagesFolder);

            string fileName = Path.GetFileName(valitudPilt);
            File.Copy(valitudPilt, Path.Combine(imagesFolder, fileName), true);
            toode.Pilt = fileName;
        }

        _db.SaveChanges();
        LaeTooted();
        UuendaNupud();
    }
    private void Button10_Click(object? sender, EventArgs e)
    {
        if (DataGridView11.SelectedRows.Count == 0)
        {
            MessageBox.Show("Palun vali toode, mida soovid kustutada.");
            return;
        }

        string toodeNimetus = DataGridView11.SelectedRows[0]
            .Cells["ToodeNimetus"].Value.ToString();

        DialogResult vastus = MessageBox.Show(
            $"Kas oled kindel, et soovid toote '{toodeNimetus}' kustutada?",
            "Kinnita kustutamine",
            MessageBoxButtons.YesNo);

        if (vastus != DialogResult.Yes)
            return;

        try
        {
            int id = (int)DataGridView11.SelectedRows[0].Cells["Id"].Value;
            var toode = _db.Tooted.Find(id);

            if (toode != null)
            {
                // build image path
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
                string imagesFolder = Path.Combine(projectDir, "Images");

                string imgPath = Path.Combine(imagesFolder, toode.Pilt ?? "");

                // delete DB record
                _db.Tooted.Remove(toode);
                _db.SaveChanges();

                // delete image file if not default and file exists
                if (toode.Pilt != "default.png" &&
                    File.Exists(imgPath))
                {
                    try
                    {
                        File.Delete(imgPath);
                    }
                    catch
                    {
                        // ignore, no crash
                    }
                }

                LaeTooted();
                PuhastaVorm();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Tekkis viga: {ex.Message}");
        }
    }
    private void UuendaNupud()
    {
        // Lisa (ADD)
        Button8.Enabled =
            !string.IsNullOrWhiteSpace(TextBox4.Text) ||
            !string.IsNullOrWhiteSpace(TextBox5.Text) ||
            !string.IsNullOrWhiteSpace(TextBox6.Text) ||
            ComboBox7.SelectedItem != null ||
            !string.IsNullOrEmpty(valitudPilt);

        // Delete
        bool rowSelected = DataGridView11.SelectedRows.Count > 0;
        Button10.Enabled = rowSelected;

        // Update
        if (!rowSelected)
        {
            Button9.Enabled = false;
            return;
        }

        var r = DataGridView11.SelectedRows[0];

        bool changed =
            (TextBox4.Text != "" &&
             TextBox4.Text != r.Cells["ToodeNimetus"].Value.ToString()) ||
            (TextBox5.Text != "" &&
             TextBox5.Text != r.Cells["Kogus"].Value.ToString()) ||
            (TextBox6.Text != "" &&
             TextBox6.Text != r.Cells["Hind"].Value.ToString()) ||
            (ComboBox7.SelectedItem != null &&
             ComboBox7.Text != r.Cells["Kategooria"].Value.ToString()) ||
            !string.IsNullOrEmpty(valitudPilt);

        Button9.Enabled = changed;
    }
    private void SetLanguage(string lang)
    {
        if (lang == "EN")
        {
            Label0.Text = "Product name";
            Label1.Text = "Product quantity";
            Label2.Text = "Product price";
            Label3.Text = "Category";

            Button8.Text = "Add";
            Button9.Text = "Update";
            Button10.Text = "Delete";
            Button13.Text = "Choose image";

            this.Text = "Store";
        }
        else // ET (default)
        {
            Label0.Text = "Toode nimetus";
            Label1.Text = "Toode kogus";
            Label2.Text = "Toode hind";
            Label3.Text = "Kategooria";

            Button8.Text = "Lisa";
            Button9.Text = "Uuenda";
            Button10.Text = "Kustuta";
            Button13.Text = "Vali pilt";

            this.Text = "Pood";
        }
    }
    private void ComboLang_SelectedIndexChanged(object? sender, EventArgs e)
    {
        string lang = ComboLang.SelectedItem.ToString();
        SetLanguage(lang);
    }
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        try
        {
            File.WriteAllText(configPath, ComboLang.SelectedItem?.ToString() ?? "ET");
        }
        catch
        {
            // ignore errors
        }
    }
}
