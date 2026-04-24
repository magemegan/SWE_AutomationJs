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

            Controls.Add(new Label { Left = 30, Top = 20, Width = 220, Text = "Current Table Layout" });
            tableListBox = new ListBox { Left = 30, Top = 50, Width = 280, Height = 280 };
            Controls.Add(tableListBox);

            Button addTableButton = new Button { Left = 360, Top = 50, Width = 140, Text = "Add Table" };
            addTableButton.Click += (sender, args) =>
            {
                int tableId = TableRepository.AddTable();
                Log("TableAdded", "Table " + tableId);
                RefreshTables();
            };
            Controls.Add(addTableButton);

            Button removeTableButton = new Button { Left = 520, Top = 50, Width = 160, Text = "Remove Selected" };
            removeTableButton.Click += (sender, args) =>
            {
                if (tableListBox.SelectedIndex < 0 || tableListBox.SelectedIndex >= tables.Count) return;
                TableRepository.RemoveTable(tables[tableListBox.SelectedIndex].TableId);
                Log("TableRemoved", "Table " + tables[tableListBox.SelectedIndex].TableId);
                RefreshTables();
            };
            Controls.Add(removeTableButton);

            Button addSeatButton = new Button { Left = 360, Top = 100, Width = 140, Text = "Add Seat" };
            addSeatButton.Click += (sender, args) =>
            {
                if (tableListBox.SelectedIndex < 0 || tableListBox.SelectedIndex >= tables.Count) return;
                TableRepository.AddSeat(tables[tableListBox.SelectedIndex].TableId);
                Log("SeatAdded", "Table " + tables[tableListBox.SelectedIndex].TableId);
                RefreshTables();
            };
            Controls.Add(addSeatButton);

            Controls.Add(new Label { Left = 360, Top = 160, Width = 220, Text = "Merge selected into target:" });
            mergeTargetComboBox = new ComboBox { Left = 360, Top = 190, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(mergeTargetComboBox);

            Button mergeButton = new Button { Left = 540, Top = 188, Width = 140, Text = "Merge Tables" };
            mergeButton.Click += (sender, args) =>
            {
                if (tableListBox.SelectedIndex < 0 || tableListBox.SelectedIndex >= tables.Count || mergeTargetComboBox.SelectedIndex == -1) return;
                int primary = int.Parse(mergeTargetComboBox.SelectedItem.ToString());
                int secondary = tables[tableListBox.SelectedIndex].TableId;
                TableRepository.MergeTables(primary, secondary);
                Log("TablesMerged", $"Primary {primary}, Secondary {secondary}");
                RefreshTables();
            };
            Controls.Add(mergeButton);

            Button backButton = new Button { Left = 360, Top = 260, Width = 90, Text = "Back" };
            backButton.Click += (sender, args) =>
            {
                AdminMenu adminMenu = new AdminMenu();
                adminMenu.Show();
                Hide();
            };
            Controls.Add(backButton);

            Load += (sender, args) => RefreshTables();
        }

        private void RefreshTables()
        {
            tables = TableRepository.GetAllTableStatuses();
            tableListBox.Items.Clear();
            mergeTargetComboBox.Items.Clear();

            foreach (TableStatusInfo table in tables)
            {
                tableListBox.Items.Add($"{table.TableId} - {table.TableCode} - {table.UiStatus}");
                mergeTargetComboBox.Items.Add(table.TableId.ToString());
            }
        }

        private static void Log(string action, string details)
        {
            string employeeId = SessionContext.IsAuthenticated ? SessionContext.CurrentEmployee.EmployeeId : "MANAGER";
            ActivityLogService.Log(employeeId, action, details);
        }
    }
}
