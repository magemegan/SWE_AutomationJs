using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void Inventory_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {//back button
            KitchenScreen kitchenScreen = new KitchenScreen();
            kitchenScreen.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {// send to request to manager
            MessageBox.Show("Inventory request sent to manager.");

        }
    }
}
