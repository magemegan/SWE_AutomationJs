using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    public partial class SalesReports : Form
    {
        private IReadOnlyList<PaymentHistoryEntry> paymentHistory = Array.Empty<PaymentHistoryEntry>();

        public SalesReports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
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
            dataGridView1.Columns.Add("TableNumber", "Table");
            dataGridView1.Columns.Add("Total", "Total");
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Status", "Status");
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadSalesData()
        {
            paymentHistory = PaymentRepository.GetPaymentHistory();
            dataGridView1.Rows.Clear();

            foreach (PaymentHistoryEntry payment in paymentHistory)
            {
                dataGridView1.Rows.Add(
                    payment.TableId,
                    "$" + payment.TotalPaid.ToString("0.00"),
                    payment.PaidAt.HasValue ? payment.PaidAt.Value.ToString("MM/dd/yyyy") : "N/A",
                    payment.PaymentStatus);
            }
        }

        private void CalcSalesSummary()
        {
            decimal totalSales = 0m;
            decimal highestSale = 0m;
            int totalTransactions = paymentHistory.Count;

            foreach (PaymentHistoryEntry payment in paymentHistory)
            {
                totalSales += payment.TotalPaid;
                if (payment.TotalPaid > highestSale)
                {
                    highestSale = payment.TotalPaid;
                }
            }

            decimal avgSale = totalTransactions > 0
                ? totalSales / totalTransactions
                : 0m;

            label8.Text = $"Total Sales: ${totalSales:F2}";
            label7.Text = $"Average Sales: ${avgSale:F2}";
            label4.Text = $"Highest Sales: ${highestSale:F2}";
            label5.Text = $"Total Transactions: {totalTransactions}";
        }
    }
}
