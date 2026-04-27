using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Swap : Form
    {
        private string previousScreen;
        public Swap(string previousScreen)
        {
            InitializeComponent();
            UiTheme.Apply(this);
            this.previousScreen = previousScreen;
        }

        private void button2_Click(object sender, System.EventArgs e)
        {//submit swap
            ScheduleRequest request = new ScheduleRequest();

            request.EmployeeName = " ";
            request.RequestType = "Swap";
            request.Details = textBox1.Text;
            request.Status = "Pending";
            request.ShiftDate = dateTimePicker1.Value;

            ScheduleRequestStorage.ScheduleRequests.Add(request);

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {//back
            Schedule schedule = new Schedule(previousScreen);
            schedule.Show();
            this.Hide();
        }

        private void Swap_Load(object sender, System.EventArgs e)
        {

        }
    }
}
