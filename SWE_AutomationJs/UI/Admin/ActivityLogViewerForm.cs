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

            Label titleLabel = new Label();
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            titleLabel.Location = new Point(18, 18);
            titleLabel.Text = "Activity Log";
            Controls.Add(titleLabel);

            Label filterLabel = new Label();
            filterLabel.AutoSize = true;
            filterLabel.Location = new Point(20, 58);
            filterLabel.Text = "Filter:";
            Controls.Add(filterLabel);

            filterTextBox = new TextBox();
            filterTextBox.Location = new Point(62, 54);
            filterTextBox.Size = new Size(330, 23);
            filterTextBox.TextChanged += (sender, args) => LoadLogs();
            Controls.Add(filterTextBox);

            Button refreshButton = new Button();
            refreshButton.Location = new Point(410, 52);
            refreshButton.Size = new Size(80, 27);
            refreshButton.Text = "Refresh";
            refreshButton.Click += (sender, args) => LoadLogs();
            Controls.Add(refreshButton);

            Button exportButton = new Button();
            exportButton.Location = new Point(500, 52);
            exportButton.Size = new Size(96, 27);
            exportButton.Text = "Export .txt";
            exportButton.Click += ExportButton_Click;
            Controls.Add(exportButton);

            Button backButton = new Button();
            backButton.Location = new Point(804, 52);
            backButton.Size = new Size(72, 27);
            backButton.Text = "Back";
            backButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
            };
            Controls.Add(backButton);

            logListBox = new ListBox();
            logListBox.HorizontalScrollbar = true;
            logListBox.Location = new Point(22, 94);
            logListBox.Size = new Size(854, 394);
            Controls.Add(logListBox);

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
                MessageBox.Show("Activity log exported to " + Path.GetFileName(dialog.FileName) + ".");
            }
        }
    }
}
