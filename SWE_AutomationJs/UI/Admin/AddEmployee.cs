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

namespace SWE_AutomationJs_UI_Design
{
    public partial class AddEmployee : Form
    {
        private int editIndex = -1; // -1 indicates adding a new employee

        public AddEmployee()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }
        public AddEmployee(int selectedIndex)
        {
            InitializeComponent();
            editIndex = selectedIndex;
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "Waiter", "Kitchen", "Busboy", "Manager", "Admin" });

            if (editIndex != -1)
            {
                Employee employeeToEdit = EmployeeStorage.Employees[editIndex];
                textBoxEmployeeId.Text = employeeToEdit.EmployeeId;
                textBox1.Text = employeeToEdit.Name;
                comboBox1.SelectedItem = employeeToEdit.Role;
                textBox2.Text = employeeToEdit.Phone;
                textBox3.Text = employeeToEdit.Email;
                comboBox2.SelectedItem = employeeToEdit.EmploymentType;
                textBoxAssignedTables.Text = string.Join(",", employeeToEdit.AssignedTables);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//save
            string employeeId = textBoxEmployeeId.Text.Trim().ToUpperInvariant();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter a Name.");
                return;
            }
            if (employeeId == "")
            {
                MessageBox.Show("Please enter an employee ID.");
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
            if (editIndex == -1 && EmployeeStorage.Employees.Any(x => x.EmployeeId == employeeId))
            {
                MessageBox.Show("Employee ID already exists.");
                return;
            }

            List<int> assignedTables = ParseAssignedTables(textBoxAssignedTables.Text);
            if (comboBox1.SelectedItem.ToString() == "Waiter" && assignedTables.Count == 0)
            {
                MessageBox.Show("Please assign at least one table to each waiter.");
                return;
            }

            if(editIndex == -1)
            {
                Employee newEmployee = new Employee();
                newEmployee.EmployeeId = employeeId;
                newEmployee.Name = textBox1.Text;
                newEmployee.Role = comboBox1.SelectedItem.ToString();
                newEmployee.Phone = textBox2.Text;
                newEmployee.Email = textBox3.Text;
                newEmployee.EmploymentType = comboBox2.SelectedItem.ToString();
                newEmployee.AssignedTables = assignedTables;

                EmployeeStorage.Employees.Add(newEmployee);
                EmployeeStorage.Save();
                ActivityLogService.Log("MANAGER", "EmployeeProfileCreated", newEmployee.EmployeeId + " " + newEmployee.Name);
                MessageBox.Show("Employee added successfully!");
            }
            else
            {
                Employee employeeToEdit = EmployeeStorage.Employees[editIndex];
                employeeToEdit.EmployeeId = employeeId;
                employeeToEdit.Name = textBox1.Text;
                employeeToEdit.Role = comboBox1.SelectedItem.ToString();
                employeeToEdit.Phone = textBox2.Text;
                employeeToEdit.Email = textBox3.Text;
                employeeToEdit.EmploymentType = comboBox2.SelectedItem.ToString();
                employeeToEdit.AssignedTables = assignedTables;
                EmployeeStorage.Save();
                ActivityLogService.Log("MANAGER", "EmployeeProfileUpdated", employeeToEdit.EmployeeId + " " + employeeToEdit.Name);
            }

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBoxEmployeeId.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBoxAssignedTables.Clear();

            //back to employee records
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords());
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to employee records
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords());
        }

        private static List<int> ParseAssignedTables(string rawText)
        {
            return rawText
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value =>
                {
                    int parsed;
                    return int.TryParse(value, out parsed) ? parsed : 0;
                })
                .Where(value => value > 0)
                .Distinct()
                .ToList();
        }
    }
}
