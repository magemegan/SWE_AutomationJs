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

            listBox2.Items.Clear();

            foreach (string item in selectedOrder.Items)
            {
                listBox2.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//preparing
            int SelectedIndex = listBox1.SelectedIndex;    

            if (SelectedIndex == -1) return;
            if (SelectedIndex >= OrderStorage.IncomingOrder.Count) return;

            OrderStorage.IncomingOrder[SelectedIndex].Status = "Preparing";

            label5.Text = "Status: Preparing";
            LoadOrderQueue();
        }

        private void button3_Click(object sender, EventArgs e)
        {//ready
            int foodindex =listBox2.SelectedIndex;
            int orderindex = listBox1.SelectedIndex;
            if (foodindex == -1) return;
            if (orderindex == -1) return;

            if (orderindex >= OrderStorage.IncomingOrder.Count) return;
            Order selectedorder = OrderStorage.IncomingOrder[orderindex];

            if (foodindex >= selectedorder.Items.Count) return;

            selectedorder.Items.RemoveAt(foodindex);

            if (selectedorder.Items.Count == 0)
            {
                OrderStorage.IncomingOrder.RemoveAt(orderindex);

                label4.Text = "Table: ";
                label5.Text = "Status: ";
                listBox2.Items.Clear();
            }
            else
            {
                listBox2.Items.Clear();

                foreach (string item in selectedorder.Items)
                {
                    listBox2.Items.Add(item);
                }

                if(listBox2.Items.Count > 0) { listBox2.SelectedIndex = 0; }
            }
            LoadOrderQueue();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
