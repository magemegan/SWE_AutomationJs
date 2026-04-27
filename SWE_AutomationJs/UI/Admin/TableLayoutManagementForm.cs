using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public sealed class TableLayoutManagementForm : Form
    {
        private readonly ListBox tableListBox;
        private readonly ComboBox mergeTargetComboBox;
        private IReadOnlyList<TableStatusInfo> tables = Array.Empty<TableStatusInfo>();

        public TableLayoutManagementForm()
        {
            Text = "Table Layout Management";
            ClientSize = new System.Drawing.Size(760, 420);

            // TITLE
            Label title = UiTheme.BuildTitle("Table Layout Management", 25, 15);
            Controls.Add(title);

            // LEFT LABEL
            Label layoutLabel = new Label
            {
                Left = 30,
                Top = 70,
                Width = 250,
                Text = "Current Table Layout"
            };
            Controls.Add(layoutLabel);

            // TABLE LIST
            tableListBox = new ListBox
            {
                Left = 30,
                Top = 100,
                Width = 300,
                Height = 260,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                BackColor = System.Drawing.Color.White
            };
            Controls.Add(tableListBox);

            // BUTTONS
            Button addTableButton = new Button
            {
                Left = 360,
                Top = 100,
                Width = 150,
                Height = 36,
                Text = "Add Table"
            };
            addTableButton.Click += (sender, args) =>
            {
                int tableId = TableRepository.AddTable();
                Log("TableAdded", "Table " + tableId);
                RefreshTables();
            };
            Controls.Add(addTableButton);

            Button removeTableButton = new Button
            {
                Left = 520,
                Top = 100,
                Width = 180,
                Height = 36,
                Text = "Remove Selected"
            };
            removeTableButton.Click += (sender, args) =>
            {
                if (tableListBox.SelectedIndex < 0 || tableListBox.SelectedIndex >= tables.Count) return;
                TableRepository.RemoveTable(tables[tableListBox.SelectedIndex].TableId);
                Log("TableRemoved", "Table " + tables[tableListBox.SelectedIndex].TableId);
                RefreshTables();
            };
            Controls.Add(removeTableButton);

            Button addSeatButton = new Button
            {
                Left = 360,
                Top = 150,
                Width = 150,
                Height = 36,
                Text = "Add Seat"
            };
            addSeatButton.Click += (sender, args) =>
            {
                RunTableAction(() =>
                {
                    int tableId = GetSelectedTableId();
                    TableRepository.AddSeat(tableId);
                    Log("SeatAdded", "Table " + tableId + " +1 seat");
                });
            };
            Controls.Add(addSeatButton);

            Button removeSeatButton = new Button
            {
                Left = 520,
                Top = 150,
                Width = 180,
                Height = 36,
                Text = "Remove Seat"
            };
            removeSeatButton.Click += (sender, args) =>
            {
                RunTableAction(() =>
                {
                    int tableId = GetSelectedTableId();
                    TableRepository.RemoveSeat(tableId);
                    Log("SeatRemoved", "Table " + tableId + " -1 seat");
                });
            };
            Controls.Add(removeSeatButton);

            // MERGE
            Label mergeLabel = new Label
            {
                Left = 360,
                Top = 210,
                Width = 250,
                Text = "Merge selected into target:"
            };
            Controls.Add(mergeLabel);

            mergeTargetComboBox = new ComboBox
            {
                Left = 360,
                Top = 240,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(mergeTargetComboBox);

            Button mergeButton = new Button
            {
                Left = 540,
                Top = 238,
                Width = 160,
                Height = 36,
                Text = "Merge Tables"
            };
            mergeButton.Click += (sender, args) =>
            {
                RunTableAction(() =>
                {
                    if (mergeTargetComboBox.SelectedIndex == -1)
                        throw new InvalidOperationException("Choose a target table.");

                    int primary = int.Parse(mergeTargetComboBox.SelectedItem.ToString());
                    int secondary = GetSelectedTableId();

                    TableRepository.MergeTables(primary, secondary);
                    Log("TablesMerged", $"Primary {primary}, Secondary {secondary}");
                });
            };
            Controls.Add(mergeButton);

            // BACK BUTTON
            Button backButton = new Button
            {
                Left = 360,
                Top = 300,
                Width = 100,
                Height = 36,
                Text = "Back"
            };
            backButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
            };
            Controls.Add(backButton);

            // APPLY THEME
            UiTheme.Apply(this);

            Load += (sender, args) => RefreshTables();
        }

        private void RefreshTables()
        {
            tables = TableRepository.GetAllTableStatuses();

            tableListBox.Items.Clear();
            mergeTargetComboBox.Items.Clear();

            foreach (TableStatusInfo table in tables)
            {
                tableListBox.Items.Add(
                    $"{table.TableId} - {table.TableCode} - {table.SeatCapacity} seats - {table.UiStatus}"
                );

                mergeTargetComboBox.Items.Add(table.TableId.ToString());
            }
        }

        private int GetSelectedTableId()
        {
            if (tableListBox.SelectedIndex < 0 || tableListBox.SelectedIndex >= tables.Count)
                throw new InvalidOperationException("Select a table first.");

            return tables[tableListBox.SelectedIndex].TableId;
        }

        private void RunTableAction(Action action)
        {
            try
            {
                action();
                RefreshTables();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Table Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static void Log(string action, string details)
        {
            string employeeId = SessionContext.IsAuthenticated
                ? SessionContext.CurrentEmployee.EmployeeId
                : "MANAGER";

            ActivityLogService.Log(employeeId, action, details);
        }
    }
}