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
    public partial class ProcessPayment : Form
    {
        private int chosenTable;
        private List<string> orderItems;
        private double subtotal;
        private double tax = 0.08;
        private double taxAmount;
        private double totalAmount;

        public ProcessPayment()
        {
            InitializeComponent();
        }

        public ProcessPayment(int tableNumber, List<string> orderItems, double subtotal, double tax, double taxAmount, double totalAmount)
        {
            InitializeComponent();
            chosenTable = tableNumber;
            this.orderItems = orderItems;
            this.subtotal = subtotal;
            this.tax = tax;
            this.taxAmount = taxAmount;
            this.totalAmount = totalAmount;
        }

        private void ProcessPayment_Load(object sender, EventArgs e)
        {
            label2.Text = $"Table: {chosenTable}";

            // Display order items in listBox1
            listBox1.Items.Clear();
            foreach (var item in orderItems)
            {
                listBox1.Items.Add(item);
            }

            taxAmount = subtotal * tax;
            totalAmount = subtotal + taxAmount;

            label5.Text = $"Subtotal: ${subtotal.ToString("F2")}";
            label6.Text = $"Tax: ${taxAmount.ToString("F2")}";
            label7.Text = $"Total: {totalAmount.ToString("F2")}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {//payment stuff
            MessageBox.Show("Payment Processed for Table " + chosenTable);
            Orders orders = new Orders(chosenTable);
            orders.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {//print receipt
            MessageBox.Show("Receipt Printed for Table " + chosenTable);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
