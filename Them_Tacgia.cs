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
    public partial class Them_Tacgia : Form
    {

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Them_Tacgia()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button_Themhinhanh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Hinhanh files (*.jpg; *.jpeg)| *.jpg; *.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Hinhanh.Image = Image.FromFile(ofd.FileName);
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
                textBox_Matacgia.Clear();
                textBox_Hovaten.Clear();
                textBox_Butdanh.Clear();
                textBox_Nghenghiep.Clear();
                textBox_Quequan.Clear();
                textBox_Theloaisangtac.Clear();
                Hinhanh.Image = null;
            }
        }
        private void button_them_Click(object sender, EventArgs e)
        {
            if (Hinhanh.Image == null)
            {
                MessageBox.Show("Yêu cầu thêm hình ảnh", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm sách nếu không có hình ảnh
            }

            if (MessageBox.Show("Thông tin sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Hovaten.Text != "" &&
                    textBox_Matacgia.Text != "" &&
                    textBox_Butdanh.Text != "" &&
                    textBox_Nghenghiep.Text != "" &&
                    textBox_Quequan.Text != "" &&
                    textBox_Theloaisangtac.Text != "")
                {
                    string Matacgia = textBox_Matacgia.Text;
                    string Hovaten = textBox_Hovaten.Text;
                    string Butdanh = textBox_Butdanh.Text;
                    string Nghenghiep = textBox_Nghenghiep.Text;
                    string Ngaysinh = dateTimePicker_Ngaysinh.Value.ToString("yyyy-MM-dd"); // Convert to the format SQL Server expects
                    string Quequan = textBox_Quequan.Text;
                    string Theloaisangtac = textBox_Theloaisangtac.Text;

                    // Kiểm tra xem "Masonhanvien" đã tồn tại trong cơ sở dữ liệu hay chưa
                    if (IsMaTacgiaExist(Matacgia))
                    {
                        MessageBox.Show("Mã tác giả " + Matacgia + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Không thêm vào cơ sở dữ liệu nếu bị trùng
                    }

                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "INSERT INTO Tacgia (Matacgia, Hovaten, Butdanh, Nghenghiep, Ngaysinh, Quequan, Theloaisangtac, Hinhanh) VALUES (@Matacgia, @Hovaten, @Butdanh, @Nghenghiep, @Ngaysinh, @Quequan, @Theloaisangtac, @Hinhanh)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            MemoryStream memstr = new MemoryStream();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Hovaten", textBox_Hovaten.Text);
                            cmd.Parameters.AddWithValue("@Butdanh", textBox_Butdanh.Text);
                            cmd.Parameters.AddWithValue("@Nghenghiep", textBox_Nghenghiep.Text);
                            cmd.Parameters.AddWithValue("@Ngaysinh", Ngaysinh); // Use the converted date
                            cmd.Parameters.AddWithValue("@Quequan", textBox_Quequan.Text);
                            cmd.Parameters.AddWithValue("@Theloaisangtac", textBox_Theloaisangtac.Text);
                            cmd.Parameters.AddWithValue("@Matacgia", textBox_Matacgia.Text);

                            Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                            cmd.Parameters.AddWithValue("@Hinhanh", memstr.ToArray());

                            cmd.ExecuteNonQuery();

                            SqlCommand cmd2 = new SqlCommand("Select * from Tacgia", con);
                            DataTable dt = new DataTable();
                            dt.Load(cmd2.ExecuteReader());
                            con.Close();
                        }
                    }

                    MessageBox.Show("Thông tin về tác giả đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_Matacgia.Text = "";
                    textBox_Hovaten.Text = "";
                    textBox_Butdanh.Text = "";
                    textBox_Nghenghiep.Text = "";
                    dateTimePicker_Ngaysinh.Text = "";
                    textBox_Quequan.Text = "";
                    textBox_Theloaisangtac.Text = "";

                    Hinhanh.Image = null;
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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

        private void Them_Tacgia_Load(object sender, EventArgs e)
        {

        }

        private void textBox_Matacgia_TextChanged(object sender, EventArgs e)
        {
            string MaTacgia = textBox_Matacgia.Text.Trim();

            if (IsMaTacgiaExist(MaTacgia))
            {
                MessageBox.Show("Mã tác giả " + MaTacgia + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private bool IsMaTacgiaExist(string MaTacgia)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Tacgia WHERE MaTacgia = @MaTacgia";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaTacgia", MaTacgia);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

    }
}
