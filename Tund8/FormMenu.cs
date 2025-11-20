using System;
using System.Windows.Forms;

namespace Tund8
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            Text = "Pood — Menüü";
            Width = 400;
            Height = 250;

            Label lbl = new Label()
            {
                Text = "Vali vaade:",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnOwner = new Button()
            {
                Text = "Omanik (Admin)",
                Dock = DockStyle.Top,
                Height = 50
            };

            Button btnCustomer = new Button()
            {
                Text = "Klient (Pood)",
                Dock = DockStyle.Top,
                Height = 50
            };

            btnOwner.Click += (s, e) =>
            {
                Hide();
                Form1 frm = new Form1();
                frm.FormClosed += (s2, e2) => Show();
                frm.Show();
            };

            btnCustomer.Click += (s, e) =>
            {
                Hide();
                FormCustomer frmCust = new FormCustomer();
                frmCust.FormClosed += (s2, e2) => Show();
                frmCust.Show();
            };

            Controls.Add(btnCustomer);
            Controls.Add(btnOwner);
            Controls.Add(lbl);
        }
    }
}