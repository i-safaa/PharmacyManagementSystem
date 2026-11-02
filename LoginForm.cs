using System;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace PharmacyManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Pharmecenter_db.mdf;Integrated Security=True;Connect Timeout=30");

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم المستخدم وكلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            con.Open();


            string query = "SELECT COUNT(*) FROM Employees WHERE Emp_name = @Emp_name AND Emp_Gender = @Emp_Gender";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Emp_name", txtUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@Emp_Gender", txtPassword.Text.Trim());

            // تنفيذ الاستعلام وجلب النتيجة
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            if (count > 0)
            {
                MessageBox.Show("تم تسجيل الدخول بنجاح!", "مرحباً", MessageBoxButtons.OK, MessageBoxIcon.Information);

                HomeForm frm = new HomeForm();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة!", "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
