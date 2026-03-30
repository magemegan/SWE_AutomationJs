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

            //list through all tables and add those that need cleaning to the listbox
            foreach (var table in TableStorage.TableStatuses)
            {
                if (table.Value == "Needs Cleaning")
                {
                    listBox1.Items.Add($"Table {table.Key}");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == null) return;
            //display details of the selected order
            string selectedIndex = listBox1.SelectedIndex.ToString();


            //display order details in labels
            label4.Text = selectedIndex;
            label5.Text = "Status: Needs Cleaning";
        }

        private void button2_Click(object sender, EventArgs e)
        {//mark table as clean
            if (listBox1.SelectedIndex == null) return;
            //display details of the selected order
            string selectedIndex = listBox1.SelectedIndex.ToString();
            //parse table number from selected index
            int tableNumber = int.Parse(selectedIndex.Replace("Table ", ""));

            TableStorage.TableStatuses[tableNumber] = "Open";

            //display order details in labels
            label4.Text = "Table:";
            label5.Text = "Status:";
            MessageBox.Show($"Table {tableNumber} marked as clean!");

            LoadCleaningQueue();
        }
    }
}
