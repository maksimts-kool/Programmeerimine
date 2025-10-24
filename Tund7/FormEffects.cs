using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Tund7
{
    public partial class FormEffects : Form
    {
        public event Action<string> EffectSelected;

        private Button btnGrayscale;
        private Button btnBorder;
        private Button btnClose;

        public FormEffects()
        {
            InitUI();
        }
        private Image CreateRoundedGradientImage(Color startColor, Color endColor, int width, int height, int borderRadius)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius * 2, borderRadius * 2, 180, 90); // Top-left
                path.AddArc(width - borderRadius * 2, 0, borderRadius * 2, borderRadius * 2, 270, 90); // Top-right
                path.AddArc(width - borderRadius * 2, height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90); // Bottom-right
                path.AddArc(0, height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90); // Bottom-left
                path.CloseAllFigures();

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(width, height),
                    startColor,
                    endColor))
                {
                    g.FillPath(brush, path);
                }
            }
            return bmp;
        }

        private Button SetupStyledButton(string text, Image backgroundImage, Color foreColor, int width, int height)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackgroundImage = backgroundImage,
                BackgroundImageLayout = ImageLayout.Stretch,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = foreColor,
                Margin = new Padding(5)
            };
            return btn;
        }

        private void InitUI()
        {
            this.Text = "Efektid";
            this.ClientSize = new Size(380, 80);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            int buttonWidth = 100;
            int buttonHeight = 40;
            int borderRadius = 15;
            Color textColor = Color.White;

            btnGrayscale = SetupStyledButton("Must-valge",
                                             CreateRoundedGradientImage(Color.DarkSlateGray, Color.Gray, buttonWidth, buttonHeight, borderRadius),
                                             textColor, buttonWidth, buttonHeight);
            btnGrayscale.Click += (s, e) => { EffectSelected("grayscale"); };

            btnBorder = SetupStyledButton("Lisa ääris",
                                          CreateRoundedGradientImage(Color.DarkGoldenrod, Color.Goldenrod, buttonWidth, buttonHeight, borderRadius),
                                          textColor, buttonWidth, buttonHeight);
            btnBorder.Click += (s, e) => { EffectSelected("border"); };

            btnClose = SetupStyledButton("Valmis",
                                        CreateRoundedGradientImage(Color.Firebrick, Color.IndianRed, buttonWidth, buttonHeight, borderRadius),
                                        textColor, buttonWidth, buttonHeight);
            btnClose.Click += (s, e) => this.Close();

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                AutoSize = true,
                WrapContents = false
            };

            panel.Controls.Add(btnGrayscale);
            panel.Controls.Add(btnBorder);
            panel.Controls.Add(btnClose);

            this.Controls.Add(panel);
        }
    }
}