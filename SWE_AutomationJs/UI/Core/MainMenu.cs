using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            AcceptButton = button1;
            textBox2.UseSystemPasswordChar = true;
            InitializeHomeDetails();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthenticatedEmployee employee = AuthRepository.Authenticate(textBox1.Text, textBox2.Text);
            if (employee == null)
            {
                ActivityLogService.Log(textBox1.Text.Trim(), "LoginFailed", "Invalid credentials");
                MessageBox.Show("Invalid employee ID or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Clear();
                textBox2.Focus();
                return;
            }

            SessionContext.Start(employee);
            ApplyCurrentEmployee(employee);
            ActivityLogService.Log(employee.EmployeeId, "LoginSuccess", employee.RoleName);
            OpenHomeScreen(employee);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {//exit button
            Application.Exit();
        }

        private static void ApplyCurrentEmployee(AuthenticatedEmployee employee)
        {
            string displayName = employee.DisplayName;
            AdminMenu.CurrentEmployee = displayName;
            WaiterScreen.CurrentEmployee = displayName;
            KitchenScreen.CurrentEmployee = displayName;
            BusboyScreen.CurrentEmployee = displayName;
        }

        private void OpenHomeScreen(AuthenticatedEmployee employee)
        {
            Form nextScreen;
            switch (employee.RoleName)
            {
                case "Admin":
                case "Manager":
                    nextScreen = new AdminMenu();
                    break;
                case "Kitchen":
                    nextScreen = new KitchenScreen();
                    break;
                case "Server":
                    nextScreen = new WaiterScreen();
                    break;
                default:
                    nextScreen = new EmployeeMenu();
                    break;
            }

            nextScreen.Show();
            Hide();
        }

        private void InitializeHomeDetails()
        {
            Label detailsLabel = new Label();
            detailsLabel.AutoSize = true;
            detailsLabel.Location = new System.Drawing.Point(261, 120);
            detailsLabel.Name = "detailsLabel";
            detailsLabel.Size = new System.Drawing.Size(260, 39);
            detailsLabel.Text = "Hours: Mon-Sun 11:00 AM - 10:00 PM\r\nContact: (555) 010-3313\r\nAddress: 123 Campus Drive, Kennesaw, GA";
            Controls.Add(detailsLabel);
        }
    }
}
