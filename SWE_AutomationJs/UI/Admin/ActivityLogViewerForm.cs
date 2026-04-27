using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    internal sealed class ActivityLogViewerForm : Form
    {
        private readonly ListBox logListBox;
        private readonly TextBox filterTextBox;

        public ActivityLogViewerForm()
        {
            Text = "Activity Log";
            ClientSize = new Size(900, 520);

            Label titleLabel = UiTheme.BuildTitle("Activity Log", 18, 18);
            Controls.Add(titleLabel);

            Label filterLabel = new Label();
            filterLabel.AutoSize = true;
            filterLabel.Location = new Point(22, 62);
            filterLabel.Text = "Filter:";
            Controls.Add(filterLabel);

            filterTextBox = new TextBox();
            filterTextBox.Location = new Point(75, 58);
            filterTextBox.Size = new Size(310, 27);
            filterTextBox.TextChanged += (sender, args) => LoadLogs();
            Controls.Add(filterTextBox);

            Button refreshButton = new Button();
            refreshButton.Location = new Point(405, 56);
            refreshButton.Size = new Size(90, 32);
            refreshButton.Text = "Refresh";
            refreshButton.Click += (sender, args) => LoadLogs();
            Controls.Add(refreshButton);

            Button exportButton = new Button();
            exportButton.Location = new Point(505, 56);
            exportButton.Size = new Size(110, 32);
            exportButton.Text = "Export .txt";
            exportButton.Click += ExportButton_Click;
            Controls.Add(exportButton);

            Button backButton = new Button();
            backButton.Location = new Point(790, 56);
            backButton.Size = new Size(86, 32);
            backButton.Text = "Back";
            backButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
            };
            Controls.Add(backButton);

            logListBox = new ListBox();
            logListBox.HorizontalScrollbar = true;
            logListBox.Location = new Point(22, 104);
            logListBox.Size = new Size(854, 384);
            logListBox.Font = new Font("Segoe UI", 9F);
            logListBox.BackColor = Color.White;
            logListBox.ForeColor = UiTheme.TextDark;
            Controls.Add(logListBox);

            UiTheme.Apply(this);
            LoadLogs();
        }

        private void LoadLogs()
        {
            string filter = filterTextBox.Text.Trim();

            string[] lines = ActivityLogService.ReadAll()
                .Where(line => string.IsNullOrWhiteSpace(filter)
                    || line.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .Reverse()
                .ToArray();

            logListBox.Items.Clear();

            foreach (string line in lines)
            {
                logListBox.Items.Add(line);
            }

            if (lines.Length == 0)
            {
                logListBox.Items.Add("No activity log entries found.");
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (ActivityLogService.ReadAll().Length == 0)
            {
                MessageBox.Show("There are no log entries to export.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text Files (*.txt)|*.txt";
                dialog.FileName = "activity-log-export.txt";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                ActivityLogService.ExportTo(dialog.FileName);

                MessageBox.Show(
                    "Activity log exported to " + Path.GetFileName(dialog.FileName) + ".",
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}