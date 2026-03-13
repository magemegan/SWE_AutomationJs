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
    public partial class IncomingOrders : Form
    {
        //public static List<Order> IncomingOrders = new List<Order>();
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
            //listBoxIncomingOrders.Items.Clear();

            //foreach (Orders order in OrderStorage.IncomingOrders)
            //{
            //    listBoxIncomingOrders.Items.Add($"Table {order.tableNumber} - {order.Status}");
            //}

            
        }

        //int selectedIndex = listBoxIncomingOrders.SelectedIndex;

        //if (selectedIndex == -1) return;

        //listBoxOrderDetails.Items.Clear();

        //Order selectedOrder = OrderStorage.IncomingOrders[selectedIndex];

        //foreach (string item in selectedOrder.Items)
        //{
        //    listBoxOrderDetails.Items.Add(item);
        //}


        //    int selectedIndex = listBoxIncomingOrders.SelectedIndex;
        //if (selectedIndex == -1) return;

        //OrderStorage.IncomingOrders[selectedIndex].Status = "Preparing";
        //RefreshIncomingOrders();

        //    int selectedIndex = listBoxIncomingOrders.SelectedIndex;
        //if (selectedIndex == -1) return;

        //OrderStorage.IncomingOrders[selectedIndex].Status = "Ready";
        //RefreshIncomingOrders();

        //private void RefreshIncomingOrders()
        //{
        //    listBoxIncomingOrders.Items.Clear();

        //    foreach (Order order in OrderStorage.IncomingOrders)
        //    {
        //        listBoxIncomingOrders.Items.Add("Table " + order.TableNumber + " - " + order.Status);
        //    }
        //}
    }
}
