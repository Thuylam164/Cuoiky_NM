using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;

namespace NMCNPM_CuoiKy
{
    public partial class CapnhatMatKhau : Form
    {
        string Taikhoan = Quenmatkhau.to;
        public CapnhatMatKhau()
        {
            InitializeComponent();
        }

        private void button_Capnhat_Click(object sender, EventArgs e)
        {
            // Khởi tạo thông báo lỗi
            string errorMessage = "";

            // Kiểm tra nếu cả hai ô mật khẩu mới và ô Nhập lại mật khẩu không được để trống
            if (string.IsNullOrEmpty(textBox_Matkhaumoi.Text) || string.IsNullOrEmpty(textBox_NhaplaiMK.Text))
            {
                errorMessage += "Vui lòng điền đầy đủ thông tin!\n";
            }

            // Kiểm tra các điều kiện của mật khẩu và thêm thông báo lỗi nếu điều kiện không đúng
            errorMessage += CheckPasswordConditions(textBox_Matkhaumoi.Text);

            // Kiểm tra nếu mật khẩu mới và Nhập lại mật khẩu trùng nhau và tất cả điều kiện hợp lệ
            if (textBox_Matkhaumoi.Text == textBox_NhaplaiMK.Text && string.IsNullOrEmpty(errorMessage))
            {
                // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(textBox_Matkhaumoi.Text);

                // Thực hiện cập nhật mật khẩu
                SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Nhanvien] SET [Matkhau] = '" + hashedPassword + "' WHERE Taikhoan= '" + Taikhoan + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Cập nhật mật khẩu thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Dangnhap DN = new Dangnhap();
                this.Hide();
                DN.Show();
            }
            else
            {
                // Hiển thị thông báo lỗi
                MessageBox.Show(!string.IsNullOrEmpty(errorMessage) ? errorMessage : "Mật khẩu không trùng khớp.", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CheckPasswordConditions(string password)
        {
            string error = "";

            if (password.Length < 6)
            {
                error += "Tối thiểu 6 ký tự.\n";
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*()]"))
            {
                error += "Có ít nhất 1 ký tự đặc biệt.\n";
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                error += "Có ít nhất 1 ký tự viết thường.\n";
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                error += "Có ít nhất 1 số.\n";
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                error += "Có ít nhất 1 ký viết hoa.\n";
            }

            return error;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                // Hiển thị hộp thoại hỏi người dùng
                DialogResult result = MessageBox.Show("Bạn muốn quay trở lại trang đăng nhập?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kiểm tra kết quả từ người dùng
                if (result == DialogResult.Yes)
                {
                Dangnhap DN = new Dangnhap();
                this.Hide();
                DN.Show();
                }
                else if (result == DialogResult.No)
                {
                    // Người dùng chọn "Không", ở lại
                }
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox_Matkhaumoi.UseSystemPasswordChar = false;
                textBox_NhaplaiMK.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_Matkhaumoi.UseSystemPasswordChar = true;
                textBox_NhaplaiMK.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dieukienmatkhau  dkmk = new Dieukienmatkhau();
            this.Hide();
            dkmk.Show();
        }
    }
}
