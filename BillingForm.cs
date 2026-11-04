using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class BillingForm : Form
    {
        // ==========================================
        // 1. المتغيرات والتهيئة الأساسية
        // ==========================================
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Pharmecenter_db.mdf;Integrated Security=True;Connect Timeout=30");

        private PrintDocument printDocument1 = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();

        int x = 0;                  // سعر البيع المؤقت للدواء المختار
        int GrdTotal = 0;           // المجموع الكلي للفاتورة
        int currentStock = 0;       // المخزون الحالي للدواء
        int serialCounter = 1;      // عداد تسلسلي لصفوف الجدول يبدأ من 1
        int invoiceNo = 1;          // رقم الفاتورة المتسلسل

        public BillingForm()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            populatecombobox();

        }

        // ==========================================
        // 2. دوال قاعدة البيانات والبيانات
        // ==========================================

        // تحميل أسماء الأدوية في الـ ComboBox
        public void populatecombobox()
        {
            string sql = "SELECT MedName FROM Medicine_tbl";

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
                            comboBox1.DataSource = dt;
                            comboBox1.DisplayMember = "MedName";
                            comboBox1.ValueMember = "MedName";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("حدث خطأ أثناء تحميل قائمة الأدوية: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // جلب الكمية المتاحة وسعر البيع عند اختيار دواء
        public void fetchQty()
        {
            if (comboBox1.SelectedValue == null) return;

            string mysql = "SELECT MedQty, Sprice FROM Medicine_tbl WHERE MedName = @MedName";

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(mysql, connection))
                {
                    cmd.Parameters.AddWithValue("@MedName", comboBox1.SelectedValue.ToString());

                    try
                    {
                        connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                currentStock = Convert.ToInt32(rdr["MedQty"]);

                                int alreadyInGridQty = 0;
                                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                                {
                                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == comboBox1.SelectedValue.ToString())
                                    {
                                        alreadyInGridQty += Convert.ToInt32(row.Cells[2].Value);
                                    }
                                }

                                int netStock = currentStock - alreadyInGridQty;
                                lblStock.Text = "Available Stock is " + netStock;
                                lblStock.Visible = true;
                                x = Convert.ToInt32(rdr["Sprice"]);
                            }
                            else
                            {
                                lblStock.Text = "Available Stock is 0";
                                lblStock.Visible = true;
                                currentStock = 0;
                                x = 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ أثناء جلب بيانات الدواء: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // خصم الكميات المباعة نهائياً من قاعدة البيانات
        public void updateMedicine()
        {
            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells[1].Value != null)
                    {
                        string medName = row.Cells[1].Value.ToString();
                        int soldQty = Convert.ToInt32(row.Cells[2].Value);

                        string query = "UPDATE Medicine_tbl SET MedQty = MedQty - @Qty WHERE MedName = @MedName";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Qty", soldQty);
                            cmd.Parameters.AddWithValue("@MedName", medName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // ==========================================
        // 3. الأحداث وأزرار الواجهة
        // ==========================================

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchQty();
        }

        // زر إضافة الدواء إلى الفاتورة المؤقتة
        private void btnAddToBill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int enteredQty))
            {
                MessageBox.Show("الرجاء إدخال كمية صحيحة!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("الرجاء اختيار دواء أولاً!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int alreadyInGridQty = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == comboBox1.SelectedValue.ToString())
                {
                    alreadyInGridQty += Convert.ToInt32(row.Cells[2].Value);
                }
            }

            int remainingStock = currentStock - alreadyInGridQty;

            if (enteredQty > remainingStock)
            {
                MessageBox.Show("عذراً، الكمية المطلوبة أكبر من المخزون المتبقي! (المتبقي الفعلي: " + remainingStock + ")", "تنبيه المخزون", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int unitPrice = x;
            int total = enteredQty * unitPrice;

            int rowIndex = guna2DataGridView1.Rows.Add();
            DataGridViewRow newRow = guna2DataGridView1.Rows[rowIndex];

            newRow.Cells[0].Value = serialCounter++;
            newRow.Cells[1].Value = comboBox1.SelectedValue.ToString();
            newRow.Cells[2].Value = enteredQty;
            newRow.Cells[3].Value = unitPrice;
            newRow.Cells[4].Value = total;

            GrdTotal += total;
            txtTotalAmount.Text = "IQD " + GrdTotal;

            int newRemainingStock = remainingStock - enteredQty;
            lblStock.Text = "Available Stock is " + newRemainingStock;
            txtQuantity.Text = "";
        }

        // زر الطباعة والحفظ الموحد (خصم المخزون + طباعة الفاتورة + تصفير الشاشة)
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.Rows.Count <= 1 && guna2DataGridView1.Rows[0].Cells[1].Value == null)
            {
                MessageBox.Show("الفاتورة فارغة، يجب إضافة أدوية أولاً!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. خصم الكميات من قاعدة البيانات
                updateMedicine();

                // 2. معاينة وطباعة الفاتورة
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();

                MessageBox.Show("تم حفظ الفاتورة وطباعتها وتحديث المخزون بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. إعادة تعيين الشاشة لفاتورة جديدة
                guna2DataGridView1.Rows.Clear();
                GrdTotal = 0;
                txtTotalAmount.Text = "IQD 0";
                serialCounter = 1;
                invoiceNo++;
                lblStock.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء عملية الحفظ أو الطباعة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 4. تصميم ورقة الطباعة
        // ==========================================
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("PHARMACY MANAGEMENT SYSTEM", new Font("Century Gothic", 18, FontStyle.Bold), Brushes.DarkBlue, new Point(130, 30));
            e.Graphics.DrawString("---------------------------------------------------------------------------------", new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(50, 65));

            e.Graphics.DrawString("Invoice No: #" + invoiceNo, new Font("Century Gothic", 11, FontStyle.Bold), Brushes.Black, new Point(60, 90));
            e.Graphics.DrawString("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(450, 90));

            e.Graphics.DrawString("---------------------------------------------------------------------------------", new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(50, 115));

            e.Graphics.DrawString("ID", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(60, 150));
            e.Graphics.DrawString("Medicine Name", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(150, 150));
            e.Graphics.DrawString("Qty", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(380, 150));
            e.Graphics.DrawString("Price", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(480, 150));
            e.Graphics.DrawString("Total", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(600, 150));

            e.Graphics.DrawString("---------------------------------------------------------------------------------", new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(50, 170));

            int yPos = 210;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    string id = row.Cells[0].Value.ToString();
                    string name = row.Cells[1].Value.ToString();
                    string qty = row.Cells[2].Value.ToString();
                    string price = row.Cells[3].Value.ToString();
                    string total = row.Cells[4].Value.ToString();

                    e.Graphics.DrawString(id, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(60, yPos));
                    e.Graphics.DrawString(name, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(150, yPos));
                    e.Graphics.DrawString(qty, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(380, yPos));
                    e.Graphics.DrawString(price, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(480, yPos));
                    e.Graphics.DrawString(total, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(600, yPos));

                    yPos += 35;
                }
            }

            e.Graphics.DrawString("---------------------------------------------------------------------------------", new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(50, yPos + 10));
            e.Graphics.DrawString("Grand Total: IQD " + GrdTotal, new Font("Century Gothic", 14, FontStyle.Bold), Brushes.Crimson, new Point(420, yPos + 50));
            e.Graphics.DrawString("********* Thank You For Your Visit *********", new Font("Century Gothic", 12, FontStyle.Italic), Brushes.DarkGreen, new Point(200, yPos + 110));
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }


    }
}