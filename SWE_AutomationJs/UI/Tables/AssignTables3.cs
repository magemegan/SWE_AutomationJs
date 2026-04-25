using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AssignTables3 : Form
    {
        private readonly Dictionary<int, Button> tableButtons;

        public AssignTables3()
        {
            InitializeComponent();
            tableButtons = new Dictionary<int, Button>
            {
                { 25, button3 }, { 26, button4 }, { 27, button5 }, { 28, button6 },
                { 29, button7 }, { 30, button8 }, { 31, button9 }, { 32, button10 },
                { 33, button11 }, { 34, button12 }, { 35, button13 }, { 36, button14 }
            };
            button1.Click += button1_Click;
        }

        private void TableStatus_Load3(object sender, System.EventArgs e)
        {
            LoadTableStatuses();
        }

        private void LoadTableStatuses()
        {
            IReadOnlyList<TableStatusInfo> statuses = TableRepository.GetAllTableStatuses();
            IReadOnlyList<int> assignedTables = SessionContext.IsAuthenticated
                ? TableRepository.GetAssignedTableIds(SessionContext.CurrentEmployee.EmployeeId)
                : new List<int>();
            foreach (KeyValuePair<int, Button> entry in tableButtons)
            {
                TableStatusInfo status = statuses.FirstOrDefault(x => x.TableId == entry.Key);
                entry.Value.Text = $"Table {entry.Key}";
                entry.Value.BackColor = GetStatusColor(status != null ? status.UiStatus : "Open");
                entry.Value.Font = assignedTables.Contains(entry.Key)
                    ? new Font(entry.Value.Font, FontStyle.Bold)
                    : new Font(entry.Value.Font, FontStyle.Regular);
            }
        }

        private static Color GetStatusColor(string uiStatus)
        {
            switch (uiStatus)
            {
                case "Occupied":
                    return Color.Orange;
                case "Needs Cleaning":
                    return Color.IndianRed;
                default:
                    return Color.LightGreen;
            }
        }

        private void OpenTableOrder(int tableId)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new Orders(tableId));
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables2());
        }

        private void button3_Click_1(object sender, System.EventArgs e) { OpenTableOrder(25); }
        private void button4_Click(object sender, System.EventArgs e) { OpenTableOrder(26); }
        private void button5_Click(object sender, System.EventArgs e) { OpenTableOrder(27); }
        private void button6_Click(object sender, System.EventArgs e) { OpenTableOrder(28); }
        private void button7_Click(object sender, System.EventArgs e) { OpenTableOrder(29); }
        private void button8_Click(object sender, System.EventArgs e) { OpenTableOrder(30); }
        private void button9_Click(object sender, System.EventArgs e) { OpenTableOrder(31); }
        private void button10_Click(object sender, System.EventArgs e) { OpenTableOrder(32); }
        private void button11_Click(object sender, System.EventArgs e) { OpenTableOrder(33); }
        private void button12_Click(object sender, System.EventArgs e) { OpenTableOrder(34); }
        private void button13_Click(object sender, System.EventArgs e) { OpenTableOrder(35); }
        private void button14_Click(object sender, System.EventArgs e) { OpenTableOrder(36); }
    }
}
