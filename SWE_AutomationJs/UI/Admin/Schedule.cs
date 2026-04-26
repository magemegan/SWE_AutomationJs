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
    public partial class Schedule : Form
    {
        private string previousScreen;
        public Schedule(string previousScreen)
        {
            InitializeComponent();
            UiTheme.Apply(this);
            this.previousScreen = previousScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {// go back
            if (previousScreen == "Busboy")
            {
                BusboyScreen busboyScreen = new BusboyScreen();
                busboyScreen.Show();
                this.Hide();
            }
            else if (previousScreen == "Kitchen")
            {
                KitchenScreen kitchenScreen = new KitchenScreen();
                kitchenScreen.Show();
                this.Hide();
            }
            else if (previousScreen == "Waiter")
            {
                WaiterScreen waiterScreen = new WaiterScreen();
                waiterScreen.Show();
                this.Hide();
            }
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            SetupScheduleGrid();
            LoadSampleSchedule();
        }

        private void SetupScheduleGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // Set up columns for employee name and days of the week
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

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
        }

        private void LoadSampleSchedule()
        {
            dataGridView1.Rows.Add("9am-5pm", "Off", "10am-6pm", "9am-5pm", "Off", "5am-1pm");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {//request off
            TimeOff dayOff = new TimeOff(previousScreen);
            dayOff.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {//swap shift
            Swap swapShift = new Swap(previousScreen);
            swapShift.Show();
            this.Hide();
        }
    }
}
