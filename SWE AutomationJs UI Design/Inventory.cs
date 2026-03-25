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
            newRequest.Priority = comboBox2.SelectedItem.ToString();
            newRequest.ItemName = comboBox1.SelectedItem.ToString();
            newRequest.Status = "Pending";

            foreach (var item in listBox2.Items)
            {
                newRequest.ItemName.Add(item.ToString());
            }

            RequestStorage.RequestOrder.Add(newRequest);

            MessageBox.Show("Request sent to Manager.");
        }
    }
}
