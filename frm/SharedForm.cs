using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_GiayTT;
using QL_GiayTT.Class;
using QL_GiayTT.frm;
using QL_GiayTT.Admin;

namespace QL_GiayTT.frm
{
    public partial class SharedForm : Form
    {
        private int checkQuyen;
        public SharedForm()
        {
            InitializeComponent();
        }

        public SharedForm(int quyen)
        {
            InitializeComponent();
            checkQuyen = quyen;
            if (checkQuyen == 2)
            {
                btnSanPham.Enabled = false;
                btnNhanVien.Enabled = false;
                btnKhachHang.Enabled = false;
            }
        }

        private void addUserControl(Form uc)
        {
            uc.TopLevel = false;
            uc.Dock = DockStyle.Fill;
            panelConTrols.Controls.Clear();
            panelConTrols.Controls.Add(uc);
            uc.Show(); // Hiển thị UserControl
            uc.BringToFront();
        }

        private void addTabControl(Form uc, string tagName)
        {
            TabControl tabControl = this.tabControl;
            uc.TopLevel = false;
            uc.Dock = DockStyle.Fill;

            TabPage tabPage = new TabPage(tagName);
            tabPage.Controls.Add(uc);
            tabControl.TabPages.Add(tabPage);
            uc.Show();

            tabControl.SelectedTab = tabPage; // Hiển thị Tab mới được thêm
        }

        int soHD = 1000;
        private void btnBanHang_Click(object sender, EventArgs e)
        {
            frmBanHang uc = new frmBanHang();
            //addUserControl(uc);
            string maHDCTT = "#SHD" + soHD.ToString();
            addTabControl(uc, maHDCTT);
            soHD++;
        }
        private Form kiemTraFormTonTai(string tagName)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Text == tagName && tabPage.Controls.Count > 0)
                {
                    Form fromTonTan = tabPage.Controls[0] as Form;
                    if (fromTonTan != null)
                    {
                        return fromTonTan;
                    }
                }
            }

            return null;
        }
        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            frmQuanLyHoaDon uc = new frmQuanLyHoaDon(checkQuyen);
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            TrangChu uc = new TrangChu();
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }
        //private bool IsTabPageExists(TabControl tabControl, TabPage tabPage)
        //{
        //    return tabControl.TabPages.Contains(tabPage);
        //}
        //private void btnDongTab_Click(object sender, EventArgs e)
        //{
        //    TabPage tab_HienTai = tabControl.SelectedTab;
        //    tabControl.TabPages.Remove(tab_HienTai);
        //    if (tabControl.TabPages.Count == 0)
        //    {
        //        TrangChu uc = new TrangChu();
        //        addTabControl(uc, uc.Text);
        //    }
        //}

        private void SharedForm_Load(object sender, EventArgs e)
        {
            TrangChu uc = new TrangChu();
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            frmQuanLyNhanVien uc = new frmQuanLyNhanVien();
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            frmQuanLySanPham uc = new frmQuanLySanPham();
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }


        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            frmQuanLyKhachHang uc = new frmQuanLyKhachHang();
            string tagName = uc.Text;
            Form fromTonTan = kiemTraFormTonTai(tagName);

            if (fromTonTan == null)
                addTabControl(uc, tagName);
            else
            {
                uc.Close();
                tabControl.SelectedTab = fromTonTan.Parent as TabPage;
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có muốn đăng xuất chứ?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void btnDongTab_Click(object sender, EventArgs e)
        {
            TabPage tab_HienTai = tabControl.SelectedTab;
            tabControl.TabPages.Remove(tab_HienTai);
            if (tabControl.TabPages.Count == 0)
            {
                TrangChu uc = new TrangChu();
                addTabControl(uc, uc.Text);
            }
        }


    }
}