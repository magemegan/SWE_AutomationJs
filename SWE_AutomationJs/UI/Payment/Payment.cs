using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

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
            NavigationHelper.ShowAtCurrentPosition(this, new WaiterScreen());
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
                listBox1.Items.Add($"Table {payment.TableId} - ${payment.NetPaid:F2} - {payment.PaymentStatus}");
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
                string details = string.IsNullOrWhiteSpace(item.Notes)
                    ? string.Empty
                    : $" [{item.Notes}]";

                listBox2.Items.Add($"- {item.ItemName} x{item.Qty}{details}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;

            if (selectedIndex < 0 || selectedIndex >= paymentHistory.Count)
            {
                MessageBox.Show("Please select a payment to refund.");
                return;
            }

            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("Please sign in again before processing a refund.");
                return;
            }

            string currentRole = SessionContext.CurrentEmployee.RoleName;

            if (currentRole != "Manager" && currentRole != "Admin")
            {
                MessageBox.Show("Refunds require Manager or Admin approval.");
                return;
            }

            PaymentHistoryEntry selectedPayment = paymentHistory[selectedIndex];

            if (!selectedPayment.PaymentId.HasValue)
            {
                MessageBox.Show("This order does not have an available payment to refund.");
                return;
            }

            if (selectedPayment.TotalRefunded >= selectedPayment.TotalPaid)
            {
                MessageBox.Show("This payment has already been fully refunded.");
                return;
            }

            decimal refundAmount = selectedPayment.TotalPaid - selectedPayment.TotalRefunded;

            DialogResult result = MessageBox.Show(
                $"Refund ${refundAmount:F2} for Table {selectedPayment.TableId}?",
                "Confirm Refund",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                PaymentRepository.RecordRefund(
                    selectedPayment.PaymentId.Value,
                    refundAmount,
                    SessionContext.CurrentEmployee.EmployeeId);

                ActivityLogService.Log(
                    SessionContext.CurrentEmployee.EmployeeId,
                    "RefundProcessed",
                    $"Order {selectedPayment.OrderId} refunded {refundAmount:F2}");

                MessageBox.Show("Refund processed successfully.");

                LoadPaymentQueue();
                listBox2.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Refund failed: " + ex.Message);
            }
        }
    }
}