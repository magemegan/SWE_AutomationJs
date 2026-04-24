using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Payment : Form
    {
        private IReadOnlyList<PaymentHistoryEntry> paymentHistory = Array.Empty<PaymentHistoryEntry>();

        public Payment()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            Hide();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            LoadPaymentQueue();
        }

        private void LoadPaymentQueue()
        {
            paymentHistory = PaymentRepository.GetPaymentHistory();
            listBox1.Items.Clear();

            foreach (PaymentHistoryEntry payment in paymentHistory)
            {
                listBox1.Items.Add($"Table {payment.TableId} - ${payment.TotalPaid:F2} - {payment.PaymentStatus}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= paymentHistory.Count)
            {
                return;
            }

            PaymentHistoryEntry selectedPayment = paymentHistory[selectedIndex];
            label4.Text = "Table: " + selectedPayment.TableId;
            label5.Text = "Status: " + selectedPayment.PaymentStatus;
            label6.Text = "Total: $" + selectedPayment.OrderTotal.ToString("F2");
            label7.Text = "Date: " + (selectedPayment.PaidAt.HasValue
                ? selectedPayment.PaidAt.Value.ToString("MM/dd/yyyy")
                : "N/A");
            label8.Text = "Items:";

            listBox2.Items.Clear();
            foreach (OrderLine item in OrderRepository.GetItems(selectedPayment.OrderId))
            {
                listBox2.Items.Add($"- {item.ItemName} x{item.Qty}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refund processing is not implemented yet.");
        }
    }
}
