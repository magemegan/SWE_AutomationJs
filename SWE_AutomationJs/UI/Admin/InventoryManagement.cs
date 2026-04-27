﻿using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    public partial class InventoryManagement : Form
    {
        public InventoryManagement()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void InventoryManagement_Load(object sender, EventArgs e)
        {
            SetupInventoryGrid();
            LoadInventoryFromDatabase();
        }

        private void SetupInventoryGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("ItemID", "Item ID");
            dataGridView1.Columns.Add("ItemName", "Item Name");
            dataGridView1.Columns.Add("UnitOfMeasure", "Unit");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Status", "Status");

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
        }

        private void LoadInventoryFromDatabase()
        {
            try
            {
                dataGridView1.Rows.Clear();

                foreach (InventoryItemInfo item in InventoryRepository.GetActiveInventoryItems())
                {
                    dataGridView1.Rows.Add(
                        item.InventoryItemId,
                        item.ItemName,
                        item.UnitOfMeasure,
                        item.QuantityOnHand.ToString("0.##"),
                        item.Status);
                }

                HighlightInventoryRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Inventory could not be loaded.\n\n" + ex.Message);
            }
        }

        private void HighlightInventoryRows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Status"].Value == null)
                {
                    continue;
                }

                string status = row.Cells["Status"].Value.ToString();

                if (status == "In Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (status == "Low Stock")
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (status == "Reorder")
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
    }
}