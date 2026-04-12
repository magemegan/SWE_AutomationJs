using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AdminMenu : Form
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//logout
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
            //SalesReports salesReports = new SalesReports();
            //salesReports.Show();
            //this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {//employee scheduling
            EmployeeScheduling employeeScheduling = new EmployeeScheduling();
            employeeScheduling.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {//employee profile
            //EmployeeProfile employeeProfile = new EmployeeProfile();
            //employeeProfile.Show();
            //this.Hide();
        }
    }
}
