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
        private int editIndex = -1; // -1 indicates adding a new employee

        public AddEmployee()
        {
            InitializeComponent();
        }
        public AddEmployee(int selectedIndex)
        {
            InitializeComponent();
            editIndex = selectedIndex;
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            if (editIndex != -1)
            {
                Employee employeeToEdit = EmployeeStorage.Employees[editIndex];

                textBox1.Text = employeeToEdit.Name;
                comboBox1.SelectedItem = employeeToEdit.Role;
                textBox2.Text = employeeToEdit.Phone;
                textBox3.Text = employeeToEdit.Email;
                comboBox2.SelectedItem = employeeToEdit.EmploymentType;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//save
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter a Name.");
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Role.");
                return;
            }
            if(textBox2.Text == "")
            {
                MessageBox.Show("Please Enter a Phone Number.");
                return;
            }
            if(textBox3.Text == "")
            {
                MessageBox.Show("Please Enter an Email.");
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter an Employment Type.");
                return;
            }

            if(editIndex == -1)
            {
                Employee newEmployee = new Employee();
                newEmployee.Name = textBox1.Text;
                newEmployee.Role = comboBox1.SelectedItem.ToString();
                newEmployee.Phone = textBox2.Text;
                newEmployee.Email = textBox3.Text;
                newEmployee.EmploymentType = comboBox2.SelectedItem.ToString();

                EmployeeStorage.Employees.Add(newEmployee);
                MessageBox.Show("Employee added successfully!");
            }
            else
            {
                Employee employeeToEdit = EmployeeStorage.Employees[editIndex];
                employeeToEdit.Name = textBox1.Text;
                employeeToEdit.Role = comboBox1.SelectedItem.ToString();
                employeeToEdit.Phone = textBox2.Text;
                employeeToEdit.Email = textBox3.Text;
                employeeToEdit.EmploymentType = comboBox2.SelectedItem.ToString();
            }

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            //back to employee records
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
