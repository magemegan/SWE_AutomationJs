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
    public partial class EmployeeMenu : Form
    {
        public EmployeeMenu()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {//log out
            SessionContext.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {//kitchen 
            KitchenScreen kitchenHomeScreen = new KitchenScreen();
            kitchenHomeScreen.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {//waiter/waitress
            WaiterScreen waitingHomeScreen = new WaiterScreen();
            waitingHomeScreen.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {//busboy
            BusboyScreen busboyHomeScreen = new BusboyScreen();
            busboyHomeScreen.Show();
            this.Hide();
        }
    }
}
