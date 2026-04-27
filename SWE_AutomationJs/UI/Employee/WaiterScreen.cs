using System;
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
            UiTheme.Apply(this);
        }

        private void WaitierScreen_Load(object sender, EventArgs e)
        {
            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("You must be logged in first.");
                NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
                return;
            }

            CurrentEmployee = SessionContext.CurrentEmployee.EmployeeId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Log out
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Assign / select tables
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Access schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Waiter"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Orders screen
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Customer payment
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