using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    internal sealed class MenuItemOptionsForm : Form
    {
        private readonly IReadOnlyList<MenuOptionGroup> optionGroups;
        private readonly Dictionary<int, ComboBox> singleSelectControls = new Dictionary<int, ComboBox>();
        private readonly Dictionary<int, CheckedListBox> multiSelectControls = new Dictionary<int, CheckedListBox>();

        public MenuItemOptionsForm(MenuCatalogItem item, IReadOnlyList<MenuOptionGroup> optionGroups)
        {
            this.optionGroups = optionGroups;
            SelectedOptions = new List<SelectedMenuOption>();

            Text = "Customize Item";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(520, 140 + (optionGroups.Count * 120));
            MinimumSize = new Size(520, 240);

            Label titleLabel = new Label();
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(16, 14);
            titleLabel.Text = item.ItemName;
            Controls.Add(titleLabel);

            Label priceLabel = new Label();
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(18, 42);
            priceLabel.Text = string.Format("Base Price: ${0:F2}", item.Price);
            Controls.Add(priceLabel);

            FlowLayoutPanel groupsPanel = new FlowLayoutPanel();
            groupsPanel.Location = new Point(16, 68);
            groupsPanel.Size = new Size(488, ClientSize.Height - 130);
            groupsPanel.AutoScroll = true;
            groupsPanel.FlowDirection = FlowDirection.TopDown;
            groupsPanel.WrapContents = false;
            Controls.Add(groupsPanel);

            foreach (MenuOptionGroup group in optionGroups)
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Text = BuildGroupLabel(group);
                groupBox.Size = new Size(456, group.MaxSelections == 1 ? 76 : 108);

                if (group.MaxSelections == 1)
                {
                    ComboBox comboBox = new ComboBox();
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Location = new Point(12, 30);
                    comboBox.Size = new Size(420, 24);
                    foreach (MenuOptionChoice option in group.Options)
                    {
                        comboBox.Items.Add(option);
                    }

                    if (!group.IsRequired)
                    {
                        comboBox.Items.Insert(0, null);
                    }

                    groupBox.Controls.Add(comboBox);
                    singleSelectControls[group.OptionGroupId] = comboBox;
                }
                else
                {
                    CheckedListBox checkedListBox = new CheckedListBox();
                    checkedListBox.Location = new Point(12, 24);
                    checkedListBox.Size = new Size(420, 68);
                    checkedListBox.CheckOnClick = true;
                    foreach (MenuOptionChoice option in group.Options)
                    {
                        checkedListBox.Items.Add(option);
                    }

                    checkedListBox.ItemCheck += (sender, args) =>
                    {
                        int checkedCount = checkedListBox.CheckedItems.Count;
                        if (args.NewValue == CheckState.Checked)
                        {
                            checkedCount++;
                        }

                        if (args.CurrentValue == CheckState.Checked)
                        {
                            checkedCount--;
                        }

                        if (checkedCount > group.MaxSelections)
                        {
                            args.NewValue = CheckState.Unchecked;
                            MessageBox.Show(
                                string.Format("You can select up to {0} option(s) for {1}.", group.MaxSelections, group.GroupName),
                                "Too Many Selections",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    };

                    groupBox.Controls.Add(checkedListBox);
                    multiSelectControls[group.OptionGroupId] = checkedListBox;
                }

                groupsPanel.Controls.Add(groupBox);
            }

            Button cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(318, ClientSize.Height - 50);
            cancelButton.Size = new Size(90, 28);
            cancelButton.DialogResult = DialogResult.Cancel;
            Controls.Add(cancelButton);

            Button addButton = new Button();
            addButton.Text = "Add Item";
            addButton.Location = new Point(414, ClientSize.Height - 50);
            addButton.Size = new Size(90, 28);
            addButton.Click += AddButton_Click;
            Controls.Add(addButton);

            AcceptButton = addButton;
            CancelButton = cancelButton;
        }

        public List<SelectedMenuOption> SelectedOptions { get; private set; }

        public string SummaryText
        {
            get
            {
                return SelectedOptions.Count == 0
                    ? null
                    : string.Join("; ", SelectedOptions.Select(x => string.Format("{0}: {1}", x.GroupName, x.OptionName)));
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            List<SelectedMenuOption> chosenOptions = new List<SelectedMenuOption>();

            foreach (MenuOptionGroup group in optionGroups)
            {
                if (group.MaxSelections == 1)
                {
                    ComboBox comboBox = singleSelectControls[group.OptionGroupId];
                    MenuOptionChoice choice = comboBox.SelectedItem as MenuOptionChoice;
                    if (choice == null)
                    {
                        if (group.IsRequired || group.MinSelections > 0)
                        {
                            MessageBox.Show(
                                string.Format("Please choose an option for {0}.", group.GroupName),
                                "Selection Required",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        continue;
                    }

                    chosenOptions.Add(CreateSelectedOption(group, choice));
                    continue;
                }

                CheckedListBox checkedListBox = multiSelectControls[group.OptionGroupId];
                List<MenuOptionChoice> checkedOptions = checkedListBox.CheckedItems.Cast<MenuOptionChoice>().ToList();
                if (checkedOptions.Count < group.MinSelections)
                {
                    MessageBox.Show(
                        string.Format("Please choose at least {0} option(s) for {1}.", group.MinSelections, group.GroupName),
                        "Selection Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                foreach (MenuOptionChoice option in checkedOptions)
                {
                    chosenOptions.Add(CreateSelectedOption(group, option));
                }
            }

            SelectedOptions = chosenOptions
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.GroupName)
                .ThenBy(x => x.OptionName)
                .ToList();

            DialogResult = DialogResult.OK;
            Close();
        }

        private static string BuildGroupLabel(MenuOptionGroup group)
        {
            if (group.IsRequired || group.MinSelections > 0)
            {
                return string.Format("{0} (required)", group.GroupName);
            }

            return group.GroupName;
        }

        private static SelectedMenuOption CreateSelectedOption(MenuOptionGroup group, MenuOptionChoice choice)
        {
            return new SelectedMenuOption
            {
                GroupName = group.GroupName,
                OptionName = choice.OptionName,
                PriceDelta = choice.PriceDelta,
                DisplayOrder = (group.DisplayOrder * 100) + choice.DisplayOrder
            };
        }
    }
}
