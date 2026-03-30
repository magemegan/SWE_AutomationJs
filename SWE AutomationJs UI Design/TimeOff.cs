using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class TimeOff : Form
    {
        public TimeOff()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {//back
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {//submit request
            ScheduleRequest request = new ScheduleRequest();
            request.EmployeeName = " ";
            request.RequestType = "Time Off";
            request.Details = textBox1.Text;
            request.Status = "Pending";
            DateTime selectedDate = dateTimePicker1.Value;
            request.ShiftDate = selectedDate;
            MessageBox.Show("Time off request submitted for " + request.ShiftDate.ToShortDateString() + ". Status: " + request.Status);

            ScheduleRequestStorage.ScheduleRequests.Add(request);

            textBox1.Clear();
        }

        private void TimeOff_Load(object sender, EventArgs e)
        {

        }
    }
}
