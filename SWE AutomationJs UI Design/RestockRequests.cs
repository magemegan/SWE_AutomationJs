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
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void RequestLoad()
        {
            listBox1.Items.Clear();
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

            Request selectedRequest = RequestStorage.RequestOrder[index];

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
            Request selectedRequest = RequestStorage.RequestOrder[index];
            selectedRequest.Status = "Approved";
            label4.Text = "Status: " + selectedRequest.Status;
            RequestStorage.RequestOrder.RemoveAt(index);
            RequestLoad();

        }

        private void button3_Click(object sender, EventArgs e)
        {//deny button
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            Request selectedRequest = RequestStorage.RequestOrder[index];
            selectedRequest.Status = "Denied";
            label4.Text = "Status: " + selectedRequest.Status;
            RequestStorage.RequestOrder.RemoveAt(index);
            RequestLoad();

        }
    }
}
