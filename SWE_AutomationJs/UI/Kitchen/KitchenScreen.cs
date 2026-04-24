using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class KitchenScreen : Form
    {
        public static string CurrentEmployee { get; set; }

        public KitchenScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//log out
            SessionContext.Clear();
            CurrentEmployee = null;
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IncomingOrders orders = new IncomingOrders();
            orders.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {//restock inventory
            Inventory inventoryScreen = new Inventory();
            inventoryScreen.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {//schedule
            Schedule scheduleScreen = new Schedule("Kitchen");
            scheduleScreen.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Notification notificationScreen = new Notification("Kitchen");
            notificationScreen.Show();
            this.Hide();
        }
    }
}
