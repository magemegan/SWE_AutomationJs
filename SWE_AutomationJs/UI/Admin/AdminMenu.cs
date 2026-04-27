using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AdminMenu : Form
    {
        public static string CurrentEmployee { get; set; }

        public AdminMenu()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            FormatMainButtons();
            InitializeAdditionalButtons();
        }

        // =========================
        // MAIN BUTTON FORMATTING
        // =========================
        private void FormatMainButtons()
        {
            int width = 180;
            int height = 55;

            int startX = 120;
            int startY = 140;
            int gapX = 220;
            int gapY = 85;

            // Row 1
            SetButton(button2, "Employee Records", startX, startY, width, height);
            SetButton(button3, "Inventory", startX + gapX, startY, width, height);
            SetButton(button4, "Restock Requests", startX + (gapX * 2), startY, width, height);

            // Row 2
            SetButton(button5, "Sales Reports", startX, startY + gapY, width, height);
            SetButton(button6, "Scheduling", startX + gapX, startY + gapY, width, height);
            SetButton(button7, "Menu Management", startX + (gapX * 2), startY + gapY, width, height);

            // Row 3
            SetButton(button8, "Notifications", startX, startY + (gapY * 2), width, height);

            // Logout bottom
            SetButton(button1, "Logout", startX, startY + (gapY * 3), width, height);
            button1.BackColor = Color.FromArgb(185, 45, 45); // red
        }

        private void SetButton(Button btn, string text, int x, int y, int w, int h)
        {
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(w, h);
            UiTheme.StyleButton(btn);
        }

        // =========================
        // EXTRA BUTTONS (FIXED)
        // =========================
        private void InitializeAdditionalButtons()
        {
            int width = 180;
            int height = 55;
            int y = 400;

            Button overrideButton = CreateAdminButton(
                "Override Approvals",
                new Point(120, y),
                width,
                height,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new OverrideApprovalForm())
            );

            Button restaurantInfoButton = CreateAdminButton(
                "Restaurant Info",
                new Point(340, y),
                width,
                height,
                (s, e) => new AboutRestaurant().ShowDialog()
            );

            Button tableLayoutButton = CreateAdminButton(
                "Table Layout",
                new Point(560, y),
                width,
                height,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new TableLayoutManagementForm())
            );

            Button activityLogButton = CreateAdminButton(
                "Activity Log",
                new Point(780, y),
                width,
                height,
                (s, e) => NavigationHelper.ShowAtCurrentPosition(this, new ActivityLogViewerForm())
            );

            Controls.Add(overrideButton);
            Controls.Add(restaurantInfoButton);
            Controls.Add(tableLayoutButton);
            Controls.Add(activityLogButton);
        }

        private Button CreateAdminButton(string text, Point location, int width, int height, EventHandler click)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = location;
            btn.Size = new Size(width, height);
            btn.Click += click;

            UiTheme.StyleButton(btn);

            return btn;
        }

        // =========================
        // BUTTON EVENTS
        // =========================

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
            NavigationHelper.ShowAtCurrentPosition(this, new SalesReports());
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