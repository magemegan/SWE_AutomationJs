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
    public partial class AssignTables3 : Form
    {
        public AssignTables3()
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
        private void TableStatus_Load3(object sender, EventArgs e)
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

            button3.Text = "Table 25";
            button4.Text = "Table 26";
            button5.Text = "Table 27";
            button6.Text = "Table 28";
            button7.Text = "Table 29";
            button8.Text = "Table 30";
            button9.Text = "Table 31";
            button10.Text = "Table 32";
            button11.Text = "Table 33";
            button12.Text = "Table 34";
            button13.Text = "Table 35";
            button14.Text = "Table 36";
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(25);
            serverName.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(26);
            serverName.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(27);
            serverName.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(28);
            serverName.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(29);
            serverName.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(30);
            serverName.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(31);
            serverName.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(32);
            serverName.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(33);
            serverName.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(34);
            serverName.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(35);
            serverName.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(36);
            serverName.Show();
            this.Hide();
        }
    }
}
