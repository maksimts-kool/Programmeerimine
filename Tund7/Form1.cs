using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Tund7
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private CheckBox chkStretch;
        private CheckBox chkFitToPicture;
        private Button btnShow, btnClear, btnClose, btnSave, btnEffects;
        private TableLayoutPanel tableLayoutPanel;
        private FlowLayoutPanel flowLayoutPanelButtons;
        private FlowLayoutPanel flowLayoutPanelCheckBoxes;
        private OpenFileDialog openFileDialog;
        private ColorDialog colorDialog;

        private FormBorderStyle previousBorderStyle;

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            // === Form setup ===
            this.Text = "Picture Viewer";
            this.Size = new Size(800, 600);

            // === Create dialogs ===
            openFileDialog = new OpenFileDialog();
            colorDialog = new ColorDialog();

            // === TableLayoutPanel setup ===
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            // === PictureBox setup ===
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            tableLayoutPanel.SetColumnSpan(pictureBox, 2);

            // === FlowLayoutPanels setup ===
            flowLayoutPanelCheckBoxes = new FlowLayoutPanel();
            flowLayoutPanelCheckBoxes.Dock = DockStyle.Fill;
            flowLayoutPanelCheckBoxes.AutoSize = true;
            flowLayoutPanelCheckBoxes.FlowDirection = FlowDirection.LeftToRight;

            flowLayoutPanelButtons = new FlowLayoutPanel();
            flowLayoutPanelButtons.Dock = DockStyle.Fill;
            flowLayoutPanelButtons.FlowDirection = FlowDirection.RightToLeft;

            // === CheckBoxes setup ===

            chkStretch = new CheckBox
            {
                Text = "Pikenda",
                AutoSize = true,
                Enabled = false
            };
            chkStretch.CheckedChanged += ChkStretch_CheckedChanged;

            chkFitToPicture = new CheckBox
            {
                Text = "Kohanda aken pildi suurusele",
                AutoSize = true,
                Enabled = false
            };
            chkFitToPicture.CheckedChanged += ChkFitToPicture_CheckedChanged;

            flowLayoutPanelCheckBoxes.Controls.Add(chkStretch);
            flowLayoutPanelCheckBoxes.Controls.Add(chkFitToPicture);

            // === Buttons setup ===
            btnShow = new Button { Text = "Näita pilti", AutoSize = true };
            btnShow.Click += BtnShowPicture_Click;

            btnClear = new Button { Text = "Puhasta pilt", AutoSize = true };
            btnClear.Click += BtnClearPicture_Click;

            btnClose = new Button { Text = "Sule", AutoSize = true };
            btnClose.Click += BtnClose_Click;

            btnEffects = new Button { Text = "Efektid", AutoSize = true };
            btnEffects.Click += BtnEffects_Click;
            flowLayoutPanelButtons.Controls.Add(btnEffects);

            btnSave = new Button { Text = "Salvesta kui...", AutoSize = true };
            btnSave.Click += BtnSave_Click;


            flowLayoutPanelButtons.Controls.Add(btnClose);
            flowLayoutPanelButtons.Controls.Add(btnEffects);
            flowLayoutPanelButtons.Controls.Add(btnSave);
            flowLayoutPanelButtons.Controls.Add(btnClear);
            flowLayoutPanelButtons.Controls.Add(btnShow);

            // === Add controls to layout ===
            tableLayoutPanel.Controls.Add(pictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(flowLayoutPanelCheckBoxes, 0, 1);
            tableLayoutPanel.Controls.Add(flowLayoutPanelButtons, 1, 1);

            // === Add layout to the form ===
            this.Controls.Add(tableLayoutPanel);

            // === OpenFileDialog setup ===
            openFileDialog.Filter =
                "JPEG failid (*.jpg)|*.jpg|PNG failid (*.png)|*.png|BMP failid (*.bmp)|*.bmp|Kõik failid (*.*)|*.*";
            openFileDialog.Title = "Valige pildifail";
        }

        private void ChkStretch_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.SizeMode = chkStretch.Checked
                ? PictureBoxSizeMode.StretchImage
                : PictureBoxSizeMode.Normal;
        }

        private void ChkFitToPicture_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFitToPicture.Checked && pictureBox.Image != null)
            {
                previousBorderStyle = this.FormBorderStyle;

                Size imgSize = pictureBox.Image.Size;

                int widthDelta = this.Width - this.ClientSize.Width;
                int heightDelta = this.Height - this.ClientSize.Height;

                this.ClientSize = new Size(imgSize.Width, imgSize.Height);
                this.Width += widthDelta;
                this.Height += heightDelta;

                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            }
            else if (!chkFitToPicture.Checked)
            {
                this.FormBorderStyle = previousBorderStyle;
            }
        }

        private void BtnShowPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);

                chkStretch.Enabled = true;
                chkFitToPicture.Enabled = true;

                if (chkFitToPicture.Checked)
                {
                    ChkFitToPicture_CheckedChanged(sender, e);
                }
            }
        }

        private void BtnClearPicture_Click(object sender, EventArgs e)
        {
            pictureBox.Image = null;
            chkStretch.Enabled = false;
            chkFitToPicture.Enabled = false;
        }

        private void BtnEffects_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("Pole pilti, millele efekte rakendada.", "Hoiatus",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FormEffects effectsForm = new FormEffects())
            {
                effectsForm.EffectSelected += effectName =>
                {
                    switch (effectName)
                    {
                        case "grayscale":
                            ApplyGrayscale();
                            break;
                        case "border":
                            ApplyBorder();
                            break;
                    }
                };

                effectsForm.ShowDialog(this);
            }
        }
        private void ApplyGrayscale()
        {
            Bitmap bitmap = new Bitmap(pictureBox.Image);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    int gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                    Color grayColor = Color.FromArgb(pixel.A, gray, gray, gray);
                    bitmap.SetPixel(x, y, grayColor);
                }
            }

            pictureBox.Image = bitmap;
        }

        private void ApplyBorder()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Sisesta ääre paksus:",
                "Lisa ääris",
                "20"
            );

            if (!int.TryParse(input, out int borderSize) || borderSize <= 0)
            {
                MessageBox.Show("Vale sisend",
                    "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            Color borderColor = dialog.Color;

            Bitmap original = new Bitmap(pictureBox.Image);
            int newWidth = original.Width + borderSize * 2;
            int newHeight = original.Height + borderSize * 2;

            Bitmap bordered = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(bordered))
            {
                g.Clear(borderColor);
                g.DrawImage(original, borderSize, borderSize, original.Width, original.Height);
            }

            pictureBox.Image = bordered;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
            {
                MessageBox.Show("Pilt pole laaditud. Pole midagi salvestada!",
                    "Hoiatus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Salvesta pilt kui";
                saveDialog.Filter = "Kõik failid (*.*)|*.*";
                saveDialog.FileName = "pilt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = saveDialog.FileName;
                    string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
                    var format = System.Drawing.Imaging.ImageFormat.Png; // default

                    switch (ext)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        case ".gif":
                            format = System.Drawing.Imaging.ImageFormat.Gif;
                            break;
                        case ".png":
                            format = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                        default:
                            path += ".png";
                            MessageBox.Show("Tundmatu faililaiend, salvestati PNG-na.",
                                "Teade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }

                    pictureBox.Image.Save(path, format);
                    MessageBox.Show("Pilt salvestatud!", "Valmis",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}