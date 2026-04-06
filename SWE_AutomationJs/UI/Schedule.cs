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
    public partial class Schedule : Form
    {
        private string previousScreen;
        public Schedule(string previousScreen)
        {
            InitializeComponent();
            this.previousScreen = previousScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {// go back
            if (previousScreen == "Busboy")
            {
                BusboyScreen busboyScreen = new BusboyScreen();
                busboyScreen.Show();
                this.Hide();
            }
            else if (previousScreen == "Kitchen")
            {
                KitchenScreen kitchenScreen = new KitchenScreen();
                kitchenScreen.Show();
                this.Hide();
            }
            else if (previousScreen == "Waiter")
            {
                WaiterScreen waiterScreen = new WaiterScreen();
                waiterScreen.Show();
                this.Hide();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {//request off
            TimeOff dayOff = new TimeOff(previousScreen);
            dayOff.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {//swap shift
            Swap swapShift = new Swap(previousScreen);
            swapShift.Show();
            this.Hide();
        }
    }
}
