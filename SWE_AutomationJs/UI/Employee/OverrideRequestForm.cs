using System;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public sealed class OverrideRequestForm : Form
    {
        private readonly ComboBox actionComboBox;
        private readonly TextBox detailsTextBox;

        public OverrideRequestForm()
        {
            Text = "Request Override";
            ClientSize = new System.Drawing.Size(540, 320);

            Controls.Add(new Label { Left = 30, Top = 30, Width = 150, Text = "Restricted Action:" });
            actionComboBox = new ComboBox
            {
                Left = 180,
                Top = 26,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            actionComboBox.Items.AddRange(new object[]
            {
                "Price Override",
                "Reopen Closed Order",
                "Manual Table Reassignment",
                "Merge Large Party Tables"
            });
            Controls.Add(actionComboBox);

            Controls.Add(new Label { Left = 30, Top = 80, Width = 150, Text = "Details:" });
            detailsTextBox = new TextBox
            {
                Left = 180,
                Top = 76,
                Width = 300,
                Height = 120,
                Multiline = true
            };
            Controls.Add(detailsTextBox);

            Button submitButton = new Button { Left = 180, Top = 230, Width = 120, Text = "Submit Request" };
            submitButton.Click += SubmitButton_Click;
            Controls.Add(submitButton);

            Button backButton = new Button { Left = 320, Top = 230, Width = 90, Text = "Back" };
            backButton.Click += (sender, args) =>
            {
                WaiterScreen waiterScreen = new WaiterScreen();
                waiterScreen.Show();
                Hide();
            };
            Controls.Add(backButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("Please sign in again before requesting an override.");
                return;
            }

            if (actionComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Select a restricted action to request.");
                return;
            }

            OverrideRequestStore.Add(new OverrideRequest
            {
                EmployeeId = SessionContext.CurrentEmployee.EmployeeId,
                EmployeeName = SessionContext.CurrentEmployee.DisplayName,
                RequestedAction = actionComboBox.SelectedItem.ToString(),
                Details = detailsTextBox.Text.Trim()
            });

            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "OverrideRequested", actionComboBox.SelectedItem.ToString());
            MessageBox.Show("Override request submitted for manager approval.");
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            Hide();
        }
    }
}
