using System;
using System.Drawing;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Security;

namespace SWE_AutomationJs_UI_Design
{
    public class ReportsForm : Form
    {
        private Label lblSummary;
        private DataGridView gridRevenueByHour;
        private DataGridView gridPopularItems;
        private DataGridView gridPersonnel;
        private Button btnRefresh;
        private Button btnClose;

        public ReportsForm()
        {
            BuildPage();
            LoadReportData();
        }

        private void BuildPage()
        {
            Text = "Manager Reports";
            Size = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "Manager Operations Report",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            lblSummary = new Label
            {
                Font = new Font("Segoe UI", 11),
                AutoSize = false,
                Location = new Point(35, 65),
                Size = new Size(1000, 100)
            };

            gridRevenueByHour = CreateGrid(new Point(35, 190), new Size(320, 400));
            gridPopularItems = CreateGrid(new Point(380, 190), new Size(320, 400));
            gridPersonnel = CreateGrid(new Point(725, 190), new Size(330, 400));

            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(100, 35),
                Location = new Point(35, 630)
            };

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(150, 630)
            };

            btnRefresh.Click += (sender, e) => LoadReportData();
            btnClose.Click += (sender, e) => Close();

            Controls.Add(title);
            Controls.Add(lblSummary);
            Controls.Add(new Label { Text = "Revenue By Hour", Location = new Point(35, 165), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            Controls.Add(new Label { Text = "Popular Menu Items", Location = new Point(380, 165), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            Controls.Add(new Label { Text = "Personnel Efficiency", Location = new Point(725, 165), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) });
            Controls.Add(gridRevenueByHour);
            Controls.Add(gridPopularItems);
            Controls.Add(gridPersonnel);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
        }

        private DataGridView CreateGrid(Point location, Size size)
        {
            return new DataGridView
            {
                Location = location,
                Size = size,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        private void LoadReportData()
        {
            try
            {
                if (!RolePermissionService.IsManager())
                {
                    MessageBox.Show("Only managers/admins can view reports.");
                    Close();
                    return;
                }

                ManagerReportSummary summary = ReportRepository.GetManagerSummary();

                lblSummary.Text =
                    $"Orders Today: {summary.OrdersToday}\n" +
                    $"Revenue Today: ${summary.RevenueToday:0.00} | Week: ${summary.RevenueWeek:0.00} | Month: ${summary.RevenueMonth:0.00}\n" +
                    $"Most Popular Item: {summary.MostPopularItem}\n" +
                    $"Avg Turnaround: {summary.AverageTurnaroundMinutes:0.0} min | Avg Prep: {summary.AveragePreparationMinutes:0.0} min | Ready-to-Served: {summary.AverageReadyToServedMinutes:0.0} min";

                gridRevenueByHour.DataSource = ReportRepository.GetRevenueByHourToday();
                gridPopularItems.DataSource = ReportRepository.GetPopularItems(10);
                gridPersonnel.DataSource = ReportRepository.GetPersonnelEfficiency();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading report data:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}