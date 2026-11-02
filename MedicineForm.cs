using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class MedicineForm : Form
    {
        public MedicineForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Pharmecenter_db.mdf;Integrated Security=True;Connect Timeout=30");

        public void populate()
        {
            string query = "select * from Medicine_tbl";

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                // تصحيح استخدام الاتصال المحلي هنا
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

        public void fillCompanyCombo()
        {
            string sql = "SELECT Com_Name FROM Company";

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // ملاحظة: تأكد أن اسم الـ ComboBox لديك هو نفسه (guna2ComboBox1) أو قم بتغييره بالاسم الموجود في التصميم
                            guna2ComboBox1.DataSource = dt;
                            guna2ComboBox1.DisplayMember = "Com_Name";
                            guna2ComboBox1.ValueMember = "Com_Name";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("حدث خطأ أثناء تحميل أسماء الشركات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void MedicineForm2_Load(object sender, EventArgs e)
        {
            populate();
            fillCompanyCombo();

            // 1. إلغاء التمدد التلقائي للأعمدة لفتح المجال للتحكم اليدوي
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // 2. تحديد عرض محدد لكل عمود (العرض بالبكسل)
            guna2DataGridView1.Columns[0].Width = 120;  // عمود اسم الدواء (MedName)
            guna2DataGridView1.Columns[1].Width = 80;   // عمود سعر الشراء (Bprice)
            guna2DataGridView1.Columns[2].Width = 80;   // عمود سعر البيع (Sprice)
            guna2DataGridView1.Columns[3].Width = 70;   // عمود الكمية (MedQty)
            guna2DataGridView1.Columns[4].Width = 100;  // عمود تاريخ الصلاحية (ExpDate)
            guna2DataGridView1.Columns[5].Width = 115;  // عمود الشركة (Company)
            guna2DataGridView1.BackgroundColor = Color.Gray;


        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox4.Text) ||
                string.IsNullOrWhiteSpace(guna2ComboBox1.Text))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "INSERT INTO Medicine_tbl (MedName, Bprice, Sprice, MedQty, ExpDate, Company) VALUES (@MedName, @Bprice, @Sprice, @MedQty, @ExpDate, @Company)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@MedName", bunifuMaterialTextbox1.Text);
                cmd.Parameters.AddWithValue("@Bprice", bunifuMaterialTextbox2.Text);
                cmd.Parameters.AddWithValue("@Sprice", bunifuMaterialTextbox3.Text);
                cmd.Parameters.AddWithValue("@MedQty", bunifuMaterialTextbox4.Text);
                cmd.Parameters.AddWithValue("@ExpDate", ExpireDate.Value.Date);
                cmd.Parameters.AddWithValue("@Company", guna2ComboBox1.Text);

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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox4.Text) ||
                string.IsNullOrWhiteSpace(guna2ComboBox1.Text))
            {
                MessageBox.Show("ادخل جميع الأعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "UPDATE Medicine_tbl SET Bprice = @Bprice, Sprice = @Sprice, MedQty = @MedQty, ExpDate = @ExpDate, Company = @Company WHERE MedName = @MedName";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Bprice", bunifuMaterialTextbox2.Text);
                cmd.Parameters.AddWithValue("@Sprice", bunifuMaterialTextbox3.Text);
                cmd.Parameters.AddWithValue("@MedQty", bunifuMaterialTextbox4.Text);
                cmd.Parameters.AddWithValue("@ExpDate", ExpireDate.Value.Date);
                cmd.Parameters.AddWithValue("@Company", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@MedName", bunifuMaterialTextbox1.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم تحديث قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("اسم الدواء غير موجود للتحديث!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text))
            {
                MessageBox.Show("ادخل اسم الدواء", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string query = "DELETE FROM Medicine_tbl WHERE MedName = @MedName";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@MedName", bunifuMaterialTextbox1.Text);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("تم اجراء الحذف على قاعدة البيانات", "صح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("اسم الدواء غير موجود للحذف!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                bunifuMaterialTextbox1.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
                bunifuMaterialTextbox2.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
                bunifuMaterialTextbox3.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
                bunifuMaterialTextbox4.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
                ExpireDate.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
                guna2ComboBox1.Text = row.Cells[5].Value?.ToString() ?? string.Empty;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}