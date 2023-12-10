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
    public partial class Dieukienmatkhau : Form
    {
        public Dieukienmatkhau()
        {
            InitializeComponent();
        }

        private void Dieukienmatkhau_Load(object sender, EventArgs e)
        {
            // Đặt vị trí xuất hiện của form vào giữa màn hình
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Đóng form hiện tại (Dieukienmatkhau)
            this.Close();

            // Tạo và hiển thị form CapnhatMatKhau
            CapnhatMatKhau capnhatMatKhauForm = new CapnhatMatKhau();
            capnhatMatKhauForm.Show();
        }
    }
    }

