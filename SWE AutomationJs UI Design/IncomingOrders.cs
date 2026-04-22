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
            // Load the incoming orders into listBox1
            foreach (Order order in OrderStorage.IncomingOrder)
            {
                listBox1.Items.Add($"Table {order.TableNumber} - {order.Status}");
            }
        }   

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected index from listBox1
            int selectedIndex = listBox1.SelectedIndex;
            // Check if a valid index is selected
            if (selectedIndex == -1) return;
            // Get the corresponding order based on the selected index
            Order selectedOrder = OrderStorage.IncomingOrder[selectedIndex];
            // Display the order details in the labels and listBox2
            label4.Text = "Table: " + selectedOrder.TableNumber;
            label5.Text = "Status: " + selectedOrder.Status;

            listBox2.Items.Clear();
            // Load the items of the selected order into listBox2
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
            // Get the selected food item index and order index
            int foodindex =listBox2.SelectedIndex;
            int orderindex = listBox1.SelectedIndex;
            if (foodindex == -1) return;
            if (orderindex == -1) return;
            // Check if the selected order index is within the bounds of the incoming orders list
            if (orderindex >= OrderStorage.IncomingOrder.Count) return;
            Order selectedorder = OrderStorage.IncomingOrder[orderindex];
            // Check if the selected food index is within the bounds of the items list for the selected order
            if (foodindex >= selectedorder.Items.Count) return;
            // Remove the ready item from the order's items list
            selectedorder.Items.RemoveAt(foodindex);

            if (selectedorder.Items.Count == 0)
            {
                // If all items are ready, update the table status to "NeedsCleaning"
                int tableNumber = selectedorder.TableNumber;
                TableStorage.TableStatuses[tableNumber] = "Needs Cleaning";
                // Remove the order from the incoming orders list
                OrderStorage.IncomingOrder.RemoveAt(orderindex);
                // Clear the details of the removed order
                label4.Text = "Table: ";
                label5.Text = "Status: ";
                listBox2.Items.Clear();

                Notifications b = new Notifications();
                b.Message = "Table " + selectedorder.TableNumber + " needs cleaning.";
                b.Role = "Busboy";
                b.TimeStamp = DateTime.Now.ToShortTimeString();

                NotificationStorage.Notify.Add(b);
            }
            else
            {
                listBox2.Items.Clear();
                // Refresh the items in listBox2 after removing the ready item
                foreach (string item in selectedorder.Items)
                {
                    listBox2.Items.Add(item);
                }
            }
            LoadOrderQueue();

            Notifications n = new Notifications();
            n.Message = "Order for Table " + selectedorder.TableNumber + " is ready.";
            n.Role = "Waiter";
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
