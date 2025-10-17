using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tund7
{
    public partial class FormEffects : Form
    {
        public event Action<string> EffectSelected;

        public FormEffects()
        {
            InitUI();
        }

        private Button btnGrayscale;
        private Button btnBorder;
        private Button btnClose;

        private void InitUI()
        {
            this.Text = "Efektid";
            this.ClientSize = new Size(250, 120);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            btnGrayscale = new Button { Text = "Must-valge", AutoSize = true };
            btnBorder = new Button { Text = "Lisa ääris", AutoSize = true };
            btnClose = new Button { Text = "Valmis", AutoSize = true };

            btnGrayscale.Click += (s, e) => EffectSelected?.Invoke("grayscale");
            btnBorder.Click += (s, e) => EffectSelected?.Invoke("border");
            btnClose.Click += (s, e) => this.Close();

            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                AutoSize = true
            };

            panel.Controls.Add(btnGrayscale);
            panel.Controls.Add(btnBorder);
            panel.Controls.Add(btnClose);

            this.Controls.Add(panel);
        }
    }
}