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
    public partial class Dieukienmatkhau2 : Form
    {
        public Dieukienmatkhau2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Đóng form hiện tại (Dieukienmatkhau)
            this.Close();

            // Tạo và hiển thị form CapnhatMatKhau
           Nhanvien NV = new Nhanvien();
           NV.Show();
        }
    }
}
