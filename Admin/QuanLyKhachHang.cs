using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using QL_GiayTT.Class;

namespace QL_GiayTT.Admin
{
    public partial class frmQuanLyKhachHang : Form
    {

        OracleConnection connsql;
        OracleDataAdapter data;
        DataTable tb_KhachHang;
        public frmQuanLyKhachHang()
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
        }

        private void loadCBO_GioiTinh()
        {
            string[] gioiTinh = { "Nam", "N?" };
            cboGioiTinh.DataSource = gioiTinh;
        }

        private void loadDGV_KhachHang()
        {
            tb_KhachHang = new DataTable();
            string selectStr = "Select * from KHACHHANG";
            data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(tb_KhachHang);
            int count = tb_KhachHang.Rows.Count;
            DataColumn[] key = new DataColumn[1];
            key[0] = tb_KhachHang.Columns["MaKH"];
            tb_KhachHang.PrimaryKey = key;

            dtGV_KhachHang.Columns["MaKH"].DataPropertyName = "MaKH";
            dtGV_KhachHang.Columns["TenKH"].DataPropertyName = "TenKH";
            dtGV_KhachHang.Columns["GioiTinhKH"].DataPropertyName = "GioiTinhKH";
            dtGV_KhachHang.Columns["NamSinhKH"].DataPropertyName = "NamSinhKH";
            dtGV_KhachHang.Columns["SDTKH"].DataPropertyName = "SDTKH";
            dtGV_KhachHang.Columns["DiaChiKH"].DataPropertyName = "DiaChiKH";

            dtGV_KhachHang.DataSource = tb_KhachHang;
        }

        void dataBingDing(DataTable pDT)
        {
            txtMaKH.DataBindings.Clear();
            txtTenKH.DataBindings.Clear();
            cboGioiTinh.DataBindings.Clear();
            txtNamSinh.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();

            txtMaKH.DataBindings.Add("Text", pDT, "MaKH");
            txtTenKH.DataBindings.Add("Text", pDT, "TenKH");
            cboGioiTinh.DataBindings.Add("Text", pDT, "GioiTinhKH");
            txtNamSinh.DataBindings.Add("Text", pDT, "NamSinhKH");
            txtSDT.DataBindings.Add("Text", pDT, "SDTKH");
            txtDiaChi.DataBindings.Add("Text", pDT, "DiaChiKH");
        }

        private void ngatDataBingDing()
        {
            txtMaKH.DataBindings.Clear();
            txtTenKH.DataBindings.Clear();
            cboGioiTinh.DataBindings.Clear();
            txtNamSinh.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
        }

        private void khoaTextBox()
        {
            foreach (Control item in gB_TTKH.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox))
                    item.Enabled = false;
            }
        }

        private void moTextBox()
        {
            foreach (Control item in gB_TTKH.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox))
                    item.Enabled = true;
            }
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            loadCBO_GioiTinh();
            loadDGV_KhachHang();
            dataBingDing(tb_KhachHang);

            dtGV_KhachHang.ReadOnly = true;
            dtGV_KhachHang.AllowUserToAddRows = false;
        }

        private void xoaThongTinNV()
        {
            foreach (Control item in gB_TTKH.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox))
                    item.Text = "";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            xoaThongTinNV();

            ngatDataBingDing();

            btnLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            moTextBox();

            txtMaKH.Enabled = false;

            dtGV_KhachHang.FirstDisplayedScrollingRowIndex = dtGV_KhachHang.Rows.Count - 1; 
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            ngatDataBingDing();

            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;

            moTextBox();

            //format khi click chu?t vào s? t? d?ng n?m bên ph?i
            foreach (Control item in gB_TTKH.Controls)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox)item;
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                }
            }

            txtMaKH.Enabled = false;
        }

        private void resetDuLieu()
        {
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            
            khoaTextBox();

            dataBingDing(tb_KhachHang);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetDuLieu();
        }

        private void openSql()
        {
            if (connsql.State == ConnectionState.Closed)
                connsql.Open();
        }

        private void closeSql()
        {
            if (connsql.State == ConnectionState.Open)
                connsql.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Luu thay d?i khách hàng ch??", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.No)
                return;
            else
            {
                try
                {
                    if (txtMaKH.Text == string.Empty) //Them
                    {
                        if (txtTenKH.Text == string.Empty)
                        {
                            MessageBox.Show("B?n ph?i nh?p Tên khách hàng!!!");
                            return;
                        }
                        openSql();
                        string insertStr = "INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH) VALUES (:TenKH, :SDTKH, :DiaChiKH, :GioiTinhKH, :NamSinhKH)";
                        OracleCommand cmdInsert = new OracleCommand(insertStr, connsql);
                        cmdInsert.Parameters.Add(":TenKH", OracleDbType.Varchar2).Value = txtTenKH.Text;
                        cmdInsert.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = txtSDT.Text;
                        cmdInsert.Parameters.Add(":DiaChiKH", OracleDbType.Varchar2).Value = txtDiaChi.Text;
                        cmdInsert.Parameters.Add(":GioiTinhKH", OracleDbType.Varchar2).Value = cboGioiTinh.Text;
                        if (string.IsNullOrWhiteSpace(txtNamSinh.Text))
                            cmdInsert.Parameters.Add(":NamSinhKH", OracleDbType.Varchar2).Value = DBNull.Value;
                        else
                            cmdInsert.Parameters.Add(":NamSinhKH", OracleDbType.Varchar2).Value = txtNamSinh.Text;
                        cmdInsert.ExecuteNonQuery();
                        closeSql();
                    }
                    else    //Sua
                    {
                        openSql();
                        string updateStr = "UPDATE KHACHHANG SET TenKH = :TenKH, SDTKH = :SDTKH, DiaChiKH = :DiaChiKH, GioiTinhKH = :GioiTinhKH, NamSinhKH = :NamSinhKH WHERE MaKH = :MaKH";
                        OracleCommand cmdUpdate = new OracleCommand(updateStr, connsql);
                        cmdUpdate.Parameters.Add(":TenKH", OracleDbType.Varchar2).Value = txtTenKH.Text;
                        cmdUpdate.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = txtSDT.Text;
                        cmdUpdate.Parameters.Add(":DiaChiKH", OracleDbType.Varchar2).Value = txtDiaChi.Text;
                        cmdUpdate.Parameters.Add(":GioiTinhKH", OracleDbType.Varchar2).Value = cboGioiTinh.Text;
                        if (string.IsNullOrWhiteSpace(txtNamSinh.Text))
                            cmdUpdate.Parameters.Add(":NamSinhKH", OracleDbType.Varchar2).Value = DBNull.Value;
                        else
                            cmdUpdate.Parameters.Add(":NamSinhKH", OracleDbType.Varchar2).Value = txtNamSinh.Text;
                        cmdUpdate.Parameters.Add(":MaKH", OracleDbType.Varchar2).Value = txtMaKH.Text;
                        cmdUpdate.ExecuteNonQuery();
                        closeSql();
                    }
                    MessageBox.Show("Thành công");
                    loadDGV_KhachHang();
                    resetDuLieu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    closeSql();
                }
            }
        }
        private void txtNamSinh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("B?n có mu?n xoá khách hàng này ch??", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                try
                {
                    openSql();
                    string deleteStr = "DELETE FROM KHACHHANG WHERE MaKH = :MaKH";
                    OracleCommand cmdDelete = new OracleCommand(deleteStr, connsql);
                    cmdDelete.Parameters.Add(":MaKH", OracleDbType.Varchar2).Value = txtMaKH.Text;
                    cmdDelete.ExecuteNonQuery();
                    tb_KhachHang.AcceptChanges();
                    closeSql();
                    MessageBox.Show("Thành công");
                    loadDGV_KhachHang();
                    dataBingDing(tb_KhachHang);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    closeSql();
                }
            }
        }
    }
}
