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
    public partial class TableStatus : Form
    {
        public TableStatus()
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

            button3.Text = "Avaliable";
            button4.Text = "Avaliable";
            button5.Text = "Avaliable";
            button6.Text = "Avaliable";
            button7.Text = "Avaliable";
            button8.Text = "Avaliable";
            button9.Text = "Avaliable";
            button10.Text = "Avaliable";
            button11.Text = "Avaliable";
            button12.Text = "Avaliable";
            button13.Text = "Avaliable";
            button14.Text = "Avaliable";
        }

        private void UpdateButton(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.BackColor == Color.LightGreen)
            {
                button.BackColor = Color.Gray;
                button.Text = "Reserved";
            }
            else if (button.BackColor == Color.Gray)
            {
                button.BackColor = Color.Orange;
                button.Text = "Occupied";
            }
            else if (button.BackColor == Color.Orange)
            {
                button.BackColor = Color.Red;
                button.Text = "Needs Cleaning";
            }
            else
            {
                button.BackColor = Color.LightGreen;
                button.Text = "Avaliable";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//go back
            WaiterScreen waiterScreen = new WaiterScreen();
            waiterScreen.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//next page
            TableStatus2 tableStatus2 = new TableStatus2();
            tableStatus2.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {//button for table 1
            UpdateButton(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {//button for table 2
            UpdateButton(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {//button for table 3
            UpdateButton(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {//button for table 4
            UpdateButton(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {//button for table 5
            UpdateButton(sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {//button for table 6
            UpdateButton(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {//button for table 7
            UpdateButton(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {//button for table 8
            UpdateButton(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {//button for table 9
            UpdateButton(sender, e);
        }

        private void button12_Click(object sender, EventArgs e)
        {//button for table 10
            UpdateButton(sender, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {//button for table 11  
            UpdateButton(sender, e);
        }

        private void button14_Click(object sender, EventArgs e)
        {//button for table 12
            UpdateButton(sender, e);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
