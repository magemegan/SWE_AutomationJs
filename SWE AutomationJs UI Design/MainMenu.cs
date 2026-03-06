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
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this logs the user in depending on their clarence level.
             if (textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                //open admin menu
                AdminMenu adminMenu = new AdminMenu();
                adminMenu.Show();
                this.Hide();
            }
             else if (textBox1.Text == "user" && textBox2.Text == "user")
            {
                //open user menu
                EmployeeMenu userMenu = new EmployeeMenu();
                userMenu.Show();
                this.Hide();
            }
             else
            {
                MessageBox.Show("Invalid username or password.");
            }
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
    }
}
