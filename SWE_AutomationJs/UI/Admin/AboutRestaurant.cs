using System.Drawing;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public class AboutRestaurant : Form
    {
        public AboutRestaurant()
        {
            BuildPage();
        }

        private void BuildPage()
        {
            Text = "About J's Corner Restaurant";
            Size = new Size(700, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label
            {
                Text = "J's Corner Restaurant",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20)
            };

            TextBox info = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, 70),
                Size = new Size(630, 430),
                Text =
@"CONTACT INFO
Website: jscorner.com
Address: 680 Arntson Dr., Marietta, GA 30060
Phone: (470) 555-1212

HOURS OF OPERATION
Saturday: 11AM–9:30PM
Sunday: Closed
Monday: 11AM–9:30PM
Tuesday: 11AM–9:30PM
Wednesday: 11:30AM–9:30PM
Thursday: 11AM–9:30PM
Friday: 11AM–9:30PM

FAQ
Employees:
- 5 waiters
- 2 busboys
- 5 chefs
- 1 manager

Payments:
- Cash and credit cards accepted.

Login:
- Employees have a 6-character alphanumeric ID.
- Passwords cannot be simple values like 1111 or 123456.

Tables:
- Tables hold 1–4 customers.
- Waiters can mark tables dirty.
- Busboys can mark tables clean.
- Managers can manage operations.

Inventory:
- Inventory is checked weekly."
            };

            Button closeButton = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(555, 515)
            };

            closeButton.Click += (sender, e) => Close();

            Controls.Add(title);
            Controls.Add(info);
            Controls.Add(closeButton);
        }
    }
}