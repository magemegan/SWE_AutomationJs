using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AssignTables : Form
    {
        private readonly Dictionary<int, Button> tableButtons;

        public AssignTables()
        {
            InitializeComponent();
            tableButtons = new Dictionary<int, Button>
            {
                { 1, button3 }, { 2, button4 }, { 3, button5 }, { 4, button6 },
                { 5, button7 }, { 6, button8 }, { 7, button9 }, { 8, button10 },
                { 9, button11 }, { 10, button12 }, { 11, button13 }, { 12, button14 }
            };
        }

        private void TableStatus_Load(object sender, System.EventArgs e)
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
            NavigationHelper.ShowAtCurrentPosition(this, new WaiterScreen());
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables2());
        }

        private void button3_Click(object sender, System.EventArgs e) { OpenTableOrder(1); }
        private void button4_Click(object sender, System.EventArgs e) { OpenTableOrder(2); }
        private void button5_Click(object sender, System.EventArgs e) { OpenTableOrder(3); }
        private void button6_Click(object sender, System.EventArgs e) { OpenTableOrder(4); }
        private void button7_Click(object sender, System.EventArgs e) { OpenTableOrder(5); }
        private void button8_Click(object sender, System.EventArgs e) { OpenTableOrder(6); }
        private void button9_Click(object sender, System.EventArgs e) { OpenTableOrder(7); }
        private void button10_Click(object sender, System.EventArgs e) { OpenTableOrder(8); }
        private void button11_Click(object sender, System.EventArgs e) { OpenTableOrder(9); }
        private void button12_Click(object sender, System.EventArgs e) { OpenTableOrder(10); }
        private void button13_Click(object sender, System.EventArgs e) { OpenTableOrder(11); }
        private void button14_Click(object sender, System.EventArgs e) { OpenTableOrder(12); }
    }
}
