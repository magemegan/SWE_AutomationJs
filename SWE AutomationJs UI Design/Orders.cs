using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Orders : Form
    {
        public int tableNumber;
        private int chosenTable;
        double currentTotal = 0.0;
        public Orders(int tableNumber)
        {
            InitializeComponent();
            chosenTable = tableNumber;

        }

        private void Orders_load(object sender, EventArgs e)
        {
            //display the table number in label2
            label2.Text = $"Table: {chosenTable}";
            label3.Text = $"Total: ${currentTotal.ToString("F2")}";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {//go back
            AssignTables assignTables = new AssignTables();
            assignTables.Show();
            this.Hide();
        }

        private void AddItemToOrder(string itemName, double price)
        {
            listBox2.Items.Add($"{itemName} - ${price.ToString("F2")}");
            currentTotal += price;
            label3.Text = $"Total: ${currentTotal.ToString("F2")}";
        }   

        private void button4_Click(object sender, EventArgs e)
        {//send to kitchen
            Order newOrder = new Order();
            newOrder.TableNumber = chosenTable;
            newOrder.Items = new List<string>();
            newOrder.Status = "Pending";
            // Add items from listBox2 to the order
            foreach (var item in listBox2.Items)
            {
                newOrder.Items.Add(item.ToString());
            }

            OrderStorage.IncomingOrder.Add(newOrder);

            MessageBox.Show("Order sent to the kitchen!");
        }

        private void button3_Click(object sender, EventArgs e)
        {//clear order
            if (listBox2.Items.Count > 0)
            {
                listBox2.Items.Clear();
            }
            currentTotal = 0.0;
            label3.Text = $"Total: ${currentTotal.ToString("F2")}";
        }

        private void button2_Click(object sender, EventArgs e)
        {//remove item
            int index = listBox2.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            string selecteditem = listBox2.Items[index].ToString();

            string[] itemParts = selecteditem.Split('$');
            double price = Convert.ToDouble(itemParts[1]);

            currentTotal -= price;

            listBox2.Items.Remove(listBox2.SelectedItem);
            label3.Text = $"Total: ${currentTotal.ToString("F2")}"; 
        }

        private void button5_Click(object sender, EventArgs e)
        {//food 1
            AddItemToOrder("CheeseBurger", 10);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chicken Alfredo", 14);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Grilled Chicken Sandwich", 11);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Fries", 4);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Side Salad", 5);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Onion Rings", 5.50);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Soda", 3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Lemonade", 3.30);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Iced Tea", 2.50);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Cheesecake", 6);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Brownie", 5);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Cookie", 3);
        }

        private void button14_Click(object sender, EventArgs e)
        {//payment stuff
            if (listBox2.Items.Count == 0)
            {
                MessageBox.Show("No items in the order to pay for.");
                return;
            }

            List<string> items = new List<string>();

            foreach (var item in listBox2.Items)
            {
                items.Add(item.ToString());
            }

            ProcessPayment paid = new ProcessPayment(chosenTable, items, currentTotal);
            paid.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
