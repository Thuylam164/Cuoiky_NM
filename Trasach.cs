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
    public partial class Trasach : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Trasach()
        {
            InitializeComponent();
        }
        private bool isSearching = false;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label12.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Mamuontra.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Masach.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Tinhtrangsachkhimuon.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            textBox_Ngaydukientra.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
        }

        int IDMuontra;
        private void textBox_Timkiemsothethuvien_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox_Timkiemsothethuvien.Text != "")
            {
                SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                // Thêm điều kiện Ngaytra IS NULL vào câu truy vấn
                cmd.CommandText = "SELECT * FROM Muontra WHERE Sothethuvien LIKE @Sothethuvien AND Ngaytra IS NULL";
                cmd.Parameters.AddWithValue("@Sothethuvien", textBox_Timkiemsothethuvien.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                dataGridView1.DataSource = null;


                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Muontra WHERE IDMuontra = " + IDMuontra + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
        }

        private void Trasach_Load(object sender, EventArgs e)
        {
            load_data();
        }
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Muontra order by IDMuontra desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.DataSource = null;

        }


        // Các sự kiện và phương thức khác



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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox
                textBox_Timkiemsothethuvien.Clear();
                textBox_Mamuontra.Clear();
                textBox_Masach.Clear();
                textBox_Tensach.Clear();
                textBox_Tinhtrangsachkhimuon.Clear();
                textBox_Tinhtrangsachkhitra.Clear();
                textBox_Ngaydukientra.Clear();
                textBox_Manhanvien.Clear();
                textBox_Hovaten.Clear();
            }
        }

        private void button_Trangchu_Click(object sender, EventArgs e)
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
  
        private void button_Trasach_Click(object sender, EventArgs e)
        {

            ////////////////
            using (MemoryStream ms = new MemoryStream())
            {


                if (MessageBox.Show("Thông tin về trả sách sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (textBox_Timkiemsothethuvien.Text != "" &&
                        textBox_Mamuontra.Text != "" &&
                        textBox_Tinhtrangsachkhitra.Text != "" &&
                        textBox_Manhanvien.Text != "")
                    {
                        string Tinhtrangsachsaukhimuon = textBox_Tinhtrangsachkhitra.Text;
                        string Ngaytra = dateTimePicker_Ngaytra.Value.ToString("yyyy-MM-dd");
                        string Manhanvienthuchientrasach = textBox_Manhanvien.Text;

                        using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                        {
                            con.Open();
                            string query = "UPDATE Muontra SET Manhanvienthuchientrasach =@Manhanvienthuchientrasach,Ngaytra = @Ngaytra, Tinhtrangsachsaukhimuon = @Tinhtrangsachsaukhimuon WHERE IDMuontra = @IDMuontra";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;

                                cmd.Parameters.AddWithValue("@IDMuontra", label12.Text);
                                cmd.Parameters.AddWithValue("@Manhanvienthuchientrasach",textBox_Manhanvien.Text);
                                cmd.Parameters.AddWithValue("@Ngaytra", Ngaytra);
                                cmd.Parameters.AddWithValue("@Tinhtrangsachsaukhimuon", textBox_Tinhtrangsachkhitra.Text);

                                cmd.ExecuteNonQuery();
                                con.Close();
                                load_data();


                                // Đặt lại nội dung trong các TextBox
                                textBox_Timkiemsothethuvien.Clear();
                                textBox_Mamuontra.Clear();
                                textBox_Masach.Clear();
                                textBox_Tensach.Clear();
                                textBox_Tinhtrangsachkhimuon.Clear();
                                textBox_Tinhtrangsachkhitra.Clear();
                                textBox_Ngaydukientra.Clear();
                                textBox_Manhanvien.Clear();
                                textBox_Hovaten.Clear();
                            }
                        }

                        MessageBox.Show("Trả sách thành công.", "Trả sách", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Trasach_Load(this, null);

                    }
                    else
                    {
                        MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }


            }

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

        private void textBox_Manhanvien_TextChanged(object sender, EventArgs e)
        {
            string Masonhanvien = textBox_Manhanvien.Text.Trim();

            string Hovaten = GetMasonhanvienFromDatabase(Masonhanvien);
            textBox_Hovaten.Text = Hovaten;
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

    }
}
