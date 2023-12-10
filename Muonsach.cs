using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Muonsach : Form
    {
        public Muonsach()
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
                textBox_Ngaybatdau.Clear();
                textBox_Ngayhethan.Clear();
                textBox_Ghichu.Clear();
                textBox_Chucvu.Clear();
                textBox_Mamuontra.Clear();
                textBox_Masach.Clear();
                textBox_Tensach.Clear();
                textBox_Soluongduocmuontoida.Clear();
                textBox_Thoiluongmuon.Clear();
                textBox_Tinhtrangsachtruockhimuon.Clear();
                textBox_Manhanvien.Clear();
                textBox_Hovatennhanvien.Clear();
            }
        }


        private void button_Muonsach_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem tất cả các TextBox đã được điền đầy đủ hay chưa
            if (string.IsNullOrEmpty(textBox_Ngaybatdau.Text) ||
                string.IsNullOrEmpty(textBox_Ngayhethan.Text) ||
                string.IsNullOrEmpty(textBox_Ghichu.Text) ||
                string.IsNullOrEmpty(textBox_Chucvu.Text) ||
                string.IsNullOrEmpty(textBox_Mamuontra.Text) ||
                string.IsNullOrEmpty(textBox_Masach.Text) ||
                string.IsNullOrEmpty(textBox_Tensach.Text) ||
                string.IsNullOrEmpty(textBox_Soluongduocmuontoida.Text) ||
                string.IsNullOrEmpty(textBox_Thoiluongmuon.Text) ||
                string.IsNullOrEmpty(textBox_Tinhtrangsachtruockhimuon.Text) ||
                string.IsNullOrEmpty(textBox_Manhanvien.Text) ||
                string.IsNullOrEmpty(textBox_Hovatennhanvien.Text) ||
                string.IsNullOrEmpty(textBox_Soluongduocmuontoida.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //////////////////////////////////////////
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();
            string Masach = textBox_Masach.Text.Trim();
            string SoluongduocmuontoidaText = textBox_Soluongduocmuontoida.Text.Trim();
            string Ngaymuon = dateTimePicker_Ngaymuon.Value.ToString("yyyy-MM-dd");
            string Ngaydukientra = dateTimePicker_Ngaydukientra.Value.ToString("yyyy-MM-dd");
            string Tinhtrangsachkhimuon = textBox_Tinhtrangsachtruockhimuon.Text.Trim();

            if (!string.IsNullOrEmpty(Masach))
            {
                if (!string.IsNullOrEmpty(Sothethuvien))
                {
                    // Kiểm tra số lượng mượn tối đa
                    int soluongduocmuontoida = 0;
                    if (int.TryParse(SoluongduocmuontoidaText, out soluongduocmuontoida))
                    {
                        int currentBorrowedQuantity = GetBorrowedQuantity(Sothethuvien, Masach);
                        if (currentBorrowedQuantity < soluongduocmuontoida)
                        {
                            // Kiểm tra số lượng sách còn để mượn
                            int availableQuantity = GetAvailableQuantity(Masach);
                            if (availableQuantity > 0)
                            {
                                // Tiếp tục thêm thông tin mượn sách
                                string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    string insertQuery = "INSERT INTO Muontra (Mamuontra, Sothethuvien,Masach, Manhanvienthuchienmuonsach,Thoiluongmuon,Ngaymuon,Ngaydukientra,Soluongsachduocmuon,Tinhtrangsachkhimuon) " +
                                        "VALUES (@Mamuontra, @Sothethuvien,@Masach, @Manhanvienthuchienmuonsach,@Thoiluongmuon,@Ngaymuon,@Ngaydukientra,@Soluongsachduocmuon,@Tinhtrangsachkhimuon)";

                                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                                    {
                                        cmd.Parameters.AddWithValue("@Mamuontra", textBox_Mamuontra.Text);
                                        cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                                        cmd.Parameters.AddWithValue("@Masach", Masach);
                                        cmd.Parameters.AddWithValue("@Manhanvienthuchienmuonsach", textBox_Manhanvien.Text);
                                        cmd.Parameters.AddWithValue("@Thoiluongmuon", Int64.Parse(textBox_Thoiluongmuon.Text));
                                        cmd.Parameters.AddWithValue("@Ngaymuon", Ngaymuon);
                                        cmd.Parameters.AddWithValue("@Tinhtrangsachkhimuon", textBox_Tinhtrangsachtruockhimuon.Text);
                                        cmd.Parameters.AddWithValue("@Ngaydukientra", Ngaydukientra);
                                        cmd.Parameters.AddWithValue("@Soluongsachduocmuon", Int64.Parse(textBox_Soluongduocmuontoida.Text));

                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Đã mượn được sách.", "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                textBox_Sothethuvien.Clear();
                                textBox_Ngaybatdau.Clear();
                                textBox_Ngayhethan.Clear();
                                textBox_Ghichu.Clear();
                                textBox_Chucvu.Clear();
                                textBox_Mamuontra.Clear();
                                textBox_Masach.Clear();
                                textBox_Tensach.Clear();
                                textBox_Soluongduocmuontoida.Clear();
                                textBox_Thoiluongmuon.Clear();
                                textBox_Tinhtrangsachtruockhimuon.Clear();
                                textBox_Manhanvien.Clear();
                                textBox_Hovatennhanvien.Clear();
                            }
                            else
                            {
                                MessageBox.Show($"Số lượng sách có mã sách {Masach} đã hết. Không thể mượn thêm.", "Không mượn được sách!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Số lượng sách của số thẻ {Sothethuvien} đã đạt lượng mượn tối đa.", "Không mượn được sách!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Số lượng được mượn tối đa không hợp lệ.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn sách mượn.", "Không mượn được sách!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private int GetAvailableQuantity(string Masach)
        {
            int totalQuantity = 0;
            int borrowedQuantity = 0;

            // Lấy tổng số lượng sách từ cơ sở dữ liệu
            string selectTotalQuantityQuery = "SELECT Soluong FROM Sach WHERE Masach = @Masach";
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(selectTotalQuantityQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Masach", Masach);
                    totalQuantity = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Lấy số lượng sách đã mượn từ cơ sở dữ liệu
            string selectBorrowedQuantityQuery = "SELECT COUNT(*) FROM Muontra WHERE Masach = @Masach AND NgayTra IS NULL";
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(selectBorrowedQuantityQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Masach", Masach);
                    borrowedQuantity = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Số lượng sách còn lại là tổng số lượng trừ đi số lượng đã mượn
            int availableQuantity = totalQuantity - borrowedQuantity;
            return availableQuantity;
        }
        private int GetBorrowedQuantity(string Sothethuvien, string Masach)
        {
            int count = 0;

            string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Muontra WHERE Sothethuvien = @Sothethuvien AND Masach = @Masach AND NgayTra IS NULL";
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    cmd.Parameters.AddWithValue("@Masach", Masach);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return count;
        }



        private int GetBorrowedBookCount(string Sothethuvien)
        {
            int count = 0;

            string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(Sothethuvien) FROM Muontra WHERE Sothethuvien = @Sothethuvien AND Masach = @Masach AND NgayTra IS NULL AND Tinhtrangsachsaukhitra IS NULL AND Manhanvienthuchienmuonsach IS NULL";
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return count;
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

        private void button4_Click(object sender, EventArgs e)
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
                Trangchu trc = new Trangchu();
                this.Hide();
                trc.Show();
            }
            else { }
        }

        private void textBox_Sothethuvien_TextChanged(object sender, EventArgs e)
        {
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();

            // Lấy Ngaybatdau và Ngayhethan từ cơ sở dữ liệu dựa trên Sothethuvien
            string Ngaybatdau = GetNgayFromDatabase("Ngaybatdau", Sothethuvien);
            string Ngayhethan = GetNgayFromDatabase("Ngayhethan", Sothethuvien);

            // Lấy Chucvu từ cơ sở dữ liệu dựa trên Sothethuvien
            string Chucvu = GetChucvuFromDatabase(Sothethuvien);

            // Lấy Ghichu từ cơ sở dữ liệu dựa trên Sothethuvien
            string Ghichu = GetGhichuFromDatabase(Sothethuvien);

            // Hiển thị giá trị lấy được lên các ô TextBox tương ứng
            textBox_Ngaybatdau.Text = Ngaybatdau;
            textBox_Ngayhethan.Text = Ngayhethan;
            textBox_Chucvu.Text = Chucvu;
            textBox_Ghichu.Text = Ghichu;

            if (!string.IsNullOrEmpty(Sothethuvien))
            {
                string chucvu = GetChucvuFromDatabase(Sothethuvien);

                textBox_Chucvu.Text = chucvu;

                int thoiluongmuon = 0;
                int soluongmuonToiDa = 0;

                switch (chucvu.ToLower())
                {
                    case "nhân viên":
                    case "giáo viên":
                    case "cán bộ":
                        thoiluongmuon = 28;
                        soluongmuonToiDa = 4;
                        break;
                    case "học sinh":
                        thoiluongmuon = 14;
                        soluongmuonToiDa = 2;
                        break;
                        // Các trường hợp khác nếu cần
                }

                textBox_Thoiluongmuon.Text = thoiluongmuon.ToString();
                textBox_Soluongduocmuontoida.Text = soluongmuonToiDa.ToString();
            }
        }

        private string GetGhichuFromDatabase(string Sothethuvien)
        {
            string ghichu = "";

            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();

                // Thực hiện truy vấn để lấy giá trị Ghichu từ cơ sở dữ liệu dựa trên Sothethuvien
                string query = "SELECT Ghichu FROM Thethuvien WHERE Sothethuvien = @Sothethuvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        ghichu = result.ToString();
                    }
                }
            }

            return ghichu;
        }

        private string GetChucvuFromDatabase(string Sothethuvien)
        {
            string chucvu = "";

            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();

                // Thực hiện truy vấn để lấy giá trị Chucvu từ cơ sở dữ liệu dựa trên Sothethuvien
                string query = "SELECT Chucvu FROM Nguoidoc WHERE Sothethuvien = @Sothethuvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Sothethuvien", Sothethuvien);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        chucvu = result.ToString();
                    }
                }
            }

            return chucvu;
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

        private void textBox_Masach_TextChanged(object sender, EventArgs e)
        {
            // Lấy giá trị từ textBox_Masach
            string masach = textBox_Masach.Text.Trim();

            // Kiểm tra xem masach có giá trị không
            if (!string.IsNullOrEmpty(masach))
            {
                // Kết nối đến cơ sở dữ liệu
                string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Truy vấn cơ sở dữ liệu để lấy Tensach từ bảng Sach
                    string selectQuery = "SELECT Tensach FROM Sach WHERE Masach = @Masach";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Masach", masach);

                        // Thực hiện truy vấn và đọc giá trị
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lấy giá trị Tensach từ dữ liệu đọc được
                                string tensach = reader["Tensach"].ToString();

                                // Hiển thị giá trị Tensach trong textBox_Tensach
                                textBox_Tensach.Text = tensach;
                            }
                            else
                            {
                                // Nếu không có kết quả, có thể xử lý hoặc xóa giá trị textBox_Tensach
                                textBox_Tensach.Clear();
                            }
                        }
                    }
                }
            }
            else
            {
                // Nếu masach trống, có thể xử lý hoặc xóa giá trị textBox_Tensach
                textBox_Tensach.Clear();
            }
        }

        private void textBox_Thoiluongmuon_TextChanged(object sender, EventArgs e)
        {
            UpdateNgayDuKienTra();
        }

        private void dateTimePicker_Ngaymuon_ValueChanged(object sender, EventArgs e)
        {
            UpdateNgayDuKienTra();
        }
        private void UpdateNgayDuKienTra()
        {
            if (int.TryParse(textBox_Thoiluongmuon.Text, out int thoiluongmuon))
            {
                if (DateTime.TryParseExact(dateTimePicker_Ngaymuon.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayMuon))
                {
                    DateTime ngayDuKienTra = ngayMuon.AddDays(thoiluongmuon);
                    // Hiển thị ngày dự kiến trả trong dateTimePicker_Ngaydukientra
                    dateTimePicker_Ngaydukientra.Value = ngayDuKienTra;
                }
            }
        }

        private void textBox_Manhanvien_TextChanged(object sender, EventArgs e)
        {
            string Masonhanvien = textBox_Manhanvien.Text.Trim();

            string Hovaten = GetMasonhanvienFromDatabase(Masonhanvien);
            textBox_Hovatennhanvien.Text = Hovaten;
        }
        private string GetMasonhanvienFromDatabase(string Masonhanvien)
        {
            // Thực hiện truy vấn đến cơ sở dữ liệu để lấy TenTheloai tương ứng với MaTheloai
            string Hovaten = "";

            // Ví dụ: sử dụng SqlConnection và SqlCommand để thực hiện truy vấn
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();


                // Nếu MaTheloai tồn tại, thực hiện truy vấn để lấy TenTheloai
                string query = "SELECT Hovaten FROM Nhanvien WHERE Masonhanvien = @Masonhanvien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Masonhanvien", Masonhanvien);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        Hovaten = result.ToString();
                    }
                }
            }

            return Hovaten;
        }

        private void Muonsach_Load(object sender, EventArgs e)
        {
            LoadTenSach();

        }
        private void LoadTenSach()
        {
            string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string selectQuery = "SELECT Masach FROM Sach";
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                      
                    }
                }
            }
          
        }

        private void textBox_Mamuontra_TextChanged(object sender, EventArgs e)
        {
            string Mamuontra = textBox_Mamuontra.Text.Trim();

            if (IsMamuontraExist(Mamuontra))
            {
                MessageBox.Show("Mã mượn trả " + Mamuontra + " đã tồn tại! Vui lòng chọn mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private bool IsMamuontraExist(string Mamuontra)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Muontra WHERE Mamuontra = @Mamuontra";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Mamuontra", Mamuontra);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
