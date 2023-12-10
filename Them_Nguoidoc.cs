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
    public partial class Them_Nguoidoc : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Them_Nguoidoc()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox
                textBox_Masonguoidoc.Text = "";
                textBox_Hovaten.Text = "";
                dateTimePicker_Ngaysinh.Text = "";
                textBox_Noisinh.Text = "";
                comboBox_Gioitinh.Text = "";
                comboBox_Chucvu.Text = "";
                textBox_Noisinh.Text = "";
                textBox_Lophoc.Text = "";
                textBox_Ngaybatdau.Text = "";
                textBox_Ngayhethan.Text = "";
                textBox_Sothethuvien.Text = "";
                Hinhanh.Image = null;
            }
        }

        private void button_Them_Click(object sender, EventArgs e)
        {
            if (Hinhanh.Image == null)
            {
                MessageBox.Show("Yêu cầu thêm hình ảnh", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm sách nếu không có hình ảnh
            }
            // Kiểm tra số thẻ thư viện trước khi thêm
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();
            if (IsSothethuvienExist(Sothethuvien))
            {
                MessageBox.Show("Số thẻ thư viện " + Sothethuvien + " đã tồn tại! Vui lòng chọn số thẻ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Không thực hiện thêm nếu số thẻ thư viện đã tồn tại
            }

            if (MessageBox.Show("Thông tin về người đọc sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Masonguoidoc.Text != "" &&
                    textBox_Hovaten.Text != "" &&
                    dateTimePicker_Ngaysinh.Text != "" &&
                    textBox_Noisinh.Text != "" &&
                    comboBox_Gioitinh.Text != "" &&
                    comboBox_Chucvu.Text != "" &&
                    textBox_Noisinh.Text != "" &&
                    textBox_Lophoc.Text != "" &&
                    textBox_Ngaybatdau.Text != "" &&
                    textBox_Ngayhethan.Text != "" &&
                    textBox_Sothethuvien.Text != "")
                {
                    string Masonguoidoc = textBox_Masonguoidoc.Text;

                    // Kiểm tra sự tồn tại của Masonguoidoc trong cơ sở dữ liệu
                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string checkQuery = "SELECT COUNT(*) FROM Nguoidoc WHERE Masonguoidoc = @Masonguoidoc";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                        {
                            checkCmd.Parameters.AddWithValue("@Masonguoidoc", Masonguoidoc);
                            int existingRecordsCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (existingRecordsCount > 0)
                            {
                                MessageBox.Show("Mã số người đọc đã tồn tại.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Không thực hiện thêm mới nếu mã đã tồn tại
                            }
                        }
                    }
                   
                    string Hovaten = textBox_Hovaten.Text;
                    string Lophoc = textBox_Lophoc.Text;
                    string Ngaysinh = dateTimePicker_Ngaysinh.Value.ToString("yyyy-MM-dd");
                    string Noisinh = textBox_Noisinh.Text;
                    string Gioitinh = comboBox_Gioitinh.Text;
                    string Chucvu = comboBox_Chucvu.Text;

                    // Nếu không có mã trùng, thực hiện thêm mới
                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "INSERT INTO Nguoidoc (Masonguoidoc, Hovaten,Ngaysinh, Chucvu, Noisinh, Gioitinh, Lophoc,Sothethuvien, Hinhanh) VALUES (@Masonguoidoc, @Hovaten, @Ngaysinh, @Chucvu, @Noisinh, @Gioitinh, @Lophoc,@Sothethuvien, @Hinhanh)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            MemoryStream memstr = new MemoryStream();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Masonguoidoc", textBox_Masonguoidoc.Text);
                            cmd.Parameters.AddWithValue("@Hovaten", textBox_Hovaten.Text);
                            cmd.Parameters.AddWithValue("@Ngaysinh", Ngaysinh);
                            cmd.Parameters.AddWithValue("@Chucvu", comboBox_Chucvu.Text);
                            cmd.Parameters.AddWithValue("@Noisinh", textBox_Noisinh.Text);
                            cmd.Parameters.AddWithValue("@Gioitinh", comboBox_Gioitinh.Text);
                            cmd.Parameters.AddWithValue("@Lophoc", textBox_Lophoc.Text);
                            cmd.Parameters.AddWithValue("@Sothethuvien", textBox_Sothethuvien.Text);

                            Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                            cmd.Parameters.AddWithValue("Hinhanh", memstr.ToArray());

                            cmd.ExecuteNonQuery();

                            SqlCommand cmd2 = new SqlCommand("Select * from Nguoidoc", con);
                            DataTable dt = new DataTable();
                            dt.Load(cmd2.ExecuteReader());
                            con.Close();
                        }
                    }

                    MessageBox.Show("Thông tin về người đọc đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox_Masonguoidoc.Text = "";
                    textBox_Hovaten.Text = "";
                    dateTimePicker_Ngaysinh.Text = "";
                    textBox_Noisinh.Text = "";
                    comboBox_Gioitinh.Text = "";
                    comboBox_Chucvu.Text = "";
                    textBox_Noisinh.Text = "";
                    textBox_Lophoc.Text = "";
                    textBox_Ngaybatdau.Text = "";
                    textBox_Ngayhethan.Text = "";
                    textBox_Sothethuvien.Text = "";
                    Hinhanh.Image = null;
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void textBox_Masonguoidoc_TextChanged(object sender, EventArgs e)
        {
            string masonguoidoc = textBox_Masonguoidoc.Text.Trim();

            if (IsMasonguoidocExist(masonguoidoc))
            {
                MessageBox.Show("Mã số " +masonguoidoc + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool IsMasonguoidocExist(string Masonguoidoc)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Nguoidoc WHERE Masonguoidoc = @Masonguoidoc";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Masonguoidoc", Masonguoidoc);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void textBox_Sothethuvien_TextChanged(object sender, EventArgs e)
        {
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();

            if (IsSothethuvienExist(Sothethuvien))
            {
                MessageBox.Show("Số thẻ thư viện " + Sothethuvien + " đã tồn tại! Vui lòng chọn số thẻ khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Nếu số thẻ thư viện đã tồn tại, bạn có thể thực hiện các hành động khác ở đây nếu cần thiết
            }
            else
            {
                // Lấy Ngaybatdau và Ngayhethan từ cơ sở dữ liệu dựa trên Sothethuvien
                string Ngaybatdau = GetNgayFromDatabase("Ngaybatdau", Sothethuvien);
                string Ngayhethan = GetNgayFromDatabase("Ngayhethan", Sothethuvien);

                textBox_Ngaybatdau.Text = Ngaybatdau;
                textBox_Ngayhethan.Text = Ngayhethan;
            }
        }
        private bool IsSothethuvienExist(string Sothethuvien)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Nguoidoc WHERE Sothethuvien = @Sothethuvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private string GetNgayFromDatabase(string columnName, string Sothethuvien)
        {
            string ngay = "";

            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();

                // Thực hiện truy vấn để lấy giá trị Ngay từ cơ sở dữ liệu dựa trên Sothethuvien
                string query = $"SELECT {columnName} FROM Thethuvien WHERE Sothethuvien = @Sothethuvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        ngay = result.ToString();
                    }
                }
            }

            return ngay;
        }
    }
}
