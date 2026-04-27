using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Dapper;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design.UI.Admin
{
    public class ReportsForm : Form
    {
        private Panel headerPanel;
        private Label titleLabel;
        private Button btnBack;
        private Button btnRefresh;

        private Panel summaryPanel;
        private Label lblTotalRevenue;
        private Label lblOrdersCount;
        private Label lblAverageOrder;

        private TabControl tabReports;
        private TabPage tabSales;
        private TabPage tabMenu;
        private TabPage tabPayments;

        private DataGridView dgvSales;
        private DataGridView dgvMenu;
        private DataGridView dgvPayments;

        public ReportsForm()
        {
            BuildLayout();
            LoadReports();
        }

        private void BuildLayout()
        {
            Text = "Reports";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 700);
            MinimumSize = new Size(950, 600);
            BackColor = Color.White;

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(25, 144, 183)
            };

            btnBack = new Button
            {
                Text = "← Back",
                Width = 100,
                Height = 38,
                Left = 15,
                Top = 16,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(25, 144, 183),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += BtnBack_Click;

            titleLabel = new Label
            {
                Text = "Manager Reports",
                AutoSize = true,
                Left = 135,
                Top = 20,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };

            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 38,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = ClientSize.Width - 120,
                Top = 16,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(25, 144, 183),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            headerPanel.Controls.Add(btnBack);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnRefresh);
            Controls.Add(headerPanel);

            summaryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 115,
                BackColor = Color.FromArgb(245, 248, 250),
                Padding = new Padding(20)
            };

            lblTotalRevenue = CreateSummaryLabel("Total Revenue: $0.00", 20);
            lblOrdersCount = CreateSummaryLabel("Orders: 0", 360);
            lblAverageOrder = CreateSummaryLabel("Average Order: $0.00", 650);

            summaryPanel.Controls.Add(lblTotalRevenue);
            summaryPanel.Controls.Add(lblOrdersCount);
            summaryPanel.Controls.Add(lblAverageOrder);
            Controls.Add(summaryPanel);

            tabReports = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Padding = new Point(12, 6)
            };

            tabSales = new TabPage("Sales Summary");
            tabMenu = new TabPage("Menu Popularity");
            tabPayments = new TabPage("Payments");

            dgvSales = CreateGrid();
            dgvMenu = CreateGrid();
            dgvPayments = CreateGrid();

            tabSales.Controls.Add(dgvSales);
            tabMenu.Controls.Add(dgvMenu);
            tabPayments.Controls.Add(dgvPayments);

            tabReports.TabPages.Add(tabSales);
            tabReports.TabPages.Add(tabMenu);
            tabReports.TabPages.Add(tabPayments);

            Controls.Add(tabReports);

            Resize += ReportsForm_Resize;
        }

        private Label CreateSummaryLabel(string text, int left)
        {
            return new Label
            {
                Text = text,
                Left = left,
                Top = 35,
                Width = 300,
                Height = 40,
                ForeColor = Color.FromArgb(35, 35, 35),
                Font = new Font("Segoe UI", 13, FontStyle.Bold)
            };
        }

        private DataGridView CreateGrid()
        {
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 144, 183);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.EnableHeadersVisualStyles = false;

            return grid;
        }

        private void LoadReports()
        {
            try
            {
                LoadSummary();
                LoadSalesReport();
                LoadMenuReport();
                LoadPaymentReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load reports.\n\n" + ex.Message,
                    "Reports Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadSummary()
        {
            using (var connection = Db.Open())
            {
                dynamic summary = connection.QueryFirstOrDefault(@"
SELECT
    IFNULL(SUM(Total), 0) AS TotalRevenue,
    COUNT(*) AS OrdersCount,
    IFNULL(AVG(Total), 0) AS AverageOrder
FROM Orders;
");

                decimal totalRevenue = Convert.ToDecimal(summary.TotalRevenue);
                int ordersCount = Convert.ToInt32(summary.OrdersCount);
                decimal averageOrder = Convert.ToDecimal(summary.AverageOrder);

                lblTotalRevenue.Text = string.Format("Total Revenue: {0:C}", totalRevenue);
                lblOrdersCount.Text = string.Format("Orders: {0}", ordersCount);
                lblAverageOrder.Text = string.Format("Average Order: {0:C}", averageOrder);
            }
        }

        private void LoadSalesReport()
        {
            using (var connection = Db.Open())
            {
                var data = connection.Query(@"
SELECT
    DATE(OpenedAt) AS SaleDate,
    COUNT(*) AS OrderCount,
    ROUND(SUM(Subtotal), 2) AS Subtotal,
    ROUND(SUM(Tax), 2) AS Tax,
    ROUND(SUM(Total), 2) AS Total
FROM Orders
GROUP BY DATE(OpenedAt)
ORDER BY SaleDate DESC;
");

                dgvSales.DataSource = ToDataTable(data);
            }
        }

        private void LoadMenuReport()
        {
            using (var connection = Db.Open())
            {
                var data = connection.Query(@"
SELECT
    mi.ItemName,
    mc.CategoryName,
    SUM(oi.Qty) AS QuantitySold,
    ROUND(SUM(oi.Qty * oi.UnitPriceAtSale), 2) AS Revenue
FROM OrderItems oi
JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
JOIN MenuCategories mc ON mi.CategoryId = mc.CategoryId
GROUP BY mi.ItemName, mc.CategoryName
ORDER BY QuantitySold DESC;
");

                dgvMenu.DataSource = ToDataTable(data);
            }
        }

        private void LoadPaymentReport()
        {
            using (var connection = Db.Open())
            {
                var data = connection.Query(@"
SELECT
    p.PaymentId,
    p.OrderId,
    p.PaymentMethod,
    ROUND(p.Amount, 2) AS Amount,
    p.PaidAt,
    p.RefundFlag,
    ROUND(p.RefundAmount, 2) AS RefundAmount
FROM Payments p
ORDER BY p.PaidAt DESC;
");

                dgvPayments.DataSource = ToDataTable(data);
            }
        }

        private DataTable ToDataTable(IEnumerable<dynamic> rows)
        {
            DataTable table = new DataTable();

            foreach (var row in rows)
            {
                IDictionary<string, object> dictionary = (IDictionary<string, object>)row;

                if (table.Columns.Count == 0)
                {
                    foreach (string column in dictionary.Keys)
                    {
                        table.Columns.Add(column);
                    }
                }

                DataRow dataRow = table.NewRow();

                foreach (string column in dictionary.Keys)
                {
                    dataRow[column] = dictionary[column] ?? DBNull.Value;
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void ReportsForm_Resize(object sender, EventArgs e)
        {
            if (btnRefresh != null)
            {
                btnRefresh.Left = ClientSize.Width - 120;
            }
        }
    }
}