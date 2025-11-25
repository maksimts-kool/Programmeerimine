using Microsoft.EntityFrameworkCore;

namespace Tund9;

public partial class Pood : Form
{
    private readonly TootedContext _db;
    public Pood()
    {
        InitializeComponent();

        _db = new TootedContext();
        LaeKategoorariad();
        LaeTooted();
        using (var context = new TootedContext())
        {
            context.EnsureCreated();
        }
    }
    private void LaeTooted()
    {
        DataGridView11.DataSource = _db.Tooted.Include(t => t.Kategooria).Select(t => new
        {
            t.Id,
            t.ToodeNimetus,
            t.Kogus,
            t.Hind,
            Kategooria = t.Kategooria.KategooriaNimetus,
            t.Pilt,
            t.Lisatud
        }).ToList();
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
        string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
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
        //PuhastaVorm();
    }
}
