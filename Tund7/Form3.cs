using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Tund7
{
    public partial class Form3 : Form
    {
        TableLayoutPanel tableLayout;
        PictureBox firstClicked = null;
        PictureBox secondClicked = null;
        bool inputLocked = false;
        Random random = new Random();
        System.Windows.Forms.Timer revealTimer, colorTimer;
        MenuStrip menuStrip;
        ToolStripMenuItem gameMenu, addIconsItem;
        string iconDir = @"../../../ikoonid";
        Label timeLabel;
        System.Windows.Forms.Timer gameTimer;
        int timeLeft = 60;
        int successStreak = 0;
        int failStreak = 0;

        public Form3()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            Text = "Kooste mäng";
            Width = 550;
            Height = 620;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            tableLayout = new TableLayoutPanel
            {
                RowCount = 4,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.CornflowerBlue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset
            };

            for (int i = 0; i < 4; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PictureBox pic = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.CornflowerBlue,
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        Margin = new Padding(5),
                        Tag = null
                    };
                    pic.Click += Picture_Click;
                    tableLayout.Controls.Add(pic, j, i);
                }
            }

            menuStrip = new MenuStrip();

            gameMenu = new ToolStripMenuItem("Mäng");
            addIconsItem = new ToolStripMenuItem("Lisa ikoone...");
            addIconsItem.Click += AddIconsItem_Click;
            gameMenu.DropDownItems.Add(addIconsItem);
            menuStrip.Items.Add(gameMenu);

            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;

            Controls.Add(tableLayout);

            timeLabel = new Label
            {
                Text = "Aeg: 60 s",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.LightSteelBlue,
                Dock = DockStyle.Bottom,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(timeLabel);

            // Игровой таймер
            gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            tableLayout.Dock = DockStyle.Fill;
            tableLayout.Padding = new Padding(0, menuStrip.Height, 0, 0);

            revealTimer = new System.Windows.Forms.Timer { Interval = 750 };
            revealTimer.Tick += RevealTimer_Tick;

            colorTimer = new System.Windows.Forms.Timer { Interval = 500 };
            colorTimer.Tick += ColorTimer_Tick;

            AssignIconsToSquares();
        }

        private void AssignIconsToSquares()
        {
            var files = Directory.GetFiles(iconDir, "*.png").ToList();

            if (files.Count < 8)
            {
                MessageBox.Show("Kaust 'ikoonid' peab sisaldama vähemalt 8 PNG-faili!");
                Close();
                return;
            }

            var iconFiles = files.Take(8).ToList();
            iconFiles.AddRange(iconFiles);
            iconFiles = iconFiles.OrderBy(_ => random.Next()).ToList();

            int index = 0;
            foreach (Control c in tableLayout.Controls)
            {
                if (c is PictureBox box)
                {
                    box.Image = null;
                    box.BackColor = Color.CornflowerBlue;
                    box.Tag = iconFiles[index];
                    box.Enabled = true;
                    index++;
                }
            }

            firstClicked = null;
            secondClicked = null;
            inputLocked = false;
            successStreak = 0;
            failStreak = 0;

            // ⚙️ Сбрасываем таймер
            timeLeft = 60;
            timeLabel.Text = $"Aeg: {timeLeft} s";
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            if (inputLocked)
                return;

            PictureBox clicked = sender as PictureBox;
            if (clicked == null)
                return;

            if (clicked.Image != null)
                return;

            clicked.BackColor = Color.Yellow;

            using (var original = Image.FromFile(clicked.Tag.ToString()))
            {
                int newWidth = (int)(clicked.Width * 0.8);
                int newHeight = (int)(clicked.Height * 0.8);
                Bitmap scaled = new Bitmap(original, new Size(newWidth, newHeight));
                clicked.Image = scaled;
            }

            if (firstClicked == null)
            {
                firstClicked = clicked;
                return;
            }

            if (secondClicked == null)
            {
                secondClicked = clicked;
                inputLocked = true;
                colorTimer.Start();
            }
        }

        private void AddIconsItem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Vali pildifailid (.png)";
                dlg.Filter = "PNG failid (*.png)|*.png";
                dlg.Multiselect = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in dlg.FileNames)
                    {
                        try
                        {
                            string dest = Path.Combine(iconDir, Path.GetFileName(file));
                            File.Copy(file, dest, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Faili ei saanud kopeerida:\n{file}\n{ex.Message}",
                                "Viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    MessageBox.Show("Ikoonid on lisatud!\nUued pildid kasutatakse järgmises mängus.",
                        "Valmis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                timeLabel.Text = $"Aeg: {timeLeft} s";
            }
            else
            {
                gameTimer.Stop();
                inputLocked = true;
                DialogResult res = MessageBox.Show(
                    "Aeg läbi!\nKas soovid uut mängu?",
                    "Mäng läbi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    timeLeft = 60;
                    timeLabel.Text = $"Aeg: {timeLeft} s";
                    AssignIconsToSquares();
                    gameTimer.Start();
                }
                else
                {
                    Close();
                }
            }
        }

        private void RevealTimer_Tick(object sender, EventArgs e)
        {
            revealTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            firstClicked.Text = string.Empty;
            secondClicked.ForeColor = secondClicked.BackColor;
            secondClicked.Text = string.Empty;

            firstClicked = null;
            secondClicked = null;
        }

        private void ColorTimer_Tick(object sender, EventArgs e)
        {
            colorTimer.Stop();

            if (firstClicked == null || secondClicked == null)
                return;

            bool match = Path.GetFileName(firstClicked.Tag.ToString()) ==
                         Path.GetFileName(secondClicked.Tag.ToString());

            if (match)
            {
                firstClicked.BackColor = Color.LightGreen;
                secondClicked.BackColor = Color.LightGreen;
                firstClicked.Enabled = false;
                secondClicked.Enabled = false;

                successStreak++;
                failStreak = 0;

                if (successStreak >= 2)
                {
                    timeLeft += 4;
                    successStreak = 0;
                    if (timeLeft > 99) timeLeft = 99;
                    timeLabel.Text = $"Aeg: {timeLeft} s (+4)";
                }

                var t = new System.Windows.Forms.Timer { Interval = 500 };
                t.Tick += (_, _) =>
                {
                    t.Stop();

                    firstClicked = null;
                    secondClicked = null;
                    inputLocked = false;
                    CheckForWinner();
                };
                t.Start();
            }
            else
            {
                firstClicked.BackColor = Color.LightCoral;
                secondClicked.BackColor = Color.LightCoral;
                failStreak++;
                successStreak = 0;

                if (failStreak >= 2)
                {
                    timeLeft -= 2;
                    if (timeLeft < 0) timeLeft = 0;
                    failStreak = 0;
                    timeLabel.Text = $"Aeg: {timeLeft} s (-2)";
                }

                var t = new System.Windows.Forms.Timer { Interval = 500 };
                t.Tick += (_, _) =>
                {
                    t.Stop();

                    firstClicked.Image = null;
                    secondClicked.Image = null;
                    firstClicked.BackColor = Color.CornflowerBlue;
                    secondClicked.BackColor = Color.CornflowerBlue;
                    firstClicked = null;
                    secondClicked = null;

                    inputLocked = false;
                };
                t.Start();
            }
        }
        private void CheckForWinner()
        {
            if (inputLocked)
                return;

            // проверяем, остались ли ещё активные карточки
            foreach (Control c in tableLayout.Controls)
            {
                if (c is PictureBox box && box.Enabled)
                    return; // ещё есть незакрытые пары
            }

            // 🏆 Победа — останавливаем игровой таймер
            gameTimer.Stop();

            DialogResult res = MessageBox.Show(
                "🎉 Suurepärane! Kõik paarid leitud!\nKas soovid uut mängu?",
                "Võit!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (res == DialogResult.Yes)
            {
                // сбрасываем время и перезапускаем игру
                timeLeft = 60;
                timeLabel.Text = $"Aeg: {timeLeft} s";
                AssignIconsToSquares();
                gameTimer.Start(); // снова включаем таймер
            }
            else
            {
                Close();
            }
        }
    }
}