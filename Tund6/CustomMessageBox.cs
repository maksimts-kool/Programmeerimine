using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tund6
{
    public class CustomForm : System.Windows.Forms.Form
    {
        private readonly Label message = new Label();
        private readonly Button[] btn = new Button[4];
        private readonly string[] texts = new string[4];
        public CustomForm()
        {

        }
        public CustomForm(string title, string body, string button1, string button2, string button3, string button4)
        {
            texts[0] = button1;
            texts[1] = button2;
            texts[2] = button3;
            texts[3] = button4;
            ClientSize = new System.Drawing.Size(490, 150);
            Text = title;
            int y = 111;
            for (int i = 0; i < 4; i++)
            {
                btn[i] = new Button
                {
                    Location = new System.Drawing.Point(y, 112),
                    Size = new System.Drawing.Size(75, 23),
                    Text = texts[i],
                    BackColor = Control.DefaultBackColor
                };
                btn[i].Click += CustomForm_Click;
                Controls.Add(btn[i]);
                y += 100;
            }
            message.Location = new System.Drawing.Point(10, 10);
            message.Text = body;
            message.Font = Control.DefaultFont;
            message.AutoSize = true;
            BackColor = Color.White;
            ShowIcon = false;
            Controls.Add(message);

        }
        private void CustomForm_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            _ = MessageBox.Show("Oli valitud " + btn.Text);
        }
    }
}
