using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            InitializeAdditionalButtons();
        }

        private void button1_Click(object sender, EventArgs e)
        {//logout
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button2_Click(object sender, EventArgs e)
        {//employee records
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords());
        }

        private void button3_Click(object sender, EventArgs e)
        {//inventory management
            NavigationHelper.ShowAtCurrentPosition(this, new InventoryManagement());
        }

        private void button4_Click(object sender, EventArgs e)
        {//restock requests
            NavigationHelper.ShowAtCurrentPosition(this, new RestockRequests());
        }

        private void button5_Click(object sender, EventArgs e)
        {//sales reports
            NavigationHelper.ShowAtCurrentPosition(this, new SalesReports());
        }

        private void button6_Click(object sender, EventArgs e)
        {//employee scheduling
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeScheduling());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new MenuManagement());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Admin"));
        }

        private void InitializeAdditionalButtons()
        {
            Button overrideButton = new Button();
            overrideButton.Location = new System.Drawing.Point(162, 317);
            overrideButton.Size = new System.Drawing.Size(92, 43);
            overrideButton.Text = "Override Approvals";
            overrideButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new OverrideApprovalForm());
            };
            Controls.Add(overrideButton);

            Button tableLayoutButton = new Button();
            tableLayoutButton.Location = new System.Drawing.Point(540, 317);
            tableLayoutButton.Size = new System.Drawing.Size(102, 43);
            tableLayoutButton.Text = "Table Layout";
            tableLayoutButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new TableLayoutManagementForm());
            };
            Controls.Add(tableLayoutButton);

            Button activityLogButton = new Button();
            activityLogButton.Location = new System.Drawing.Point(662, 317);
            activityLogButton.Size = new System.Drawing.Size(102, 43);
            activityLogButton.Text = "Activity Log";
            activityLogButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new ActivityLogViewerForm());
            };
            Controls.Add(activityLogButton);
            Button restaurantInfoButton = new Button();
            restaurantInfoButton.Location = new System.Drawing.Point(284, 317);
            restaurantInfoButton.Size = new System.Drawing.Size(102, 43);
            restaurantInfoButton.Text = "Restaurant Info";
            restaurantInfoButton.Click += (sender, args) =>
            {
                new AboutRestaurant().ShowDialog();
            };
            Controls.Add(restaurantInfoButton);
        }
    }
}
