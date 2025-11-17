using QL_GiayTT.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_GiayTT; // Thêm để thấy TrangChu

namespace QL_GiayTT.frm
{
    public partial class frmDangNhap : Form
    {
        private KetNoi ketNoi;

        public frmDangNhap(KetNoi kn)
        {
            CenterToScreen();
            InitializeComponent();
            ketNoi = kn;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DangNhap dn = new DangNhap(ketNoi, txtTK.Text.Trim(), txtMK.Text.Trim());
            int kq = dn.KiemTraDangNhap();

            switch (kq)
            {
                case 1: // Admin
                case 2: // User
                    string chucVu = (kq == 1) ? "Admin" : "User";
                    MessageBox.Show($"Đăng nhập {chucVu} thành công!");

                    // --- ĐÂY LÀ PHẦN QUAN TRỌNG ---
                    this.Hide(); // Ẩn form đăng nhập

                    OracleSession.Initialize(ketNoi);

                    SharedForm sharedForm = new SharedForm(kq);
                    sharedForm.StartPosition = FormStartPosition.CenterScreen;
                    sharedForm.ShowDialog();

                    this.Close();
                    // --- KẾT THÚC PHẦN QUAN TRỌNG ---
                    break;

                case 3:
                    MessageBox.Show("Sai mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMK.Focus();
                    txtMK.SelectAll();
                    break;

                case 0:
                    MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTK.Focus();
                    txtTK.SelectAll();
                    break;

                default:
                    MessageBox.Show("Vui lòng nhập đầy đủ!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void chk_HienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_HienMK.Checked == true)
                txtMK.PasswordChar = '\0';
            else txtMK.PasswordChar = '*';
        }
    }
}