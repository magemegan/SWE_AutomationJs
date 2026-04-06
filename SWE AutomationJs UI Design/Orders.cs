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
        public Orders(int tableNumber)
        {
            InitializeComponent();
            chosenTable = tableNumber;

        }

        private void Orders_load(object sender, EventArgs e)
        {
            //display the table number in label2
            label2.Text = $"Table: {chosenTable}";
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

            listBox2.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {//clear order
            if (listBox2.Items.Count > 0)
            {
                listBox2.Items.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//remove item
            if (listBox2.SelectedItem != null)
            {
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//food 1
            listBox2.Items.Add("Food 1");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 3");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 4");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 5");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 6");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 7");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 8");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add("Food 9");
        }

        private void button14_Click(object sender, EventArgs e)
        {//payment stuff
            PastPayment pastPayment = new PastPayment();    

            pastPayment.TableNumber = chosenTable;
            pastPayment.Items = new List<string>();

            foreach (var item in listBox2.Items)
            {
                pastPayment.Items.Add(item.ToString());
            }
            pastPayment.Total = pastPayment.Items.Count * 10; // Assuming each item costs 10 units
            pastPayment.Status = "Paid";
            pastPayment.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            PastPaymentStorage.Payments.Add(pastPayment);
            listBox2.Items.Clear();
        }
    }
}
