using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class KitchenScreen : Form
    {
        public static string CurrentEmployee { get; set; }

        public KitchenScreen()
        {
            InitializeComponent();
            UiTheme.Apply(this);

            Button restaurantInfoButton = new Button
            {
                Location = new Point(534, 293),
                Size = new Size(100, 59),
                Text = "Restaurant Info"
            };

            restaurantInfoButton.Click += (sender, args) =>
            {
                new AboutRestaurant().ShowDialog();
            };

            Controls.Add(restaurantInfoButton);
        }

        private void KitchenScreen_Load(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Incoming orders
            NavigationHelper.ShowAtCurrentPosition(this, new IncomingOrders());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Restock inventory
            NavigationHelper.ShowAtCurrentPosition(this, new Inventory());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Kitchen"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Notifications
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Kitchen"));
        }
    }
}