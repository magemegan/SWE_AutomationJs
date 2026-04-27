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

            nextForm.StartPosition = FormStartPosition.Manual;
            nextForm.Location = location;
            nextForm.Show();
            currentForm.Hide();
        }
    }

    internal static class UiTheme
    {
        private static readonly Color Background = Color.FromArgb(245, 248, 252);
        private static readonly Color Primary = Color.FromArgb(25, 144, 183);
        private static readonly Color PrimaryDark = Color.FromArgb(15, 94, 122);
        private static readonly Color Danger = Color.FromArgb(185, 45, 45);
        private static readonly Color TextDark = Color.FromArgb(35, 45, 55);

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            form.StartPosition = FormStartPosition.CenterScreen;

            foreach (Control control in form.Controls)
            {
                ApplyControl(control);
            }
        }

        private static void ApplyControl(Control control)
        {
            if (control is Label label)
            {
                label.ForeColor = TextDark;
            }

            if (control is Button button)
            {
                StyleButton(button);
            }

            if (control is TextBox textBox)
            {
                textBox.UseWaitCursor = false;
                textBox.Font = new Font("Segoe UI", 10F);
            }

            if (control is ComboBox comboBox)
            {
                comboBox.Font = new Font("Segoe UI", 10F);
            }

            if (control is DataGridView grid)
            {
                grid.Font = new Font("Segoe UI", 9F);
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                grid.RowHeadersVisible = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                text.Contains("log out") ||
                text.Contains("logout") ||
                text.Contains("exit") ||
                text.Contains("delete") ||
                text.Contains("remove");

            button.BackColor = danger ? Danger : Primary;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
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