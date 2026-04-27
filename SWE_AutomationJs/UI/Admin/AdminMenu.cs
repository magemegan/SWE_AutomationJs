using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Session;
using SWE_AutomationJs_UI_Design.UI.Admin;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AdminMenu : Form
    {
        public static string CurrentEmployee { get; set; }

        public AdminMenu()
        {
            InitializeComponent();

            ClientSize = new Size(1050, 700);
            Text = "Admin Dashboard";

            Controls.Clear();

            BuildDashboard();
        }

        private void BuildDashboard()
        {
            BackColor = UiTheme.Background;
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "Admin Dashboard",
                Location = new Point(55, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 30F, FontStyle.Bold),
                ForeColor = UiTheme.PrimaryDark
            };
            Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Manage restaurant operations, staff, reports, tables, menu, and system activity.",
                Location = new Point(60, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.DimGray
            };
            Controls.Add(subtitle);

            Button restaurantInfo = CreateTopButton("Restaurant Info", 775, 45,
                (s, e) => new AboutRestaurant().ShowDialog());
            Controls.Add(restaurantInfo);

            Button overrideApprovals = CreateTopButton("Override Approvals", 775, 90,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new OverrideApprovalForm()));
            Controls.Add(overrideApprovals);

            int cardW = 275;
            int cardH = 120;
            int gapX = 35;
            int gapY = 32;

            int startX = 60;
            int startY = 165;

            AddDashboardCard("Employees", "Manage staff records", startX, startY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords()));

            AddDashboardCard("Inventory", "Track restaurant supplies", startX + cardW + gapX, startY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new InventoryManagement()));

            AddDashboardCard("Restock", "Review restock requests", startX + (cardW + gapX) * 2, startY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new RestockRequests()));

            AddDashboardCard("Reports", "View sales analytics", startX, startY + cardH + gapY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new ReportsForm()));

            AddDashboardCard("Scheduling", "Manage employee shifts", startX + cardW + gapX, startY + cardH + gapY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new EmployeeScheduling()));

            AddDashboardCard("Menu", "Edit menu items", startX + (cardW + gapX) * 2, startY + cardH + gapY, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new MenuManagement()));

            AddDashboardCard("Notifications", "View admin alerts", startX, startY + (cardH + gapY) * 2, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new Notification("Admin")));

            AddDashboardCard("Table Layout", "Manage tables and seats", startX + cardW + gapX, startY + (cardH + gapY) * 2, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new TableLayoutManagementForm()));

            AddDashboardCard("Activity Log", "Audit system activity", startX + (cardW + gapX) * 2, startY + (cardH + gapY) * 2, cardW, cardH,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new ActivityLogViewerForm()));

            Button logout = new Button
            {
                Text = "Logout",
                Location = new Point(60, 625),
                Size = new Size(170, 42),
                BackColor = UiTheme.Danger,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            logout.FlatAppearance.BorderSize = 0;
            logout.Click += button1_Click;
            Controls.Add(logout);
        }

        private void AddDashboardCard(
            string title,
            string description,
            int x,
            int y,
            int width,
            int height,
            EventHandler click)
        {
            Button card = new Button
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.White,
                ForeColor = UiTheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(22, 12, 12, 12),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = title + Environment.NewLine + Environment.NewLine + description
            };

            card.FlatAppearance.BorderSize = 1;
            card.FlatAppearance.BorderColor = Color.FromArgb(220, 230, 238);
            card.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 247, 252);
            card.Click += click;

            Controls.Add(card);
        }

        private Button CreateTopButton(string text, int x, int y, EventHandler click)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(210, 35),
                BackColor = UiTheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.Click += click;

            return button;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new InventoryManagement());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new RestockRequests());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new ReportsForm());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeScheduling());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new MenuManagement());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Admin"));
        }
    }
}