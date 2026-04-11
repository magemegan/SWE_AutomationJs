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
    public partial class AddEmployee : Form
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {//save
            string name = textBox1.Text;
            string role = comboBox1.SelectedItem.ToString();
            Employee newEmployee = new Employee();
            EmployeeStorage.Employees.Add(newEmployee);
            MessageBox.Show("Employee added successfully!");
            EmployeeRecords employeeRecords = new EmployeeRecords();
            employeeRecords.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to employee records
            EmployeeRecords employeeRecords = new EmployeeRecords();
            employeeRecords.Show();
            this.Hide();
        }
    }
}
