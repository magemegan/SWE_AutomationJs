using System;
using System.Drawing;
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

            UiTheme.Apply(this);

            AcceptButton = button1;
            textBox2.UseSystemPasswordChar = true;

            BuildCleanLoginUI();
        }

        private void BuildCleanLoginUI()
        {
            Text = "Automation of J's - Login";

            // TITLE
            label3.Text = "Automation of J's";
            label3.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            label3.ForeColor = UiTheme.PrimaryDark;
            label3.AutoSize = true;
            label3.Location = new Point(250, 80);

            // EMPLOYEE ID
            label1.Text = "Employee ID";
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(250, 170);

            textBox1.Font = new Font("Segoe UI", 11F);
            textBox1.Size = new Size(300, 30);
            textBox1.Location = new Point(250, 195);

            // PASSWORD
            label2.Text = "Password";
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(250, 240);

            textBox2.Font = new Font("Segoe UI", 11F);
            textBox2.Size = new Size(300, 30);
            textBox2.Location = new Point(250, 265);

            // BUTTONS
            button1.Text = "Login";
            button1.Size = new Size(140, 36);
            button1.Location = new Point(250, 320);

            button2.Text = "Exit";
            button2.Size = new Size(140, 36);
            button2.Location = new Point(410, 320);

            // INFO TEXT
            label4.Text =
                "Seed Logins:\n" +
                "Admin: E00001 / Admin@123\n" +
                "Waiter: E00003 / Server@123\n" +
                "Kitchen: E00005 / Kitchen@123\n" +
                "Busboy: E00010 / Busboy@123";

            label4.Font = new Font("Segoe UI", 9F);
            label4.ForeColor = Color.DimGray;
            label4.AutoSize = true;
            label4.Location = new Point(250, 380);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthenticatedEmployee employee =
                AuthRepository.Authenticate(textBox1.Text, textBox2.Text);

            if (employee == null)
            {
                ActivityLogService.Log(
                    textBox1.Text.Trim(),
                    "LoginFailed",
                    "Invalid credentials"
                );

                MessageBox.Show(
                    "Invalid employee ID or password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                textBox2.Clear();
                textBox2.Focus();
                return;
            }

            SessionContext.Start(employee);
            ApplyCurrentEmployee(employee);

            ActivityLogService.Log(
                employee.EmployeeId,
                "LoginSuccess",
                employee.RoleName
            );

            OpenHomeScreen(employee);
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
                case "Waiter":
                case "Waitress":
                    nextScreen = new WaiterScreen();
                    break;

                case "Busboy":
                    nextScreen = new BusboyScreen();
                    break;

                default:
                    nextScreen = new EmployeeMenu();
                    break;
            }

            NavigationHelper.ShowAtCurrentPosition(this, nextScreen);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}