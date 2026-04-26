using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    public partial class ReportsForm : Form
    {
        private Label lblOrders;
        private Label lblRevenue;
        private Label lblPopularItem;
        private Button btnRefresh;
        private Button btnClose;

        public ReportsForm()
        {
            InitializeComponent();
            BuildPage();
            LoadReportData();
        }

        private void BuildPage()
        {
            Text = "Manager Reports";
            Size = new Size(500, 350);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "Manager Sales Report",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            lblOrders = new Label
            {
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(35, 90)
            };

            lblRevenue = new Label
            {
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(35, 130)
            };

            lblPopularItem = new Label
            {
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(35, 170)
            };

            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(100, 35),
                Location = new Point(35, 230)
            };

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(150, 230)
            };

            btnRefresh.Click += (sender, e) => LoadReportData();
            btnClose.Click += (sender, e) => Close();

            Controls.Add(title);
            Controls.Add(lblOrders);
            Controls.Add(lblRevenue);
            Controls.Add(lblPopularItem);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
        }

        private void LoadReportData()
        {
            lblOrders.Text = "Orders Today: " + ReportRepository.GetTotalOrdersToday();
            lblRevenue.Text = "Revenue Today: $" + ReportRepository.GetRevenueToday().ToString("0.00");
            lblPopularItem.Text = "Most Popular Item: " + ReportRepository.GetMostPopularItem();
        }
    }
}