using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//go back
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            this.Hide();
        }

        private void LoadPaymentQueue()
        {
            listBox1.Items.Clear();
            // Load the incoming orders into listBox1
            foreach (PastPayment payment in PastPaymentStorage.Payments)
            {
                listBox1.Items.Add($"Table {payment.TableNumber} - {payment.Total} - {payment.Status}");
            }
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            LoadPaymentQueue();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected index from listBox1
            int selectedIndex = listBox1.SelectedIndex;
            // Check if a valid index is selected
            if (selectedIndex == -1) return;
            // Get the corresponding order based on the selected index
            PastPayment selectedPayment = PastPaymentStorage.Payments[selectedIndex];
            // Display the order details in the labels and listBox2
            label4.Text = "Table: " + selectedPayment.TableNumber;
            label5.Text = "Status: " + selectedPayment.Status;
            label6.Text = "Total: $" + selectedPayment.Total;
            label7.Text = "Date: " + selectedPayment.Date;
            label8.Text = "Items:";

            listBox2.Items.Clear();
            // Load the items of the selected order into listBox2
            foreach (string item in selectedPayment.Items)
            {
                listBox2.Items.Add($"- {item}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//refund?
            // // Get the selected index from listBox1
            //int selectedIndex = listBox1.SelectedIndex;
            //// Check if a valid index is selected
            //if (selectedIndex == -1) return;
            //// Get the corresponding order based on the selected index
            //PastPayment selectedPayment = PastPaymentStorage.Payments[selectedIndex];
            //// Update the status of the selected payment to "Refunded"
            //selectedPayment.Status = "Refunded";
            //// Refresh the payment queue to reflect the updated status
            //LoadPaymentQueue();

        }
    }
}
