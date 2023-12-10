using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Xoa__sua_the_thu_vien : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Xoa__sua_the_thu_vien()
        {
            InitializeComponent();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

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
                textBox_Ngaybatdau.Clear();
                textBox_Ngayhethan.Clear();
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

        int IDThethuvien;
        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về thẻ thư viện sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Thethuvien WHERE  IDThethuvien = @IDThethuvien";
                cmd.Parameters.AddWithValue("IDThethuvien", label2.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Sothethuvien.Clear();
                textBox_Ghichu.Clear();
                textBox_Ngaybatdau.Clear();
                textBox_Ngayhethan.Clear();

            }

        }
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Thethuvien order by IDThethuvien desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
           
        }

        private void button_Sua_Click(object sender, EventArgs e)
        {

            using (MemoryStream ms = new MemoryStream())
            {


                if (MessageBox.Show("Thông tin về thẻ thư viện sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (textBox_Sothethuvien.Text != "" &&
                        textBox_Ngaybatdau.Text != "" &&
                        textBox_Ngayhethan.Text != "" &&
                        textBox_Ghichu.Text != "")
                    {
                        string Sothethuvien = textBox_Sothethuvien.Text;
                        string Ngaybatdau = textBox_Ngaybatdau.Text;
                        string Ngayhethan = textBox_Ngayhethan.Text;
                        string Ghichu = textBox_Ghichu.Text;

                        using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                        {
                            con.Open();
                            string query = "UPDATE Thethuvien SET Sothethuvien =@Sothethuvien,Ngaybatdau = @Ngaybatdau, Ngayhethan= @Ngayhethan,Ghichu=@Ghichu WHERE IDThethuvien = @IDThethuvien";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;

                                cmd.Parameters.AddWithValue("@IDThethuvien", label2.Text);
                                cmd.Parameters.AddWithValue("@Sothethuvien", textBox_Sothethuvien.Text);
                                cmd.Parameters.AddWithValue("@Ngaybatdau", textBox_Ngaybatdau.Text);
                                cmd.Parameters.AddWithValue("@Ngayhethan", textBox_Ngayhethan.Text);
                                cmd.Parameters.AddWithValue("@Ghichu", textBox_Ghichu.Text);

                                cmd.ExecuteNonQuery();
                                con.Close();
                                load_data();

                                textBox_Sothethuvien.Text = "";
                                textBox_Ngaybatdau.Text = "";
                                textBox_Ngayhethan.Text = "";
                                textBox_Ghichu.Text = "";
                            }
                        }

                        MessageBox.Show("Thông tin về thẻ thư viện đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

               
            }
        }

        private void Xoa__sua_the_thu_vien_Load(object sender, EventArgs e)
        {
            load_data();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Sothethuvien.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Ngaybatdau.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox_Ngayhethan.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Ghichu.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
          
        }

        private void textBox_Timkiem_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Timkiem.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Thethuvien where Sothethuvien LIKE '" + textBox_Timkiem.Text + "%'";
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

                cmd.CommandText = "SELECT * FROM Thethuvien WHERE IDThethuvien = " + IDThethuvien + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
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
                Trangchu trc = new Trangchu();
                this.Hide();
                trc.Show();
            }
            else { }
        }
    }
}
