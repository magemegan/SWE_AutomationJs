using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public sealed class OverrideApprovalForm : Form
    {
        private readonly ListBox requestListBox;
        private readonly Label detailLabel;
        private IReadOnlyList<OverrideRequest> requests = Array.Empty<OverrideRequest>();

        public OverrideApprovalForm()
        {
            Text = "Override Approvals";
            ClientSize = new System.Drawing.Size(760, 420);

            Controls.Add(new Label { Left = 30, Top = 20, Width = 200, Text = "Pending / Reviewed Overrides" });

            requestListBox = new ListBox { Left = 30, Top = 50, Width = 300, Height = 280 };
            requestListBox.SelectedIndexChanged += RequestListBox_SelectedIndexChanged;
            Controls.Add(requestListBox);

            detailLabel = new Label { Left = 360, Top = 50, Width = 360, Height = 180, AutoSize = false };
            Controls.Add(detailLabel);

            Button approveButton = new Button { Left = 360, Top = 260, Width = 100, Text = "Approve" };
            approveButton.Click += (sender, args) => UpdateSelected("Approved");
            Controls.Add(approveButton);

            Button denyButton = new Button { Left = 480, Top = 260, Width = 100, Text = "Deny" };
            denyButton.Click += (sender, args) => UpdateSelected("Denied");
            Controls.Add(denyButton);

            Button backButton = new Button { Left = 600, Top = 260, Width = 90, Text = "Back" };
            backButton.Click += (sender, args) =>
            {
                AdminMenu adminMenu = new AdminMenu();
                adminMenu.Show();
                Hide();
            };
            Controls.Add(backButton);

            Load += (sender, args) => RefreshRequests();
        }

        private void RefreshRequests()
        {
            requests = OverrideRequestStore.GetAll();
            requestListBox.Items.Clear();
            foreach (OverrideRequest request in requests)
            {
                requestListBox.Items.Add($"{request.EmployeeName} - {request.RequestedAction} - {request.Status}");
            }
            detailLabel.Text = "";
        }

        private void RequestListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (requestListBox.SelectedIndex < 0 || requestListBox.SelectedIndex >= requests.Count)
            {
                return;
            }

            OverrideRequest request = requests[requestListBox.SelectedIndex];
            detailLabel.Text =
                $"Employee: {request.EmployeeName} [{request.EmployeeId}]\r\n" +
                $"Action: {request.RequestedAction}\r\n" +
                $"Status: {request.Status}\r\n" +
                $"Requested: {request.RequestedAtUtc.ToLocalTime()}\r\n" +
                $"Details: {request.Details}";
        }

        private void UpdateSelected(string status)
        {
            if (!SessionContext.IsAuthenticated || requestListBox.SelectedIndex < 0 || requestListBox.SelectedIndex >= requests.Count)
            {
                return;
            }

            OverrideRequest request = requests[requestListBox.SelectedIndex];
            OverrideRequestStore.SetStatus(request.RequestId, status, SessionContext.CurrentEmployee.EmployeeId);
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "Override" + status, request.RequestedAction + " for " + request.EmployeeId);
            RefreshRequests();
        }
    }
}
