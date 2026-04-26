using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class WaiterScreen : Form
    {
        public static string CurrentEmployee { get; set; }
        public WaiterScreen()
        {
            InitializeComponent();
        }

        private void WaitierScreen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {//log out
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button3_Click(object sender, EventArgs e)
        {//assign tables
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables());
        }

        private void button7_Click(object sender, EventArgs e)
        {//access schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Waiter"));
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {//customer payment
            NavigationHelper.ShowAtCurrentPosition(this, new Payment());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Waiter"));
        }

        private void buttonOverride_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new OverrideRequestForm());
        }

        private void buttonFaq_Click(object sender, EventArgs e)
        {
            new AboutRestaurant().ShowDialog();
        }
    }
}
