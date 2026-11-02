using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class EmployeesForm : Form
    {
        public EmployeesForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Pharmecenter_db.mdf;Integrated Security=True;Connect Timeout=30");

        public void populate()
        {
            string query = "select * from Employees";

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.ReadOnly = true;
                        guna2DataGridView1.DataSource = dt;
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeAge.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeSalary.Text) ||
               string.IsNullOrWhiteSpace(guna2ComboBox1.Text) ||
               string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "INSERT INTO Employees (Emp_id, Emp_name, Emp_age, Emp_salary, Emp_gender, Emp_phonenumber) VALUES (@Emp_id, @Emp_Name, @Emp_age, @Emp_salary, @Emp_gender, @Emp_phonenumber)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Emp_id", txtEmployeeID.Text);
                cmd.Parameters.AddWithValue("@Emp_Name", txtEmployeeName.Text);
                cmd.Parameters.AddWithValue("@Emp_age", txtEmployeeAge.Text);
                cmd.Parameters.AddWithValue("@Emp_salary", txtEmployeeSalary.Text);
                cmd.Parameters.AddWithValue("@Emp_gender", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@Emp_phonenumber", txtPhoneNumber.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("تمت الاضافة الى قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الاضافة " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            populate();
        }

        private void EmployeesForm_Load(object sender, EventArgs e)
        {
            populate();
            // 1. إلغاء التمدد التلقائي للأعمدة لفتح المجال للتحكم اليدوي
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // 2. تحديد عرض محدد لكل عمود (العرض بالبكسل)
            guna2DataGridView1.Columns[0].Width = 20;   // عمود المعرف (Emp_id)
            guna2DataGridView1.Columns[1].Width = 110;  // عمود الاسم (Emp_name)
            guna2DataGridView1.Columns[2].Width = 80;   // عمود العمر (Emp_age)
            guna2DataGridView1.Columns[3].Width = 80;  // عمود الراتب (Emp_salary)
            guna2DataGridView1.Columns[4].Width = 80;   // عمود الجنس (Emp_Gender)
            guna2DataGridView1.Columns[5].Width = 115;  // عمود الهاتف (Emp_phonenumber)
            guna2DataGridView1.BackgroundColor = Color.Gray;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeAge.Text) ||
               string.IsNullOrWhiteSpace(txtEmployeeSalary.Text) ||
               string.IsNullOrWhiteSpace(guna2ComboBox1.Text) ||
               string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "UPDATE Employees SET Emp_name = @Emp_name, Emp_age = @Emp_age, Emp_salary = @Emp_salary, Emp_gender = @Emp_gender, Emp_phonenumber = @Emp_phonenumber WHERE Emp_id = @Emp_id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Emp_id", txtEmployeeID.Text);
                cmd.Parameters.AddWithValue("@Emp_name", txtEmployeeName.Text);
                cmd.Parameters.AddWithValue("@Emp_age", txtEmployeeAge.Text);
                cmd.Parameters.AddWithValue("@Emp_salary", txtEmployeeSalary.Text);
                cmd.Parameters.AddWithValue("@Emp_gender", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@Emp_phonenumber", txtPhoneNumber.Text);

                // التحقق مما إذا تم العثور على الموظف وتحديثه فعلياً
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم تحديث قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("رقم الموظف غير موجود في قاعدة البيانات!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء التحديث " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            populate();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                txtEmployeeID.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
                txtEmployeeName.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
                txtEmployeeAge.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
                txtEmployeeSalary.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
                guna2ComboBox1.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
                txtPhoneNumber.Text = row.Cells[5].Value?.ToString() ?? string.Empty;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
            {
                MessageBox.Show("ادخل رقم الموظف ", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "DELETE FROM Employees WHERE Emp_id = @Emp_id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Emp_id", txtEmployeeID.Text);

                // التحقق مما إذا تم العثور على الموظف وحذفه فعلياً
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم اجراء الحذف على قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("رقم الموظف غير موجود ليتم حذفه!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الحذف " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }

            populate();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}