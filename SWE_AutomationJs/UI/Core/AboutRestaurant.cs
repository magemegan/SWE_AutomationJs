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
            ClientSize = new Size(720, 600);

            Label title = UiTheme.BuildTitle("J's Corner Restaurant", 25, 20);
            Controls.Add(title);

            TextBox info = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 11),
                Location = new Point(25, 75),
                Size = new Size(670, 440),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
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

            Controls.Add(info);

            Button closeButton = new Button
            {
                Text = "Close",
                Size = new Size(110, 36),
                Location = new Point(585, 530)
            };

            closeButton.Click += (sender, e) => Close();
            Controls.Add(closeButton);

            UiTheme.Apply(this);
        }
    }
}