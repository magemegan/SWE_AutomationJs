using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Swap : Form
    {
        public Swap()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {//submit swap

        }

        private void button1_Click(object sender, System.EventArgs e)
        {//back
            Schedule schedule = new Schedule();
            schedule.Show();
            this.Hide();
        }
    }
}
