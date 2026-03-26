using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class CleaningScreen : Form
    {
        public CleaningScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to busboy screen
            BusboyScreen busboyScreen = new BusboyScreen();
            busboyScreen.Show();
            this.Hide();
        }

        private void CleaningScreen_Load(object sender, EventArgs e)
        {
            LoadCleaningQueue();
        }

        private void LoadCleaningQueue()
        {
            listBox1.Items.Clear();
            foreach (Order order in OrderStorage.IncomingOrder)
            {
                listBox1.Items.Add($"Table {order.TableNumber} - {order.Status}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;

            Order selectedOrder = OrderStorage.IncomingOrder[selectedIndex];

            label4.Text = "Table: " + selectedOrder.TableNumber;
            label5.Text = "Status: " + selectedOrder.Status;

            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {//mark table as clean
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex == -1) return;
            Order selectedOrder = OrderStorage.IncomingOrder[selectedIndex];
            selectedOrder.Status = "Cleaned";
            LoadCleaningQueue();
        }
    }
}
