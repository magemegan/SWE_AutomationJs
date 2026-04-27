using System;
using System.Drawing;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    internal static class NavigationHelper
    {
        public static void ShowAtCurrentPosition(Form currentForm, Form nextForm)
        {
            Point location = currentForm.WindowState == FormWindowState.Normal
                ? currentForm.Location
                : currentForm.RestoreBounds.Location;

            UiTheme.Apply(nextForm);

            nextForm.StartPosition = FormStartPosition.Manual;
            nextForm.Location = location;
            nextForm.Show();
            currentForm.Hide();
        }
    }

    internal static class UiTheme
    {
        public static readonly Color Background = Color.FromArgb(245, 248, 252);
        public static readonly Color Primary = Color.FromArgb(25, 144, 183);
        public static readonly Color PrimaryDark = Color.FromArgb(15, 94, 122);
        public static readonly Color Danger = Color.FromArgb(185, 45, 45);
        public static readonly Color TextDark = Color.FromArgb(35, 45, 55);
        public static readonly Color Card = Color.White;

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;

            foreach (Control control in form.Controls)
            {
                ApplyControl(control);
            }
        }

        public static void ApplyControl(Control control)
        {
            if (control is Label label)
            {
                label.ForeColor = TextDark;

                if (label.Font.Size >= 16)
                {
                    label.Font = new Font("Segoe UI", label.Font.Size, FontStyle.Bold);
                    label.ForeColor = PrimaryDark;
                }
                else
                {
                    label.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
            }

            if (control is Button button)
            {
                StyleButton(button);
            }

            if (control is TextBox textBox)
            {
                textBox.UseWaitCursor = false;
                textBox.Font = new Font("Segoe UI", 10F);
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }

            if (control is ComboBox comboBox)
            {
                comboBox.Font = new Font("Segoe UI", 10F);
                comboBox.FlatStyle = FlatStyle.Flat;
            }

            if (control is DataGridView grid)
            {
                StyleGrid(grid);
            }

            if (control is Panel panel)
            {
                panel.BackColor = Card;
            }

            if (control is GroupBox groupBox)
            {
                groupBox.ForeColor = TextDark;
                groupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

            foreach (Control child in control.Controls)
            {
                ApplyControl(child);
            }
        }

        public static void StyleButton(Button button)
        {
            string text = button.Text.ToLower();

            bool danger =
                text.Contains("logout") ||
                text.Contains("log out") ||
                text.Contains("exit") ||
                text.Contains("delete") ||
                text.Contains("remove") ||
                text.Contains("cancel");

            button.BackColor = danger ? Danger : Primary;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Height = Math.Max(button.Height, 36);
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.Font = new Font("Segoe UI", 9F);
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = TextDark;
            grid.DefaultCellStyle.SelectionBackColor = Primary;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static Label BuildTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y),
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = PrimaryDark
            };
        }
    }
}