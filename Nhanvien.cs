using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt;

namespace NMCNPM_CuoiKy
{
    public partial class Nhanvien : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Nhanvien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        int IDNhanvien;
     
  

        private void pictureBox1_Click(object sender, EventArgs e)
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
        private bool IsMasonhanvienExist(string masonhanvien)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Nhanvien WHERE Masonhanvien = @Masonhanvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Masonhanvien", masonhanvien);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void button_Them_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về nhân viên sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Masonhanvien.Text != "" &&
                    textBox_Hovaten.Text != "" &&
                    textBox_Noisinh.Text != "" &&
                    comboBox_Gioitinh.Text != "" &&
                    textBox_Sodienthoai.Text != "" &&
                    textBox_Email.Text != "" &&
                    textBox_Taikhoan.Text != "" &&
                    textBox_Matkhau.Text != "" &&
                    comboBox_Phanquyen.Text != "")
                {
                    string Masonhanvien = textBox_Masonhanvien.Text;

                    // Kiểm tra xem "Masonhanvien" đã tồn tại trong cơ sở dữ liệu hay chưa
                    if (IsMasonhanvienExist(Masonhanvien))
                    {
                        MessageBox.Show("Mã số nhân viên " + Masonhanvien + " đã tồn tại! Vui lòng chọn mã số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Không thêm vào cơ sở dữ liệu nếu bị trùng
                    }

                    string Taikhoan = textBox_Taikhoan.Text;

                    //Kiểm tra xem "Taikhoan" đã tồn tại trong cơ sở dữ liệu hay chưa
                    if (IsTaikhoanExist(Taikhoan))
                    {
                        MessageBox.Show("Tên tài khoản " + Taikhoan + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    string Hovaten = textBox_Hovaten.Text;
                    string Noisinh = textBox_Noisinh.Text;
                    string Gioitinh = comboBox_Gioitinh.Text;
                    string Email = textBox_Email.Text;
                    string Matkhau = textBox_Matkhau.Text;
                    string Phanquyen = comboBox_Phanquyen.Text;
                    string Ngaysinh = dateTimePicker_Ngaysinh.Value.ToString("yyyy-MM-dd");
                    Int64 Sodienthoai = Int64.Parse(textBox_Sodienthoai.Text);

                    // Kiểm tra điều kiện của mật khẩu
                    string errorMessage = CheckPasswordConditions(Matkhau);

                    // Nếu có lỗi, hiển thị thông báo và không thêm vào cơ sở dữ liệu
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Không thêm vào cơ sở dữ liệu nếu có lỗi
                    }

                    // Mã hóa Mật khẩu trước khi thêm vào cơ sở dữ liệu
                    string hashedMatkhau = BCrypt.Net.BCrypt.HashPassword(Matkhau);

                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "INSERT INTO Nhanvien (Masonhanvien, Hovaten, Ngaysinh, Noisinh, Gioitinh, Sodienthoai, Email,Taikhoan,Matkhau, Phanquyen) VALUES (@Masonhanvien, @Hovaten, @Ngaysinh, @Noisinh, @Gioitinh, @Sodienthoai, @Email,@Taikhoan,@Matkhau, @Phanquyen)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Masonhanvien", Masonhanvien);
                            cmd.Parameters.AddWithValue("@Hovaten", Hovaten);
                            cmd.Parameters.AddWithValue("@Ngaysinh", Ngaysinh);
                            cmd.Parameters.AddWithValue("@Noisinh", Noisinh);
                            cmd.Parameters.AddWithValue("@Gioitinh", Gioitinh);
                            cmd.Parameters.AddWithValue("@Sodienthoai", Sodienthoai);
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Taikhoan", Taikhoan);
                            cmd.Parameters.AddWithValue("@Matkhau", hashedMatkhau);
                            cmd.Parameters.AddWithValue("@Phanquyen", Phanquyen);

                            cmd.ExecuteNonQuery();

                            SqlCommand cmd2 = new SqlCommand("Select * from Nhanvien", con);
                            DataTable dt = new DataTable();
                            dt.Load(cmd2.ExecuteReader());
                            con.Close();
                        }
                    }

                    MessageBox.Show("Thông tin về nhân viên đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_Masonhanvien.Text = "";
                    textBox_Hovaten.Text = "";
                    textBox_Noisinh.Text = "";
                    textBox_Sodienthoai.Text = "";
                    textBox_Email.Text = "";
                    textBox_Taikhoan.Text = "";
                    textBox_Matkhau.Text = "";
                    comboBox_Gioitinh.Text = "";
                    comboBox_Phanquyen.Text = "";
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // Kiểm tra sự tồn tại của Masonhanvien trong cơ sở dữ liệu
        
        private void Nhanvien_Load(object sender, EventArgs e)
        {

        }

        private void textBox_Masonhanvien_TextChanged(object sender, EventArgs e)
        {
            string Masonhanvien = textBox_Masonhanvien.Text.Trim();

            if (IsMasonhanvienExist(Masonhanvien))
            {
                MessageBox.Show("Mã số nhân viên " + Masonhanvien + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox_Matkhau.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_Matkhau.UseSystemPasswordChar = true;
            }
        }

        private void checkBox_Hientaikhoan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dieukienmatkhau2 dkmk = new Dieukienmatkhau2();
            this.Hide();
            dkmk.Show();
        }

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox
                textBox_Matkhau.Clear();
                textBox_Masonhanvien.Text = "";
                textBox_Hovaten.Text = "";
                textBox_Noisinh.Text = "";
                textBox_Sodienthoai.Text = "";
                textBox_Email.Text = "";
                textBox_Taikhoan.Text = "";
                comboBox_Gioitinh.Text = "";
                comboBox_Phanquyen.Text = "";
            }
        }

        private void button_Quaylaitrangchu_Click(object sender, EventArgs e)
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

        private void textBox_Matkhau_TextChanged(object sender, EventArgs e)
        {
       
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

        private void textBox_Taikhoan_TextChanged(object sender, EventArgs e)
        {
            string Taikhoan = textBox_Taikhoan.Text.Trim();

            if (IsTaikhoanExist(Taikhoan))
            {
                MessageBox.Show("Tên tài khoản " + Taikhoan + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool IsTaikhoanExist(string Taikhoan)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Nhanvien WHERE Taikhoan = @Taikhoan";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Taikhoan", Taikhoan);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

    }
}
