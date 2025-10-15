using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Tund6
{
    public partial class Form1 : Form
    {
        TreeView tree;
        Button btn;
        Label lbl;
        PictureBox pic;
        CheckBox c_btn1, c_btn2;
        RadioButton r_btn1, r_btn2;
        TabControl tabC;
        TabPage tabP1, tabP2, tabP3;
        ListBox lb;
        bool t = true;
        public Form1()
        {

            this.Height = 600;
            this.Width = 800;
            this.Text = "Vorm elementidega";
            tree = new TreeView();
            tree.Dock = DockStyle.Left;
            tree.AfterSelect += Tree_AfterSelect;
            TreeNode tn = new TreeNode("Elemendid");
            tn.Nodes.Add(new TreeNode("Nupp"));
            tn.Nodes.Add(new TreeNode("Silt"));
            tn.Nodes.Add(new TreeNode("Pilt"));
            tn.Nodes.Add(new TreeNode("Märkeruut"));
            tn.Nodes.Add(new TreeNode("Radionupp"));
            tn.Nodes.Add(new TreeNode("Kaart"));
            tn.Nodes.Add(new TreeNode("MessageBox"));
            tn.Nodes.Add(new TreeNode("ListBox"));
            tn.Nodes.Add(new TreeNode("MainMenu"));
            //nupp
            btn = new Button();
            btn.Text = "Vajuta siia";
            btn.Location = new Point(150, 30);
            btn.Height = 30;
            btn.Width = 100;
            btn.Click += Btn_Click;
            //pealkiri
            lbl = new Label();
            lbl.Text = "Elementide loomine c# abil";
            lbl.Font = new Font("Arial", 24);
            lbl.Size = new Size(400, 30);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;

            pic = new PictureBox();
            pic.Size = new Size(50, 50);
            pic.Location = new Point(150, 60);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = Image.FromFile(@"..\..\Images\close_box_red.png");
            pic.DoubleClick += Pic_DoubleClick;

            tree.Nodes.Add(tn);
            this.Controls.Add(tree);
        }
        int click = 0;
        private void Pic_DoubleClick(object sender, EventArgs e)
        {   //Double_Click -> carusel (3-4 images) 1-2-3-4-1-2-3-4-... 
            string[] images = { "esimene.jpg", "teine.jpg", "kolmas.jpg" };
            string fail = images[click];
            pic.Image = Image.FromFile(@"..\..\Images\" + fail);
            click++;
            if (click == 3) { click = 0; }
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.BackColor = Color.Transparent;
            Form1 Form = new Form1();
            Form.Show();
            this.Hide();


        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.BackColor = Color.FromArgb(200, 10, 20);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            using (var soundPlayer = new SoundPlayer(@"..\..\Sound\mixkit.wav")) { soundPlayer.Play(); }

            CustomForm customMessage = new CustomForm(
           "Minu oma teade",
           "Tee oma valik",
           "Kivi!",
           "Käärid!",
           "Paber!",
           "Vali ise!"
           );
            customMessage.StartPosition = FormStartPosition.CenterParent;
            customMessage.ShowDialog();
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Nupp")
            {
                this.Controls.Add(btn);

                // Lisa teine nupp, mis muudab akna värvi
                Button btn2 = new Button();
                btn2.Text = "Muuda taust";
                btn2.Location = new Point(150, 70);
                btn2.Click += (s, args) =>
                {
                    this.BackColor = (this.BackColor == Color.LightGray) ? Color.LightPink : Color.LightGray;
                };
                this.Controls.Add(btn2);
            }

            else if (e.Node.Text == "Silt")
            {
                this.Controls.Add(lbl);

                // Silt, millele vajutades tekib MessageBox
                Label lbl_click = new Label();
                lbl_click.Text = "Vajuta minule!";
                lbl_click.Location = new Point(150, 60);
                lbl_click.Font = new Font("Arial", 12, FontStyle.Bold);
                lbl_click.Click += (s, args) =>
                {
                    MessageBox.Show("Sa vajutasid sildile!", "Tegevus");
                };
                this.Controls.Add(lbl_click);
            }

            else if (e.Node.Text == "Pilt")
            {
                this.Controls.Add(pic);

                // Lisa teine pilt, mis vahetab teist pilti klõpsul
                PictureBox pic2 = new PictureBox();
                pic2.Size = new Size(60, 60);
                pic2.Location = new Point(220, 60);
                pic2.SizeMode = PictureBoxSizeMode.StretchImage;
                pic2.Image = Image.FromFile(@"..\..\Images\teine.jpg");
                pic2.Click += (s, args) =>
                {
                    pic2.Image = Image.FromFile(@"..\..\Images\kolmas.jpg");
                };
                this.Controls.Add(pic2);
            }

            else if (e.Node.Text == "Märkeruut")
            {
                c_btn1 = new CheckBox();
                c_btn1.Text = "Suurenda aken";
                c_btn1.Location = new Point(310, 420);
                c_btn1.CheckedChanged += (s, args) =>
                {
                    if (c_btn1.Checked) this.Size = new Size(1000, 800);
                    else this.Size = new Size(800, 600);
                };

                // Näitab pilti kui vajutatakse
                c_btn2 = new CheckBox();
                c_btn2.Text = "Näita pilti";
                c_btn2.Location = new Point(310, 450);
                c_btn2.CheckedChanged += (s, args) =>
                {
                    if (c_btn2.Checked)
                    {
                        PictureBox pk = new PictureBox();
                        pk.Size = new Size(80, 80);
                        pk.Location = new Point(430, 450);
                        pk.SizeMode = PictureBoxSizeMode.StretchImage;
                        pk.Image = Image.FromFile(@"..\..\Images\teine.jpg");
                        this.Controls.Add(pk);
                    }
                    else
                    {
                        foreach (Control c in this.Controls)
                        {
                            if (c is PictureBox && c.Location == new Point(430, 450))
                            {
                                this.Controls.Remove(c);
                                break;
                            }
                        }
                    }
                };

                this.Controls.Add(c_btn1);
                this.Controls.Add(c_btn2);
            }

            else if (e.Node.Text == "Radionupp")
            {
                
                r_btn1 = new RadioButton();
                r_btn1.Text = "Must teema";
                r_btn1.Location = new Point(200, 420);

                r_btn2 = new RadioButton();
                r_btn2.Text = "Valge teema";
                r_btn2.Location = new Point(200, 440);

                // Sinine teema
                RadioButton r_btn3 = new RadioButton();
                r_btn3.Text = "Sinine teema";
                r_btn3.Location = new Point(200, 460);

                r_btn1.CheckedChanged += (s, args) =>
                {
                    if (r_btn1.Checked)
                    {
                        this.BackColor = Color.Black;
                        ForeColor = Color.White;
                    }
                };

                r_btn2.CheckedChanged += (s, args) =>
                {
                    if (r_btn2.Checked)
                    {
                        this.BackColor = Color.White;
                        ForeColor = Color.Black;
                    }
                };

                r_btn3.CheckedChanged += (s, args) =>
                {
                    if (r_btn3.Checked)
                    {
                        this.BackColor = Color.LightBlue;
                        ForeColor = Color.DarkBlue;
                    }
                };

                this.Controls.Add(r_btn1);
                this.Controls.Add(r_btn2);
                this.Controls.Add(r_btn3);
            }

            else if (e.Node.Text == "MessageBox")
            {
                var choice = MessageBox.Show(
                    "Kas soovid muuta vormi välimust?",
                    "Küsimus",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (choice == DialogResult.Yes)
                {
                    // Kui kasutaja vajutab "Jah" — muudame taustavärvi ja lisame pildi
                    this.BackColor = Color.LightGreen;

                    lbl.Text = "Vormi taust muudeti roheliseks!";
                    lbl.ForeColor = Color.DarkGreen;
                    this.Controls.Add(lbl);

                    MessageBox.Show("Vorm muudeti edukalt!", "Tulemus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (choice == DialogResult.No)
                {
                }
                else if (choice == DialogResult.Cancel)
                {
                    // Kui kasutaja vajutab "Loobu" – taastame algse värvi ja eemaldame kõik dünaamilised pildid
                    this.BackColor = SystemColors.Control;
                    foreach (Control c in this.Controls)
                    {
                        if (c is PictureBox && c != pic) // jätame algse pildi alles
                        {
                            this.Controls.Remove(c);
                            break;
                        }
                    }

                    lbl.Text = "Kõik taastatud!";
                    lbl.ForeColor = Color.Black;
                    this.Controls.Add(lbl);

                    MessageBox.Show("Taastasime algse seisundi!", "Tehtud", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else if (e.Node.Text == "Kaart")
            {
                tabC = new TabControl();
                tabC.Location = new Point(450, 50);
                tabC.Size = new Size(300, 200);

                tabP1 = new TabPage("TTHK");
                //WebBrowser wb = new WebBrowser();
                //wb.Url = new Uri("https://www.tthk.ee/");
                //tabP1.Controls.Add(wb);

                tabP2 = new TabPage("Teine kaart");
                Button addTabBTN = new Button();
                addTabBTN.Text = "Lisa uus kaart";
                addTabBTN.Click += (s, args) =>
                {
                    TabPage uus = new TabPage("Loodud kaart " + tabC.TabCount);
                    Label infolbl = new Label();
                    infolbl.Text = "See kaart loodi!";
                    infolbl.Location = new Point(10, 10);
                    uus.Controls.Add(infolbl);
                    tabC.TabPages.Add(uus);
                };
                tabP2.Controls.Add(addTabBTN);

                tabC.Controls.Add(tabP1);
                tabC.Controls.Add(tabP2);
                this.Controls.Add(tabC);
            }

            else if (e.Node.Text == "ListBox")
            {
                lb = new ListBox();
                lb.Items.Add("Roheline");
                lb.Items.Add("Punane");
                lb.Items.Add("Sinine");
                lb.Items.Add("Hall");
                lb.Items.Add("Kollane");
                lb.Location = new Point(150, 120);

                Label lbl_valik = new Label();
                lbl_valik.Text = "Vali värv ja midagi muutub!";
                lbl_valik.Location = new Point(150, 250);
                lbl_valik.AutoSize = true;
                lbl_valik.Font = new Font("Arial", 12, FontStyle.Bold);

                // Kui kasutaja valib midagi, muutub midagi vormil nähtavalt
                lb.SelectedIndexChanged += (s, args) =>
                {
                    string valik = lb.SelectedItem.ToString();

                    switch (valik)
                    {
                        case "Roheline":
                            lbl_valik.Text = "Valisid rohelise – see toob kasvu!";
                            lbl_valik.ForeColor = Color.Green;
                            break;

                        case "Punane":
                            lbl_valik.Text = "Punane – kirg ja energia!";
                            lbl_valik.ForeColor = Color.Red;
                            break;

                        case "Sinine":
                            lbl_valik.Text = "Sinine – rahulik ja kindel!";
                            lbl_valik.ForeColor = Color.Blue;
                            break;

                        case "Hall":
                            lbl_valik.Text = "Hall – tasakaal ja rahu.";
                            lbl_valik.ForeColor = Color.Black;
                            break;

                        case "Kollane":
                            lbl_valik.Text = "Kollane – rõõm ja soojus!";
                            lbl_valik.ForeColor = Color.Gold;
                            break;
                    }
                };

                // Lisa objektid vormile
                this.Controls.Add(lb);
                this.Controls.Add(lbl_valik);
            }

            else if (e.Node.Text == "MainMenu")
            {
                MainMenu menu = new MainMenu();
                MenuItem menuFile = new MenuItem("File");
                menuFile.MenuItems.Add("Exit", new EventHandler(menuFile_Exit_Select));
                menu.MenuItems.Add(menuFile);

                // Lisa Help menüü, millel on tegevus
                MenuItem menuHelp = new MenuItem("Help");
                menuHelp.MenuItems.Add("About", (s, args) =>
                {
                    MessageBox.Show("See on projekti aken", "About");
                });
                menu.MenuItems.Add(menuHelp);

                this.Menu = menu;
            }
            tree.SelectedNode = null;
        }

        private void menuFile_Exit_Select(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ls_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lb.SelectedItem.ToString())
            {
                case ("Sinine"): tree.BackColor = Color.Blue; break;
                case ("Kollane"): tree.BackColor = Color.Yellow; break;
                case ("Punane"): tree.BackColor = Color.Red; break;
                case ("Hall"): tree.BackColor = Color.Gray; break;
                case ("Roheline"): tree.BackColor = Color.Green; break;
            }
        }

        private void TabC_Selected(object sender, TabControlEventArgs e)
        {
            //this.tabC.TabPages.Clear();
            this.tabC.TabPages.Remove(tabC.SelectedTab);
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
                this.BackColor = Color.Black;
                r_btn2.ForeColor = Color.White;
                r_btn1.ForeColor = Color.White;
            }
            else if (r_btn2.Checked)
            {
                this.BackColor = Color.White;
                r_btn2.ForeColor = Color.Black;
                r_btn1.ForeColor = Color.Black;
            }
        }
        private void C_btn1_CheckedChanged(object sender, EventArgs e)
        {
            if (t)
            {
                this.Size = new Size(1000, 1000);
                pic.BorderStyle = BorderStyle.Fixed3D;
                c_btn1.Text = "Teeme väiksem";
                c_btn1.Font = new Font("Arial", 36, FontStyle.Bold);
                t = false;
            }
            else
            {
                this.Size = new Size(700, 500);
                c_btn1.Text = "Suurendame";
                c_btn1.Font = new Font("Arial", 36, FontStyle.Regular);
                t = true;
            }
        }
    }
}