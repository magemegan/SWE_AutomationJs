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
            InitializeOverrideButton();
        }

        private void WaitierScreen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {//log out
            SessionContext.Clear();
            CurrentEmployee = null;
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {//assign tables
            AssignTables assignTables = new AssignTables();
            assignTables.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {//access schedule
            Schedule accessSchedule = new Schedule("Waiter");
            accessSchedule.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {//customer payment
            Payment payment = new Payment();
            payment.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Notification notificationScreen = new Notification("Waiter");
            notificationScreen.Show();
            this.Hide();
        }

        private void InitializeOverrideButton()
        {
            Button overrideButton = new Button();
            overrideButton.Location = new System.Drawing.Point(106, 312);
            overrideButton.Size = new System.Drawing.Size(142, 52);
            overrideButton.Text = "Request Override";
            overrideButton.Click += (sender, args) =>
            {
                OverrideRequestForm form = new OverrideRequestForm();
                form.Show();
                Hide();
            };
            Controls.Add(overrideButton);
        }
    }
}
