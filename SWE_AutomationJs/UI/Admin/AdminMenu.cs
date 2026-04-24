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
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//employee records
            EmployeeRecords employeeRecords = new EmployeeRecords();
            employeeRecords.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {//inventory management
            InventoryManagement inventoryManagement = new InventoryManagement();
            inventoryManagement.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {//restock requests
            RestockRequests restockRequests = new RestockRequests();
            restockRequests.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {//sales reports
            SalesReports salesReports = new SalesReports();
            salesReports.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {//employee scheduling
            EmployeeScheduling employeeScheduling = new EmployeeScheduling();
            employeeScheduling.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            MenuManagement menuManagement = new MenuManagement();
            menuManagement.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Notification notification = new Notification("Admin");
            notification.Show();
            this.Hide();
        }

        private void InitializeAdditionalButtons()
        {
            Button overrideButton = new Button();
            overrideButton.Location = new System.Drawing.Point(162, 317);
            overrideButton.Size = new System.Drawing.Size(92, 43);
            overrideButton.Text = "Override Approvals";
            overrideButton.Click += (sender, args) =>
            {
                OverrideApprovalForm form = new OverrideApprovalForm();
                form.Show();
                Hide();
            };
            Controls.Add(overrideButton);

            Button tableLayoutButton = new Button();
            tableLayoutButton.Location = new System.Drawing.Point(540, 317);
            tableLayoutButton.Size = new System.Drawing.Size(102, 43);
            tableLayoutButton.Text = "Table Layout";
            tableLayoutButton.Click += (sender, args) =>
            {
                TableLayoutManagementForm form = new TableLayoutManagementForm();
                form.Show();
                Hide();
            };
            Controls.Add(tableLayoutButton);
        }
    }
}
