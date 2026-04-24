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
    public partial class BusboyScreen : Form
    {
        public static string CurrentEmployee { get; set; }

        public BusboyScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//log out button
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {//view schedule
            Schedule scheduleScreen = new Schedule("Busboy");
            scheduleScreen.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//view tables
            CleaningScreen tableStatus = new CleaningScreen();
            tableStatus.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Notification notificationScreen = new Notification("Busboy");
            notificationScreen.Show();
            this.Hide();
        }

        private void BusboyScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
