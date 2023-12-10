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
    public partial class Xoa_sua_Nhanvien : Form
    {
        public Xoa_sua_Nhanvien()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Timkiem_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Timkiem.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Nhanvien where Masonhanvien LIKE '" + textBox_Timkiem.Text + "%'";
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

                cmd.CommandText = "SELECT * FROM Nhanvien WHERE IDNhanvien = " + IDNhanvien + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
        }
            int IDNhanvien;
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Nhanvien order by IDNhanvien desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        
        }

        private void Xoa_sua_Nhanvien_Load(object sender, EventArgs e)
        {
           load_data();
        }

        private void button5_Click(object sender, EventArgs e)
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

                textBox_Masonhanvien.Text = "";
                textBox_Hovaten.Text = "";
                textBox_Noisinh.Text = "";
                textBox_Sodienthoai.Text = "";
                textBox_Email.Text = "";
                textBox_Taikhoan.Text = "";
                textBox_Phanquyen.Text = "";
                textBox_Gioitinh.Text = "";
                textBox_Ngaysinh.Text = "";
            }
        }

        private void button_Sua_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Thông tin về nhân viên sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Masonhanvien.Text != "" &&
                textBox_Hovaten.Text != "" &&
                textBox_Noisinh.Text != "" &&
                textBox_Sodienthoai.Text != "" &&
                textBox_Email.Text != "" &&
                textBox_Taikhoan.Text != "" &&
                textBox_Phanquyen.Text != "" &&
                textBox_Gioitinh.Text != "" &&
                textBox_Ngaysinh.Text != "")
                {
                   
                    string Masonhanvien = textBox_Masonhanvien.Text;
                    string Hovaten = textBox_Hovaten.Text;
                    string Noisinh = textBox_Noisinh.Text;
                    string Email = textBox_Email.Text;
                    string Taikhoan = textBox_Taikhoan.Text;
                    string Phanquyen =textBox_Phanquyen.Text;
                    string Gioitinh = textBox_Gioitinh.Text;
                    string Ngaysinh = textBox_Ngaysinh.Text;
             
                    Int64 Sodienthoai = Int64.Parse(textBox_Sodienthoai.Text);

                 

                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "UPDATE Nhanvien SET Masonhanvien = @Masonhanvien, Hovaten = @Hovaten, Ngaysinh = @Ngaysinh, Noisinh = @Noisinh, Gioitinh = @Gioitinh, Sodienthoai = @Sodienthoai, Email = @Email, Taikhoan = @Taikhoan,  Phanquyen = @Phanquyen WHERE IDNhanvien = @IDNhanvien";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@IDNhanvien", label2.Text);
                            cmd.Parameters.AddWithValue("@Masonhanvien", textBox_Masonhanvien.Text);
                            cmd.Parameters.AddWithValue("@Hovaten", textBox_Hovaten.Text);
                            cmd.Parameters.AddWithValue("@Ngaysinh", textBox_Ngaysinh.Text);
                            cmd.Parameters.AddWithValue("@Noisinh", textBox_Noisinh.Text); 
                            cmd.Parameters.AddWithValue("@Gioitinh", textBox_Gioitinh.Text);
                            cmd.Parameters.AddWithValue("@Sodienthoai", textBox_Sodienthoai.Text);
                            cmd.Parameters.AddWithValue("@Email", textBox_Email.Text);
                            cmd.Parameters.AddWithValue("@Taikhoan", textBox_Taikhoan.Text);
                            cmd.Parameters.AddWithValue("@Phanquyen", textBox_Phanquyen.Text);


                            

                            cmd.ExecuteNonQuery();
                            con.Close();
                            load_data();
                            
                            textBox_Masonhanvien.Text = "";
                            textBox_Hovaten.Text = "";
                            textBox_Noisinh.Text = "";
                            textBox_Sodienthoai.Text = "";
                            textBox_Email.Text = "";
                            textBox_Taikhoan.Text = "";
                            textBox_Phanquyen.Text = "";
                            textBox_Gioitinh.Text = "";
                            textBox_Ngaysinh.Text = "";
                        }
                    }


                    MessageBox.Show("Thông tin về nhân viên đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

       
       

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về nhân viên sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Nhanvien WHERE  IDNhanvien = @IDNhanvien";
                cmd.Parameters.AddWithValue("IDNhanvien", label2.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Masonhanvien.Text = "";
                textBox_Hovaten.Text = "";
                textBox_Noisinh.Text = "";
                textBox_Sodienthoai.Text = "";
                textBox_Email.Text = "";
                textBox_Taikhoan.Text = "";
                textBox_Phanquyen.Text = "";
                textBox_Gioitinh.Text = "";
                textBox_Ngaysinh.Text = "";

            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Masonhanvien.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Hovaten.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox_Ngaysinh.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Noisinh.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox_Gioitinh.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

            // Chuyển đổi giá trị Sodienthoai từ object sang int
            int sodienthoai;
            if (int.TryParse(dataGridView1.CurrentRow.Cells[6].Value.ToString(), out sodienthoai))
            {
                textBox_Sodienthoai.Text = sodienthoai.ToString();
            }
            else
            {
                // Xử lý nếu giá trị không phải là kiểu int
                MessageBox.Show("Giá trị số điện thoại không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_Sodienthoai.Text = string.Empty;
            }

            textBox_Email.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox_Taikhoan.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            textBox_Phanquyen.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
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
                Trangchu trc = new Trangchu();
                this.Hide();
                trc.Show();
            }
            else { }
        }
    }
}
