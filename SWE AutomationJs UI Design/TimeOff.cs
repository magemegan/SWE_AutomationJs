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
            Schedule schedule = new Schedule();
            schedule.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {//submit request
            //ScheduleRequest chosenRequest = new ScheduleRequest();
            //chosenRequest.RequestType = new List<string>();
            //chosenRequest.Status = "Pending";

            //foreach (var item in listBox2.Items)
            //{
            //    newOrder.Items.Add(item.ToString());
            //}

            //OrderStorage.IncomingOrders.Add(newOrder);

            //MessageBox.Show("Requests sent to Manager.");

            //textBox1.Requests.Clear();
        }
    }
}
