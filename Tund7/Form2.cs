

namespace Tund7
{
    public partial class Form2 : Form
    {
        enum Difficulty { Lihtne, Keskmine, Raske }

        Difficulty currentDifficulty = Difficulty.Keskmine;
        Random random = new Random();
        char[] operators = new char[4];

        int sectionCount = 1;
        int currentSection = 1;
        int timeLeft;
        bool quizRunning = false;

        Label[] leftLabels, rightLabels, signLabels, equalsLabels;
        NumericUpDown[] resultInputs;
        Button startButton, nextButton;
        ProgressBar progressBar;
        System.Windows.Forms.Timer timer;
        MenuStrip menuStrip;
        ToolStripMenuItem difficultyMenu, easyItem, mediumItem, hardItem, pauseMenuItem;
        ComboBox sectionBox;
        Label sectionTitle;
        bool isPaused = false;

        List<(int Section, List<(int left, int right, char op, int answer, int correct)> Items)> sections = new();

        public Form2()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            Width = 460;
            Height = 520;
            Text = "Matemaatika viktoriin";
            BackColor = Color.FromArgb(248, 249, 252);
            Font = new Font("Segoe UI", 13F);

            menuStrip = new MenuStrip();
            // Menüü "Mäng"
            var gameMenu = new ToolStripMenuItem("Mäng");
            pauseMenuItem = new ToolStripMenuItem("Paus");
            pauseMenuItem.Enabled = false;
            pauseMenuItem.Click += (_, _) => TogglePauseMenu(pauseMenuItem);
            gameMenu.DropDownItems.Add(pauseMenuItem);
            menuStrip.Items.Add(gameMenu);
            difficultyMenu = new ToolStripMenuItem("Raskusaste");
            easyItem = new ToolStripMenuItem("Lihtne");
            mediumItem = new ToolStripMenuItem("Keskmine");
            hardItem = new ToolStripMenuItem("Raske");
            easyItem.Click += (_, _) => SetDifficulty(Difficulty.Lihtne);
            mediumItem.Click += (_, _) => SetDifficulty(Difficulty.Keskmine);
            hardItem.Click += (_, _) => SetDifficulty(Difficulty.Raske);
            difficultyMenu.DropDownItems.AddRange(new ToolStripItem[] { easyItem, mediumItem, hardItem });
            menuStrip.Items.Add(difficultyMenu);
            Controls.Add(menuStrip);

            // Ülesannete elemendid
            leftLabels = new Label[4];
            rightLabels = new Label[4];
            signLabels = new Label[4];
            equalsLabels = new Label[4];
            resultInputs = new NumericUpDown[4];

            int top = 70;
            for (int i = 0; i < 4; i++)
            {
                leftLabels[i] = MakeLabel(50, top, "?");
                signLabels[i] = MakeLabel(115, top, "+");
                rightLabels[i] = MakeLabel(160, top, "?");
                equalsLabels[i] = MakeLabel(205, top, "=");
                resultInputs[i] = MakeNumeric(240, top);
                top += 55;

                Controls.Add(leftLabels[i]);
                Controls.Add(signLabels[i]);
                Controls.Add(rightLabels[i]);
                Controls.Add(equalsLabels[i]);
                Controls.Add(resultInputs[i]);
            }

            // Sektsioonide arv
            var lblSections = new Label
            {
                Text = "Sektsioone:",
                Location = new Point(50, 300),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F)
            };
            sectionBox = new ComboBox
            {
                Location = new Point(160, 295),
                Width = 80,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 5; i++)
                sectionBox.Items.Add(i);
            sectionBox.SelectedIndex = 0;

            // Nupud
            startButton = new Button
            {
                Text = "Alusta viktoriini",
                Size = new Size(160, 40),
                Location = new Point(250, 292),
                BackColor = Color.FromArgb(64, 156, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += StartButton_Click;

            nextButton = new Button
            {
                Text = "Järgmine ➜",
                Size = new Size(160, 40),
                Location = new Point(250, 340),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            nextButton.Click += NextButton_Click;
            nextButton.EnabledChanged += (_, _) =>
            {
                nextButton.BackColor = nextButton.Enabled ? Color.LightGreen : Color.LightGray;
            };

            progressBar = new ProgressBar
            {
                Location = new Point(50, 400),
                Size = new Size(320, 25),
                Minimum = 0,
                Maximum = 30,
                Value = 30,
                ForeColor = Color.MediumSeaGreen
            };

            // Sektsioon X / Y
            sectionTitle = new Label
            {
                Text = "Sektsioon 0 / 0",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(50, 330),
                AutoSize = true
            };
            Controls.Add(sectionTitle);

            Controls.Add(lblSections);
            Controls.Add(sectionBox);
            Controls.Add(startButton);
            Controls.Add(nextButton);
            Controls.Add(progressBar);

            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;

            SetAnswerInputsEnabled(false);
            SetDifficulty(Difficulty.Keskmine);
        }

        private Label MakeLabel(int x, int y, string text) => new()
        {
            Text = text,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            Location = new Point(x, y),
            AutoSize = true
        };

        private NumericUpDown MakeNumeric(int x, int y) => new()
        {
            Location = new Point(x, y),
            Size = new Size(100, 30),
            Font = new Font("Segoe UI", 14F),
            TextAlign = HorizontalAlignment.Center,
            Maximum = 1000,
            BackColor = Color.White
        };

        private void SetDifficulty(Difficulty level)
        {
            currentDifficulty = level;
            easyItem.Checked = level == Difficulty.Lihtne;
            mediumItem.Checked = level == Difficulty.Keskmine;
            hardItem.Checked = level == Difficulty.Raske;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            sectionCount = (int)sectionBox.SelectedItem;
            currentSection = 1;
            sections.Clear();

            startButton.Enabled = false;
            startButton.BackColor = Color.LightGray;
            UpdateSectionTitle();
            sectionBox.Enabled = false;
            difficultyMenu.Enabled = false;
            nextButton.Enabled = true;
            quizRunning = true;
            SetAnswerInputsEnabled(true);
            pauseMenuItem.Enabled = true;
            pauseMenuItem.Text = "Paus";

            switch (currentDifficulty)
            {
                case Difficulty.Lihtne:
                    timeLeft = sectionCount * 20;
                    operators = new[] { '+', '-', '+', '-' };
                    break;
                case Difficulty.Keskmine:
                    timeLeft = sectionCount * 30;
                    operators = new[] { '+', '-', '×', '÷' };
                    break;
                case Difficulty.Raske:
                    timeLeft = sectionCount * 40;
                    operators = new[] { '÷', '×', '÷', '×' };
                    break;
            }

            progressBar.Maximum = timeLeft;
            progressBar.Value = progressBar.Maximum;
            progressBar.ForeColor = Color.MediumSeaGreen;
            progressBar.Refresh();
            DrawProgressBarText(timeLeft);

            GenerateAllSections();
            LoadSection(currentSection);
            timer.Start();
        }

        private void UpdateSectionTitle()
        {
            sectionTitle.Text = $"Sektsioon {currentSection} / {sectionCount}";
        }

        private void GenerateAllSections()
        {
            sections.Clear();

            for (int s = 1; s <= sectionCount; s++)
            {
                var items = new List<(int left, int right, char op, int answer, int correct)>();

                for (int i = 0; i < 4; i++)
                {
                    char op = operators[i % operators.Length];
                    int left = 0, right = 0;

                    switch (op)
                    {
                        case '+': left = random.Next(0, 51); right = random.Next(0, 51); break;
                        case '-': left = random.Next(1, 101); right = random.Next(1, left); break;
                        case '×': left = random.Next(2, 11); right = random.Next(2, 11); break;
                        case '÷': right = random.Next(2, 11); int q = random.Next(2, 11); left = right * q; break;
                    }

                    items.Add((left, right, op, 0, GetCorrectAnswer(op, left, right)));
                }

                sections.Add((s, items));
            }
        }

        private void LoadSection(int index)
        {
            if (index < 1 || index > sections.Count)
                return;

            var res = sections[index - 1];
            for (int i = 0; i < 4; i++)
            {
                var ex = res.Items[i];
                signLabels[i].Text = ex.op.ToString();
                leftLabels[i].Text = ex.left.ToString();
                rightLabels[i].Text = ex.right.ToString();
                resultInputs[i].Value = ex.answer;
                resultInputs[i].BackColor = Color.White;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (isPaused)
                return;

            SaveCurrentSection();
            currentSection++;

            if (currentSection <= sectionCount)
            {
                LoadSection(currentSection);
                UpdateSectionTitle();
            }
            else
            {
                EndQuiz();
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!quizRunning)
                return;

            if (!isPaused)
            {
                timer.Stop();
                isPaused = true;
                nextButton.Enabled = false;
                SetAnswerInputsEnabled(false);

                progressBar.ForeColor = Color.Gray;
                DrawProgressBarText(timeLeft);
            }
            else
            {
                timer.Start();
                isPaused = false;
                nextButton.Enabled = true;
                SetAnswerInputsEnabled(true);

                if (timeLeft < progressBar.Maximum * 0.25)
                    progressBar.ForeColor = Color.IndianRed;
                else if (timeLeft < progressBar.Maximum * 0.5)
                    progressBar.ForeColor = Color.Gold;
                else
                    progressBar.ForeColor = Color.MediumSeaGreen;
            }
        }

        private void SaveCurrentSection()
        {
            if (currentSection < 1 || currentSection > sections.Count)
                return;

            var res = sections[currentSection - 1];
            for (int i = 0; i < 4; i++)
            {
                var ex = res.Items[i];
                ex.answer = (int)resultInputs[i].Value;
                res.Items[i] = ex;
            }
            sections[currentSection - 1] = res;
        }

        private void EndQuiz()
        {
            quizRunning = false;
            if (timer.Enabled) timer.Stop();

            startButton.Enabled = true;
            startButton.BackColor = Color.FromArgb(64, 156, 255);
            nextButton.Enabled = false;
            sectionBox.Enabled = true;
            difficultyMenu.Enabled = true;
            isPaused = false;

            SetAnswerInputsEnabled(false);
            ShowResults();
        }

        private void ShowResults()
        {
            int totalRight = 0, totalQ = 0;
            var report = new System.Text.StringBuilder();
            report.AppendLine("Tulemused\n");

            foreach (var section in sections)
            {
                report.AppendLine($"=== Sektsioon {section.Section} ===");
                foreach (var ex in section.Items)
                {
                    bool correct = ex.answer == ex.correct;
                    totalQ++;
                    if (correct) totalRight++;

                    string ans = ex.answer == 0 ? "0" : ex.answer.ToString();
                    string status = correct ? "✅ Õige" : "❌ Vale";
                    string line = $"{ex.left} {ex.op} {ex.right} = {ans}";
                    if (!correct)
                        line += $" (õige: {ex.correct})";
                    report.AppendLine($"{line}  {status}");
                }
                report.AppendLine();
            }

            report.AppendLine($"Kokku: {totalRight} / {totalQ} õiget vastust.");
            MessageBox.Show(report.ToString(), "Üksikasjalikud tulemused",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!quizRunning) return;

            if (timeLeft > 0)
            {
                timeLeft--;

                if (timeLeft < progressBar.Maximum * 0.25)
                    progressBar.ForeColor = Color.IndianRed;
                else if (timeLeft < progressBar.Maximum * 0.5)
                    progressBar.ForeColor = Color.Gold;
                else
                    progressBar.ForeColor = Color.MediumSeaGreen;

                progressBar.Value = Math.Max(timeLeft, progressBar.Minimum);
                progressBar.Refresh();
                DrawProgressBarText(timeLeft);
            }
            else
            {
                timer.Stop();
                quizRunning = false;
                progressBar.Value = progressBar.Minimum;
                progressBar.Refresh();
                DrawProgressBarText(0);
                MessageBox.Show("Aeg sai otsa! Mäng lõppes.", "Aeg läbi!");
                EndQuiz();
            }
        }


        private void TogglePauseMenu(ToolStripMenuItem item)
        {
            if (!quizRunning)
                return;

            if (!isPaused)
            {
                timer.Stop();
                isPaused = true;
                item.Text = "Jätka";
                nextButton.Enabled = false;
                SetAnswerInputsEnabled(false);

                progressBar.ForeColor = Color.Gray;
                DrawProgressBarText(timeLeft);
            }
            else
            {
                timer.Start();
                isPaused = false;
                item.Text = "Paus";
                nextButton.Enabled = true;
                SetAnswerInputsEnabled(true);

                if (timeLeft < progressBar.Maximum * 0.25)
                    progressBar.ForeColor = Color.IndianRed;
                else if (timeLeft < progressBar.Maximum * 0.5)
                    progressBar.ForeColor = Color.Gold;
                else
                    progressBar.ForeColor = Color.MediumSeaGreen;
            }
        }
        private void DrawProgressBarText(int seconds)
        {
            using (Graphics g = progressBar.CreateGraphics())
            {
                string text = $"{seconds} s";
                Font font = new Font("Segoe UI", 10, FontStyle.Bold);
                SizeF ts = g.MeasureString(text, font);

                Rectangle rect = new Rectangle(0, 0, progressBar.Width, progressBar.Height);
                ProgressBarRenderer.DrawHorizontalBar(g, rect);

                int fillWidth = (int)((float)progressBar.Value / progressBar.Maximum * (progressBar.Width - 4));
                Color fillColor = progressBar.ForeColor;
                using (Brush b = new SolidBrush(fillColor))
                    g.FillRectangle(b, 2, 2, fillWidth, progressBar.Height - 4);

                float x = (progressBar.Width - ts.Width) / 2;
                float y = (progressBar.Height - ts.Height) / 2;
                g.DrawString(text, font, Brushes.Black, x, y);
            }
        }

        private int GetCorrectAnswer(char op, int left, int right) =>
            op switch
            {
                '+' => left + right,
                '-' => left - right,
                '×' => left * right,
                '÷' => left / right,
                _ => 0
            };

        private void SetAnswerInputsEnabled(bool v)
        {
            foreach (var i in resultInputs)
                i.Enabled = v;
        }
    }
}