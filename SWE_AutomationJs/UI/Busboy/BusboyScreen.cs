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

            Button restaurantInfoButton = new Button();
            restaurantInfoButton.Location = new System.Drawing.Point(379, 305);
            restaurantInfoButton.Size = new System.Drawing.Size(100, 58);
            restaurantInfoButton.Text = "Restaurant Info";

            restaurantInfoButton.Click += (sender, args) =>
            {
                new AboutRestaurant().ShowDialog();
            };

            Controls.Add(restaurantInfoButton);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Busboy"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new CleaningScreen());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Busboy"));
        }

        private void BusboyScreen_Load(object sender, EventArgs e)
        {

        }
    }
}