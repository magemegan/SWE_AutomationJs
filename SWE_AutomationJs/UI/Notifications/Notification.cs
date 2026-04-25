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
    public partial class Notification : Form
    {
        private string previousScreen;

        public Notification()
        {
            InitializeComponent();
        }
        public Notification(string previousScreen)
        {
            InitializeComponent();
            this.previousScreen = previousScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {//back
            if (previousScreen == "Busboy")
            {
                NavigationHelper.ShowAtCurrentPosition(this, new BusboyScreen());
            }
            else if (previousScreen == "Kitchen")
            {
                NavigationHelper.ShowAtCurrentPosition(this, new KitchenScreen());
            }
            else if (previousScreen == "Waiter")
            {
                NavigationHelper.ShowAtCurrentPosition(this, new WaiterScreen());
            }
            else if (previousScreen == "Admin")
            {
                NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
            }
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            string role = "";
            string employee = "";
            if (previousScreen == "Busboy")
            {
                role = "Busboy";
                employee = BusboyScreen.CurrentEmployee;
            }
            else if (previousScreen == "Kitchen")
            {
                role = "Kitchen";
                employee = KitchenScreen.CurrentEmployee;
            }
            else if (previousScreen == "Waiter")
            {
                role = "Waiter";
                employee = WaiterScreen.CurrentEmployee;
            }
            else if (previousScreen == "Admin")
            {
                role = "Admin";
                employee = AdminMenu.CurrentEmployee;
            }
            LoadNotification(role, employee);
        }

        private void LoadNotification(string role, string employee)
        {
            listBox1.Items.Clear();

            for(int i = NotificationStorage.Notify.Count - 1; i >= 0; i--)
            {
                foreach (Notifications n in NotificationStorage.Notify)
                {
                    bool matchesRole = n.Role == role;
                    bool matchesEmployee = n.Employee == employee;

                    if (matchesRole || matchesEmployee)
                    {
                        listBox1.Items.Add($"{n.TimeStamp} - {n.Message}");
                    } 
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//clear
            NotificationStorage.Notify.Clear();
            listBox1.Items.Clear();
        }
    }
}
