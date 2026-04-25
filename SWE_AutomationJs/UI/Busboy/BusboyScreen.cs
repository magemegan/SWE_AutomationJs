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
    public partial class BusboyScreen : Form
    {
        public static string CurrentEmployee { get; set; }

        public BusboyScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//log out button
            SessionContext.Clear();
            CurrentEmployee = null;
            NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
        }

        private void button5_Click(object sender, EventArgs e)
        {//view schedule
            NavigationHelper.ShowAtCurrentPosition(this, new Schedule("Busboy"));
        }

        private void button2_Click(object sender, EventArgs e)
        {//view tables
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
