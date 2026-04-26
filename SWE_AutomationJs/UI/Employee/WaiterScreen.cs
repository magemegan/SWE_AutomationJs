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
            ConfigureRestaurantDetails();
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

        private void ConfigureRestaurantDetails()
        {
            labelRestaurantDetails.Text =
                "CONTACT INFO:\r\n" +
                "jscorner.com\r\n" +
                "680 Arntson Dr., Marietta, GA 30060\r\n" +
                "(470) 555-1212\r\n\r\n" +
                "HOURS OPERATING:\r\n" +
                "Saturday: 11AM-9:30PM\r\n" +
                "Sunday: Closed\r\n" +
                "Monday: 11AM-9:30PM\r\n" +
                "Tuesday: 11AM-9:30PM\r\n" +
                "Wednesday: 11:30AM-9:30PM\r\n" +
                "Thursday: 11AM-9:30PM\r\n" +
                "Friday: 11AM-9:30PM";
        }
    }
}
