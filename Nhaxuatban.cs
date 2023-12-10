using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Nhaxuatban : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Nhaxuatban()
        {
            InitializeComponent();
        }
        int IDNXB;
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Nhaxuatban order by IDNXB desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        private void textBox_Timkiem_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Timkiem.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Nhaxuatban where MaNXB LIKE '" + textBox_Timkiem.Text + "%'";
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

                cmd.CommandText = "SELECT * FROM Nhaxuatban WHERE IDNXB = " + IDNXB + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
        }

        private void button_them_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về nhà xuất bản sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Manhaxuatban.Text != "" &&
                    textBox_Tennhaxuatban.Text != "" &&
                    textBox_Diachinhaxuatban.Text != "" &&
                    textBox_Email.Text != "" &&
                    textBox_Nguoidaidien.Text != "" &&
                    textBox_Sodienthoai.Text != "")
                {
                    string MaNXB = textBox_Manhaxuatban.Text;
                    string TenNXB = textBox_Tennhaxuatban.Text;
                    string Diachi = textBox_Diachinhaxuatban.Text;
                    string Email = textBox_Email.Text;
                    string Nguoidaidien = textBox_Nguoidaidien.Text;
                    Int64 Sodienthoai = Int64.Parse(textBox_Sodienthoai.Text);

                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();

                        // Kiểm tra sự tồn tại của MaTheloai trong cơ sở dữ liệu
                        string checkQuery = "SELECT COUNT(*) FROM Nhaxuatban WHERE MaNXB = @MaNXB";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                        {
                            checkCmd.Parameters.AddWithValue("@MaNXB", MaNXB);
                            int existingRecordsCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (existingRecordsCount > 0)
                            {
                                MessageBox.Show("Mã nhà xuất bản " + MaNXB + " đã tồn tại.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Không thực hiện thêm mới nếu mã đã tồn tại
                            }
                        }

                        // Nếu không có mã trùng, thực hiện thêm mới
                        string insertQuery = "INSERT INTO Nhaxuatban (MaNXB, TenNXB, Diachi, Email, Nguoidaidien, Sodienthoai) VALUES (@MaNXB, @TenNXB, @Diachi, @Email, @Nguoidaidien, @Sodienthoai)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@MaNXB", MaNXB);
                            cmd.Parameters.AddWithValue("@TenNXB", TenNXB);
                            cmd.Parameters.AddWithValue("@Diachi", Diachi);
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Nguoidaidien", Nguoidaidien);
                            cmd.Parameters.AddWithValue("@Sodienthoai", Sodienthoai);

                            cmd.ExecuteNonQuery();
                            load_data();
                        }

                        MessageBox.Show("Thông tin về nhà xuất bản đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox_Manhaxuatban.Text = "";
                        textBox_Tennhaxuatban.Text = "";
                        textBox_Diachinhaxuatban.Text = "";
                        textBox_Email.Text = "";
                        textBox_Nguoidaidien.Text = "";
                        textBox_Sodienthoai.Text = "";
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
                textBox_Manhaxuatban.Text = "";
                textBox_Tennhaxuatban.Text = "";
                textBox_Diachinhaxuatban.Text = "";
                textBox_Email.Text = "";
                textBox_Nguoidaidien.Text = "";
                textBox_Sodienthoai.Text = "";

            }
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về nhà xuất bản sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Nhaxuatban WHERE  IDNXB = @IDNXB";
                cmd.Parameters.AddWithValue("IDNXB", label10.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Manhaxuatban.Text = "";
                textBox_Tennhaxuatban.Text = "";
                textBox_Diachinhaxuatban.Text = "";
                textBox_Email.Text = "";
                textBox_Nguoidaidien.Text = "";
                textBox_Sodienthoai.Text = "";

            }
        }

        private void button_Sua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về nhà xuất bản sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Manhaxuatban.Text != "" &&
                    textBox_Tennhaxuatban.Text != "" &&
                    textBox_Diachinhaxuatban.Text != "" &&
                    textBox_Email.Text != "" &&
                    textBox_Nguoidaidien.Text != "" &&
                    textBox_Sodienthoai.Text != "")
                {
                    string MaNXB = textBox_Manhaxuatban.Text;
                    string TenNXB = textBox_Tennhaxuatban.Text;
                    string Diachi = textBox_Diachinhaxuatban.Text;
                    string Email = textBox_Email.Text;
                    string Nguoidaidien = textBox_Nguoidaidien.Text;
                    Int64 Sodienthoai = Int64.Parse(textBox_Sodienthoai.Text);
                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "UPDATE Nhaxuatban SET MaNXB = @MaNXB, TenNXB = @TenNXB, Diachi = @Diachi, Email = @Email, Nguoidaidien = @Nguoidaidien, Sodienthoai = @Sodienthoai WHERE IDNXB = @IDNXB";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@MaNXB", MaNXB);
                            cmd.Parameters.AddWithValue("@TenNXB", TenNXB);
                            cmd.Parameters.AddWithValue("@Diachi", Diachi);
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Nguoidaidien", Nguoidaidien);
                            cmd.Parameters.AddWithValue("@Sodienthoai", Sodienthoai);
                            cmd.Parameters.AddWithValue("@IDNXB", Convert.ToInt32(label10.Text)); // Assuming label10 contains IDNXB value

                            cmd.ExecuteNonQuery();
                            con.Close();
                            load_data();

                            textBox_Manhaxuatban.Text = "";
                            textBox_Tennhaxuatban.Text = "";
                            textBox_Diachinhaxuatban.Text = "";
                            textBox_Email.Text = "";
                            textBox_Nguoidaidien.Text = "";
                            textBox_Sodienthoai.Text = "";
                        }
                    }

                    MessageBox.Show("Thông tin về nhà xuất bản đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Nhaxuatban_Load(object sender, EventArgs e)
        {
            load_data(); 
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label10.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Manhaxuatban.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Tennhaxuatban.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox_Diachinhaxuatban.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Email.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox_Nguoidaidien.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox_Sodienthoai.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();


        }
    }
}
