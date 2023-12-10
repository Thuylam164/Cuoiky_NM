using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMCNPM_CuoiKy
{
    public partial class Trangchu : Form
    {
        public Trangchu()
        {
            InitializeComponent();
        }

        private void quảnLýTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

     

        private void Trangchu_Load(object sender, EventArgs e)
        {
            if (Dangnhap.UserRole == "Quản trị viên")
            {
                // Hiển thị tất cả chức năng cho quản trị viên
           
            }
            else if (Dangnhap.UserRole == "Người dùng")
            {
                // Ẩn chức năng quản lý tài khoản cho người dùng
               
                NhânVienToolStripMenuItem.Visible = false;
            }
        }

        private void đăgXuấtKhỏiTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận đăng xuất
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất khỏi tài khoản không?", "Đăng xuất khỏi tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Có", thực hiện đăng xuất và quay về trang "Dangnhap"
                Dangnhap dangnhapForm = new Dangnhap();
                dangnhapForm.Show();
                this.Hide();
            }
            // Nếu người dùng chọn "Không", không thực hiện bất kỳ hành động nào
        }

        private void thêmSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Them_Sach them = new Them_Sach();
            them.Show();
            this.Hide();
        }

        private void sửaXoáSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xoa_sua_sach sach = new Xoa_sua_sach();
            sach.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void xoáCậpNhậtThôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thêmTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Them_Tacgia them = new Them_Tacgia();
            them.Show();
            this.Hide();
        }

        private void sửaXoáTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xoa__sua_Tacgia them = new Xoa__sua_Tacgia();
            them.Show();
            this.Hide();
        }

        private void thêmNgườiĐọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Them_Nguoidoc them = new Them_Nguoidoc();
            them.Show();
            this.Hide();
        }

        private void xoáSửaThôngTinNgườiĐọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Xoa__sua_nguoi_doc them = new Xoa__sua_nguoi_doc();
            them.Show();
            this.Hide();
        }

        private void trảSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Muonsach them = new Muonsach();
            them.Show();
            this.Hide();
        }

        private void thêmNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nhanvien them = new Nhanvien();
            them.Show();
            this.Hide();
        }

        private void xóaSửaNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xoa_sua_Nhanvien them = new Xoa_sua_Nhanvien();
            them.Show();
            this.Hide();
        }

        private void thểLoạiSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Theloai them = new Theloai();
            them.Show();
            this.Hide();
        }

        private void nhàXuấtBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Nhaxuatban them = new Nhaxuatban();
            them.Show();
            this.Hide();
        }

        private void thêmTácGiảToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Them_Tacgia them = new Them_Tacgia();
            them.Show();
            this.Hide();
        }

        private void sửaXoáTácGiảToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
           Xoa__sua_Tacgia them = new Xoa__sua_Tacgia();
            them.Show();
            this.Hide();
        }

        private void thêmSáchToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Them_Sach them = new Them_Sach();
            them.Show();
            this.Hide();
        }

        private void sửaXoáSáchToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Xoa_sua_sach them = new Xoa_sua_sach();
            them.Show();
            this.Hide();
        }

        private void thêmThẻThưViệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Them_Thethuvien them = new Them_Thethuvien();
            them.Show();
            this.Hide();
        }

        private void xóaSửaThẻThưViệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xoa__sua_the_thu_vien them = new Xoa__sua_the_thu_vien();
            them.Show();
            this.Hide();
        }

        private void thêmNgườiĐọcToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Them_Nguoidoc them = new Them_Nguoidoc();
            them.Show();
            this.Hide();
        }

        private void xoáSửaThôngTinNgườiĐọcToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Xoa__sua_nguoi_doc them = new Xoa__sua_nguoi_doc();
            them.Show();
            this.Hide();
        }

        private void mượnSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Trasach them = new Trasach();
            them.Show();
            this.Hide();
        }

        private void mượnSáchQuáHạnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xemthongtintramuon them = new Xemthongtintramuon();
            them.Show();
            this.Hide();
        }

        private void tìmKiếmThôngTinNgườiĐọcTheoSốThẻThưViệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xemthongtinnguoidoctheosothethuviencs them = new Xemthongtinnguoidoctheosothethuviencs();
            them.Show();
            this.Hide();

        }
    }
}
