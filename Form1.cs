using System;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class Form1 : Form
    {
        int StartPoint = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartPoint++;

            bunifuCircleProgressbar1.Value = StartPoint;
            if (bunifuCircleProgressbar1.Value == 100)
            {
                bunifuCircleProgressbar1.Value = 0;
                timer1.Stop();

                LoginForm MyLogin = new LoginForm();
                this.Hide();
                MyLogin.Show();
            }
        }
    }
}
