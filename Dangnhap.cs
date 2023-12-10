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
using System.Security.Cryptography;
using BCrypt;

namespace NMCNPM_CuoiKy
{
    public partial class Dangnhap : Form
    {
        public static string UserRole; // Biến tĩnh để lưu vai trò của người dùng

        public Dangnhap()
        {
            InitializeComponent();
        
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Quenmatkhau qmk = new Quenmatkhau();
            this.Hide();
            qmk.Show();
        }

        private void checkBox_Hienmatkhau_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Hienmatkhau.Checked == true)
            {
                txt_Matkhau.UseSystemPasswordChar = false;
            }
            else
            {
                txt_Matkhau.UseSystemPasswordChar = true;
            }
        }

        private void button_Dangnhap_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Nhanvien where Taikhoan = @Taikhoan";
            cmd.Parameters.AddWithValue("@Taikhoan", txt_Taikhoan.Text);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            con.Close();

            if (ds.Tables[0].Rows.Count > 0)
            {
                string hashedStoredPassword = ds.Tables[0].Rows[0]["Matkhau"].ToString();
                string enteredPassword = txt_Matkhau.Text;

                // So sánh mật khẩu đã mã hóa trong cơ sở dữ liệu với mật khẩu đã nhập
                if (BCrypt.Net.BCrypt.Verify(enteredPassword, hashedStoredPassword))
                {
                    // Đăng nhập thành công
                    UserRole = ds.Tables[0].Rows[0]["Phanquyen"].ToString();
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Trangchu tc = new Trangchu();
                    tc.Show();
                }
                else
                {
                    MessageBox.Show("Mật khẩu không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tài khoản không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt_Taikhoan_MouseClick(object sender, MouseEventArgs e)
        {
            if (txt_Taikhoan.Text == "Tài khoản")
            {
                txt_Taikhoan.Clear();
            }
        }

        private void txt_Matkhau_MouseClick(object sender, MouseEventArgs e)
        {
            if (txt_Matkhau.Text == "Mật khẩu")
            {
                txt_Matkhau.Clear();
            }
        }
    }
}
