using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class BusboyScreen : Form
    {
        public static string CurrentEmployee { get; set; }

        public BusboyScreen()
        {
            InitializeComponent();
            UiTheme.Apply(this);

            Button restaurantInfoButton = new Button
            {
                Location = new Point(379, 305),
                Size = new Size(100, 58),
                Text = "Restaurant Info"
            };

            restaurantInfoButton.Click += (sender, args) =>
            {
                new AboutRestaurant().ShowDialog();
            };

            Controls.Add(restaurantInfoButton);
        }

        private void BusboyScreen_Load(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            // Schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Busboy"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Cleaning screen
            NavigationHelper.ShowAtCurrentPosition(this, new CleaningScreen());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Notifications
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Busboy"));
        }
    }
}