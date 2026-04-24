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
    public partial class AssignTables2 : Form
    {
        public AssignTables2()
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
        private void TableStatus_Load2(object sender, EventArgs e)
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

            button3.Text = "Table 13";
            button4.Text = "Table 14";
            button5.Text = "Table 15";
            button6.Text = "Table 16";
            button7.Text = "Table 17";
            button8.Text = "Table 18";
            button9.Text = "Table 19";
            button10.Text = "Table 20";
            button11.Text = "Table 21";
            button12.Text = "Table 22";
            button13.Text = "Table 23";
            button14.Text = "Table 24";
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
            AssignTables assignTables = new AssignTables();
            assignTables.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AssignTables3 assignTables3 = new AssignTables3();
            assignTables3.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(13);
            serverName.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(14);
            serverName.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(15);
            serverName.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(16);
            serverName.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(17);
            serverName.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(18);
            serverName.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(19);
            serverName.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(20);
            serverName.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(21);
            serverName.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(22);
            serverName.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(23);
            serverName.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UpdateButton(sender, e);
            Orders serverName = new Orders(24);
            serverName.Show();
            this.Hide();
        }
    }
}
