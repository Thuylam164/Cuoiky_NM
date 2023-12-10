using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Xemthongtintramuon : Form
    {
        private string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";

   
        public Xemthongtintramuon()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox_Timkiemsothethuvien_TextChanged(object sender, EventArgs e)
        {
            string sothethuvien = textBox_Timkiemsothethuvien.Text.Trim();

            if (!string.IsNullOrEmpty(sothethuvien))
            {
                // Tạo kết nối SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Truy vấn dữ liệu từ bảng "Muontra" với các điều kiện
                    string query = "SELECT Mamuontra, Sothethuvien, Masach, Manhanvienthuchienmuonsach, Thoiluongmuon, Ngaymuon, Ngaydukientra, Ngaytra, Soluongsachduocmuon, Tinhtrangsachkhimuon FROM Muontra WHERE Sothethuvien = @Sothethuvien AND Ngaytra IS NULL";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Sothethuvien", sothethuvien);

                        // Tạo DataAdapter để lấy dữ liệu từ cơ sở dữ liệu
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        // Đổ dữ liệu vào DataTable
                        da.Fill(dt);

                        // Hiển thị dữ liệu trong dataGridView1
                        dataGridView1.DataSource = dt;

                        // Truy vấn dữ liệu từ bảng "Muontra" chỉ dựa trên giá trị "Sothethuvien"
                        string query2 = "SELECT Mamuontra, Sothethuvien, Masach, Manhanvienthuchienmuonsach,Manhanvienthuchientrasach, Thoiluongmuon, Ngaymuon, Ngaydukientra, Ngaytra, Soluongsachduocmuon, Tinhtrangsachkhimuon, Tinhtrangsachsaukhimuon FROM Muontra WHERE Sothethuvien = @Sothethuvien AND Ngaytra IS NOT NULL";
                        using (SqlCommand cmd2 = new SqlCommand(query2, connection))
                        {
                            cmd2.Parameters.AddWithValue("@Sothethuvien", sothethuvien);

                            // Tạo DataAdapter để lấy dữ liệu từ cơ sở dữ liệu
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                            DataTable dt2 = new DataTable();

                            // Đổ dữ liệu vào DataTable
                            da2.Fill(dt2);

                            // Hiển thị dữ liệu trong dataGridView2
                            dataGridView2.DataSource = dt2;
                        }
                    }
                }
            }
            else
            {
                // Nếu textBox_Timkiemsothethuvien trống, có thể xử lý hoặc xóa dữ liệu hiển thị
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Bạn có muốn quay về trang chủ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                Trangchu trc = new Trangchu();
                this.Hide();
                trc.Show();
            }
            else { }
        }
    }

}
  