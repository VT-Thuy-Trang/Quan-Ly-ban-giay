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
    public partial class frmQuanLyHoaDon : Form
    {
        OracleConnection connsql;
        OracleDataAdapter data;
        DataSet QL_ShopQuanAo = new DataSet();
        public frmQuanLyHoaDon()
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
        }

        public frmQuanLyHoaDon(int checkQuyen)
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
            if (checkQuyen == 2)
            {
                btnXoa.Enabled = false;
            }
        }

        private void OpenConnection()
        {
            if (connsql.State == ConnectionState.Closed)
                connsql.Open();
        }

        private void CloseConnection()
        {
            if (connsql.State == ConnectionState.Open)
                connsql.Close();
        }

        public void load_DtGV_HoaDon()
        {
            string selectStr = "select * from HOADON";
            data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(QL_ShopQuanAo, "HOADON");

            DataColumn[] key = new DataColumn[1];
            key[0] = QL_ShopQuanAo.Tables["HOADON"].Columns["MaHD"];
            QL_ShopQuanAo.Tables["HOADON"].PrimaryKey = key;

            dtGV_HoaDon.Columns["MaHD"].DataPropertyName = "MaHD";
            dtGV_HoaDon.Columns["MaNV"].DataPropertyName = "MaNV";
            dtGV_HoaDon.Columns["MaKH"].DataPropertyName = "MaKH";
            dtGV_HoaDon.Columns["NgayBan"].DataPropertyName = "NgayBan";
            dtGV_HoaDon.Columns["TongThanhToan"].DataPropertyName = "TongThanhToan";
            dtGV_HoaDon.Columns["TrangThai"].DataPropertyName = "TrangThai";
            dtGV_HoaDon.Columns["HinhThucThanhToan"].DataPropertyName = "HinhThucThanhToan";
            dtGV_HoaDon.Columns["GhiChu"].DataPropertyName = "GhiChu";

            dtGV_HoaDon.DataSource = QL_ShopQuanAo.Tables["HOADON"];
        }

        public void load_DtGV_CTHD()
        {
            string selectStr = "select * from CTHD";
            data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(QL_ShopQuanAo, "CTHD");

            DataColumn[] key = new DataColumn[3];
            key[0] = QL_ShopQuanAo.Tables["CTHD"].Columns["MaHD"];
            key[1] = QL_ShopQuanAo.Tables["CTHD"].Columns["MaSP"];
            key[2] = QL_ShopQuanAo.Tables["CTHD"].Columns["KichCo"];
            QL_ShopQuanAo.Tables["CTHD"].PrimaryKey = key;

            dtGV_CTHD.Columns["MaHD_CTHD"].DataPropertyName = "MaHD";
            dtGV_CTHD.Columns["MaSP"].DataPropertyName = "MaSP";
            dtGV_CTHD.Columns["KichCo"].DataPropertyName = "KichCo";
            dtGV_CTHD.Columns["SoLuong"].DataPropertyName = "SoLuong";
            dtGV_CTHD.Columns["GiaBan"].DataPropertyName = "GiaBan";
            dtGV_CTHD.Columns["ThanhTien"].DataPropertyName = "ThanhTien";

            dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];
        }

        private void tinhTongDoanhThu()
        {
            double tongDT  = 0;
            double tongTienMat = 0;
            double tongChuyenKhoang = 0;
            double tongTT;
            string trangThai;
            string htThanhToan;
            foreach (DataGridViewRow row in dtGV_HoaDon.Rows)
            {
                if (row.Cells["TongThanhToan"].Value != null)
                {
                    trangThai = row.Cells["TrangThai"].Value.ToString();
                    htThanhToan = row.Cells["HinhThucThanhToan"].Value.ToString();
                    if (string.Compare(trangThai, "Ðã thanh toán", true) == 0 && string.Compare(htThanhToan, "Tiền mặt", true) == 0)
                    {
                        tongTT = double.Parse(row.Cells["TongThanhToan"].Value.ToString());
                        tongDT += tongTT;
                        tongTienMat += tongTT;
                    }
                    else if (string.Compare(trangThai, "Ðã thanh toán", true) == 0 && string.Compare(htThanhToan, "Chuy?n kho?n", true) == 0)
                    {
                        tongTT = double.Parse(row.Cells["TongThanhToan"].Value.ToString());
                        tongDT += tongTT;
                        tongChuyenKhoang += tongTT;
                    }
                }
            }
            lb_TongDoanhThu.Text = tongDT.ToString("N0") + "d";
            lb_TM.Text = tongTienMat.ToString("N0") + "d";
            lb_CK.Text = tongChuyenKhoang.ToString("N0") + "d";
        }

        private void load_Cbo_TrangThai()
        {
            string[] listTrangThai = { "--Trạng thái hóa đơn--", "Đã thanh toán", "Chưa thanh toán", "Đặt cọc", "Hủy hóa đơn" };
            cbo_TrangThai.DataSource = listTrangThai;
            cbo_TrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void frmQuanLyHoaDon_Load(object sender, EventArgs e)
        {
            load_DtGV_HoaDon();
            load_Cbo_TrangThai();

            dtGV_HoaDon.ReadOnly = true;
            dtGV_CTHD.ReadOnly = true;

            dtGV_HoaDon.AllowUserToAddRows = false;
            dtGV_CTHD.AllowUserToAddRows = false;

            tinhTongDoanhThu();
            load_DtGV_CTHD();
        }

        private void loadDGV_CTHD(string maHD)
        {
            if (maHD != string.Empty)
            {
                DataTable dtTable = QL_ShopQuanAo.Tables["CTHD"];
                DataRow[] tableTheoMa = dtTable.Select("MaHD = '" + maHD + "'");
                if (tableTheoMa.Length == 0)
                {
                    dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];
                    return;
                }
                DataTable TableCopy = tableTheoMa.CopyToDataTable();
                dtGV_CTHD.DataSource = TableCopy;
            }
            else load_DtGV_HoaDon();
        }

        private void dtGV_HoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtGV_HoaDon.Rows[e.RowIndex];

                string maTK = selectedRow.Cells["MaHD"].Value.ToString();
                loadDGV_CTHD(maTK.Trim());
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("B?n có Hu? hoá donô đang được chọn ch??", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                if (dtGV_HoaDon.SelectedCells.Count > 0)
                {
                    try
                    {
                        // L?y ô d?u tiên du?c ch?n
                        DataGridViewCell selectedCell = dtGV_HoaDon.SelectedCells[0];

                        // L?y dòng tuong ?ng v?i ch? s? dòng
                        DataGridViewRow selectedRow = dtGV_HoaDon.Rows[selectedCell.RowIndex];

                        string maHD = selectedRow.Cells["MaHD"].Value.ToString();
                        DataRow dataRow = QL_ShopQuanAo.Tables["HOADON"].Rows.Find(maHD);

                        if (dataRow != null)
                        {
                            dataRow["TrangThai"] = "Hu? hoá don";

                            OpenConnection();
                            OracleCommand command = new OracleCommand("UPDATE HOADON SET TrangThai = :TrangThai WHERE MaHD = :MaHD", connsql);
                            command.Parameters.Add(":TrangThai", OracleDbType.Varchar2).Value = "Hu? hoá don";
                            command.Parameters.Add(":MaHD", OracleDbType.Varchar2).Value = maHD;
                            command.ExecuteNonQuery();

                            MessageBox.Show("Thành công");
                            CloseConnection();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("L?i: " + ex.Message);
                        CloseConnection();
                    }

                }
                else MessageBox.Show("Th?t b?i");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("B?n có Xoá hoá don cùng nh?ng chi ti?t c?a hoá donô đang được chọn ch??", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                if (dtGV_HoaDon.SelectedCells.Count > 0)
                {
                    try
                    {
                        // L?y ô d?u tiên du?c ch?n
                        DataGridViewCell selectedCell = dtGV_HoaDon.SelectedCells[0];

                        // L?y dòng tuong ?ng v?i ch? s? dòng
                        DataGridViewRow selectedRow = dtGV_HoaDon.Rows[selectedCell.RowIndex];

                        string maHD = selectedRow.Cells["MaHD"].Value.ToString();
                        //Xoá chi ti?t hoá don tru?c
                        OpenConnection();
                        OracleCommand command = new OracleCommand("DELETE FROM CTHD WHERE MaHD = :MaHD", connsql);
                        command.Parameters.Add(":MaHD", OracleDbType.Varchar2).Value = maHD;
                        command.ExecuteNonQuery();
                        // C?p nh?t l?i dtGV_CTHD
                        DataRow[] dataRow = QL_ShopQuanAo.Tables["CTHD"].Select("MaHD = '" + maHD + "'");
                        foreach (DataRow deleteRow in dataRow)
                        {
                            deleteRow.Delete();
                        }
                        QL_ShopQuanAo.Tables["CTHD"].AcceptChanges();
                        dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];

                        //Xoá Hoá don
                        command = new OracleCommand("DELETE FROM HOADON WHERE MaHD = :MaHD", connsql);
                        command.Parameters.Add(":MaHD", OracleDbType.Varchar2).Value = maHD;
                        command.ExecuteNonQuery();
                        QL_ShopQuanAo.Tables["HOADON"].Rows.Find(maHD).Delete();
                        QL_ShopQuanAo.Tables["HOADON"].AcceptChanges();
                        CloseConnection();
                        MessageBox.Show("Thành công");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("L?i: " + ex.Message);
                        CloseConnection();
                    }

                }
                else MessageBox.Show("Th?t b?i");
            }
        }

        private void btnResetDuLieu_Click(object sender, EventArgs e)
        {
            dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

        }

        private void loadDGV_HoaDon(string trangThai)
        {
                DataTable dtTable = QL_ShopQuanAo.Tables["HOADON"];
                //string findStr = string.Format("MaHD = '{0}' AND TrangThai = '{1}'", maHD, trangThai);
                DataRow[] tableTheoMa = dtTable.Select("TrangThai = '" + trangThai + "'");
                if (tableTheoMa.Length == 0)
                {
                    dtGV_HoaDon.DataSource = dtTable.Clone();
                    return;
                }
                DataTable TableCopy = tableTheoMa.CopyToDataTable();
                dtGV_HoaDon.DataSource = TableCopy;
        }

        private void cbo_TrangThai_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbo_TrangThai.SelectedIndex == 0)
                dtGV_HoaDon.DataSource = QL_ShopQuanAo.Tables["HOADON"];
            else loadDGV_HoaDon(cbo_TrangThai.SelectedItem.ToString());
        }
    }
}
