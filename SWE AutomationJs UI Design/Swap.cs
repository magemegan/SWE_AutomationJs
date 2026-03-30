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
            ScheduleRequest request = new ScheduleRequest();

            request.EmployeeName = " ";
            request.RequestType = "Swap";
            request.Details = textBox1.Text;
            request.Status = "Pending";
            request.ShiftDate = dateTimePicker1.Value;

            ScheduleRequestStorage.ScheduleRequests.Add(request);

            textBox1.Clear();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {//back
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            this.Hide();
        }
    }
}
