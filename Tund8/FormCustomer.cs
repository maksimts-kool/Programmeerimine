using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Tund8
{
    public partial class FormCustomer : Form
    {
        private readonly SqlConnection _connect = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\opilane\source\repos\ProgrammeerimineTARpv24\Tund8\ShopDB.mdf;
              Integrated Security=True;"
        );

        private FlowLayoutPanel? panelProducts;
        private ListBox? listBoxCategories;
        private ListView? cartList;
        private Label? lblTotal;
        private Label? lblBalance;
        private Dictionary<string, (int qty, decimal price)> cart = new();

        private decimal balance = 200.00m;

        public FormCustomer()
        {
            //InitializeComponent();
            BuildLayout();
            LoadCategories();
        }

        private void BuildLayout()
        {
            Text = "Pood — Klient";
            WindowState = FormWindowState.Normal;
            Size = new Size(1200, 800);

            const double CATEGORY_RATIO = 0.20;
            const double PRODUCT_RATIO = 0.60;
            const double CART_RATIO = 0.20;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };

            var rightSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = false
            };

            var categoryPanel = new Panel { Dock = DockStyle.Fill };

            lblBalance = new Label
            {
                Text = $"💰 Raha: {balance:N2} €",
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.LightYellow
            };

            listBoxCategories = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11)
            };
            listBoxCategories.SelectedIndexChanged += ListBoxCategories_SelectedIndexChanged;

            categoryPanel.Controls.Add(listBoxCategories);
            categoryPanel.Controls.Add(lblBalance);

            split.Panel1.Controls.Add(categoryPanel);

            panelProducts = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.WhiteSmoke
            };
            rightSplit.Panel1.Controls.Add(panelProducts);

            var cartPanel = new Panel { Dock = DockStyle.Fill };
            var lblCart = new Label
            {
                Text = "🛒 Ostukorv",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            cartList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                AllowColumnReorder = false
            };
            cartList.Columns.Add("Toode", 100);
            cartList.Columns.Add("Kogus", 60);
            cartList.Columns.Add("Kokku (€)", 80);

            lblTotal = new Label
            {
                Text = "Kokku: 0.00 €",
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Padding = new Padding(0, 0, 10, 0)
            };

            var btnBuy = new Button
            {
                Text = "🛍️ Ostan",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnBuy.Click += BtnBuy_Click;

            cartPanel.Controls.Add(cartList);
            cartPanel.Controls.Add(lblTotal);
            cartPanel.Controls.Add(btnBuy);
            cartPanel.Controls.Add(lblCart);

            rightSplit.Panel2.Controls.Add(cartPanel);
            split.Panel2.Controls.Add(rightSplit);
            Controls.Add(split);

            Resize += (s, e) =>
            {
                int totalWidth = ClientSize.Width;
                int categoryWidth = (int)(totalWidth * CATEGORY_RATIO);

                int rightSplitTotalWidth = totalWidth - categoryWidth;

                double productSplitRatio = PRODUCT_RATIO / (PRODUCT_RATIO + CART_RATIO);

                int rightSplitDistance = (int)(rightSplitTotalWidth * productSplitRatio);

                split.SplitterDistance = categoryWidth;
                rightSplit.SplitterDistance = rightSplitDistance;
            };

            PerformLayout();
            var eventArgs = new EventArgs();
            GetType().GetMethod("OnResize",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            )?.Invoke(this, new object[] { eventArgs });
        }

        private void LoadCategories()
        {
            try
            {
                _connect.Open();
                var adapter = new SqlDataAdapter("SELECT Id, Kategooria_nim FROM KatTabel", _connect);
                DataTable dt = new();
                adapter.Fill(dt);

                _connect.Close();

                if (listBoxCategories != null)
                {
                    listBoxCategories.DisplayMember = "Kategooria_nim";
                    listBoxCategories.ValueMember = "Id";
                    listBoxCategories.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga: " + ex.Message);
            }
        }

        private void ListBoxCategories_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            if (listBoxCategories?.SelectedValue == null) return;

            if (listBoxCategories.SelectedValue is DataRowView) return;

            if (int.TryParse(listBoxCategories.SelectedValue.ToString(), out int catId))
            {
                LoadProducts(catId);
            }
        }

        private void LoadProducts(int catId)
        {
            if (panelProducts != null)
            {
                panelProducts.Controls.Clear();
            }

            try
            {
                _connect.Open();
                var cmd = new SqlCommand(
                    "SELECT Toode_nim, Hind, Kogus, Pilt FROM ToodeTabel WHERE Kategooriad=@id",
                    _connect
                );
                cmd.Parameters.AddWithValue("@id", catId);
                SqlDataAdapter adp = new(cmd);
                DataTable dt = new();
                adp.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    if (panelProducts != null)
                    {
                        panelProducts.Controls.Add(CreateProductCard(row));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga toodete kuvamisel: " + ex.Message);
            }
            finally
            {
                _connect.Close();
            }
        }

        private Panel CreateProductCard(DataRow row)
        {
            string name = row["Toode_nim"].ToString() ?? "??";
            decimal price = Convert.ToDecimal(row["Hind"]);
            int stock = Convert.ToInt32(row["Kogus"]);

            int currentCartQty = cart.ContainsKey(name) ? cart[name].qty : 0;

            int remainingStock = stock - currentCartQty;

            var card = new Panel
            {
                Width = 200,
                Height = 280,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            string imgPath = Path.Combine(Path.GetFullPath(@"..\..\..\Pildid"),
                row["Pilt"]?.ToString() ?? "epood.png");
            PictureBox pb = new PictureBox
            {
                Image = File.Exists(imgPath)
                    ? Image.FromFile(imgPath)
                    : Image.FromFile(Path.Combine(Path.GetFullPath(@"..\..\..\Pildid"), "epood.png")),
                Width = 180,
                Height = 140,
                Dock = DockStyle.Top,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Label lblName = new Label
            {
                Text = name,
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblStock = new Label
            {
                Text = remainingStock > 0 ? "Laos" : "Otsas",
                ForeColor = remainingStock > 0 ? Color.LimeGreen : Color.Red,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            int qty = 0;

            Label lblQty = new Label
            {
                Text = qty.ToString(),
                Width = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnMinus = new() { Text = "-", Width = 30 };
            Button btnPlus = new() { Text = "+", Width = 30 };

            btnPlus.Click += (s, ev) =>
            {
                int cartQtyCheck = cart.ContainsKey(name) ? cart[name].qty : 0;
                int available = stock - cartQtyCheck;

                if (available > 0 && qty < available)
                {
                    qty++;
                    lblQty.Text = qty.ToString();
                }
            };
            btnMinus.Click += (s, ev) => { if (qty > 0) { qty--; lblQty.Text = qty.ToString(); } };

            FlowLayoutPanel qtyPanel = new()
            {
                Dock = DockStyle.Top,
                Height = 30
            };
            qtyPanel.Controls.Add(btnMinus);
            qtyPanel.Controls.Add(lblQty);
            qtyPanel.Controls.Add(btnPlus);

            Label lblPrice = new()
            {
                Text = $"{price:N2} €",
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnAdd = new()
            {
                Text = "Lisa ostukorvi",
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.SkyBlue,
                Enabled = remainingStock > 0
            };

            btnAdd.Click += (s, ev) =>
            {
                if (qty == 0)
                {
                    MessageBox.Show("Vali kogus!");
                    return;
                }

                int cartQty = cart.ContainsKey(name) ? cart[name].qty : 0;
                int newTotalQty = cartQty + qty;

                if (newTotalQty > stock)
                {
                    return;
                }

                if (cart.ContainsKey(name))
                    cart[name] = (newTotalQty, price);
                else
                    cart.Add(name, (qty, price));

                qty = 0;
                lblQty.Text = qty.ToString();

                int updatedRemainingStock = stock - newTotalQty;
                lblStock.Text = updatedRemainingStock > 0 ? "Laos" : "Otsas";
                lblStock.ForeColor = updatedRemainingStock > 0 ? Color.LimeGreen : Color.Red;
                btnAdd.Enabled = updatedRemainingStock > 0;

                RefreshCart();
            };

            card.Controls.Add(btnAdd);
            card.Controls.Add(lblPrice);
            card.Controls.Add(qtyPanel);
            card.Controls.Add(lblStock);
            card.Controls.Add(lblName);
            card.Controls.Add(pb);

            return card;
        }

        private void RefreshCart()
        {
            if (cartList == null) return;
            cartList.Items.Clear();
            decimal total = 0;
            foreach (var item in cart)
            {
                decimal subtotal = item.Value.qty * item.Value.price;
                total += subtotal;
                var lvi = new ListViewItem(new[]
                {
                    item.Key,
                    item.Value.qty.ToString(),
                    subtotal.ToString("N2")
                });
                cartList.Items.Add(lvi);
            }

            if (lblTotal != null)
            {
                lblTotal.Text = $"Kokku: {total:N2} €";
            }
        }

        private void BtnBuy_Click(object? sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Ostukorv on tühi!");
                return;
            }

            decimal total = 0;
            foreach (var item in cart)
            {
                total += item.Value.qty * item.Value.price;
            }

            if (total > balance)
            {
                MessageBox.Show($"Makse ebaõnnestus! Summa {total:N2} € ületab teie raha {balance:N2} €.");
                return;
            }

            try
            {
                _connect.Open();

                foreach (var item in cart)
                {
                    var cmd = new SqlCommand(
                        "UPDATE ToodeTabel SET Kogus = Kogus - @kogus WHERE Toode_nim = @nim",
                        _connect
                    );
                    cmd.Parameters.AddWithValue("@kogus", item.Value.qty);
                    cmd.Parameters.AddWithValue("@nim", item.Key);
                    cmd.ExecuteNonQuery();
                }

                balance -= total;
                if (lblBalance != null)
                {
                    lblBalance.Text = $"💰 Raha: {balance:N2} €";
                }

                _connect.Close();

                MessageBox.Show("Ost sooritatud! Aitäh.");


                // Küsi e-post
                string email = AskEmailForReceipt();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    SendReceiptEmail(email, total);
                }
                else
                {
                    MessageBox.Show("Kviitungi saatmine tühistati.");
                }
                cart.Clear();
                RefreshCart();
                if (listBoxCategories != null && listBoxCategories.SelectedValue != null)
                {
                    int id = Convert.ToInt32(listBoxCategories.SelectedValue);
                    LoadProducts(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga ostu kinnitamisel: " + ex.Message);
            }
            finally
            {
                _connect.Close();
            }
        }
        private string AskEmailForReceipt()
        {
            Form prompt = new Form
            {
                Width = 350,
                Height = 150,
                Text = "Kviitungi saatmine",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lbl = new Label
            {
                Text = "Sisestage oma e-post:",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            TextBox txtEmail = new TextBox
            {
                Dock = DockStyle.Top,
                Margin = new Padding(10)
            };

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };

            Button btnSaada = new Button { Text = "Saada", Width = 80 };
            Button btnLoobu = new Button { Text = "Loobu", Width = 80 };

            btnSaada.Click += (s, e) =>
            {
                prompt.DialogResult = DialogResult.OK;
                prompt.Close();
            };

            btnLoobu.Click += (s, e) =>
            {
                prompt.DialogResult = DialogResult.Cancel;
                prompt.Close();
            };

            buttons.Controls.Add(btnSaada);
            buttons.Controls.Add(btnLoobu);

            prompt.Controls.Add(buttons);
            prompt.Controls.Add(txtEmail);
            prompt.Controls.Add(lbl);

            return prompt.ShowDialog(this) == DialogResult.OK
                ? txtEmail.Text.Trim()
                : "";
        }
        private void SendReceiptEmail(string toEmail, decimal total)
        {
            try
            {
                string body = "Aitäh ostu eest!\n\n";
                body += "Ostetud tooted:\n";

                foreach (var item in cart)
                {
                    body += $"- {item.Key} x {item.Value.qty} = {item.Value.qty * item.Value.price:N2} €\n";
                }

                body += "\nKokku makstud: " + total.ToString("N2") + " €\n\n";
                body += "Head päeva!\nE‑pood";

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("maksimtsitkool@gmail.com", "app-pass")
                };

                var mail = new MailMessage();
                mail.From = new MailAddress("maksimtsitkool@gmail.com", "E‑pood");
                mail.To.Add(toEmail);
                mail.Subject = "Teie ostu kviitung";
                mail.Body = body;

                smtp.Send(mail);
                MessageBox.Show("Kviitung saadeti aadressile: " + toEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-kirja saatmine ebaõnnestus: " + ex.Message);
            }
        }
    }
}