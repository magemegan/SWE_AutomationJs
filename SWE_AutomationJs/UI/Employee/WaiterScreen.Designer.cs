namespace SWE_AutomationJs_UI_Design
{
    partial class WaiterScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(177, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome Waiter/Waitress";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Log Out";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(106, 244);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 53);
            this.button3.TabIndex = 3;
            this.button3.Text = "Assign Tables";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(540, 244);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(133, 53);
            this.button7.TabIndex = 7;
            this.button7.Text = "Access Schedule";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(319, 244);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(142, 53);
            this.button5.TabIndex = 5;
            this.button5.Text = "Past Payments";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(319, 313);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 52);
            this.button2.TabIndex = 8;
            this.button2.Text = "Notifications";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonOverride
            // 
            this.buttonOverride.Location = new System.Drawing.Point(106, 313);
            this.buttonOverride.Name = "buttonOverride";
            this.buttonOverride.Size = new System.Drawing.Size(142, 52);
            this.buttonOverride.TabIndex = 9;
            this.buttonOverride.Text = "Request Override";
            this.buttonOverride.UseVisualStyleBackColor = true;
            this.buttonOverride.Click += new System.EventHandler(this.buttonOverride_Click);
            // 
            // buttonFaq
            // 
            this.buttonFaq.Location = new System.Drawing.Point(540, 313);
            this.buttonFaq.Name = "buttonFaq";
            this.buttonFaq.Size = new System.Drawing.Size(133, 52);
            this.buttonFaq.TabIndex = 10;
            this.buttonFaq.Text = "FAQ";
            this.buttonFaq.UseVisualStyleBackColor = true;
            this.buttonFaq.Click += new System.EventHandler(this.buttonFaq_Click);
            // 
            // WaiterScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Text = "WaitierScreen";
            this.Load += new System.EventHandler(this.WaitierScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
