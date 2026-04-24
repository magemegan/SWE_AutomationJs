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
        private string previousScreen;
        public TimeOff(string previousScreen)
        {
            InitializeComponent();
            this.previousScreen = previousScreen;
        }

        private void button2_Click(object sender, EventArgs e)
        {//back
            Schedule schedule = new Schedule(previousScreen);
            schedule.Show();
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

            ScheduleRequestStorage.ScheduleRequests.Add(request);

            textBox1.Clear();

            Notifications n = new Notifications();
            n.Message = "Your TimeOff Request has been sent.";
            n.Role = "Manager";
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }

        private void TimeOff_Load(object sender, EventArgs e)
        {

        }
    }
}
