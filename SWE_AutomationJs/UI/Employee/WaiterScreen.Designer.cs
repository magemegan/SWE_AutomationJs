namespace SWE_AutomationJs_UI_Design
{
    partial class WaiterScreen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonOverride = new System.Windows.Forms.Button();
            this.buttonFaq = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // label
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.label1.Location = new System.Drawing.Point(177, 70);
            this.label1.Text = "Welcome Waiter/Waitress";

            // Logout
            this.button1.Location = new System.Drawing.Point(29, 388);
            this.button1.Text = "Log Out";
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // Assign Tables
            this.button3.Location = new System.Drawing.Point(106, 244);
            this.button3.Size = new System.Drawing.Size(142, 53);
            this.button3.Text = "Assign Tables";
            this.button3.Click += new System.EventHandler(this.button3_Click);

            // Schedule
            this.button7.Location = new System.Drawing.Point(540, 244);
            this.button7.Size = new System.Drawing.Size(133, 53);
            this.button7.Text = "Access Schedule";
            this.button7.Click += new System.EventHandler(this.button7_Click);

            // Payments
            this.button5.Location = new System.Drawing.Point(319, 244);
            this.button5.Size = new System.Drawing.Size(142, 53);
            this.button5.Text = "Past Payments";
            this.button5.Click += new System.EventHandler(this.button5_Click);

            // Notifications
            this.button2.Location = new System.Drawing.Point(319, 313);
            this.button2.Size = new System.Drawing.Size(142, 52);
            this.button2.Text = "Notifications";
            this.button2.Click += new System.EventHandler(this.button2_Click);

            // Override
            this.buttonOverride.Location = new System.Drawing.Point(106, 313);
            this.buttonOverride.Size = new System.Drawing.Size(142, 52);
            this.buttonOverride.Text = "Request Override";
            this.buttonOverride.Click += new System.EventHandler(this.buttonOverride_Click);

            // ✅ Restaurant Info (FIXED)
            this.buttonFaq.Location = new System.Drawing.Point(540, 313);
            this.buttonFaq.Size = new System.Drawing.Size(133, 52);
            this.buttonFaq.Text = "Restaurant Info";
            this.buttonFaq.Click += new System.EventHandler(this.buttonFaq_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonFaq);
            this.Controls.Add(this.buttonOverride);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "WaiterScreen";
            this.Text = "WaiterScreen";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonOverride;
        private System.Windows.Forms.Button buttonFaq;
    }
}