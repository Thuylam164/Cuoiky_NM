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
    public partial class Them_Thethuvien : Form
    {
        public Them_Thethuvien()
        {
            InitializeComponent();
        }

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox
                textBox_Sothethuvien.Clear();
                textBox_Ghichu.Clear();            
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

        private void button_Them_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Sothethuvien.Text != "" &&
                    textBox_Ghichu.Text != "")
                {
                    // Kiểm tra xem "MaSach" đã tồn tại trong cơ sở dữ liệu hay chưa
                    string Sothethuvien = textBox_Sothethuvien.Text;

                    if (IsSothethuvienExist(Sothethuvien))
                    {
                        MessageBox.Show("Mã số sách " + Sothethuvien + " đã tồn tại! Vui lòng chọn mã số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Không thêm vào cơ sở dữ liệu nếu bị trùng
                    }

                    string Ghichu = textBox_Ghichu.Text;  
                    string Ngaybatdau = dateTimePicker_Ngaybatdau.Value.ToString("yyyy-MM-dd");
                    string Ngayhethan = dateTimePicker_Ngayhethan.Value.ToString("yyyy-MM-dd");
                    
                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                        {
                            con.Open();
                            string query = "INSERT INTO Thethuvien (Sothethuvien, Ngaybatdau, Ngayhethan,Ghichu) VALUES (@Sothethuvien, @Ngaybatdau, @Ngayhethan, @Ghichu)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                MemoryStream memstr = new MemoryStream();
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@Sothethuvien", textBox_Sothethuvien.Text);
                                cmd.Parameters.AddWithValue("@Ghichu", textBox_Ghichu.Text);
                                cmd.Parameters.AddWithValue("@Ngaybatdau", Ngaybatdau);
                                cmd.Parameters.AddWithValue("@Ngayhethan", Ngayhethan);
                                           
                                cmd.ExecuteNonQuery();

                                SqlCommand cmd2 = new SqlCommand("Select * from Thethuvien", con);
                                DataTable dt = new DataTable();
                                dt.Load(cmd2.ExecuteReader());
                                con.Close();
                            }
                        }

                        MessageBox.Show("Thông tin về thẻ thư viện đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_Sothethuvien.Clear();
                    textBox_Ghichu.Clear();

                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox_Sothethuvien_TextChanged(object sender, EventArgs e)
        {
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();

            if (IsSothethuvienExist(Sothethuvien))
            {
                MessageBox.Show("Số thẻ thư viện " + Sothethuvien + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private bool IsSothethuvienExist(string Sothethuvien)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Thethuvien WHERE Sothethuvien = @Sothethuvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

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
    }
}
