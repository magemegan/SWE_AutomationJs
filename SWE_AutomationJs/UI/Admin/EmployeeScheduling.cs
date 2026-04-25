using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class EmployeeScheduling : Form
    {
        public EmployeeScheduling()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {// admin menu
            NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
        }

        private void button3_Click(object sender, EventArgs e)
        {//view requests
            NavigationHelper.ShowAtCurrentPosition(this, new ViewRequests());
        }

        private void EmployeeScheduling_Load(object sender, EventArgs e)
        {
            SetupScheduleGrid();
            LoadSampleSchedule();
        }

        private void SetupScheduleGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // Set up columns for employee name and days of the week
            dataGridView1.Columns.Add("Employee", "Employee");
            dataGridView1.Columns.Add("Sunday", "Sunday");
            dataGridView1.Columns.Add("Monday", "Monday");
            dataGridView1.Columns.Add("Tuesday", "Tuesday");
            dataGridView1.Columns.Add("Wednesday", "Wednesday");
            dataGridView1.Columns.Add("Thursday", "Thursday");
            dataGridView1.Columns.Add("Friday", "Friday");
            dataGridView1.Columns.Add("Saturday", "Saturday");

            // Set properties for better appearance
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.Columns["Employee"].ReadOnly = true;
        }

        private void LoadSampleSchedule()
        {
            dataGridView1.Rows.Add("Alice", "9am-5pm", "9am-5pm", "Off", "9am-5pm", "9am-5pm", "Off");
            dataGridView1.Rows.Add("Bob", "Off", "9am-5pm", "9am-5pm", "Off", "9am-5pm", "9am-5pm");
            dataGridView1.Rows.Add("Charlie", "9am-5pm", "Off", "9am-5pm", "9am-5pm", "Off", "9am-5pm");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
