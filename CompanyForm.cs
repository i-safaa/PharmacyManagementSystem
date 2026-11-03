using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class CompanyForm : Form
    {
        public CompanyForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Pharmecenter_db.mdf;Integrated Security=True;Connect Timeout=30");

        public void populate()
        {
            string query = "select * from Company";

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

        private void btnADD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompanyID.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyPhone.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyAdress.Text))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "INSERT INTO Company (Com_id,Com_name,Com_phone,Com_adress) VALUES (@Com_id,@Com_name,@Com_phone,@Com_adress)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Com_id", txtCompanyID.Text);
                cmd.Parameters.AddWithValue("@Com_name", txtCompanyName.Text);
                cmd.Parameters.AddWithValue("@Com_phone", txtCompanyPhone.Text);
                cmd.Parameters.AddWithValue("@Com_adress", txtCompanyAdress.Text);

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
            ApplyColumnWidths(); // تطبيق القياسات بعد التحديث
        }

        private void CompanyForm_Load(object sender, EventArgs e)
        {
            populate();
            ApplyColumnWidths(); // تطبيق القياسات عند التحميل
        }

        // الدالة المعدلة لتثبيت العرض حسب الترتيب الفعلي للأعمدة
        private void ApplyColumnWidths()
        {
            if (guna2DataGridView1.Columns.Count >= 4)
            {
                // إيقاف التمدد التلقائي
                guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // تثبيت عرض الأعمدة بالتسلسل (المجموع = 100 + 236 + 200 + 200 = 736)
                guna2DataGridView1.Columns[0].Width = 100; // Com_id
                guna2DataGridView1.Columns[1].Width = 236; // Com_name (أو الاسم)
                guna2DataGridView1.Columns[2].Width = 200; // Com_phone
                guna2DataGridView1.Columns[3].Width = 200; // Com_adress
            }
        }

        private void btnUPDATE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompanyID.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyPhone.Text) ||
               string.IsNullOrWhiteSpace(txtCompanyAdress.Text))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "UPDATE Company SET Com_id = @Com_id , Com_name = @Com_name , Com_phone = @Com_phone , Com_adress = @Com_adress WHERE Com_id = @Com_id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Com_id", txtCompanyID.Text);
                cmd.Parameters.AddWithValue("@Com_name", txtCompanyName.Text);
                cmd.Parameters.AddWithValue("@Com_phone", txtCompanyPhone.Text);
                cmd.Parameters.AddWithValue("@Com_adress", txtCompanyAdress.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم تحديث قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("رقم الشركة غير موجود في قاعدة البيانات!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            ApplyColumnWidths(); // تطبيق القياسات بعد التحديث
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                txtCompanyID.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
                txtCompanyName.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
                txtCompanyPhone.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
                txtCompanyAdress.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
            }
        }

        private void btnDELETE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompanyID.Text))
            {
                MessageBox.Show("ادخل رقم الشركة ", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "DELETE FROM Company WHERE Com_id = @Com_id";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Com_id", txtCompanyID.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم اجراء الحذف على قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("رقم الشركة غير موجود ليتم حذفه!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            ApplyColumnWidths(); // تطبيق القياسات بعد الحذف
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}