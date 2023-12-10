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
using System.IO;

namespace NMCNPM_CuoiKy
{
    public partial class Them_Sach : Form
    {

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Them_Sach()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_Taihinhanh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Hinhanh files (*.jpg; *.jpeg)| *.jpg; *.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Hinhanh.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void button_Them_Click(object sender, EventArgs e)
        {
            if (Hinhanh.Image == null)
            {
                MessageBox.Show("Yêu cầu thêm hình ảnh", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm sách nếu không có hình ảnh
            }

            if (MessageBox.Show("Thông tin sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Masach.Text != "" &&
                    textBox_Tensach.Text != "" &&
                    textBox_Matacgia.Text != "" &&
                    textBox_Tacgia.Text != "" &&
                    textBox_Matheloai.Text != "" &&
                    textBox_Theloaisach.Text != "" &&
                    textBox_Manhaxuatban.Text != "" &&
                    textBox_Nhaxuatban.Text != "" &&
                    textBox_Namxuatban.Text != "" &&
                    textBox_Soluong.Text != "" &&
                    textBox_Gia1cuonsach.Text != "")
                {
                    // Kiểm tra xem "MaSach" đã tồn tại trong cơ sở dữ liệu hay chưa
                    string MaSach = textBox_Masach.Text;
                   

                    if (IsMasachExist(MaSach))
                    {
                        MessageBox.Show("Mã số sách " + MaSach + " đã tồn tại! Vui lòng chọn mã số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Không thêm vào cơ sở dữ liệu nếu bị trùng
                    }
                  
                    string TenSach = textBox_Tensach.Text;
                    string MaTacGia = textBox_Matacgia.Text;
                    string Matheloai = textBox_Matheloai.Text;
                    string MaNXB = textBox_Theloaisach.Text;
                    string Gia1cuonsach = textBox_Gia1cuonsach.Text;
                    Int64 Soluong = Int64.Parse(textBox_Soluong.Text);
                    Int64 Namxuatban = Int64.Parse(textBox_Namxuatban.Text);

                   
                    if (int.TryParse(textBox_Soluong.Text, out int Soluongsach))
                    {
                        using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                        {
                            con.Open();
                            string query = "INSERT INTO Sach (Masach, Tensach, MaTacgia,Matheloai, MaNXB, Namxuatban,Soluong, Gia1cuonsach, Hinhanh) VALUES (@Masach, @Tensach, @MaTacgia, @Matheloai, @MaNXB, @Namxuatban, @Soluong, @Gia1cuonsach, @Hinhanh)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                MemoryStream memstr = new MemoryStream();
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@Masach", textBox_Masach.Text);
                                cmd.Parameters.AddWithValue("@Tensach", textBox_Tensach.Text);
                                cmd.Parameters.AddWithValue("@MaTacgia", textBox_Matacgia.Text);
                                cmd.Parameters.AddWithValue("@Matheloai", textBox_Matheloai.Text);
                                cmd.Parameters.AddWithValue("@MaNXB", textBox_Manhaxuatban.Text);
                                cmd.Parameters.AddWithValue("@Namxuatban", textBox_Namxuatban.Text);
                                cmd.Parameters.AddWithValue("@Soluong", textBox_Soluong.Text);
                                cmd.Parameters.AddWithValue("@Gia1cuonsach", textBox_Gia1cuonsach.Text);

                                Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                                cmd.Parameters.AddWithValue("Hinhanh", memstr.ToArray());

                                cmd.ExecuteNonQuery();

                                SqlCommand cmd2 = new SqlCommand("Select * from Sach", con);
                                DataTable dt = new DataTable();
                                dt.Load(cmd2.ExecuteReader());
                                con.Close();
                            }
                        }

                        MessageBox.Show("Thông tin sách đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox_Masach.Text = "";
                        textBox_Tensach.Text = "";
                        textBox_Matacgia.Text = "";
                        textBox_Tacgia.Text = "";
                        textBox_Matheloai.Text = "";
                        textBox_Theloaisach.Text = "";
                        textBox_Manhaxuatban.Text = "";
                        textBox_Nhaxuatban.Text = "";
                        textBox_Namxuatban.Text = "";
                        textBox_Soluong.Text = "";
                        textBox_Gia1cuonsach.Text = "";
                        Hinhanh.Image = null;
                    }
                    else
                    {
                        MessageBox.Show("Số lượng sách không hợp lệ. Yêu cầu ký tự là số.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox
                textBox_Masach.Text = "";
                textBox_Tensach.Text = "";
                textBox_Matacgia.Text = "";
                textBox_Tacgia.Text = "";
                textBox_Matheloai.Text = "";
                textBox_Theloaisach.Text = "";
                textBox_Manhaxuatban.Text = "";
                textBox_Nhaxuatban.Text = "";
                textBox_Namxuatban.Text = "";
                textBox_Soluong.Text = "";
                textBox_Gia1cuonsach.Text = "";
                Hinhanh.Image = null;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Bạn có muốn quay về trang chủ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                Trangchu form1 = new Trangchu();
                this.Hide();
                form1.Show();
            }
            else { }
        }

        private void textBox_Matacgia_TextChanged(object sender, EventArgs e)
        {
            string MaTacgia = textBox_Matacgia.Text.Trim();
            string Butdanh = GetButdanhTacgiaFromDatabase(MaTacgia);
            textBox_Tacgia.Text = Butdanh;
        }
        private string GetButdanhTacgiaFromDatabase(string MaTacgia)
        {
            // Thực hiện truy vấn đến cơ sở dữ liệu để lấy Butdanh tương ứng với MaTacgia
            string Butdanh = "";

            // Ví dụ: sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();

                // Nếu MaTacgia tồn tại, thực hiện truy vấn để lấy Butdanh
                string query = "SELECT Butdanh FROM Tacgia WHERE MaTacgia = @MaTacgia";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaTacgia", MaTacgia);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        Butdanh = result.ToString();
                    }
                }
            }

            return Butdanh;
        }

        private void textBox_Matheloai_TextChanged(object sender, EventArgs e)
        {
            string MaTheloai = textBox_Matheloai.Text.Trim();
         
            string TenTheloai = GetTenTheloaiFromDatabase(MaTheloai);
            textBox_Theloaisach.Text = TenTheloai;
        }
        private string GetTenTheloaiFromDatabase(string MaTheloai)
        {
            // Thực hiện truy vấn đến cơ sở dữ liệu để lấy TenTheloai tương ứng với MaTheloai
            string TenTheloai = "";

            // Ví dụ: sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();


                // Nếu MaTheloai tồn tại, thực hiện truy vấn để lấy TenTheloai
                string query = "SELECT TenTheloai FROM Theloai WHERE MaTheloai = @MaTheloai";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaTheloai", MaTheloai);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        TenTheloai = result.ToString();
                    }
                }
            }

            return TenTheloai;
        }

        private void textBox_Manhaxuatban_TextChanged(object sender, EventArgs e)
        {
            string MaNXB = textBox_Manhaxuatban.Text.Trim();

            string TenNXB = GetTenNXBFromDatabase(MaNXB);
            textBox_Nhaxuatban.Text = TenNXB;
        }

        private string GetTenNXBFromDatabase(string MaNXB)
        {
            // Thực hiện truy vấn đến cơ sở dữ liệu để lấy TenTheloai tương ứng với MaTheloai
            string TenNXB = "";

            // Ví dụ: sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();


                // Nếu MaTheloai tồn tại, thực hiện truy vấn để lấy TenTheloai
                string query = "SELECT TenNXB FROM Nhaxuatban WHERE MaNXB = @MaNXB";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaNXB", MaNXB);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        TenNXB = result.ToString();
                    }
                }
            }

            return TenNXB;
        }

        private void textBox_Masach_TextChanged(object sender, EventArgs e)
        {
            string MaSach = textBox_Masach.Text.Trim();

            if (IsMasachExist(MaSach))
            {
                MessageBox.Show("Mã sách " + MaSach + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private bool IsMasachExist(string MaSach)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Sach WHERE MaSach = @Masach";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Masach", MaSach);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
