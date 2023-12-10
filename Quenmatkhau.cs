using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NMCNPM_CuoiKy
{
    public partial class Quenmatkhau : Form
    {
        string randomCode;
        public static string to;
        public Quenmatkhau()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_Guima_Click(object sender, EventArgs e)
        {
            string from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            to = textBox_Email.Text;

            // Thực hiện kết nối đến cơ sở dữ liệu
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                // Thực hiện truy vấn kiểm tra email
                string checkEmailQuery = "SELECT COUNT(*) FROM Nhanvien WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(checkEmailQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Email", textBox_Email.Text);
                    int emailCount = (int)cmd.ExecuteScalar();

                    if (string.IsNullOrWhiteSpace(textBox_Email.Text))
                    {
                        MessageBox.Show("Hãy nhập email.", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!IsValidEmail(textBox_Email.Text))
                    {
                        MessageBox.Show("Email không hợp lệ.", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (emailCount == 0)
                    {
                        MessageBox.Show("Email này chưa được đăng ký tài khoản!", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Email hợp lệ và tồn tại trong cơ sở dữ liệu, tiếp tục gửi mã
                        from = "cnpm2023cnpm@gmail.com";
                        pass = "kaaonucygmatmzbe";
                        messageBody = $"Mã lấy lại mật khẩu của bạn là {randomCode}";
                        message.To.Add(to);
                        message.From = new MailAddress(from);
                        message.Body = messageBody;
                        message.Subject = "QUẢN LÝ THƯ VIỆN - ĐẶT LẠI MẬT KHẨU";
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential(from, pass);

                        try
                        {
                            smtp.Send(message);
                            MessageBox.Show("Gửi mã về email thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Chưa gửi được mã.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Hàm kiểm tra email có hợp lệ không
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void button_Xacnhanma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_nhaplai.Text))
            {
                MessageBox.Show("Mã xác nhận không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (randomCode == textBox_nhaplai.Text)
            {
                to = textBox_Email.Text;
                MessageBox.Show("Xác nhận mã thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CapnhatMatKhau capnhat = new CapnhatMatKhau();
                this.Hide();
                capnhat.Show();
            }
            else
            {
                MessageBox.Show("Nhập sai mã.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Dangnhap DN = new Dangnhap();
            this.Hide();
            DN.Show();
        }
    }
}
