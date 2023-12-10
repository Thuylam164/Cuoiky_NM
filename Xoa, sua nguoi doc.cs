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
using System.Xml;

namespace NMCNPM_CuoiKy
{
    public partial class Xoa__sua_nguoi_doc : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Xoa__sua_nguoi_doc()
        {
            InitializeComponent();
        }
        int IDNguoidoc;
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Nguoidoc order by IDNguoidoc desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            DataGridViewImageColumn Hinhanh = new DataGridViewImageColumn();
            Hinhanh = (DataGridViewImageColumn)dataGridView1.Columns[9];
            Hinhanh.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        private void Xoa__sua_nguoi_doc_Load(object sender, EventArgs e)
        {
            load_data();

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
                textBox_Masonguoidoc.Clear();
                textBox_Hovaten.Clear();
                textBox_Ngaysinh.Clear();
                textBox_Chucvu.Clear();
                textBox_Noisinh.Clear();
                textBox_Gioitinh.Clear();
                textBox_Lophoc.Clear();
                textBox_Sothethuvien.Clear();
                textBox_Ngaybatdau.Clear();
                textBox_Ngayhethan.Clear();

                Hinhanh.Image = null;
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
        private void button_Sua_Click(object sender, EventArgs e)
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


                    if (MessageBox.Show("Thông tin về người đọc sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (textBox_Masonguoidoc.Text != "" &&
                            textBox_Hovaten.Text != "" &&
                            textBox_Ngaysinh.Text != "" &&
                            textBox_Chucvu.Text != "" &&
                            textBox_Noisinh.Text != "" &&
                            textBox_Gioitinh.Text != "" &&
                            textBox_Lophoc.Text != "" &&
                            textBox_Sothethuvien.Text != "" &&
                            textBox_Ngaybatdau.Text != "" &&
                            textBox_Ngayhethan.Text != "")
                        {
                            string Masonguoidoc = textBox_Masonguoidoc.Text;
                            string Hovaten = textBox_Hovaten.Text;
                            string Ngaysinh = textBox_Ngaysinh.Text;
                            string Noisinh = textBox_Noisinh.Text;
                            string Gioitinh = textBox_Gioitinh.Text;
                            string Chucvu = textBox_Chucvu.Text;
                            string Lophoc = textBox_Lophoc.Text;
                            string Sothethuvien = textBox_Sothethuvien.Text;
                       

                            if (Chucvu == "Học sinh" && string.IsNullOrWhiteSpace(Lophoc))
                            {
                                MessageBox.Show("Thông tin 'Lớp học' không được để trống khi chức vụ là 'Học sinh'.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Không thực hiện thêm người đọc nếu thông tin Lớp học bị bỏ trống.
                            }

                            using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                            {
                                con.Open();
                                string query = "UPDATE Nguoidoc SET Masonguoidoc = @Masonguoidoc, Hovaten = @Hovaten,Ngaysinh = @Ngaysinh, Chucvu = @Chucvu, Noisinh = @Noisinh, Gioitinh = @Gioitinh, Lophoc = @Lophoc,Sothethuvien =@Sothethuvien, Hinhanh = @Hinhanh WHERE IDNguoidoc = @IDNguoidoc";
                                using (SqlCommand cmd = new SqlCommand(query, con))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@IDNguoidoc", label13.Text);
                                    cmd.Parameters.AddWithValue("@Masonguoidoc", textBox_Masonguoidoc.Text);
                                    cmd.Parameters.AddWithValue("@Hovaten", textBox_Hovaten.Text);
                                    cmd.Parameters.AddWithValue("@Ngaysinh", textBox_Ngaysinh.Text);
                                    cmd.Parameters.AddWithValue("@Chucvu", textBox_Chucvu.Text);
                                    cmd.Parameters.AddWithValue("@Noisinh", textBox_Noisinh.Text);
                                    cmd.Parameters.AddWithValue("@Gioitinh", textBox_Gioitinh.Text);
                                    cmd.Parameters.AddWithValue("@Lophoc", textBox_Lophoc.Text);
                                    cmd.Parameters.AddWithValue("@Sothethuvien",textBox_Sothethuvien.Text);

                                    MemoryStream memstr = new MemoryStream();
                                    Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                                    cmd.Parameters.AddWithValue("@Hinhanh", memstr.ToArray());

                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    load_data();

                                    textBox_Masonguoidoc.Clear();
                                    textBox_Hovaten.Clear();
                                    textBox_Ngaysinh.Clear();
                                    textBox_Chucvu.Clear();
                                    textBox_Noisinh.Clear();
                                    textBox_Gioitinh.Clear();
                                    textBox_Lophoc.Clear();
                                    textBox_Sothethuvien.Clear();
                                    textBox_Ngaybatdau.Clear();
                                    textBox_Ngayhethan.Clear();


                                    Hinhanh.Image = null;
                                }
                            }


                            MessageBox.Show("Thông tin về người đọc đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label13.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Masonguoidoc.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Hovaten.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox_Ngaysinh.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Chucvu.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox_Noisinh.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox_Gioitinh.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox_Lophoc.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox_Sothethuvien.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();

            MemoryStream ms = new MemoryStream((byte[])dataGridView1.CurrentRow.Cells[9].Value);
            Hinhanh.Image = Image.FromStream(ms);
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về người đọc sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Nguoidoc WHERE  IDNguoidoc = @IDNGuoidoc";
                cmd.Parameters.AddWithValue("IDNguoidoc", label13.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Masonguoidoc.Clear();
                textBox_Hovaten.Clear();
                textBox_Ngaysinh.Clear();
                textBox_Chucvu.Clear();
                textBox_Noisinh.Clear();
                textBox_Gioitinh.Clear();
                textBox_Lophoc.Clear();
                textBox_Sothethuvien.Clear();
                textBox_Ngaybatdau.Clear();
                textBox_Ngayhethan.Clear();

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


        private void textBox_Timkiemmaso_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Timkiemmaso.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Nguoidoc where Masonguoidoc LIKE '" + textBox_Timkiemmaso.Text + "%'";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            else
            {
               

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Nguoidoc WHERE IDNguoidoc = " + IDNguoidoc + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
        }

        private void textBox_Sothethuvien_TextChanged(object sender, EventArgs e)
        {
            string Sothethuvien = textBox_Sothethuvien.Text.Trim();

            // Lấy Ngaybatdau và Ngayhethan từ cơ sở dữ liệu dựa trên Sothethuvien
            string Ngaybatdau = GetNgayFromDatabase("Ngaybatdau", Sothethuvien);
            string Ngayhethan = GetNgayFromDatabase("Ngayhethan", Sothethuvien);

            textBox_Ngaybatdau.Text = Ngaybatdau;
            textBox_Ngayhethan.Text = Ngayhethan;
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
