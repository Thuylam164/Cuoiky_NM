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
    public partial class Xoa__sua_Tacgia : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        public Xoa__sua_Tacgia()
        {
            InitializeComponent();
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
                // Đặt lại nội dung trong các
                textBox_Matacgia.Clear();
                textBox_Hovaten.Clear();
                textBox_Butdanh.Clear();
                textBox_Nghenghiep.Clear();
                textBox_Quequan.Clear();
                textBox_Theloaisangtac.Clear();
                Hinhanh.Image = null;
            }
        }
        private void load_data()
        {
            cmd = new SqlCommand("Select * from Tacgia order by IDTacgia desc", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            DataGridViewImageColumn Hinhanh = new DataGridViewImageColumn();
            Hinhanh = (DataGridViewImageColumn)dataGridView1.Columns[8];
            Hinhanh.ImageLayout = DataGridViewImageCellLayout.Zoom;
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


                    if (MessageBox.Show("Thông tin về tác giả sẽ được cập nhật. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (textBox_Hovaten.Text != "" &&
                            textBox_Matacgia.Text != "" &&
                            textBox_Butdanh.Text != "" &&
                            textBox_Ngaysinh.Text != "" &&
                            textBox_Nghenghiep.Text != "" &&
                            textBox_Quequan.Text != "" &&
                            textBox_Theloaisangtac.Text != "")
                        {
                            string Matacgia = textBox_Matacgia.Text;
                            string Hovaten = textBox_Hovaten.Text;
                            string Butdanh = textBox_Butdanh.Text;
                            string Ngaysinh = textBox_Ngaysinh.Text;
                            string Nghenghiep =  textBox_Nghenghiep.Text;
                            string Theloaisangtac = textBox_Theloaisangtac.Text;
                            string Quequan = textBox_Quequan.Text;

                          
                                using (SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True"))
                                {
                                    con.Open();
                                    string query = "UPDATE Tacgia SET MaTacgia =@MaTacgia, Hovaten = @Hovaten, Ngaysinh= @Ngaysinh,Butdanh =@Butdanh, Nghenghiep=@Nghenghiep, Quequan= @Quequan, Theloaisangtac =@Theloaisangtac, Hinhanh = @Hinhanh WHERE IDTacgia = @IDTacgia";
                                    using (SqlCommand cmd = new SqlCommand(query, con))
                                    {
                                        cmd.CommandType = CommandType.Text;

                                        cmd.Parameters.AddWithValue("@IDTacgia", label13.Text);
                                        cmd.Parameters.AddWithValue("@MaTacgia", textBox_Matacgia.Text);
                                        cmd.Parameters.AddWithValue("@Hovaten", textBox_Hovaten.Text);
                                        cmd.Parameters.AddWithValue("@Ngaysinh", textBox_Ngaysinh.Text);
                                        cmd.Parameters.AddWithValue("@Butdanh", textBox_Butdanh.Text);
                                        cmd.Parameters.AddWithValue("@Nghenghiep", textBox_Nghenghiep.Text);
                                        cmd.Parameters.AddWithValue("@Quequan", textBox_Quequan.Text);
                                        cmd.Parameters.AddWithValue("@Theloaisangtac", textBox_Theloaisangtac.Text);
                                 

                                        MemoryStream memstr = new MemoryStream();

                                        Hinhanh.Image.Save(memstr, Hinhanh.Image.RawFormat);
                                        cmd.Parameters.AddWithValue("Hinhanh", memstr.ToArray());
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        load_data();

                                    textBox_Ngaysinh.Clear();
                                    textBox_Matacgia.Clear();
                                    textBox_Hovaten.Clear();
                                    textBox_Butdanh.Clear();
                                    textBox_Nghenghiep.Clear();
                                    textBox_Quequan.Clear();
                                    textBox_Theloaisangtac.Clear();
                                    Hinhanh.Image = null;
                                }
                                }

                                MessageBox.Show("Thông tin về tác giả đã được cập nhật", "Đã cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           
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

        private void Xoa__sua_Tacgia_Load(object sender, EventArgs e)
        {
            load_data();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Sử dụng thuộc tính Cells để truy cập dữ liệu trong từng ô
            label13.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            textBox_Matacgia.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox_Hovaten.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox_Ngaysinh.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox_Butdanh.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox_Nghenghiep.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox_Quequan.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox_Theloaisangtac.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();

            MemoryStream ms = new MemoryStream((byte[])dataGridView1.CurrentRow.Cells[8].Value);
            Hinhanh.Image = Image.FromStream(ms);
        }

        private void button_Xoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thông tin về tác giả sẽ được xoá. Xác nhận?", "Thành công", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "DELETE FROM Tacgia WHERE  IDTacgia = @IDTacgia";
                cmd.Parameters.AddWithValue("IDTacgia", label13.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                load_data();

                textBox_Matacgia.Clear();
                textBox_Matacgia.Clear();
                textBox_Hovaten.Clear();
                textBox_Butdanh.Clear();
                textBox_Nghenghiep.Clear();
                textBox_Quequan.Clear();
                textBox_Theloaisangtac.Clear();
                Hinhanh.Image = null;

            }

        }
        int IDTacgia;
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

        private void textBox_Timkiembutdanh_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Timkiembutdanh.Text != "")
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NMCNPM;Integrated Security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM Tacgia where MaTacgia LIKE '" + textBox_Timkiembutdanh.Text + "%'";
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

                cmd.CommandText = "SELECT * FROM Tacgia WHERE IDTacgia = " +IDTacgia+ ""; 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

            }
        }
    }
}
