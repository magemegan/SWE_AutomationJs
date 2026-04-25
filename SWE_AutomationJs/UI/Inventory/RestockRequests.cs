using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class RestockRequests : Form
    {
        public RestockRequests()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back button
            NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
        }

        private void RequestLoad()
        {
            listBox1.Items.Clear();
            // Load requests from storage and display in listbox
            foreach (Request request in RequestStorage.RequestOrder)
            {
                listBox1.Items.Add($"{request.ItemName} - Quantity: {request.Quantity} - Priority: {request.Priority} - Status: {request.Status}");
            }
        }

        private void RestockRequests_Load(object sender, EventArgs e)
        {
            RequestLoad();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            // Get the corresponding request based on the selected index
            Request selectedRequest = RequestStorage.RequestOrder[index];
            // Display request details in labels and textbox
            label2.Text = "Item: " + selectedRequest.ItemName;
            label3.Text = "Quantity: " + selectedRequest.Quantity;
            label4.Text = "Priority: " + selectedRequest.Priority;
            label5.Text = "Status: " + selectedRequest.Status;
            textBox1.Text = selectedRequest.Notes;
        }

        private void button2_Click(object sender, EventArgs e)
        {//approve button
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            // Get the corresponding request based on the selected index
            Request selectedRequest = RequestStorage.RequestOrder[index];
            selectedRequest.Status = "Approved";
            // Update the status label to reflect the change
            label4.Text = "Status: " + selectedRequest.Status;
            RequestStorage.RequestOrder.RemoveAt(index);// Remove the request from storage after approval
            RequestLoad();

            Notifications n = new Notifications();
            n.Message = "Restock request approved. Supplies will be ordered soon.";
            n.SentBy = "Manager";
            n.Role = "Kitchen";
            n.Employee = "";
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);

        }

        private void button3_Click(object sender, EventArgs e)
        {//deny button
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            // Get the corresponding request based on the selected index
            Request selectedRequest = RequestStorage.RequestOrder[index];
            selectedRequest.Status = "Denied";
            // Update the status label to reflect the change
            label4.Text = "Status: " + selectedRequest.Status;
            RequestStorage.RequestOrder.RemoveAt(index);// Remove the request from storage after denial
            RequestLoad();

            Notifications n = new Notifications();
            n.Message = "Restock request was denied.";
            n.SentBy = "Manager";
            n.Role = "Kitchen";
            n.Employee = "";
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }
    }
}
