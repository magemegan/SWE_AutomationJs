using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void Inventory_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {//back button
            KitchenScreen kitchenScreen = new KitchenScreen();
            kitchenScreen.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {// send to request to manager
            Request newRequest = new Request();
            newRequest.Quantity = (int)numericUpDown1.Value;
            newRequest.Priority = comboBox2.Text;
            newRequest.ItemName = textBox2.Text;
            newRequest.Notes = textBox1.Text;   
            newRequest.Status = "Pending";

            RequestStorage.RequestOrder.Add(newRequest);

            MessageBox.Show("Request sent to Manager.");

            // Clear the form for the next request
            textBox2.Clear();
            comboBox2.SelectedIndex = -1;
            numericUpDown1.Value = 1;
            textBox1.Clear();

            Notifications notifications = new Notifications();
            notifications.Message = $"New inventory request for {newRequest.ItemName} has been sent.";
            notifications.Role = "Manager";
            notifications.TimeStamp = DateTime.Now.ToString("g");
            NotificationStorage.Notify.Add(notifications);
        }
    }
}
