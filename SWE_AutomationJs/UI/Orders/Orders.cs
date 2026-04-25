using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design
{
    public partial class Orders : Form
    {
        private const string AllMenuCategory = "All Menu";
        private static readonly string[] CategoryDisplayOrder =
        {
            "Appetizers",
            "Salads",
            "Entrees",
            "Sides",
            "Sandwiches",
            "Burgers",
            "Beverages"
        };

        private sealed class OrderItemListEntry
        {
            public long OrderItemId { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private readonly int chosenTable;
        private readonly Button[] menuButtons;
        private readonly Label[] legacySectionLabels;
        private long currentOrderId;
        private IReadOnlyList<MenuCatalogItem> menuCatalog = Array.Empty<MenuCatalogItem>();
        private ComboBox categoryComboBox;
        private List<MenuCatalogItem> currentCategoryItems = new List<MenuCatalogItem>();

        public Orders(int tableNumber)
        {
            InitializeComponent();
            chosenTable = tableNumber;
            menuButtons = new[]
            {
                button5, button6, button7, button8, button9, button10,
                button11, button12, button13, button15, button16, button17
            };
            legacySectionLabels = new[] { label6, label8, label9 };
            InitializeCategorySelector();
            ConfigureMenuGrid();
        }

        private void Orders_load(object sender, EventArgs e)
        {
            if (!SessionContext.IsAuthenticated)
            {
                MessageBox.Show("Please sign in again before taking orders.");
                NavigationHelper.ShowAtCurrentPosition(this, new MainMenu());
                return;
            }

            currentOrderId = OrderRepository.GetOrCreateDraftOrder(chosenTable, SessionContext.CurrentEmployee.EmployeeId, 4);
            TableRepository.SetStatus(chosenTable, "Occupied", SessionContext.CurrentEmployee.EmployeeId);
            label2.Text = $"Table: {chosenTable}";
            label3.Text = $"Server: {SessionContext.CurrentEmployee.DisplayName}";
            LoadMenuCatalog();
            LoadOrderState();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new AssignTables());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderHeader header = OrderRepository.GetHeader(currentOrderId);
            if (header == null)
            {
                MessageBox.Show("Order could not be found.");
                return;
            }

            if (header.StatusName != "Open")
            {
                MessageBox.Show($"This order is already {header.StatusName.ToLowerInvariant()}.");
                return;
            }

            IReadOnlyList<OrderLine> items = OrderRepository.GetItems(currentOrderId);
            if (items.Count == 0)
            {
                MessageBox.Show("Add at least one item before sending the order to the kitchen.");
                return;
            }

            OrderRepository.SubmitToKitchen(currentOrderId, SessionContext.CurrentEmployee.EmployeeId);
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "OrderSubmitted", $"Order {currentOrderId} table {chosenTable}");

            Notifications notification = new Notifications();
            notification.Message = "New order for Table " + chosenTable;
            notification.Role = "Kitchen";
            notification.TimeStamp = DateTime.Now.ToShortTimeString();
            NotificationStorage.Notify.Add(notification);

            MessageBox.Show("Order sent to the kitchen.");
            LoadOrderState();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IReadOnlyList<OrderLine> items = OrderRepository.GetItems(currentOrderId);
            foreach (OrderLine item in items)
            {
                OrderRepository.RemoveItem(item.OrderItemId);
            }
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "OrderCleared", $"Order {currentOrderId}");

            LoadOrderState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrderItemListEntry selected = listBox2.SelectedItem as OrderItemListEntry;
            if (selected == null)
            {
                return;
            }

            OrderRepository.RemoveItem(selected.OrderItemId);
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "OrderItemRemoved", $"Order {currentOrderId} item {selected.OrderItemId}");
            LoadOrderState();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            OrderHeader header = OrderRepository.GetHeader(currentOrderId);
            if (header == null)
            {
                MessageBox.Show("Order could not be found.");
                return;
            }

            if (header.StatusName != "Ready")
            {
                MessageBox.Show("Payment can only be completed after the kitchen marks the order ready.");
                return;
            }

            IReadOnlyList<OrderLine> items = OrderRepository.GetItems(currentOrderId);
            if (items.Count == 0)
            {
                MessageBox.Show("No items in the order to pay for.");
                return;
            }

            NavigationHelper.ShowAtCurrentPosition(this, new ProcessPayment(currentOrderId, chosenTable));
        }

        private void AddCatalogItemByButtonIndex(int index)
        {
            if (index < 0 || index >= currentCategoryItems.Count)
            {
                return;
            }

            OrderHeader header = OrderRepository.GetHeader(currentOrderId);
            if (header != null && header.StatusName != "Open")
            {
                MessageBox.Show($"This order is already {header.StatusName.ToLowerInvariant()} and can no longer be edited.");
                return;
            }

            MenuCatalogItem item = currentCategoryItems[index];
            IReadOnlyList<MenuOptionGroup> optionGroups = OrderRepository.GetOptionGroups(item.MenuItemId);
            List<SelectedMenuOption> selectedOptions = null;
            string notes = null;
            if (optionGroups.Count > 0)
            {
                using (MenuItemOptionsForm optionsForm = new MenuItemOptionsForm(item, optionGroups))
                {
                    if (optionsForm.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    selectedOptions = optionsForm.SelectedOptions;
                    notes = optionsForm.SummaryText;
                }
            }

            OrderRepository.AddItem(currentOrderId, item.MenuItemId, 1, null, notes, selectedOptions);
            ActivityLogService.Log(SessionContext.CurrentEmployee.EmployeeId, "OrderItemAdded", $"Order {currentOrderId} {item.ItemName}");
            LoadOrderState();
        }

        private void LoadMenuCatalog()
        {
            menuCatalog = OrderRepository.GetMenuCatalog();
            categoryComboBox.Items.Clear();
            categoryComboBox.Items.Add(AllMenuCategory);
            foreach (string category in menuCatalog
                .Select(x => x.CategoryName)
                .Distinct()
                .OrderBy(GetCategorySortOrder)
                .ThenBy(x => x))
            {
                categoryComboBox.Items.Add(category);
            }

            if (categoryComboBox.Items.Count > 0)
            {
                categoryComboBox.SelectedIndex = 0;
            }
            else
            {
                UpdateCategoryItems();
            }
        }

        private void UpdateCategoryItems()
        {
            string selectedCategory = categoryComboBox.SelectedItem as string;
            IEnumerable<MenuCatalogItem> visibleItems = menuCatalog;

            if (!string.Equals(selectedCategory, AllMenuCategory, StringComparison.Ordinal))
            {
                visibleItems = visibleItems.Where(x => x.CategoryName == selectedCategory);
            }

            currentCategoryItems = visibleItems
                .Take(menuButtons.Length)
                .ToList();

            for (int i = 0; i < menuButtons.Length; i++)
            {
                if (i < currentCategoryItems.Count)
                {
                    menuButtons[i].Text = currentCategoryItems[i].DisplayText;
                    menuButtons[i].Enabled = true;
                }
                else
                {
                    menuButtons[i].Text = "Unavailable";
                    menuButtons[i].Enabled = false;
                }
            }
        }

        private void InitializeCategorySelector()
        {
            categoryComboBox = new ComboBox();
            categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryComboBox.Location = new System.Drawing.Point(143, 10);
            categoryComboBox.Size = new System.Drawing.Size(224, 21);
            categoryComboBox.SelectedIndexChanged += (sender, args) => UpdateCategoryItems();
            panel2.Controls.Add(categoryComboBox);
            panel2.Controls.SetChildIndex(categoryComboBox, 0);
            label5.Text = "Menu Items";
        }

        private void ConfigureMenuGrid()
        {
            label6.Visible = false;
            label8.Visible = false;
            label9.Visible = false;

            System.Drawing.Point[] positions =
            {
                new System.Drawing.Point(13, 46),
                new System.Drawing.Point(143, 46),
                new System.Drawing.Point(276, 46),
                new System.Drawing.Point(13, 98),
                new System.Drawing.Point(143, 98),
                new System.Drawing.Point(276, 98),
                new System.Drawing.Point(13, 150),
                new System.Drawing.Point(143, 150),
                new System.Drawing.Point(276, 150),
                new System.Drawing.Point(13, 202),
                new System.Drawing.Point(143, 202),
                new System.Drawing.Point(276, 202)
            };

            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].Location = positions[i];
                menuButtons[i].Size = new System.Drawing.Size(104, 44);
            }
        }

        private void LoadOrderState()
        {
            OrderHeader header = OrderRepository.GetHeader(currentOrderId);
            IReadOnlyList<OrderLine> items = OrderRepository.GetItems(currentOrderId);

            listBox2.Items.Clear();
            foreach (OrderLine item in items)
            {
                    listBox2.Items.Add(new OrderItemListEntry
                {
                    OrderItemId = item.OrderItemId,
                    DisplayText = BuildOrderLineText(item)
                });
            }

            decimal total = header != null ? header.Total : 0m;
            string status = header != null ? header.StatusName : "Open";
            label7.Text = $"Total: ${total:F2}";
            label4.Text = $"Order List ({status})";
            button4.Enabled = items.Count > 0 && status == "Open";
            button3.Enabled = items.Count > 0 && status == "Open";
            button2.Enabled = items.Count > 0 && status == "Open";
            button14.Enabled = items.Count > 0 && status == "Ready";
        }

        private void button5_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(0); }
        private void button6_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(1); }
        private void button7_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(2); }
        private void button8_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(3); }
        private void button9_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(4); }
        private void button10_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(5); }
        private void button11_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(6); }
        private void button12_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(7); }
        private void button13_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(8); }
        private void button15_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(9); }
        private void button16_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(10); }
        private void button17_Click(object sender, EventArgs e) { AddCatalogItemByButtonIndex(11); }

        private static string BuildOrderLineText(OrderLine item)
        {
            if (string.IsNullOrWhiteSpace(item.Notes))
            {
                return $"{item.ItemName} x{item.Qty} - ${item.UnitPriceAtSale:F2}";
            }

            return $"{item.ItemName} x{item.Qty} - ${item.UnitPriceAtSale:F2} [{item.Notes}]";
        }

        private static int GetCategorySortOrder(string categoryName)
        {
            int index = Array.IndexOf(CategoryDisplayOrder, categoryName);
            return index >= 0 ? index : int.MaxValue;
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
