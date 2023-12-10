using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NMCNPM_CuoiKy
{
    public partial class Xoa_sua_sach : Form
    {

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();

        public Xoa_sua_sach()
        {
            InitializeComponent();
        }
        private void button_Close_Click(object sender, EventArgs e)
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

        private void button_Datlai_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại thông báo với lựa chọn "Có" và "Không"
            DialogResult result = MessageBox.Show("Tất cả thông tin sẽ được đặt lại. Xác nhận?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra lựa chọn của người dùng
            if (result == DialogResult.Yes)
            {
                // Đặt lại nội dung trong các TextBox

            
                label13.Text = "";
                textBox_Masach.Text = "";
                textBox_Tensach.Text = "";
                textBox_Matacgia.Text = "";
                textBox_Butdanh.Text = "";
                textBox_Matheloai.Text = "";
                textBox_Theloaisach.Text = "";
                textBox_MaNXB.Text = "";
                textBox_TenNXB.Text = "";
                textBox_Namxuatban.Text = "";
                textBox_Soluongsach.Text = "";
                textBox_Gia1cuonsach.Text = "";
                Hinhanh.Image = null;
            }
        }
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Sach order by IDSach desc", con );
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill( dt );
            dataGridView1.DataSource = dt;
            DataGridViewImageColumn Hinhanh = new DataGridViewImageColumn();
            Hinhanh = (DataGridViewImageColumn)dataGridView1.Columns[9];
            Hinhanh.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        int id;
        Int64 Rowid;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label13.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
           
           textBox_Masach.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
           textBox_Tensach.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
           textBox_Matacgia.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
           textBox_Matheloai.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
           textBox_MaNXB.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
           textBox_Namxuatban.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
           textBox_Soluongsach.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
           textBox_Gia1cuonsach.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();

           MemoryStream ms =  new MemoryStream((byte[])dataGridView1.CurrentRow.Cells[9].Value);
           Hinhanh.Image = Image.FromStream(ms) ;

            // Hiển thị Butdanh tương ứng với MaTacgia từ database.
            string MaTacgia = row.Cells[3].Value.ToString();
            textBox_Butdanh.Text = GetButdanhTacgiaFromDatabase(MaTacgia);

            string Matheloai = row.Cells[4].Value.ToString();
            textBox_Theloaisach.Text = GetTenTheloaiFromDatabase(Matheloai);

            string MaNXB = row.Cells[5].Value.ToString();
            textBox_TenNXB.Text = GetTenNXBFromDatabase(MaNXB);
        }
        

        private void Xoa_sua_sach_Load(object sender, EventArgs e)
        {
            load_data();
        }

        private void button_Capnhatthongtinsach_Click(object sender, EventArgs e)
        {
            if (Hinhanh.Image == null)
            {
                MessageBox.Show("Yêu cầu thêm hình ảnh", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra xem hình ảnh có hợp lệ không
            if (Hinhanh.Image is Bitmap img)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (MessageBox.Show("Thông tin sách sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (textBox_Masach.Text != "" &&
                            textBox_Tensach.Text != "" &&
                            textBox_Matacgia.Text != "" &&
                            textBox_Butdanh.Text != "" &&
                            textBox_Matheloai.Text != "" &&
                            textBox_Theloaisach.Text != "" &&
                            textBox_MaNXB.Text != "" &&
                            textBox_TenNXB.Text != "" &&
                            textBox_Namxuatban.Text != "" &&
                            textBox_Soluongsach.Text != "" &&
                            textBox_Gia1cuonsach.Text != "")
                        {
                            string MaSach = textBox_Masach.Text;
                            string TenSach = textBox_Tensach.Text;
                            string MaTacGia = textBox_Matacgia.Text;
                            string Matheloai = textBox_Matheloai.Text;
                            string MaNXB = textBox_MaNXB.Text;
                            Int64 SoluongValue = Int64.Parse(textBox_Soluongsach.Text);
                            Int64 Namxuatban = Int64.Parse(textBox_Namxuatban.Text);

                            string Gia1cuonsach = textBox_Gia1cuonsach.Text;

                            if (int.TryParse(textBox_Soluongsach.Text, out int Soluong))
                            {
                                using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                                {
                                    con.Open();
                                    string query = "UPDATE Sach SET Masach = @Masach, Tensach = @Tensach, MaTacgia = @MaTacgia, Matheloai = @Matheloai, MaNXB = @MaNXB, Namxuatban = @Namxuatban, Soluong = @Soluong ,Gia1cuonsach = @Gia1cuonsach, Hinhanh = @Hinhanh WHERE IDSach = @IDSach";
                                    using (SqlCommand cmd = new SqlCommand(query, con))
                                    {
                                        cmd.CommandType = CommandType.Text;

                                        cmd.Parameters.AddWithValue("@IDSach", label13.Text);
                                        cmd.Parameters.AddWithValue("@Masach", textBox_Masach.Text);
                                        cmd.Parameters.AddWithValue("@Tensach", textBox_Tensach.Text);
                                        cmd.Parameters.AddWithValue("@MaTacgia", textBox_Matacgia.Text);
                                        cmd.Parameters.AddWithValue("@Matheloai", textBox_Matheloai.Text);
                                        cmd.Parameters.AddWithValue("@MaNXB", textBox_MaNXB.Text);
                                        cmd.Parameters.AddWithValue("@Namxuatban", Namxuatban);
                                        cmd.Parameters.AddWithValue("@Soluong", Soluong);
                                        cmd.Parameters.AddWithValue("@Gia1cuonsach", textBox_Gia1cuonsach.Text);

                                        MemoryStream memstr = new MemoryStream();
                                        Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                                        cmd.Parameters.AddWithValue("Hinhanh", memstr.ToArray());
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        load_data();

                                        Hinhanh.Image = null;
                                        label13.Text = "";
                                        textBox_Masach.Text = "";
                                        textBox_Tensach.Text = "";
                                        textBox_Matacgia.Text = "";
                                        textBox_Butdanh.Text = "";
                                        textBox_Matheloai.Text = "";
                                        textBox_Theloaisach.Text = "";
                                        textBox_MaNXB.Text = "";
                                        textBox_TenNXB.Text = "";
                                        textBox_Namxuatban.Text = "";
                                        textBox_Soluongsach.Text = "";
                                        textBox_Gia1cuonsach.Text = "";
                                    }
                                }

                                MessageBox.Show("Thông tin sách đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
            else
            {
                MessageBox.Show("Hình ảnh không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Xoasach_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin sách sẽ được xoá.Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Sach WHERE  IDSach = @IDSach";
                cmd.Parameters.AddWithValue("IDSach", label13.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();


                Hinhanh.Image = null;
                label13.Text = "";
                textBox_Masach.Text = "";
                textBox_Tensach.Text = "";
                textBox_Matacgia.Text = "";
                textBox_Butdanh.Text = "";
                textBox_Matheloai.Text = "";
                textBox_Theloaisach.Text = "";
                textBox_MaNXB.Text = "";
                textBox_TenNXB.Text = "";
                textBox_Namxuatban.Text = "";
                textBox_Soluongsach.Text = "";
                textBox_Gia1cuonsach.Text = "";


            }
        }
        int IDSach;

        private void textBox_timkiemmasach_TextChanged(object sender, EventArgs e)
        {
            if (textBox_timkiemmasach.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Sach where Masach LIKE '" + textBox_timkiemmasach.Text + "%'";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            else
            {
                panel2.Visible = true;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Sach WHERE IDSach = " +IDSach+ "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }

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

        private void textBox_Matacgia_TextChanged(object sender, EventArgs e)
        {
            string MaTacgia = textBox_Matacgia.Text.Trim();
            string Butdanh = GetButdanhTacgiaFromDatabase(MaTacgia);
            textBox_Butdanh.Text = Butdanh;
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
            string Matheloai = textBox_Matheloai.Text.Trim();

            string TenTheloai = GetTenTheloaiFromDatabase(Matheloai);
            textBox_Theloaisach.Text = TenTheloai;
        }

        private string GetTenTheloaiFromDatabase(string Matheloai)
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
                    cmd.Parameters.AddWithValue("@MaTheloai", Matheloai);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        TenTheloai = result.ToString();
                    }
                }
            }

            return TenTheloai;
        }

        private void textBox_MaNXB_TextChanged(object sender, EventArgs e)
        {
            string MaNXB = textBox_MaNXB.Text.Trim();

            string TenNXB = GetTenNXBFromDatabase(MaNXB);
            textBox_TenNXB.Text = TenNXB;
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

    }
}
