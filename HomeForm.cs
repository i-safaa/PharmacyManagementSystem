using System;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            MedicineForm medform = new MedicineForm();
            this.Hide();
            medform.Show();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            CompanyForm compform = new CompanyForm();
            this.Hide();
            compform.Show();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            EmployeesForm employeesform = new EmployeesForm();
            this.Hide();
            employeesform.Show();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            BillingForm billingform = new BillingForm();
            this.Hide();
            billingform.Show();
        }

        private void guna2ImageButton5_Click_1(object sender, EventArgs e)
        {
            LoginForm loginform = new LoginForm();
            this.Hide();
            loginform.Show();
        }
    }
}
