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
            // Xóa dữ liệu cũ trước khi load lại
            if (QL_ShopQuanAo.Tables.Contains("HOADON"))
            {
                QL_ShopQuanAo.Tables["HOADON"].Clear();
            }
            
            string selectStr = "select * from HOADON ORDER BY MaHD DESC";
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
            // Xóa dữ liệu cũ trước khi load lại
            if (QL_ShopQuanAo.Tables.Contains("CTHD"))
            {
                QL_ShopQuanAo.Tables["CTHD"].Clear();
            }
            
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
            if (maHD != string.Empty && !string.IsNullOrWhiteSpace(maHD))
            {
                // Đảm bảo DataTable CTHD đã được load
                if (!QL_ShopQuanAo.Tables.Contains("CTHD") || QL_ShopQuanAo.Tables["CTHD"].Rows.Count == 0)
                {
                    // Reload lại từ database nếu chưa có dữ liệu
                    load_DtGV_CTHD();
                }
                
                DataTable dtTable = QL_ShopQuanAo.Tables["CTHD"];
                
                // MaHD là Int32, cần convert để so sánh đúng
                if (int.TryParse(maHD, out int maHDInt))
                {
                    // Sử dụng LINQ để filter an toàn hơn
                    // Kiểm tra kiểu dữ liệu của MaHD trong DataTable
                    var filteredRows = dtTable.AsEnumerable()
                        .Where(row => {
                            object maHDValue = row["MaHD"];
                            if (maHDValue == null || maHDValue == DBNull.Value)
                                return false;
                            
                            // Chuyển đổi sang int để so sánh
                            int rowMaHD = Convert.ToInt32(maHDValue);
                            return rowMaHD == maHDInt;
                        })
                        .ToArray();
                    
                    if (filteredRows.Length == 0)
                    {
                        // Tạo DataTable rỗng với cùng cấu trúc
                        DataTable emptyTable = dtTable.Clone();
                        dtGV_CTHD.DataSource = emptyTable;
                        return;
                    }
                    
                    // Copy các dòng tìm được vào DataTable mới
                    DataTable TableCopy = dtTable.Clone();
                    foreach (var row in filteredRows)
                    {
                        TableCopy.ImportRow(row);
                    }
                    dtGV_CTHD.DataSource = TableCopy;
                }
                else
                {
                    // Nếu không parse được, hiển thị bảng rỗng
                    DataTable emptyTable = dtTable.Clone();
                    dtGV_CTHD.DataSource = emptyTable;
                }
            }
            else
            {
                // Hiển thị tất cả chi tiết hóa đơn
                if (!QL_ShopQuanAo.Tables.Contains("CTHD"))
                {
                    load_DtGV_CTHD();
                }
                dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];
            }
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
            DialogResult r = MessageBox.Show("Bạn có muốn xóa hóa đơn cùng những chi tiết của hóa đơn đang được chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                if (dtGV_HoaDon.SelectedCells.Count > 0)
                {
                    try
                    {
                        // Lấy ô đầu tiên được chọn
                        DataGridViewCell selectedCell = dtGV_HoaDon.SelectedCells[0];

                        // Lấy dòng tương ứng với chỉ số dòng
                        DataGridViewRow selectedRow = dtGV_HoaDon.Rows[selectedCell.RowIndex];

                        if (selectedRow.Cells["MaHD"].Value == null)
                        {
                            MessageBox.Show("Không thể lấy mã hóa đơn!");
                            return;
                        }

                        string maHD = selectedRow.Cells["MaHD"].Value.ToString();
                        
                        // Chuyển đổi MaHD sang Int32 để xóa
                        if (!int.TryParse(maHD, out int maHDInt))
                        {
                            MessageBox.Show("Mã hóa đơn không hợp lệ!");
                            return;
                        }

                        OpenConnection();
                        
                        // Sử dụng transaction để đảm bảo tính nhất quán
                        OracleTransaction transaction = connsql.BeginTransaction();
                        
                        try
                        {
                            // Xóa chi tiết hóa đơn trước
                            OracleCommand command = new OracleCommand("DELETE FROM CTHD WHERE MaHD = :MaHD", connsql);
                            command.Transaction = transaction;
                            command.Parameters.Add(":MaHD", OracleDbType.Int32).Value = maHDInt;
                            int rowsDeletedCTHD = command.ExecuteNonQuery();
                            
                            // Xóa Hóa đơn
                            command = new OracleCommand("DELETE FROM HOADON WHERE MaHD = :MaHD", connsql);
                            command.Transaction = transaction;
                            command.Parameters.Add(":MaHD", OracleDbType.Int32).Value = maHDInt;
                            int rowsDeletedHD = command.ExecuteNonQuery();
                            
                            if (rowsDeletedHD == 0)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Không tìm thấy hóa đơn cần xóa!");
                                CloseConnection();
                                return;
                            }
                            
                            // Commit transaction
                            transaction.Commit();
                            CloseConnection();
                            
                            // Reload lại dữ liệu từ database để đảm bảo đồng bộ
                            load_DtGV_HoaDon();
                            load_DtGV_CTHD();
                            
                            // Xóa chi tiết hóa đơn đang hiển thị (nếu đang hiển thị hóa đơn đã xóa)
                            DataTable emptyTable = QL_ShopQuanAo.Tables["CTHD"].Clone();
                            dtGV_CTHD.DataSource = emptyTable;
                            
                            // Cập nhật lại tổng doanh thu
                            tinhTongDoanhThu();
                            
                            MessageBox.Show("Xóa hóa đơn thành công!");
                        }
                        catch (Exception exTrans)
                        {
                            // Rollback nếu có lỗi
                            transaction.Rollback();
                            throw exTrans;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                        CloseConnection();
                    }

                }
                else MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Hủy chế độ chỉnh sửa
            dtGV_HoaDon.ReadOnly = true;
            btnLuu.Enabled = false;
            btnSua.Enabled = true;

            // Reload lại dữ liệu từ database để hủy các thay đổi chưa lưu
            load_DtGV_HoaDon();
            tinhTongDoanhThu();

            MessageBox.Show("Đã hủy chế độ chỉnh sửa và reload lại dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetDuLieu_Click(object sender, EventArgs e)
        {
            dtGV_CTHD.DataSource = QL_ShopQuanAo.Tables["CTHD"];
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dtGV_HoaDon.SelectedCells.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dòng được chọn
            DataGridViewCell selectedCell = dtGV_HoaDon.SelectedCells[0];
            DataGridViewRow selectedRow = dtGV_HoaDon.Rows[selectedCell.RowIndex];

            if (selectedRow.Cells["MaHD"].Value == null)
            {
                MessageBox.Show("Không thể lấy mã hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cho phép chỉnh sửa các cột có thể sửa (không cho sửa MaHD, MaNV, MaKH, NgayBan)
            dtGV_HoaDon.ReadOnly = false;
            
            // Chỉ cho phép sửa các cột: TrangThai, HinhThucThanhToan, GhiChu, TongThanhToan
            dtGV_HoaDon.Columns["MaHD"].ReadOnly = true;
            dtGV_HoaDon.Columns["MaNV"].ReadOnly = true;
            dtGV_HoaDon.Columns["MaKH"].ReadOnly = true;
            dtGV_HoaDon.Columns["NgayBan"].ReadOnly = true;
            dtGV_HoaDon.Columns["TrangThai"].ReadOnly = false;
            dtGV_HoaDon.Columns["HinhThucThanhToan"].ReadOnly = false;
            dtGV_HoaDon.Columns["GhiChu"].ReadOnly = false;
            dtGV_HoaDon.Columns["TongThanhToan"].ReadOnly = false;

            // Bật nút Lưu
            btnLuu.Enabled = true;
            btnSua.Enabled = false;

            // Focus vào dòng được chọn
            selectedRow.Selected = true;
            dtGV_HoaDon.CurrentCell = selectedRow.Cells["TrangThai"];
            dtGV_HoaDon.BeginEdit(true);

            MessageBox.Show("Bạn có thể chỉnh sửa thông tin hóa đơn. Nhấn 'Lưu hóa đơn' để lưu thay đổi.", 
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dtGV_HoaDon.SelectedCells.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dòng được chọn
                DataGridViewCell selectedCell = dtGV_HoaDon.SelectedCells[0];
                DataGridViewRow selectedRow = dtGV_HoaDon.Rows[selectedCell.RowIndex];

                if (selectedRow.Cells["MaHD"].Value == null)
                {
                    MessageBox.Show("Không thể lấy mã hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string maHD = selectedRow.Cells["MaHD"].Value.ToString();
                
                // Chuyển đổi MaHD sang Int32
                if (!int.TryParse(maHD, out int maHDInt))
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy giá trị từ DataGridView
                string trangThai = selectedRow.Cells["TrangThai"].Value != null ? 
                    selectedRow.Cells["TrangThai"].Value.ToString() : "";
                string hinhThucThanhToan = selectedRow.Cells["HinhThucThanhToan"].Value != null ? 
                    selectedRow.Cells["HinhThucThanhToan"].Value.ToString() : "";
                string ghiChu = selectedRow.Cells["GhiChu"].Value != null ? 
                    selectedRow.Cells["GhiChu"].Value.ToString() : "";
                
                // Lấy TongThanhToan
                decimal tongThanhToan = 0;
                if (selectedRow.Cells["TongThanhToan"].Value != null)
                {
                    if (!decimal.TryParse(selectedRow.Cells["TongThanhToan"].Value.ToString(), out tongThanhToan))
                    {
                        MessageBox.Show("Tổng thanh toán không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                OpenConnection();

                // Cập nhật vào database
                string updateSql = @"UPDATE HOADON 
                                    SET TrangThai = :TrangThai, 
                                        HinhThucThanhToan = :HinhThucThanhToan, 
                                        GhiChu = :GhiChu,
                                        TongThanhToan = :TongThanhToan
                                    WHERE MaHD = :MaHD";

                OracleCommand command = new OracleCommand(updateSql, connsql);
                command.Parameters.Add(":TrangThai", OracleDbType.NVarchar2).Value = trangThai;
                command.Parameters.Add(":HinhThucThanhToan", OracleDbType.NVarchar2).Value = 
                    string.IsNullOrEmpty(hinhThucThanhToan) ? DBNull.Value : (object)hinhThucThanhToan;
                command.Parameters.Add(":GhiChu", OracleDbType.NVarchar2).Value = 
                    string.IsNullOrEmpty(ghiChu) ? DBNull.Value : (object)ghiChu;
                command.Parameters.Add(":TongThanhToan", OracleDbType.Decimal).Value = tongThanhToan;
                command.Parameters.Add(":MaHD", OracleDbType.Int32).Value = maHDInt;

                int rowsAffected = command.ExecuteNonQuery();
                CloseConnection();

                if (rowsAffected > 0)
                {
                    // Cập nhật vào DataSet
                    DataRow dataRow = QL_ShopQuanAo.Tables["HOADON"].Rows.Find(maHDInt);
                    if (dataRow != null)
                    {
                        dataRow["TrangThai"] = trangThai;
                        dataRow["HinhThucThanhToan"] = string.IsNullOrEmpty(hinhThucThanhToan) ? DBNull.Value : (object)hinhThucThanhToan;
                        dataRow["GhiChu"] = string.IsNullOrEmpty(ghiChu) ? DBNull.Value : (object)ghiChu;
                        dataRow["TongThanhToan"] = tongThanhToan;
                    }

                    // Tắt chế độ chỉnh sửa
                    dtGV_HoaDon.ReadOnly = true;
                    btnLuu.Enabled = false;
                    btnSua.Enabled = true;

                    // Cập nhật lại tổng doanh thu
                    tinhTongDoanhThu();

                    MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn cần cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseConnection();
            }
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
