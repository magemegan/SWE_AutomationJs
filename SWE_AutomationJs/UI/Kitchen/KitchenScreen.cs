using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Button restaurantInfoButton = new Button();
            restaurantInfoButton.Location = new System.Drawing.Point(534, 293);
            restaurantInfoButton.Size = new System.Drawing.Size(100, 59);
            restaurantInfoButton.Text = "Restaurant Info";

            restaurantInfoButton.Click += (sender, args) =>
            {
                new AboutRestaurant().ShowDialog();
            };

    Controls.Add(restaurantInfoButton);
}

        private void button1_Click(object sender, EventArgs e)
        {//log out
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new IncomingOrders());
        }

        private void button5_Click(object sender, EventArgs e)
        {//restock inventory
            NavigationHelper.ShowAtCurrentPosition(this, new Inventory());
        }

        private void button6_Click(object sender, EventArgs e)
        {//schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Kitchen"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Notification("Kitchen"));
        }
    }
}
