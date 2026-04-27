using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using SWE_AutomationJs_UI_Design.Data;

namespace SWE_AutomationJs_UI_Design
{
    public partial class MenuManagement : Form
    {
        private Panel headerPanel;
        private Label titleLabel;
        private Button btnBack;
        private Button btnRefresh;

        private Panel formPanel;
        private DataGridView grid;

        private ComboBox cboCategory;
        private TextBox txtName;
        private NumericUpDown numPrice;
        private CheckBox chkActive;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        private int selectedMenuItemId = -1;

        private readonly Color Primary = Color.FromArgb(25, 144, 183);
        private readonly Color LightBackground = Color.FromArgb(245, 248, 250);
        private readonly Color Danger = Color.FromArgb(185, 45, 45);

        public MenuManagement()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            BuildLayout();
            LoadCategories();
            LoadMenuItems();
        }

        private void BuildLayout()
        {
            Text = "Menu Management";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 700);
            MinimumSize = new Size(950, 600);
            BackColor = Color.White;

            Controls.Clear();

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Primary
            };

            btnBack = new Button
            {
                Text = "← Back",
                Width = 100,
                Height = 38,
                Left = 15,
                Top = 16,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Primary,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += btnBack_Click;

            titleLabel = new Label
            {
                Text = "Menu Management",
                AutoSize = true,
                Left = 135,
                Top = 20,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };

            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 38,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = ClientSize.Width - 120,
                Top = 16,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Primary,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += btnRefresh_Click;

            headerPanel.Controls.Add(btnBack);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnRefresh);
            Controls.Add(headerPanel);

            formPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 135,
                BackColor = LightBackground,
                Padding = new Padding(20)
            };

            Label lblCategory = MakeLabel("Category", 20, 20);
            cboCategory = new ComboBox
            {
                Left = 20,
                Top = 48,
                Width = 210,
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            Label lblName = MakeLabel("Item Name", 250, 20);
            txtName = new TextBox
            {
                Left = 250,
                Top = 48,
                Width = 280,
                Height = 30,
                Font = new Font("Segoe UI", 10)
            };

            Label lblPrice = MakeLabel("Price", 550, 20);
            numPrice = new NumericUpDown
            {
                Left = 550,
                Top = 48,
                Width = 110,
                Height = 30,
                DecimalPlaces = 2,
                Maximum = 999,
                Font = new Font("Segoe UI", 10)
            };

            chkActive = new CheckBox
            {
                Text = "Active",
                Left = 685,
                Top = 51,
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };

            btnAdd = MakeActionButton("Add", 20, 90, Primary);
            btnUpdate = MakeActionButton("Update", 125, 90, Primary);
            btnDelete = MakeActionButton("Delete", 230, 90, Danger);
            btnClear = MakeActionButton("Clear", 335, 90, Color.FromArgb(90, 90, 90));

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            formPanel.Controls.Add(lblCategory);
            formPanel.Controls.Add(cboCategory);
            formPanel.Controls.Add(lblName);
            formPanel.Controls.Add(txtName);
            formPanel.Controls.Add(lblPrice);
            formPanel.Controls.Add(numPrice);
            formPanel.Controls.Add(chkActive);
            formPanel.Controls.Add(btnAdd);
            formPanel.Controls.Add(btnUpdate);
            formPanel.Controls.Add(btnDelete);
            formPanel.Controls.Add(btnClear);

            Controls.Add(formPanel);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                EnableHeadersVisualStyles = false
            };

            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersHeight = 38;
            grid.RowTemplate.Height = 34;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 238, 245);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.CellClick += grid_CellClick;

            Controls.Add(grid);

            Resize += MenuManagement_Resize;
        }

        private Label MakeLabel(string text, int left, int top)
        {
            return new Label
            {
                Text = text,
                Left = left,
                Top = top,
                AutoSize = true,
                ForeColor = Color.FromArgb(35, 35, 35),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
        }

        private Button MakeActionButton(string text, int left, int top, Color color)
        {
            Button button = new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = 90,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadCategories()
        {
            DataTable table = new DataTable();
            table.Columns.Add("CategoryId", typeof(int));
            table.Columns.Add("CategoryName", typeof(string));

            using (SqliteConnection conn = Db.Open())
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT CategoryId, CategoryName
                    FROM MenuCategories
                    ORDER BY CategoryName;";

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        table.Rows.Add(
                            Convert.ToInt32(reader["CategoryId"]),
                            reader["CategoryName"].ToString()
                        );
                    }
                }
            }

            cboCategory.DataSource = table;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";
        }

        private void LoadMenuItems()
        {
            DataTable table = new DataTable();

            table.Columns.Add("MenuItemId", typeof(int));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Active", typeof(string));
            table.Columns.Add("CategoryId", typeof(int));
            table.Columns.Add("RawPrice", typeof(decimal));
            table.Columns.Add("IsActive", typeof(int));

            using (SqliteConnection conn = Db.Open())
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT 
                        mi.MenuItemId,
                        mc.CategoryName,
                        mi.ItemName,
                        mi.Price,
                        mi.CategoryId,
                        mi.IsActive
                    FROM MenuItems mi
                    INNER JOIN MenuCategories mc 
                        ON mi.CategoryId = mc.CategoryId
                    ORDER BY mc.CategoryName, mi.ItemName;";

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal price = Convert.ToDecimal(reader["Price"]);
                        int active = Convert.ToInt32(reader["IsActive"]);

                        table.Rows.Add(
                            Convert.ToInt32(reader["MenuItemId"]),
                            reader["CategoryName"].ToString(),
                            reader["ItemName"].ToString(),
                            "$" + price.ToString("0.00"),
                            active == 1 ? "Yes" : "No",
                            Convert.ToInt32(reader["CategoryId"]),
                            price,
                            active
                        );
                    }
                }
            }

            grid.DataSource = table;

            grid.Columns["MenuItemId"].Visible = false;
            grid.Columns["CategoryId"].Visible = false;
            grid.Columns["RawPrice"].Visible = false;
            grid.Columns["IsActive"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            using (SqliteConnection conn = Db.Open())
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO MenuItems (CategoryId, ItemName, Price, IsActive)
                    VALUES (@CategoryId, @ItemName, @Price, @IsActive);";

                cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(cboCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@ItemName", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", numPrice.Value);
                cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked ? 1 : 0);
                cmd.ExecuteNonQuery();
            }

            LoadMenuItems();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMenuItemId < 0)
            {
                MessageBox.Show("Select a menu item first.");
                return;
            }

            if (!ValidateForm()) return;

            using (SqliteConnection conn = Db.Open())
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE MenuItems
                    SET CategoryId = @CategoryId,
                        ItemName = @ItemName,
                        Price = @Price,
                        IsActive = @IsActive
                    WHERE MenuItemId = @MenuItemId;";

                cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(cboCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@ItemName", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", numPrice.Value);
                cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@MenuItemId", selectedMenuItemId);
                cmd.ExecuteNonQuery();
            }

            LoadMenuItems();
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMenuItemId < 0)
            {
                MessageBox.Show("Select a menu item first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this menu item?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            using (SqliteConnection conn = Db.Open())
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM MenuItems WHERE MenuItemId = @MenuItemId;";
                cmd.Parameters.AddWithValue("@MenuItemId", selectedMenuItemId);
                cmd.ExecuteNonQuery();
            }

            LoadMenuItems();
            ClearForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCategories();
            LoadMenuItems();
            ClearForm();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigationHelper.ShowAtCurrentPosition(this, new AdminMenu());
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = grid.Rows[e.RowIndex];

            selectedMenuItemId = Convert.ToInt32(row.Cells["MenuItemId"].Value);
            cboCategory.SelectedValue = Convert.ToInt32(row.Cells["CategoryId"].Value);
            txtName.Text = row.Cells["Item"].Value.ToString();
            numPrice.Value = Convert.ToDecimal(row.Cells["RawPrice"].Value);
            chkActive.Checked = Convert.ToInt32(row.Cells["IsActive"].Value) == 1;
        }

        private bool ValidateForm()
        {
            if (cboCategory.SelectedValue == null)
            {
                MessageBox.Show("Select a category.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter an item name.");
                return false;
            }

            if (numPrice.Value <= 0)
            {
                MessageBox.Show("Enter a price greater than 0.");
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            selectedMenuItemId = -1;
            txtName.Clear();
            numPrice.Value = 0;
            chkActive.Checked = true;

            if (cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;

            grid.ClearSelection();
        }

        private void MenuManagement_Resize(object sender, EventArgs e)
        {
            if (btnRefresh != null)
            {
                btnRefresh.Left = ClientSize.Width - 120;
            }
        }
    }
}