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

namespace NMCNPM_CuoiKy
{
    public partial class Theloai : Form
    {
        public Theloai()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();

        private void button6_Click(object sender, EventArgs e)
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
    

        private void button2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Thông tin về thể loại sách sẽ được thêm. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
    {
                if (textBox_Matheloaisach.Text != "" &&
                    textBox_Tentheloaisach.Text != "")
                {
                    string MaTheloai = textBox_Matheloaisach.Text;
                    string TenTheloai = textBox_Tentheloaisach.Text;

                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();

                        // Kiểm tra sự tồn tại của MaTheloai trong cơ sở dữ liệu
                        string checkQuery = "SELECT COUNT(*) FROM Theloai WHERE MaTheloai = @MaTheloai";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                        {
                            checkCmd.Parameters.AddWithValue("@MaTheloai", MaTheloai);
                            int existingRecordsCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (existingRecordsCount > 0)
                            {
                                MessageBox.Show("Mã thể loại sách " + MaTheloai + " đã tồn tại! Vui lòng chọn mã khác.", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Không thực hiện thêm mới nếu mã đã tồn tại
                            }
                        }

                        // Nếu không có mã trùng, thực hiện thêm mới
                        string insertQuery = "INSERT INTO Theloai (MaTheloai, TenTheloai) VALUES (@MaTheloai, @TenTheloai)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@MaTheloai", MaTheloai);
                            cmd.Parameters.AddWithValue("@TenTheloai", TenTheloai);

                            cmd.ExecuteNonQuery();
                            load_data();
                        }

                        MessageBox.Show("Thông tin về thể loại sách đã được lưu lại", "Đã lưu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox_Matheloaisach.Text = "";
                        textBox_Tentheloaisach.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về thể loại sách sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Theloai WHERE  IDTheloai = @IDTheloai";
                cmd.Parameters.AddWithValue("IDTheloai", label2.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Matheloaisach.Text = "";
                textBox_Tentheloaisach.Text = "";
            }
        }
        int IDTheloai;
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Theloai order by IDTheloai desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void textBox_timkiem_TextChanged(object sender, EventArgs e)
        {
            if (textBox_timkiem.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Theloai where MaTheloai LIKE '" + textBox_timkiem.Text + "%'";
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

                cmd.CommandText = "SELECT * FROM Theloai WHERE IDTheloai = " + IDTheloai + "";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

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

                textBox_Matheloaisach.Text = "";
                textBox_Tentheloaisach.Text = "";
            }
        }

        private void button_Sua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về thể loại sách sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (textBox_Matheloaisach.Text != "" &&
                    textBox_Tentheloaisach.Text != "")
                {
                    string MaTheloai = textBox_Matheloaisach.Text;
                    string TenTheloai = textBox_Tentheloaisach.Text;  
               


                    using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "UPDATE Theloai SET MaTheloai = @MaTheloai, TenTheloai = @TenTheloai WHERE IDTheloai = @IDTheloai";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@IDTheloai", label2.Text);
                            cmd.Parameters.AddWithValue("@MaTheloai", MaTheloai);
                            cmd.Parameters.AddWithValue("@TenTheloai", TenTheloai);



                            cmd.ExecuteNonQuery();
                            con.Close();
                            load_data();

                            textBox_Matheloaisach.Text = "";
                            textBox_Tentheloaisach.Text = "";
                        }
                    }


                    MessageBox.Show("Thông tin về thể loại sách đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
           label2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
           textBox_Matheloaisach.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
           textBox_Tentheloaisach.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        
        }

        private void Theloai_Load(object sender, EventArgs e)
        {
            load_data();
        }
    }
}
