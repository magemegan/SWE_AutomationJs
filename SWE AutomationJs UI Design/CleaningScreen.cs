using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class CleaningScreen : Form
    {
        private List<int> dirtyTables = new List<int>();
        public CleaningScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to busboy screen
            BusboyScreen busboyScreen = new BusboyScreen();
            busboyScreen.Show();
            this.Hide();
        }

        private void CleaningScreen_Load(object sender, EventArgs e)
        {
            LoadCleaningQueue();
        }

        private void LoadCleaningQueue()
        {
            listBox1.Items.Clear();
            dirtyTables.Clear();

            //list through all tables and add those that need cleaning to the listbox
            foreach (var table in TableStorage.TableStatuses)
            {
                if (table.Value == "Needs Cleaning")
                {
                    dirtyTables.Add(table.Key);
                    listBox1.Items.Add($"Table {table.Key}");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            //check if selected index is valid
            if (selectedIndex == -1) return;
            if (selectedIndex >= dirtyTables.Count) return;
            //get table number from dirty tables list
            int selectedTableNumber = dirtyTables[selectedIndex];

            //display order details in labels
            label4.Text = $"Table: {selectedTableNumber}";
            label5.Text = "Status: Needs Cleaning";
        }

        private void button2_Click(object sender, EventArgs e)
        {//mark table as clean
            int selectedIndex = listBox1.SelectedIndex;

            if (selectedIndex == -1) return;
            if (selectedIndex >= dirtyTables.Count) return;

            int selectedTableNumber = dirtyTables[selectedIndex];


            TableStorage.TableStatuses[selectedTableNumber] = "Open";
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            listBox1.ClearSelected();

            //display order details in labels
            label4.Text = "Table:";
            label5.Text = "Status:";

            LoadCleaningQueue();

            Notifications n = new Notifications();
            n.Message = "Table " + selectedTableNumber + " is now clean.";
            n.Role = "Waiter";
            n.TimeStamp = DateTime.Now.ToShortTimeString();

            NotificationStorage.Notify.Add(n);
        }
    }
}
