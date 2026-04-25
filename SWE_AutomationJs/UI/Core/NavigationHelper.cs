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
}
