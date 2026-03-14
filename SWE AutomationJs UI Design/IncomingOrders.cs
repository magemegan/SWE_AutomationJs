using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class IncomingOrders : Form
    {
        Order order;
        public IncomingOrders()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back
            KitchenScreen kitchenScreen = new KitchenScreen();
            kitchenScreen.Show();
            this.Hide();
        }

        private void IncomingOrders_Load(object sender, EventArgs e)
        {
            LoadOrderQueue();
        }

        private void LoadOrderQueue()
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

            foreach (string item in selectedOrder.Items)
            {
                listBox2.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//preparing
            int SelectedIndex = listBox1.SelectedIndex;    

            if (SelectedIndex == -1) return;

            OrderStorage.IncomingOrder[SelectedIndex].Status = "Preparing";
            LoadOrderQueue();
        }

        private void button3_Click(object sender, EventArgs e)
        {//ready
            int index =listBox2.SelectedIndex;
            if (index == -1) return;

            listBox2.Items.Clear();

            OrderStorage.IncomingOrder[index].Status = "Ready";
            LoadOrderQueue();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
