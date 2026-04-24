using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AssignTables2 : Form
    {
        private readonly Dictionary<int, Button> tableButtons;

        public AssignTables2()
        {
            InitializeComponent();
            tableButtons = new Dictionary<int, Button>
            {
                { 13, button3 }, { 14, button4 }, { 15, button5 }, { 16, button6 },
                { 17, button7 }, { 18, button8 }, { 19, button9 }, { 20, button10 },
                { 21, button11 }, { 22, button12 }, { 23, button13 }, { 24, button14 }
            };
        }

        private void TableStatus_Load2(object sender, System.EventArgs e)
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
            Orders orders = new Orders(tableId);
            orders.Show();
            Hide();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            AssignTables assignTables = new AssignTables();
            assignTables.Show();
            Hide();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            AssignTables3 assignTables3 = new AssignTables3();
            assignTables3.Show();
            Hide();
        }

        private void button3_Click_1(object sender, System.EventArgs e) { OpenTableOrder(13); }
        private void button4_Click(object sender, System.EventArgs e) { OpenTableOrder(14); }
        private void button5_Click(object sender, System.EventArgs e) { OpenTableOrder(15); }
        private void button6_Click(object sender, System.EventArgs e) { OpenTableOrder(16); }
        private void button7_Click(object sender, System.EventArgs e) { OpenTableOrder(17); }
        private void button8_Click(object sender, System.EventArgs e) { OpenTableOrder(18); }
        private void button9_Click(object sender, System.EventArgs e) { OpenTableOrder(19); }
        private void button10_Click(object sender, System.EventArgs e) { OpenTableOrder(20); }
        private void button11_Click(object sender, System.EventArgs e) { OpenTableOrder(21); }
        private void button12_Click(object sender, System.EventArgs e) { OpenTableOrder(22); }
        private void button13_Click(object sender, System.EventArgs e) { OpenTableOrder(23); }
        private void button14_Click(object sender, System.EventArgs e) { OpenTableOrder(24); }
    }
}
