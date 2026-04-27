using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class CleaningScreen : Form
    {
        private readonly List<int> dirtyTables = new List<int>();

        public CleaningScreen()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new BusboyScreen());
        }

        private void CleaningScreen_Load(object sender, EventArgs e)
        {
            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("You must be logged in first.");
                NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
                return;
            }

            LoadCleaningQueue();
        }

        private void LoadCleaningQueue()
        {
            listBox1.Items.Clear();
            dirtyTables.Clear();

            foreach (TableStatusInfo table in TableRepository
                         .GetAllTableStatuses()
                         .Where(x => x.StatusName == "Dirty"))
            {
                dirtyTables.Add(table.TableId);
                listBox1.Items.Add($"Table {table.TableId}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;

            if (selectedIndex < 0 || selectedIndex >= dirtyTables.Count)
            {
                return;
            }

            int selectedTableNumber = dirtyTables[selectedIndex];

            label4.Text = $"Table: {selectedTableNumber}";
            label5.Text = "Status: Needs Cleaning";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;

            if (selectedIndex < 0 || selectedIndex >= dirtyTables.Count)
            {
                MessageBox.Show("Please select a dirty table first.");
                return;
            }

            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("Please sign in again before updating table status.");
                NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
                return;
            }

            int selectedTableNumber = dirtyTables[selectedIndex];

            try
            {
                TableRepository.SetStatus(
                    selectedTableNumber,
                    "Available",
                    SessionContext.CurrentEmployee.EmployeeId
                );

                label4.Text = "Table:";
                label5.Text = "Status:";

                LoadCleaningQueue();

                Notifications notification = new Notifications
                {
                    Message = "Table " + selectedTableNumber + " is now clean.",
                    Role = "Waiter",
                    TimeStamp = DateTime.Now.ToShortTimeString()
                };

                NotificationStorage.Notify.Add(notification);

                MessageBox.Show("Table marked as clean.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Unable to Update Table",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}