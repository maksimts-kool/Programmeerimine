using System.Windows.Forms;

namespace Tund10;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private TabControl tabs;
    private TabPage ownersTab;
    private TabPage carsTab;
    private TabPage servicesTab;

    // Owners Controls
    private DataGridView ownersGrid;
    private TextBox ownerName;
    private TextBox ownerPhone;
    private Button addOwnerBtn;
    private Button updateOwnerBtn;
    private Button deleteOwnerBtn;
    private GroupBox grpOwnersAction;

    // Cars Controls
    private DataGridView carsGrid;
    private TextBox carBrand;
    private TextBox carModel;
    private TextBox carReg;
    private ComboBox ownerCombo;
    private Button addCarBtn;
    private Button updateCarBtn;
    private Button deleteCarBtn;
    private GroupBox grpCarsAction;

    // Services Controls
    private DataGridView servicesGrid;
    private TextBox serviceName;
    private TextBox servicePrice;
    private Button addServiceBtn;
    private Button updateServiceBtn;
    private Button deleteServiceBtn;
    private GroupBox grpServicesAction;

    // Car Service Controls
    private DataGridView carServiceGrid;
    private ComboBox carSelect;
    private ComboBox serviceSelect;
    private DateTimePicker serviceDate;
    private TextBox mileageInput;
    private Button addCarServiceBtn;
    private Button updateCarServiceBtn;
    private Button deleteCarServiceBtn;
    private GroupBox grpCarServiceAction;
    private Label lblCar;
    private Label lblService;
    private Label lblDate;
    private Label lblMileage;

    private void InitializeComponent()
    {
        // --- STYLING CONSTANTS ---
        var btnBackColor = Color.FromArgb(0, 122, 204); // VS Blue
        var btnForeColor = Color.White;
        var tabBackColor = Color.WhiteSmoke;
        var gridHeaderColor = Color.FromArgb(45, 45, 48);
        var gridSelectionColor = Color.FromArgb(0, 122, 204);

        tabs = new TabControl();
        ownersTab = new TabPage();
        carsTab = new TabPage();
        servicesTab = new TabPage();

        // --- OWNERS INIT ---
        ownersGrid = CreateStyledGrid(gridHeaderColor, gridSelectionColor);
        grpOwnersAction = new GroupBox { Text = "Manage Owners", Location = new Point(10, 270), Size = new Size(760, 80) };
        ownerName = new TextBox { PlaceholderText = "Full Name", Location = new Point(15, 30), Width = 200 };
        ownerPhone = new TextBox { PlaceholderText = "Phone", Location = new Point(230, 30), Width = 150 };
        addOwnerBtn = CreateStyledButton("Add", btnBackColor, btnForeColor);
        updateOwnerBtn = CreateStyledButton("Update", Color.DarkOrange, Color.White);
        deleteOwnerBtn = CreateStyledButton("Delete", Color.IndianRed, Color.White);

        // --- CARS INIT ---
        carsGrid = CreateStyledGrid(gridHeaderColor, gridSelectionColor);
        grpCarsAction = new GroupBox { Text = "Manage Cars", Location = new Point(10, 270), Size = new Size(760, 80) };
        carBrand = new TextBox { PlaceholderText = "Brand", Location = new Point(15, 30), Width = 100 };
        carModel = new TextBox { PlaceholderText = "Model", Location = new Point(125, 30), Width = 100 };
        carReg = new TextBox { PlaceholderText = "Reg Num", Location = new Point(235, 30), Width = 100 };
        ownerCombo = new ComboBox { Location = new Point(345, 29), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
        addCarBtn = CreateStyledButton("Add", btnBackColor, btnForeColor);
        updateCarBtn = CreateStyledButton("Update", Color.DarkOrange, Color.White);
        deleteCarBtn = CreateStyledButton("Delete", Color.IndianRed, Color.White);

        // --- SERVICES INIT ---
        servicesGrid = CreateStyledGrid(gridHeaderColor, gridSelectionColor);
        grpServicesAction = new GroupBox { Text = "Manage Services", Location = new Point(10, 170), Size = new Size(760, 70) };
        serviceName = new TextBox { PlaceholderText = "Service Name", Location = new Point(15, 25), Width = 150 };
        servicePrice = new TextBox { PlaceholderText = "Price", Location = new Point(180, 25), Width = 100 };
        addServiceBtn = CreateStyledButton("Add", btnBackColor, btnForeColor);
        updateServiceBtn = CreateStyledButton("Update", Color.DarkOrange, Color.White);
        deleteServiceBtn = CreateStyledButton("Delete", Color.IndianRed, Color.White);

        // --- CAR SERVICES INIT ---
        carServiceGrid = CreateStyledGrid(gridHeaderColor, gridSelectionColor);
        grpCarServiceAction = new GroupBox { Text = "Log Maintenance", Location = new Point(10, 430), Size = new Size(760, 80) };

        lblCar = new Label { Text = "Car:", Location = new Point(15, 25), AutoSize = true };
        carSelect = new ComboBox { Location = new Point(45, 22), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };

        lblService = new Label { Text = "Svc:", Location = new Point(200, 25), AutoSize = true };
        serviceSelect = new ComboBox { Location = new Point(235, 22), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };

        lblDate = new Label { Text = "Date:", Location = new Point(390, 25), AutoSize = true };
        serviceDate = new DateTimePicker { Location = new Point(430, 22), Width = 110, Format = DateTimePickerFormat.Short };

        lblMileage = new Label { Text = "km:", Location = new Point(550, 25), AutoSize = true };
        mileageInput = new TextBox { Location = new Point(575, 22), Width = 80, PlaceholderText = "123000" };

        addCarServiceBtn = CreateStyledButton("Log", btnBackColor, btnForeColor);
        updateCarServiceBtn = CreateStyledButton("Edit", Color.DarkOrange, Color.White);
        deleteCarServiceBtn = CreateStyledButton("Del", Color.IndianRed, Color.White);

        SuspendLayout();

        // ================= TABS SETUP =================
        tabs.Dock = DockStyle.Fill;
        tabs.Padding = new Point(10, 6);
        tabs.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        // ---------------- OWNERS TAB ----------------
        ownersTab.Text = "  OWNERS  ";
        ownersTab.BackColor = tabBackColor;

        ownersGrid.Location = new Point(10, 10);
        ownersGrid.Size = new Size(760, 250);

        // Group Box Layout
        addOwnerBtn.Location = new Point(400, 25);
        updateOwnerBtn.Location = new Point(510, 25);
        deleteOwnerBtn.Location = new Point(620, 25);

        // Events
        addOwnerBtn.Click += addOwnerBtn_Click;
        updateOwnerBtn.Click += updateOwnerBtn_Click;
        deleteOwnerBtn.Click += deleteOwnerBtn_Click;

        grpOwnersAction.Controls.AddRange(new Control[] { ownerName, ownerPhone, addOwnerBtn, updateOwnerBtn, deleteOwnerBtn });
        ownersTab.Controls.Add(ownersGrid);
        ownersTab.Controls.Add(grpOwnersAction);

        // ---------------- CARS TAB ----------------
        carsTab.Text = "  CARS  ";
        carsTab.BackColor = tabBackColor;

        carsGrid.Location = new Point(10, 10);
        carsGrid.Size = new Size(760, 250);

        // Group Box Layout
        addCarBtn.Location = new Point(510, 25);
        updateCarBtn.Location = new Point(595, 25); // Adjusted mainly for spacing
        deleteCarBtn.Location = new Point(680, 25);

        // Re-adjusting button widths for this tab specifically if needed, 
        // but standard styled buttons work fine.
        addCarBtn.Width = 80; updateCarBtn.Width = 80; deleteCarBtn.Width = 70;

        // Events
        addCarBtn.Click += addCarBtn_Click;
        updateCarBtn.Click += updateCarBtn_Click;
        deleteCarBtn.Click += deleteCarBtn_Click;

        grpCarsAction.Controls.AddRange(new Control[] { carBrand, carModel, carReg, ownerCombo, addCarBtn, updateCarBtn, deleteCarBtn });
        carsTab.Controls.Add(carsGrid);
        carsTab.Controls.Add(grpCarsAction);

        // ---------------- SERVICES TAB ----------------
        servicesTab.Text = "  SERVICES & LOGS  ";
        servicesTab.BackColor = tabBackColor;

        // 1. Service List
        servicesGrid.Location = new Point(10, 10);
        servicesGrid.Size = new Size(760, 150);

        // Service Actions
        addServiceBtn.Location = new Point(300, 20);
        updateServiceBtn.Location = new Point(410, 20);
        deleteServiceBtn.Location = new Point(520, 20);

        addServiceBtn.Click += addServiceBtn_Click;
        updateServiceBtn.Click += updateServiceBtn_Click;
        deleteServiceBtn.Click += deleteServiceBtn_Click;

        grpServicesAction.Controls.AddRange(new Control[] { serviceName, servicePrice, addServiceBtn, updateServiceBtn, deleteServiceBtn });

        // 2. Car Service Grid
        carServiceGrid.Location = new Point(10, 250);
        carServiceGrid.Size = new Size(760, 170);

        // Car Service Actions
        addCarServiceBtn.Location = new Point(660, 15);
        addCarServiceBtn.Width = 80;
        addCarServiceBtn.Height = 25; // Compact

        updateCarServiceBtn.Location = new Point(660, 45); // Stacked buttons
        updateCarServiceBtn.Width = 80;
        updateCarServiceBtn.Height = 25;

        deleteCarServiceBtn.Location = new Point(570, 45);
        deleteCarServiceBtn.Width = 80;
        deleteCarServiceBtn.Height = 25;

        addCarServiceBtn.Click += addCarServiceBtn_Click;
        updateCarServiceBtn.Click += updateCarServiceBtn_Click;
        deleteCarServiceBtn.Click += deleteCarServiceBtn_Click;

        grpCarServiceAction.Controls.AddRange(new Control[] {
                lblCar, carSelect, lblService, serviceSelect,
                lblDate, serviceDate, lblMileage, mileageInput,
                addCarServiceBtn, updateCarServiceBtn, deleteCarServiceBtn
            });

        servicesTab.Controls.Add(servicesGrid);
        servicesTab.Controls.Add(grpServicesAction);
        servicesTab.Controls.Add(carServiceGrid);
        servicesTab.Controls.Add(grpCarServiceAction);

        // ================= FORM INIT =================
        tabs.TabPages.AddRange(new TabPage[] { ownersTab, carsTab, servicesTab });

        ClientSize = new Size(800, 560);
        Controls.Add(tabs);
        Text = "Auto Management System";
        BackColor = Color.White;
        StartPosition = FormStartPosition.CenterScreen;

        ResumeLayout(false);
    }

    // --- HELPER METHODS FOR STYLING ---

    private Button CreateStyledButton(string text, Color backColor, Color foreColor)
    {
        var btn = new Button();
        btn.Text = text;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.BackColor = backColor;
        btn.ForeColor = foreColor;
        btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btn.Size = new Size(100, 30);
        btn.Cursor = Cursors.Hand;
        return btn;
    }

    private DataGridView CreateStyledGrid(Color headerColor, Color selectionColor)
    {
        var grid = new DataGridView();
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;

        // Header Style
        grid.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grid.ColumnHeadersHeight = 35;

        // Row Style
        grid.DefaultCellStyle.SelectionBackColor = selectionColor;
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        grid.DefaultCellStyle.Padding = new Padding(5);

        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AllowUserToAddRows = false;
        grid.ReadOnly = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Alternating Rows
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

        return grid;
    }

    #endregion
}
