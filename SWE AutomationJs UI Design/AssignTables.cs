using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWE_AutomationJs_UI_Design
{
    public partial class AssignTables : Form
    {
        public AssignTables()
        {
            InitializeComponent();
        }

        enum tableStatus
        {
            Available,
            Occupied,
            Reserved,
            NeedsCleaning
        }

        //tableStatus currentStatus = tableStatus.Available;
        private void TableStatus_Load(object sender, EventArgs e)
        {
            button3.BackColor = Color.LightGreen;
            button4.BackColor = Color.LightGreen;
            button5.BackColor = Color.LightGreen;
            button6.BackColor = Color.LightGreen;
            button7.BackColor = Color.LightGreen;
            button8.BackColor = Color.LightGreen;
            button9.BackColor = Color.LightGreen;
            button10.BackColor = Color.LightGreen;
            button11.BackColor = Color.LightGreen;
            button12.BackColor = Color.LightGreen;
            button13.BackColor = Color.LightGreen;
            button14.BackColor = Color.LightGreen;

            button3.Text = "Table 1";
            button4.Text = "Table 2";
            button5.Text = "Table 3";
            button6.Text = "Table 4";
            button7.Text = "Table 5";
            button8.Text = "Table 6";
            button9.Text = "Table 7";
            button10.Text = "Table 8";
            button11.Text = "Table 9";
            button12.Text = "Table 10";
            button13.Text = "Table 11";
            button14.Text = "Table 12";
        }

        private void UpdateButton(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.BackColor == Color.LightGreen)
            {
                button.BackColor = Color.Orange;
                //button.Text = "Occupied";
            }
            else
            {
                button.BackColor = Color.LightGreen;
                //button.Text = "Avaliable";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//go back
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AssignTables2 assignTables2 = new AssignTables2();
            assignTables2.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            // lead to order?
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
        }
    }
}
