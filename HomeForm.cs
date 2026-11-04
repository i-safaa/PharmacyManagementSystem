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

        MedicineForm medform = new MedicineForm();
        CompanyForm compform = new CompanyForm();
        EmployeesForm employeesform = new EmployeesForm();
        LoginForm loginform = new LoginForm();
        BillingForm billingform = new BillingForm();

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            medform.Show();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {

            this.Hide();
            compform.Show();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            employeesform.Show();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            billingform.Show();
        }

        private void guna2ImageButton5_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            loginform.Show();
        }
    }
}
