using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Xemthongtinnguoidoctheosothethuviencs : Form
    {
        public Xemthongtinnguoidoctheosothethuviencs()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox_Timkiemtheosothethuvien_TextChanged(object sender, EventArgs e)
        {
            string sothethuvien = textBox_Timkiemtheosothethuvien.Text.Trim();

            if (!string.IsNullOrEmpty(sothethuvien))
            {
                // Tạo kết nối SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Truy vấn dữ liệu từ bảng "Nguoidoc" với điều kiện "Sothethuvien"
                    string query = "SELECT Hovaten, Masonguoidoc, Ngaysinh, Chucvu, Noisinh, Gioitinh, Lophoc, Hinhanh FROM Nguoidoc WHERE Sothethuvien = @Sothethuvien";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Sothethuvien", sothethuvien);

                        // Thực hiện truy vấn và đọc dữ liệu
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Hiển thị dữ liệu vào các TextBox
                                textBox_Hovaten.Text = reader["Hovaten"].ToString();
                                textBox_Masonguoidoc.Text = reader["Masonguoidoc"].ToString();
                                textBox_Ngaysinh.Text = reader["Ngaysinh"].ToString();
                                textBox_Chucvu.Text = reader["Chucvu"].ToString();
                                textBox_Noisinh.Text = reader["Noisinh"].ToString();
                                textBox_Gioitinh.Text = reader["Gioitinh"].ToString();
                                textBox_Lophoc.Text = reader["Lophoc"].ToString();

                                // Hiển thị hình ảnh từ dữ liệu byte[]
                                byte[] imageData = (byte[])reader["Hinhanh"];
                                DisplayImage(imageData);
                            }
                            else
                            {
                                // Nếu không có kết quả, có thể xử lý hoặc xóa dữ liệu hiển thị
                                ClearTextBoxes();
                               Hinhanh.Image = null;
                            }
                        }
                    }
                }
            }
            else
            {
                // Nếu textBox_Timkiemtheosothethuvien trống, có thể xử lý hoặc xóa dữ liệu hiển thị
                ClearTextBoxes();
                Hinhanh.Image = null;
            }
        }

        // Phương thức để xóa dữ liệu từ các TextBox
        private void ClearTextBoxes()
        {
            textBox_Hovaten.Clear();
            textBox_Masonguoidoc.Clear();
            textBox_Ngaysinh.Clear();
            textBox_Chucvu.Clear();
            textBox_Noisinh.Clear();
            textBox_Gioitinh.Clear();
            textBox_Lophoc.Clear();
        }
        // Phương thức để hiển thị hình ảnh từ dữ liệu byte[]
        private void DisplayImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Hinhanh.Image = Image.FromStream(ms);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox_Timkiemtheosothethuvien.Clear();
            ClearTextBoxes();
            Hinhanh.Image = null;
        }

        private void button3_Click(object sender, EventArgs e)
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
