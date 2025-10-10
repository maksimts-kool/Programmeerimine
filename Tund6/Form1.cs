using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Tund6
{
    public partial class Form1 : Form
    {
        private readonly TreeView tree;
        private readonly Button btn;
        private readonly Label lbl;
        private readonly PictureBox pic;
        private CheckBox c_btn1, c_btn2;
        private RadioButton r_btn1, r_btn2;
        private TabControl tabC;
        private TabPage tabP1, tabP2, tabP3;
        private ListBox lb;
        private bool t = true;
        public Form1()
        {

            Height = 600;
            Width = 800;
            Text = "Vorm elementidega";
            tree = new TreeView
            {
                Dock = DockStyle.Left
            };
            tree.AfterSelect += Tree_AfterSelect;
            TreeNode tn = new TreeNode("Elemendid");
            _ = tn.Nodes.Add(new TreeNode("Nupp"));
            _ = tn.Nodes.Add(new TreeNode("Silt"));
            _ = tn.Nodes.Add(new TreeNode("Pilt"));
            _ = tn.Nodes.Add(new TreeNode("Märkeruut"));
            _ = tn.Nodes.Add(new TreeNode("Radionupp"));
            _ = tn.Nodes.Add(new TreeNode("Tekstkast-TextBox"));
            _ = tn.Nodes.Add(new TreeNode("Kaart"));
            _ = tn.Nodes.Add(new TreeNode("MessageBox"));
            _ = tn.Nodes.Add(new TreeNode("ListBox"));
            _ = tn.Nodes.Add(new TreeNode("DataGridView"));
            _ = tn.Nodes.Add(new TreeNode("MainMenu"));
            //nupp
            btn = new Button
            {
                Text = "Vajuta siia",
                Location = new Point(150, 30),
                Height = 30,
                Width = 100
            };
            btn.Click += Btn_Click;
            //pealkiri
            lbl = new Label
            {
                Text = "Elementide loomine c# abil",
                Font = new Font("Arial", 24),
                Size = new Size(400, 30),
                Location = new Point(150, 0)
            };
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;

            pic = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(150, 60),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile(@"..\..\Images\close_box_red.png")
            };
            pic.DoubleClick += Pic_DoubleClick;



            _ = tree.Nodes.Add(tn);
            Controls.Add(tree);
        }
        private readonly int click = 0;
        private void Pic_DoubleClick(object sender, EventArgs e)
        {   //Double_Click -> carusel (3-4 images) 1-2-3-4-1-2-3-4-... 
            string[] images = { "esimene.jpg", "teine.jpg", "kolmas.jpg" };
            Random rand = new Random();
            int index = rand.Next(images.Length);
            string fail = images[index];
            pic.Image = Image.FromFile(@"..\..\Images\" + fail);
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.BackColor = Color.Transparent;
            Form1 Form = new Form1();
            Form.Show();
            Hide();


        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.BackColor = Color.FromArgb(200, 10, 20);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            using (SoundPlayer soundPlayer = new SoundPlayer(@"..\..\Sound\mixkit.wav")) { soundPlayer.Play(); }

            CustomForm customMessage = new CustomForm(
           "Minu oma teade",
           "Tee oma valik",
           "Kivi!",
           "Käärid!",
           "Paber!",
           "Vali ise!"
           )
            {
                StartPosition = FormStartPosition.CenterParent
            };
            _ = customMessage.ShowDialog();
        }
        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Nupp")
            {
                Controls.Add(btn);
            }
            else if (e.Node.Text == "Silt")
            {
                Controls.Add(lbl);
            }
            else if (e.Node.Text == "Pilt")
            {
                Controls.Add(pic);
            }
            else if (e.Node.Text == "Märkeruut")
            {
                c_btn1 = new CheckBox
                {
                    Text = "suur/väike aken",
                    Location = new Point(310, 420)
                };
                c_btn1.CheckedChanged += (s, ev) =>
                {
                    Size = c_btn1.Checked ? new Size(1000, 800) : new Size(800, 600);
                };

                c_btn2 = new CheckBox
                {
                    Text = "näita pilte",
                    Location = new Point(310, 450),
                    Checked = true
                };
                c_btn2.CheckedChanged += (s, ev) =>
                {
                    pic.Visible = c_btn2.Checked;
                };

                CheckBox c_btn3 = new CheckBox
                {
                    Text = "suur tekst",
                    Location = new Point(310, 480)
                };
                c_btn3.CheckedChanged += (s, ev) =>
                {
                    lbl.Font = c_btn3.Checked ? new Font("Arial", 28, FontStyle.Bold) : new Font("Arial", 14, FontStyle.Regular);
                };

                CheckBox c_btn4 = new CheckBox
                {
                    Text = "mängi heli",
                    Location = new Point(310, 510)
                };
                c_btn4.CheckedChanged += (s, ev) =>
                {
                    if (c_btn4.Checked)
                    {
                        using (SoundPlayer soundPlayer = new System.Media.SoundPlayer(@"..\..\Sound\mixkit.wav"))
                        {
                            soundPlayer.Play();
                        }
                    }
                };

                Controls.AddRange(new Control[] { c_btn1, c_btn2, c_btn3, c_btn4 });
            }
            else if (e.Node.Text == "Radionupp")
            {
                r_btn1 = new RadioButton
                {
                    Text = "Tume teema",
                    Location = new Point(200, 420)
                };
                r_btn1.CheckedChanged += r_btn_Checked;

                r_btn2 = new RadioButton
                {
                    Text = "Hele teema",
                    Location = new Point(200, 440)
                };
                r_btn2.CheckedChanged += r_btn_Checked;

                Controls.AddRange(new Control[] { r_btn1, r_btn2 });
            }
            else if (e.Node.Text == "MessageBox")
            {

                CustomForm cf = new CustomForm(
        "Minu oma teade",
        "Vali üks neist:",
        "Esimene", "Teine", "Kolmas", "Neljas"
    )
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                _ = cf.ShowDialog();
                _ = MessageBox.Show("MessageBox", "Kõige lihtsam aken");
                DialogResult answer = MessageBox.Show("Tahad InputBoxi näha?", "Aken koos nupudega", MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes)
                {
                    string text = Interaction.InputBox("Sisesta siia mingi tekst", "InputBox", "Mingi tekst");
                    if (MessageBox.Show("Kas tahad tekst saada Tekskastisse?", "Teksti salvestamine", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        lbl.Text = text;
                        Controls.Add(lbl);
                    }
                    else
                    {
                        lbl.Text = "Siis mina lisan oma teksti!";
                        Controls.Add(lbl);
                    }
                }
                else
                {
                    _ = MessageBox.Show("Veel MessageBox", "Kõige lihtsam aken");
                }
            }
            else if (e.Node.Text == "Kaart")
            {
                tabC = new TabControl
                {
                    Location = new Point(450, 50),
                    Size = new Size(300, 200)
                };

                tabP1 = new TabPage("Koduleht");
                WebBrowser wb = new WebBrowser
                {
                    Url = new Uri("https://www.tthk.ee/")
                };
                tabP1.Controls.Add(wb);

                tabP2 = new TabPage("Moodle");
                WebBrowser mo = new WebBrowser
                {
                    Url = new Uri("https://moodle.edu.ee/")
                };
                tabP1.Controls.Add(mo);

                tabP3 = new TabPage("+");
                tabP3.DoubleClick += TabP3_DoubleClick;

                tabC.Controls.AddRange(new TabPage[] { tabP1, tabP2, tabP3 });
                tabC.Selected += TabC_Selected;
                Controls.Add(tabC);
            }
            else if (e.Node.Text == "ListBox")
            {
                lb = new ListBox();
                _ = lb.Items.Add("Roheline");
                _ = lb.Items.Add("Punane");
                _ = lb.Items.Add("Sinine");
                _ = lb.Items.Add("Hall");
                _ = lb.Items.Add("Kollane");
                lb.Location = new Point(150, 120);
                lb.SelectedIndexChanged += new EventHandler(ls_SelectedIndexChanged);
                Controls.Add(lb);
            }
            else if (e.Node.Text == "DataGridView")
            {
                DataSet ds = new DataSet("XML fail. Menüü");
                _ = ds.ReadXml(@"..\..\Images\menu.xml");
                DataGridView dg = new DataGridView
                {
                    Width = 400,
                    Height = 160,
                    Location = new Point(150, 250),
                    AutoGenerateColumns = true,
                    DataSource = ds,
                    DataMember = "food"
                };
                Controls.Add(dg);
            }
            else if (e.Node.Text == "MainMenu")
            {
                MainMenu menu = new MainMenu();
                MenuItem menuFile = new MenuItem("File");
                _ = menuFile.MenuItems.Add("Ava", (s, ev) => MessageBox.Show("Fail avatud!"));
                _ = menuFile.MenuItems.Add("About", (s, ev) => MessageBox.Show("See programm on loodud C#-is."));
                _ = menuFile.MenuItems.Add("Sule", new EventHandler(menuFile_Exit_Select));
                _ = menu.MenuItems.Add(menuFile);
                Menu = menu;
            }
        }

        private void menuFile_Exit_Select(object sender, EventArgs e)
        {
            Close();
        }

        private void ls_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lb.SelectedItem.ToString())
            {
                case "Sinine": tree.BackColor = Color.Blue; break;
                case "Kollane": tree.BackColor = Color.Yellow; break;
                case "Punane": tree.BackColor = Color.Red; break;
                case "Hall": tree.BackColor = Color.Gray; break;
                case "Roheline": tree.BackColor = Color.Green; break;
            }
        }

        private void TabC_Selected(object sender, TabControlEventArgs e)
        {
            //this.tabC.TabPages.Clear();
            tabC.TabPages.Remove(tabC.SelectedTab);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TabC_DoubleClick(object sender, EventArgs e)
        {
            string title = "tabP" + (tabC.TabCount - 1).ToString();
            TabPage tb = new TabPage(title);

            tabC.TabPages.Add(tb);
        }

        private void TabP3_DoubleClick(object sender, EventArgs e)
        {
            string title = "tabP" + (tabC.TabCount + 1).ToString();
            TabPage tb = new TabPage(title);
            tabC.TabPages.Add(tb);
        }

        private void r_btn_Checked(object sender, EventArgs e)
        {
            if (r_btn1.Checked)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
            }
            else if (r_btn2.Checked)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
        }
        private void C_btn1_CheckedChanged(object sender, EventArgs e)
        {
            if (t)
            {
                Size = new Size(1000, 1000);
                pic.BorderStyle = BorderStyle.Fixed3D;
                c_btn1.Text = "Teeme väiksem";
                c_btn1.Font = new Font("Arial", 36, FontStyle.Bold);
                t = false;
            }
            else
            {
                Size = new Size(700, 500);
                c_btn1.Text = "Suurendame";
                c_btn1.Font = new Font("Arial", 36, FontStyle.Regular);
                t = true;
            }
        }
    }
}
