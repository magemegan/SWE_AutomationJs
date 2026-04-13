using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class SalesReports : Form
    {
        public SalesReports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//back
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }

        private void SalesReports_Load(object sender, EventArgs e)
        {
            SetupSalesGrid();
            LoadSalesData();
            CalcSalesSummary();
        }

        private void SetupSalesGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // Set up columns for the sales data grid
            dataGridView1.Columns.Add("TableNumber", "Table");
            dataGridView1.Columns.Add("Total", "Total");
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Status", "Status");

            // Set up additional properties for the sales data grid
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadSalesData()
        {
            dataGridView1.Rows.Clear();

            foreach (PastPayment payment in PastPaymentStorage.Payments)
            {
                dataGridView1.Rows.Add($"{payment.TableNumber}, ${payment.Total}, {payment.Date}, {payment.Status}");
            }
        }

        private void CalcSalesSummary()
        {
            double totalSales = 0;
            double highestSale = 0;
            int totalTransactions = PastPaymentStorage.Payments.Count;

            foreach (PastPayment payment in PastPaymentStorage.Payments)
            {
                totalSales += payment.Total;
                if (payment.Total > highestSale)
                {
                    highestSale = payment.Total;
                }
            }

            double avgSale = 0;

            if (totalTransactions > 0) { avgSale = totalTransactions / totalTransactions; }

            label8.Text = $"Total Sales: ${totalSales:F2}";
            label7.Text = $"Average Sales: ${avgSale:F2}";
            label4.Text = $"Highest Sales: ${highestSale:F2}";
            label5.Text = $"Total Transactions: {totalTransactions}";
        }
    }
}
