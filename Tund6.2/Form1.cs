using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tund6._2
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private Button btnNextImage;
        private CheckBox chbMirror, chbFixed, chbShowTab, chbMenu;
        private RadioButton rbLight, rbDark;
        private ListBox listColors;
        private TabControl tabControl;
        private MenuStrip menuStrip;
        private Random rnd = new Random();
        private string[] images = { "img1.png", "img2.png", "img3.png", "img4.png" };
        private Dictionary<string, Color> colorMap;
        private bool customColorMode = false;
        private Label lblTabs;

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            Text = "Windows Forms paneel";
            Size = new Size(500, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            int centerX = 50;
            int currentY = 50;

            // === Menüü ===
            menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("Fail");
            var itemInfo = new ToolStripMenuItem("Teave", null, (s, e) =>
                MessageBox.Show("See on Windows Forms paneeli loomine", "Teave"));
            var itemExit = new ToolStripMenuItem("Välju", null, (s, e) => Close());
            fileMenu.DropDownItems.AddRange(new[] { itemInfo, itemExit });
            menuStrip.Items.Add(fileMenu);
            Controls.Add(menuStrip);

            // === Pilt ja nupp ===
            var lblImage = new Label
            {
                Text = "Pilt",
                Location = new Point(centerX, currentY),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };
            Controls.Add(lblImage);
            currentY += 35;

            pictureBox = new PictureBox
            {
                Location = new Point(centerX, currentY),
                Size = new Size(380, 220),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pictureBox);
            currentY += 230;

            btnNextImage = new Button
            {
                Text = "Järgmine pilt",
                Location = new Point(centerX + 90, currentY),
                BackColor = Color.LightGray,
                AutoSize = true
            };
            btnNextImage.Click += (s, e) => ShowRandomImage();
            Controls.Add(btnNextImage);
            currentY += 50;

            AddSeparator(currentY);
            currentY += 20;

            // === Valikud ===
            var lblOptions = new Label
            {
                Text = "Valikud",
                Location = new Point(centerX, currentY),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };
            Controls.Add(lblOptions);
            currentY += 35;

            chbMirror = new CheckBox
            {
                Text = "Peegelda pilti",
                Location = new Point(centerX, currentY),
                AutoSize = true
            };
            Controls.Add(chbMirror);
            currentY += 30;

            chbMirror.Click += (s, e) =>
            {
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    pictureBox.Refresh();
                }
            };

            chbFixed = new CheckBox
            {
                Text = "Fikseeritud aken",
                Location = new Point(centerX, currentY),
                AutoSize = true
            };
            chbFixed.CheckedChanged += CheckOptions;
            Controls.Add(chbFixed);
            currentY += 30;

            chbShowTab = new CheckBox
            {
                Text = "Näita kaarte",
                Location = new Point(centerX, currentY),
                Checked = true,
                AutoSize = true
            };
            chbShowTab.CheckedChanged += CheckOptions;
            Controls.Add(chbShowTab);
            currentY += 30;

            chbMenu = new CheckBox
            {
                Text = "Menüü aktiivne",
                Location = new Point(centerX, currentY),
                Checked = true,
                AutoSize = true
            };
            chbMenu.CheckedChanged += CheckOptions;
            Controls.Add(chbMenu);
            currentY += 50;

            AddSeparator(currentY);
            currentY += 20;

            // === Teema ===
            var lblTheme = new Label
            {
                Text = "Teema",
                Location = new Point(centerX, currentY),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };
            Controls.Add(lblTheme);
            currentY += 35;

            rbLight = new RadioButton
            {
                Text = "Hele teema",
                Location = new Point(centerX, currentY),
                Checked = true,
                AutoSize = true
            };
            rbLight.CheckedChanged += ThemeChanged;
            Controls.Add(rbLight);
            currentY += 30;

            rbDark = new RadioButton
            {
                Text = "Tume teema",
                Location = new Point(centerX, currentY),
                AutoSize = true
            };
            rbDark.CheckedChanged += ThemeChanged;
            Controls.Add(rbDark);
            currentY += 50;

            AddSeparator(currentY);
            currentY += 20;

            // === Taustavärv ===
            var lblColors = new Label
            {
                Text = "Taustavärvi valik",
                Location = new Point(centerX, currentY),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };
            Controls.Add(lblColors);
            currentY += 35;

            listColors = new ListBox
            {
                Location = new Point(centerX, currentY),
                Size = new Size(200, 90),
                Font = new Font("Segoe UI", 9)
            };
            Controls.Add(listColors);

            colorMap = new Dictionary<string, Color>
            {
                { "Hall", Color.LightGray },
                { "Sinine", Color.SkyBlue },
                { "Roheline", Color.LightGreen },
                { "Roosa", Color.LightPink }
            };
            foreach (var c in colorMap.Keys)
                listColors.Items.Add(c);

            listColors.SelectedIndexChanged += (s, e) =>
            {
                if (listColors.SelectedIndex == -1)
                    return;

                string name = listColors.SelectedItem.ToString();
                Color selected = colorMap[name];
                BackColor = selected;

                double brightness = selected.GetBrightness();
                ForeColor = brightness > 0.6 ? Color.Black : Color.WhiteSmoke;

                customColorMode = true;
                rbLight.Enabled = false;
                rbDark.Enabled = false;
                ApplyThemeToAllControls(this, BackColor, ForeColor);
            };
            currentY += 100;

            var btnResetColor = new Button
            {
                Text = "Lähtesta värv",
                Location = new Point(centerX, currentY),
                AutoSize = true
            };
            btnResetColor.Click += (s, e) =>
            {
                listColors.ClearSelected();
                customColorMode = false;

                Color back = rbLight.Checked ? Color.WhiteSmoke : Color.FromArgb(40, 42, 54);
                Color fore = rbLight.Checked ? Color.Black : Color.WhiteSmoke;

                BackColor = back;
                ForeColor = fore;

                rbLight.Enabled = true;
                rbDark.Enabled = true;
                ApplyThemeToAllControls(this, back, fore);
            };
            Controls.Add(btnResetColor);
            currentY += 50;

            AddSeparator(currentY);
            currentY += 20;

            // === Kaardid ===
            lblTabs = new Label
            {
                Text = "Kaardid",
                Location = new Point(centerX, currentY),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };
            Controls.Add(lblTabs);
            currentY += 35;

            tabControl = new TabControl
            {
                Location = new Point(centerX, currentY),
                Size = new Size(380, 140)
            };

            var tab1 = new TabPage("Peamine");
            tab1.Controls.Add(new Label
            {
                Text = "Sinu märkmed",
                Location = new Point(15, 20),
                AutoSize = true
            });
            tabControl.TabPages.Add(tab1);

            var tabAdd = new TabPage("+");
            tabControl.TabPages.Add(tabAdd);
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            tabControl.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < tabControl.TabPages.Count; i++)
                    {
                        if (tabControl.GetTabRect(i).Contains(e.Location))
                        {
                            var page = tabControl.TabPages[i];
                            if (page.Text != "+")
                            {
                                if (MessageBox.Show(
                                    $"Kas kustutada kaart '{page.Text}'?",
                                    "Kinnitus",
                                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    tabControl.TabPages.Remove(page);
                                }
                            }
                        }
                    }
                }
            };
            Controls.Add(tabControl);
        }

        private void AddSeparator(int y)
        {
            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 2,
                Width = 400,
                Location = new Point(50, y)
            };
            Controls.Add(separator);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Text == "+")
            {
                var newPage = new TabPage($"Kaart {tabControl.TabCount}");
                newPage.BackColor = BackColor;
                newPage.ForeColor = ForeColor;

                var textBox = new TextBox
                {
                    Multiline = true,
                    Size = new Size(250, 100),
                    Location = new Point(20, 20),
                    Text = "Kirjuta siia midagi..."
                };
                newPage.Controls.Add(textBox);

                tabControl.TabPages.Insert(tabControl.TabCount - 1, newPage);
                tabControl.SelectedTab = newPage;
            }
        }

        private void ShowRandomImage()
        {
            string path = "../../../Pildid/" + images[rnd.Next(images.Length)];
            try
            {
                pictureBox.Image = Image.FromFile(path);
                if (chbMirror != null)
                {
                    chbMirror.Checked = false;
                }
                pictureBox.Refresh();
            }
            catch
            {
                MessageBox.Show($"Faili {path} ei leitud.", "Viga");
            }
        }

        private void CheckOptions(object sender, EventArgs e)
        {
            FormBorderStyle =
                chbFixed.Checked ? FormBorderStyle.FixedDialog : FormBorderStyle.Sizable;
            bool showTabs = chbShowTab.Checked;
            tabControl.Visible = showTabs;
            lblTabs.Visible = showTabs;
            menuStrip.Enabled = chbMenu.Checked;
        }

        private void ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme(rbLight.Checked);
        }

        private void ApplyTheme(bool light)
        {
            Color back = customColorMode ? BackColor : (light ? Color.WhiteSmoke : Color.FromArgb(40, 42, 54));
            Color fore = light ? Color.Black : Color.WhiteSmoke;

            BackColor = back;
            ForeColor = fore;
            ApplyThemeToAllControls(this, back, fore);
        }

        // Rekursiivne meetod kõikide elementide värvimiseks
        private void ApplyThemeToAllControls(Control parent, Color back, Color fore)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is PictureBox)
                    continue;

                if (ctrl is TabControl tc)
                {
                    tc.BackColor = back;
                    tc.ForeColor = fore;

                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = back;
                        tp.ForeColor = fore;
                        ApplyThemeToAllControls(tp, back, fore);
                    }
                }
                else
                {
                    ctrl.BackColor = back;
                    ctrl.ForeColor = fore;
                    if (ctrl.HasChildren)
                        ApplyThemeToAllControls(ctrl, back, fore);
                }
            }
        }
    }
}