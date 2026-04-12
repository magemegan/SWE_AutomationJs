namespace SWE_AutomationJs_UI_Design
{
    partial class EmployeeProfile
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(256, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 39);
            this.label1.TabIndex = 13;
            this.label1.Text = "Employee Profile";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(165, 246);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(262, 31);
            this.label6.TabIndex = 18;
            this.label6.Text = "Employement Type: ";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(165, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 31);
            this.label5.TabIndex = 17;
            this.label5.Text = "Email:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(165, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 31);
            this.label4.TabIndex = 16;
            this.label4.Text = "Phone: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(165, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 31);
            this.label7.TabIndex = 15;
            this.label7.Text = "Role: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(165, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 31);
            this.label8.TabIndex = 14;
            this.label8.Text = "Name: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(165, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 31);
            this.label2.TabIndex = 20;
            this.label2.Text = "Hire Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(165, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 31);
            this.label3.TabIndex = 19;
            this.label3.Text = "Status: ";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(244, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 49);
            this.button2.TabIndex = 22;
            this.button2.Text = "View Schedule";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(491, 376);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 49);
            this.button3.TabIndex = 23;
            this.button3.Text = "Edit Employee";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // EmployeeProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Name = "EmployeeProfile";
            this.Text = "EmployeeProfile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}