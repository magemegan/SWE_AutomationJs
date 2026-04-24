using System;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class ProcessPayment : Form
    {
        private readonly long orderId;
        private readonly int chosenTable;
        private OrderHeader currentOrder;

        public ProcessPayment()
        {
            InitializeComponent();
        }

        public ProcessPayment(long orderId, int tableNumber)
        {
            InitializeComponent();
            this.orderId = orderId;
            chosenTable = tableNumber;
        }

        private void ProcessPayment_Load(object sender, EventArgs e)
        {
            currentOrder = OrderRepository.GetHeader(orderId);
            if (currentOrder == null)
            {
                MessageBox.Show("Order could not be loaded.");
                Close();
                return;
            }

            label2.Text = $"Table: {chosenTable}";
            label3.Text = $"Server: {currentOrder.ServerDisplayName}";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            listBox1.Items.Clear();
            foreach (OrderLine item in OrderRepository.GetItems(orderId))
            {
                listBox1.Items.Add($"{item.ItemName} x{item.Qty} - ${item.UnitPriceAtSale:F2}");
            }

            label5.Text = $"Subtotal: ${currentOrder.Subtotal:F2}";
            label6.Text = $"Tax: ${currentOrder.Tax:F2}";
            label7.Text = $"Total: {currentOrder.Total:F2}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }

            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("Please sign in again before processing a payment.");
                return;
            }

            currentOrder = OrderRepository.GetHeader(orderId);
            if (currentOrder == null)
            {
                MessageBox.Show("Order could not be loaded.");
                return;
            }

            if (currentOrder.StatusName != "Ready")
            {
                MessageBox.Show("This order is not ready for payment yet.");
                return;
            }

            string paymentMethod = NormalizePaymentMethod(comboBox2.SelectedItem.ToString());
            if (paymentMethod == null)
            {
                MessageBox.Show("Unsupported payment method selected.");
                return;
            }

            PaymentRepository.RecordPayment(
                orderId,
                currentOrder.Total,
                paymentMethod,
                SessionContext.CurrentEmployee.EmployeeId);

            currentOrder = OrderRepository.GetHeader(orderId);
            if (currentOrder == null)
            {
                MessageBox.Show("Payment was recorded, but the order could not be reloaded.");
                return;
            }

            if (currentOrder.StatusName != "Closed")
            {
                MessageBox.Show("Payment was recorded, but the order is not closed yet.");
                return;
            }

            TableRepository.SetStatus(chosenTable, "Dirty", SessionContext.CurrentEmployee.EmployeeId);
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "PaymentProcessed", $"Order {orderId} amount {currentOrder.Total:F2}");

            Notifications busboyNotification = new Notifications();
            busboyNotification.Message = "Table " + chosenTable + " is ready for cleaning.";
            busboyNotification.Role = "Busboy";
            busboyNotification.TimeStamp = DateTime.Now.ToShortTimeString();
            NotificationStorage.Notify.Add(busboyNotification);

            MessageBox.Show("Payment processed for Table " + chosenTable);
            Payment paymentHistory = new Payment();
            paymentHistory.Show();
            Hide();
        }

        private static string NormalizePaymentMethod(string selectedValue)
        {
            switch (selectedValue)
            {
                case "Cash":
                    return "Cash";
                case "Credit Card":
                case "Card":
                    return "Card";
                default:
                    return null;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Receipt Printed for Table " + chosenTable);
        }

        private void button1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
