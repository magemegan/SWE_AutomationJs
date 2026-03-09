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
    }
}
