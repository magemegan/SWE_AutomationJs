using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class InventoryManagement : Form
    {
        public InventoryManagement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back to admin menu
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void InventoryManagement_Load(object sender, EventArgs e)
        {
            SetupInventoryGrid();
            LoadSampleInventory();
        }

        private void SetupInventoryGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            // Set up columns for inventory items
            dataGridView1.Columns.Add("ItemID", "Item ID");
            dataGridView1.Columns.Add("ItemName", "Item Name");
            dataGridView1.Columns.Add("Category", "Category");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Status", "Status");
            // Set properties for better appearance
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Make all read-only
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
        }

        private void LoadSampleInventory()
        {
            dataGridView1.Rows.Add("001", "Coffee Beans", "Dry Goods ", "50", "In Stock");
            dataGridView1.Rows.Add("002", "Milk", "Diary ", "30", "Low Stock");
            dataGridView1.Rows.Add("003", "Sugar", "Bakery ","100", "In Stock");
            dataGridView1.Rows.Add("004", "Cups", "Supplies ", "200", "Reorder");

            ColorRows();
        }

        private void ColorRows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string status = row.Cells["Status"].Value.ToString();
                if (status == "In Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (status == "Low Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                else if (status == "Reorder")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }
    }
}
