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
    public partial class EmployeeProfile : Form
    {
        private int selectedIndex;
        private Label labelAssignedTables;
        public EmployeeProfile(int employeeIndex)
        {
            InitializeComponent();
            selectedIndex = employeeIndex;
            labelAssignedTables = new Label();
            labelAssignedTables.AutoSize = true;
            labelAssignedTables.Font = label8.Font;
            labelAssignedTables.Location = new System.Drawing.Point(114, 315);
            Controls.Add(labelAssignedTables);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void EmployeeProfile_Load(object sender, EventArgs e)
        {
            if (selectedIndex < 0 || selectedIndex >= EmployeeStorage.Employees.Count)
            {
                MessageBox.Show("Invalid employee index.");
                this.Close();
                return;
            }

            Employee selectedEmployee = EmployeeStorage.Employees[selectedIndex];

            label8.Text = $"Name: {selectedEmployee.Name} [{selectedEmployee.EmployeeId}]";
            label7.Text = $"Role: {selectedEmployee.Role}";
            label4.Text = $"Phone: {selectedEmployee.Phone}";
            label5.Text = $"Email: {selectedEmployee.Email}";
            label6.Text = $"Employment Type: {selectedEmployee.EmploymentType}";
            labelAssignedTables.Text = $"Assigned Tables: {string.Join(",", selectedEmployee.AssignedTables)}";
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to employee records
            NavigationHelper.ShowAtCurrentPosition(this, new EmployeeRecords());
        }
    }
}
