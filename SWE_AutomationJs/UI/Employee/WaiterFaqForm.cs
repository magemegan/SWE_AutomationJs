using System.Drawing;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public sealed class WaiterFaqForm : Form
    {
        public WaiterFaqForm()
        {
            Text = "Waiter FAQ";
            ClientSize = new Size(800, 450);

            Label titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(260, 30),
                Text = "Restaurant FAQ"
            };
            Controls.Add(titleLabel);

            Label detailsLabel = new Label
            {
                AutoSize = true,
                Location = new Point(165, 100),
                Text =
                    "CONTACT INFO:\r\n" +
                    "jscorner.com\r\n" +
                    "680 Arntson Dr., Marietta, GA 30060\r\n" +
                    "(470) 555-1212\r\n\r\n" +
                    "HOURS OPERATING:\r\n" +
                    "Saturday: 11AM-9:30PM\r\n" +
                    "Sunday: Closed\r\n" +
                    "Monday: 11AM-9:30PM\r\n" +
                    "Tuesday: 11AM-9:30PM\r\n" +
                    "Wednesday: 11:30AM-9:30PM\r\n" +
                    "Thursday: 11AM-9:30PM\r\n" +
                    "Friday: 11AM-9:30PM"
            };
            Controls.Add(detailsLabel);

            Button backButton = new Button
            {
                Location = new Point(35, 390),
                Size = new Size(75, 23),
                Text = "Back"
            };
            backButton.Click += (sender, args) =>
            {
                NavigationHelper.ShowAtCurrentPosition(this, new WaiterScreen());
            };
            Controls.Add(backButton);
        }
    }
}
