using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class ViewRequests : Form
    {
        public ViewRequests()
        {
            InitializeComponent();
        }

        private void LoadRequestQueue()
        {
            listBox1.Items.Clear();
            // Load the incoming orders into listBox1
            foreach (ScheduleRequest request in ScheduleRequestStorage.ScheduleRequests)
            {
                listBox1.Items.Add($"{request.EmployeeName} - {request.RequestType} - {request.Status}");
            }
        }

        private void ViewRequests_Load(object sender, EventArgs e)
        {
            LoadRequestQueue();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;

            ScheduleRequest selectedRequest = ScheduleRequestStorage.ScheduleRequests[selectedIndex];

            // Display the details of the selected request
            label8.Text = $"Employee: {selectedRequest.EmployeeName}";
            label7.Text = $"Type: {selectedRequest.RequestType}";
            label4.Text = $"Date: {selectedRequest.ShiftDate}";
            label5.Text = $"Status: {selectedRequest.Status}";
            label6.Text = $"Reason: {selectedRequest.Details}";
        }

        private void button5_Click(object sender, EventArgs e)
        {//approve
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;
            ScheduleRequest selectedRequest = ScheduleRequestStorage.ScheduleRequests[selectedIndex];
            ScheduleRequestStorage.ScheduleRequests[selectedIndex].Status = "Approved";
            selectedRequest.Status = "Approved";
            LoadRequestQueue();

            Notifications n = new Notifications();
            n.Message = "Your request for " + selectedRequest.ShiftDate + " was approved.";
            n.SentBy = "Manager";
            n.Role = "";
            n.Employee = selectedRequest.EmployeeName;
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }

        private void button4_Click(object sender, EventArgs e)
        {//deny
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;
            ScheduleRequest selectedRequest = ScheduleRequestStorage.ScheduleRequests[selectedIndex];
            ScheduleRequestStorage.ScheduleRequests[selectedIndex].Status = "Denied";
            selectedRequest.Status = "Denied";
            LoadRequestQueue();

            Notifications n = new Notifications();
            n.Message = "Your request was denied";
            n.SentBy = "Manager";
            n.Role = "";
            n.Employee = selectedRequest.EmployeeName;
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }

        private void button1_Click(object sender, EventArgs e)
        {//back
            EmployeeScheduling scheduling = new EmployeeScheduling();
            scheduling.Show();
            this.Hide();
        }
    }
}
