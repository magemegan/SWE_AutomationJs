using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class IncomingOrders : Form
    {
        private IReadOnlyList<OrderHeader> kitchenQueue = Array.Empty<OrderHeader>();

        public IncomingOrders()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new KitchenScreen());
        }

        private void IncomingOrders_Load(object sender, EventArgs e)
        {
            button2.Text = "Refresh";
            button3.Text = "Mark Ready";
            LoadOrderQueue();
        }

        private void LoadOrderQueue()
        {
            kitchenQueue = OrderRepository.GetKitchenQueue();
            listBox1.Items.Clear();

            foreach (OrderHeader order in kitchenQueue)
            {
                listBox1.Items.Add($"Table {order.TableId} - {order.StatusName} - ${order.Total:F2}");
            }

            if (kitchenQueue.Count == 0)
            {
                label4.Text = "Table: ";
                label5.Text = "Status: ";
                listBox2.Items.Clear();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= kitchenQueue.Count)
            {
                return;
            }

            OrderHeader selectedOrder = kitchenQueue[selectedIndex];
            label4.Text = "Table: " + selectedOrder.TableId;
            label5.Text = "Status: " + selectedOrder.StatusName;

            listBox2.Items.Clear();
            foreach (OrderLine item in OrderRepository.GetItems(selectedOrder.OrderId))
            {
                string details = string.IsNullOrWhiteSpace(item.Notes)
                    ? string.Empty
                    : $" [{item.Notes}]";
                listBox2.Items.Add($"{item.ItemName} x{item.Qty}{details}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadOrderQueue();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= kitchenQueue.Count)
            {
                return;
            }

            OrderHeader selectedOrder = kitchenQueue[selectedIndex];
            if (selectedOrder.StatusName == "Ready")
            {
                MessageBox.Show("This order is already marked ready.");
                return;
            }

            string employeeId = SessionContext.IsAuthenticated
                ? SessionContext.CurrentEmployee.EmployeeId
                : "E00005";

            OrderRepository.MarkReady(selectedOrder.OrderId, employeeId);

            Notifications waiterNotification = new Notifications();
            waiterNotification.Message = "Order for Table " + selectedOrder.TableId + " is ready.";
            waiterNotification.Role = "Waiter";
            waiterNotification.Employee = selectedOrder.ServerDisplayName;
            waiterNotification.TimeStamp = DateTime.Now.ToShortTimeString();
            NotificationStorage.Notify.Add(waiterNotification);
            LoadOrderQueue();
        }

        private void label4_Click(object sender, EventArgs e) { }
    }
}
