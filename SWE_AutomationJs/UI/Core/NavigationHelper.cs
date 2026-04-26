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

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            foreach (Control control in form.Controls)
            {
                ApplyControl(control);
            }
        }

        private static void ApplyControl(Control control)
        {
            if (control is Label label)
            {
                label.ForeColor = PrimaryDark;
            }

            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.BackColor =
                    button.Text.ToLower().Contains("log out") ||
                    button.Text.ToLower().Contains("logout") ||
                    button.Text.ToLower().Contains("exit")
                        ? Danger
                        : Primary;

                button.ForeColor = Color.White;
                button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                button.Cursor = Cursors.Hand;
            }

            if (control is TextBox textBox)
            {
                textBox.UseWaitCursor = false;
                textBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }

            foreach (Control child in control.Controls)
            {
                ApplyControl(child);
            }
        }
    }
}