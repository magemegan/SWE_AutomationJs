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
    public partial class EmployeeRecords : Form
    {
        public EmployeeRecords()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {//add employee
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//back to admin menu
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {//edit employee
            int selectedIndex = listBox1.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select an employee to edit.");
                return;
            }

            AddEmployee addEmployee = new AddEmployee(selectedIndex);
            addEmployee.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {//delete employee
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void LoadEmployeeQueue()
        {
            listBox1.Items.Clear();
            // Load the incoming orders into listBox1
            foreach (Employee employee in EmployeeStorage.Employees)
            {
                listBox1.Items.Add($"{employee.Name} ({employee.Role})");
            }
        }

        private void EmployeeRecords_Load(object sender, EventArgs e)
        {
            LoadEmployeeQueue();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;

            Employee worker = EmployeeStorage.Employees[selectedIndex];

            // Display the details of the selected request
            label8.Text = $"Name: {worker.Name}";
            label7.Text = $"Role: {worker.Role}";
        }

        private void button3_Click(object sender, EventArgs e)
        {//view employee
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select an employee to view.");
                return;
            }

            EmployeeProfile employeeProfile = new EmployeeProfile(selectedIndex);
            employeeProfile.Show();
            this.Hide();
        }
    }
}
