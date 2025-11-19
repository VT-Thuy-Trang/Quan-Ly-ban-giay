using Oracle.ManagedDataAccess.Client;
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
using static QL_GiayTT.Class.MaHoa;
using static QL_GiayTT.Class.RSAEncryption;

namespace QL_GiayTT.Admin
{
    public partial class frmQuanLyNhanVien : Form
    {
        OracleConnection connsql;
        OracleDataAdapter data;
        DataSet listShopQuanAo;
        DataSet dtNhanVien = new DataSet();
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
        }

        private void loadCBO_GioiTinh()
        {
            string[] gioiTinh = { "Nam", "N?" };
            cbo_GioiTinh.DataSource = gioiTinh;
        }

        private void loadDGV_NhanVien()
        {
            // Clear dữ liệu cũ trước khi load lại
            if (dtNhanVien.Tables.Contains("NHANVIEN"))
            {
                dtNhanVien.Tables["NHANVIEN"].Clear();
            }
            
            string selectStr = "Select * from NHANVIEN";
            data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(dtNhanVien, "NHANVIEN");
            //
            foreach (DataRow row in dtNhanVien.Tables["NHANVIEN"].Rows)
            {
                if (row["SDTNV"] != DBNull.Value)
                {
                    string sdtValue = row["SDTNV"].ToString();
                    // Chỉ giải mã nếu là Base64 (đã mã hóa)
                    if (IsBase64(sdtValue))
                    {
                        row["SDTNV"] = MaHoa.Decrypt(sdtValue);
                    }
                    // Nếu không phải Base64 thì giữ nguyên (chưa mã hóa)
                }

                // CCCD không mã hóa nữa, giữ nguyên giá trị gốc
            }
            //
            DataColumn[] key = new DataColumn[1];
            key[0] = dtNhanVien.Tables["NHANVIEN"].Columns["MaNV"];
            dtNhanVien.Tables["NHANVIEN"].PrimaryKey = key;

            dtGV_NhanVien.Columns["MaNV"].DataPropertyName = "MaNV";
            dtGV_NhanVien.Columns["TenNV"].DataPropertyName = "TenNV";
            dtGV_NhanVien.Columns["SDTNV"].DataPropertyName = "SDTNV";
            dtGV_NhanVien.Columns["NgaySinhNV"].DataPropertyName = "NgaySinhNV";
            dtGV_NhanVien.Columns["DiaChiNV"].DataPropertyName = "DiaChiNV";
            dtGV_NhanVien.Columns["GioiTinhNV"].DataPropertyName = "GioiTinhNV";
            dtGV_NhanVien.Columns["CCCD"].DataPropertyName = "CCCD";
            dtGV_NhanVien.Columns["NVL"].DataPropertyName = "NVL";
            dtGV_NhanVien.Columns["Lương"].DataPropertyName = "Luong";
            dtGV_NhanVien.Columns["MATK"].DataPropertyName = "MATK";

            dtGV_NhanVien.DataSource = dtNhanVien.Tables["NHANVIEN"];
        }

        private void loadDGV_TaiKhoan()
        {
            // Clear dữ liệu cũ trước khi load lại
            if (listShopQuanAo.Tables.Contains("DANGNHAP"))
            {
                listShopQuanAo.Tables["DANGNHAP"].Clear();
            }
            else
            {
                listShopQuanAo = new DataSet();
            }

            string selectStr = "Select * from DANGNHAP";
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listShopQuanAo, "DANGNHAP");
            //
            foreach (DataRow row in listShopQuanAo.Tables["DANGNHAP"].Rows)
            {
                if (row["MATKHAU"] != DBNull.Value)
                {
                    string matKhauValue = row["MATKHAU"].ToString();
                    // Chỉ giải mã nếu là Base64 (đã mã hóa)
                    if (IsBase64(matKhauValue))
                    {
                        row["MATKHAU"] = MaHoa.Decrypt(matKhauValue);
                    }
                    // Nếu không phải Base64 thì giữ nguyên (chưa mã hóa)
                }
            }
            //
            dtGV_TaiKhoan.Columns["MaTKDN"].DataPropertyName = "MaTK";
            dtGV_TaiKhoan.Columns["TAIKHOAN"].DataPropertyName = "TAIKHOAN";
            dtGV_TaiKhoan.Columns["MATKHAU"].DataPropertyName = "MATKHAU";
            dtGV_TaiKhoan.Columns["LOAITK"].DataPropertyName = "LOAITK";

            string[] loaiTK = { "admin", "user" };

            var cboLoaiTK = (DataGridViewComboBoxColumn)dtGV_TaiKhoan.Columns["LOAITK"];

            cboLoaiTK.DataSource = loaiTK;

            dtGV_TaiKhoan.DataSource = listShopQuanAo.Tables["DANGNHAP"];
        }

        private void load_CBOMaTK()
        {
            listShopQuanAo = new DataSet();
            string selectStr = "Select * from DANGNHAP";
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listShopQuanAo, "CBOMaTK");
            cboMaTK.DataSource = listShopQuanAo.Tables["CBOMaTK"];
            cboMaTK.DisplayMember = "MaTK";

            cboMaTK.SelectedIndex = -1;
        }

        void dataBingDing(DataTable pDT)
        {
            txtMaNV.DataBindings.Clear();
            txtTenNV.DataBindings.Clear();
            cbo_GioiTinh.DataBindings.Clear();
            txtSDTNV.DataBindings.Clear();
            txtNgaySinh.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtCCCD.DataBindings.Clear();
            txtLương.DataBindings.Clear();
            txtNamVaoLam.DataBindings.Clear();
            cboMaTK.DataBindings.Clear();

            txtMaNV.DataBindings.Add("Text", pDT, "MaNV");
            txtTenNV.DataBindings.Add("Text", pDT, "TenNV");
            cbo_GioiTinh.DataBindings.Add("Text", pDT, "GioiTinhNV");
            txtSDTNV.DataBindings.Add("Text", pDT, "SDTNV");
            txtNgaySinh.DataBindings.Add("Text", pDT, "NgaySinhNV");
            txtDiaChi.DataBindings.Add("Text", pDT, "DiaChiNV");
            txtCCCD.DataBindings.Add("Text", pDT, "CCCD");
            txtLương.DataBindings.Add("Text", pDT, "Luong");
            txtNamVaoLam.DataBindings.Add("Text", pDT, "NVL");
            cboMaTK.DataBindings.Add("Text", pDT, "MATK");
        }

        private void ngatDataBingDing()
        {
            txtMaNV.DataBindings.Clear();
            txtTenNV.DataBindings.Clear();
            cbo_GioiTinh.DataBindings.Clear();
            txtSDTNV.DataBindings.Clear();
            txtNgaySinh.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtCCCD.DataBindings.Clear();
            txtLương.DataBindings.Clear();
            txtNamVaoLam.DataBindings.Clear();
            cboMaTK.DataBindings.Clear();
        }

        private void khoaTextBox()
        {
            foreach (Control item in gb_TTNV.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox) || item.GetType() == typeof(MaskedTextBox))
                    item.Enabled = false;
            }
        }

        private void moTextBox()
        {
            foreach (Control item in gb_TTNV.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox) || item.GetType() == typeof(MaskedTextBox))
                    item.Enabled = true;
            }
        }

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            loadCBO_GioiTinh();
            load_CBOMaTK();
            loadDGV_NhanVien();
            dataBingDing(dtNhanVien.Tables["NHANVIEN"]);
            loadDGV_TaiKhoan();

            dtGV_NhanVien.ReadOnly = true;
            dtGV_TaiKhoan.ReadOnly = true;

            dtGV_NhanVien.AllowUserToAddRows = false;
            dtGV_TaiKhoan.AllowUserToAddRows = false;

            khoaTextBox();
        }

        private void xoaThongTinNV()
        {
            foreach (Control item in gb_TTNV.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox))
                    item.Text = "";
            }
            cboMaTK.SelectedIndex = -1;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            xoaThongTinNV();

            ngatDataBingDing();

            btnLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

            btnTaoTK.Enabled = false;
            btnXoaTK.Enabled = false;
            btnSuaTK.Enabled = false;
            btnLuuTK.Enabled = false;

            moTextBox();

            dtGV_NhanVien.FirstDisplayedScrollingRowIndex = dtGV_NhanVien.Rows.Count - 1; 
        }

        private void loadDGV_TaiKhoan(string maTK)
        {
            if (maTK != string.Empty)
            {
                DataTable dtTable = listShopQuanAo.Tables["DANGNHAP"];
                DataRow[] tableTheoMa = dtTable.Select("MATK = '" + maTK + "'");
                if (tableTheoMa.Length == 0)
                {
                    dtGV_TaiKhoan.DataSource = listShopQuanAo.Tables["DANGNHAP"];
                    return;
                }
                DataTable TableCopy = tableTheoMa.CopyToDataTable();

                dtGV_TaiKhoan.DataSource = TableCopy;
            }
            else loadDGV_TaiKhoan();
        }

        private void dtGV_NhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtGV_NhanVien.Rows[e.RowIndex];

                string maTK = selectedRow.Cells["MaTK"].Value.ToString();
                loadDGV_TaiKhoan(maTK.Trim());
            }
        }

        private void resetDuLieu()
        {
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;

            btnTaoTK.Enabled = true;
            btnXoaTK.Enabled = true;
            btnSuaTK.Enabled = true;
            btnLuuTK.Enabled = false;

            dtGV_NhanVien.Enabled = true;

            dtGV_TaiKhoan.ReadOnly = true;
            dtGV_TaiKhoan.AllowUserToAddRows = false;

            khoaTextBox();

            dataBingDing(dtNhanVien.Tables["NHANVIEN"]);
        }

        private void xoaDuLieuTrong_TrongDTGV_TaiKhoan()
        {
            for (int i = dtGV_TaiKhoan.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dtGV_TaiKhoan.Rows[i];

                bool isRowEmpty = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        isRowEmpty = false;
                        break;
                    }
                }

                if (isRowEmpty)
                {
                    dtGV_TaiKhoan.Rows.Remove(row);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetDuLieu();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Trim() == "NV000")
            {
                resetDuLieu();
                MessageBox.Show("Không th? xoá!!!");
                return;
            }
            DialogResult r = MessageBox.Show("B?n có mu?n xoá nhân viên này ch??", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                try
                {
                    DataRow deleteRow = dtNhanVien.Tables["NHANVIEN"].Rows.Find(txtMaNV.Text);
                    deleteRow.Delete();
                    OracleCommandBuilder cB = new OracleCommandBuilder(data);
                    data.Update(dtNhanVien, "NHANVIEN");
                    MessageBox.Show("Thành công");
                    loadDGV_TaiKhoan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("L?i: " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Trim() == "NV000")
            {
                resetDuLieu();
                MessageBox.Show("Không th? s?a!!!");
                return;
            }

            ngatDataBingDing();

            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;

            btnTaoTK.Enabled = false;
            btnXoaTK.Enabled = false;
            btnSuaTK.Enabled = false;
            btnLuuTK.Enabled = false;

            foreach (Control item in gb_TTNV.Controls)
            {
                if (item.GetType() == typeof(TextBox) || item.GetType() == typeof(ComboBox) || item.GetType() == typeof(MaskedTextBox))
                    item.Enabled = true;
            }

            txtMaNV.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Lưu thay d?i nhân viên?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.No)
                return;
            else
            {
                try
                {
                    if (txtMaNV.Enabled == true) //Them
                    {
                        if (txtMaNV.Text == string.Empty)
                        {
                            MessageBox.Show("B?n ph?i nh?p Mã nhân viên!!!");
                            return;
                        }
                        if (txtTenNV.Text == string.Empty)
                        {
                            MessageBox.Show("B?n ph?i nh?p Tên nhân viên!!!");
                            return;
                        }
                        DataRow dr = dtNhanVien.Tables["NHANVIEN"].Rows.Find(txtMaNV.Text);
                        if (dr == null)
                        {
                            DataRow insertRow = dtNhanVien.Tables["NHANVIEN"].NewRow();
                            insertRow["MaNV"] = txtMaNV.Text;
                            insertRow["TenNV"] = txtTenNV.Text;
                            insertRow["GioiTinhNV"] = cbo_GioiTinh.Text;
                            //insertRow["SDTNV"] = txtSDTNV.Text;
                            //insertRow["SDTNV"] = MaHoa.Encrypt(txtSDTNV.Text); // Mã hóa
                            // Gọi hàm Encrypt trước khi gán vào Row
                            if (!string.IsNullOrEmpty(txtSDTNV.Text))
                            {
                                insertRow["SDTNV"] = MaHoa.Encrypt(txtSDTNV.Text);
                            }

                            if (txtNgaySinh.Text != string.Empty)
                            {
                                DateTime NgaySinhNV = DateTime.Parse(txtNgaySinh.Text);
                                insertRow["NgaySinhNV"] = NgaySinhNV.ToString("yyyy-MM-dd");
                            }
                            else insertRow["NgaySinhNV"] = DBNull.Value;
                            insertRow["DiaChiNV"] = txtDiaChi.Text;
                            // CCCD không mã hóa, lưu giá trị gốc
                            if (!string.IsNullOrEmpty(txtCCCD.Text))
                            {
                                insertRow["CCCD"] = txtCCCD.Text;
                            }
                            else
                            {
                                insertRow["CCCD"] = DBNull.Value;
                            }

                            if (txtLương.Text == string.Empty)
                                insertRow["Luong"] = 0;
                            else insertRow["Luong"] = txtLương.Text;
                            if (txtNamVaoLam.Text == string.Empty)
                                insertRow["NVL"] = DateTime.Now.Year;
                            else insertRow["NVL"] = txtNamVaoLam.Text;
                            if (cboMaTK.SelectedIndex == -1)
                                insertRow["MATK"] = DBNull.Value;
                            else insertRow["MATK"] = cboMaTK.Text;
                            dtNhanVien.Tables["NHANVIEN"].Rows.Add(insertRow);
                        }
                        else
                        {
                            MessageBox.Show("Trung Mã nhân viên!!!");
                            return;
                        }
                    }
                    else    //Sua
                    {

                        DataRow updateRow = dtNhanVien.Tables["NHANVIEN"].Rows.Find(txtMaNV.Text);
                        if (updateRow != null)
                        {
                            updateRow["TenNV"] = txtTenNV.Text;
                            updateRow["GioiTinhNV"] = cbo_GioiTinh.Text;
                            //updateRow["SDTNV"] = txtSDTNV.Text;
                            updateRow["SDTNV"] = MaHoa.Encrypt(txtSDTNV.Text); // Mã hóa

                            if (txtNgaySinh.Text != string.Empty)
                            {
                                DateTime NgaySinhNV = DateTime.Parse(txtNgaySinh.Text);
                                updateRow["NgaySinhNV"] = NgaySinhNV.ToString("yyyy-MM-dd");
                            }
                            else updateRow["NgaySinhNV"] = DBNull.Value;

                            updateRow["DiaChiNV"] = txtDiaChi.Text;
                            // CCCD không mã hóa, lưu giá trị gốc
                            if (!string.IsNullOrEmpty(txtCCCD.Text))
                            {
                                updateRow["CCCD"] = txtCCCD.Text;
                            }
                            else
                            {
                                updateRow["CCCD"] = DBNull.Value;
                            }

                            if (txtLương.Text == string.Empty)
                                updateRow["Luong"] = 0;
                            else updateRow["Luong"] = txtLương.Text;
                            if (txtNamVaoLam.Text == string.Empty)
                                updateRow["NVL"] = DateTime.Now.Year;
                            else updateRow["NVL"] = txtNamVaoLam.Text;
                            if (cboMaTK.SelectedIndex == -1)
                                updateRow["MATK"] = DBNull.Value;
                            else updateRow["MATK"] = cboMaTK.Text;
                        }
                    }
                    OracleCommandBuilder cB = new OracleCommandBuilder(data);
                    data.Update(dtNhanVien, "NHANVIEN");
                    MessageBox.Show("Thành công");
                    xoaThongTinNV();
                    resetDuLieu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void txtLương_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((txtLương.Text.Length == 0 && e.KeyChar == '0') || (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) || (txtLương.Text.Length > 0 && txtLương.Text[0] == '0'))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
                return;
            }
        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
            }
        }

        private void txtNamVaoLam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((txtNamVaoLam.Text.Length == 0 && e.KeyChar == '0') || (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) || (txtNamVaoLam.Text.Length > 0 && txtNamVaoLam.Text[0] == '0'))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
                return;
            }
        }

        private void txtSDTNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // H?y b? ký t? không h?p l?
            }
        }

        private void btnTaoTK_Click(object sender, EventArgs e)
        {
            btnLuuTK.Enabled = true;
            btnXoaTK.Enabled = false;
            btnSuaTK.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = false;

            dtGV_TaiKhoan.ReadOnly = false;
            dtGV_TaiKhoan.AllowUserToAddRows = true;

            for (int i = 0; i < dtGV_TaiKhoan.Rows.Count - 1; i++)
            {
                dtGV_TaiKhoan.Rows[i].ReadOnly = true;
            }

            dtGV_TaiKhoan.FirstDisplayedScrollingRowIndex = dtGV_TaiKhoan.Rows.Count - 1;
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            btnLuuTK.Enabled = true;
            btnXoaTK.Enabled = false;
            btnTaoTK.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = false;

            dtGV_TaiKhoan.ReadOnly = false;
            for (int i = 0; i < dtGV_TaiKhoan.Rows.Count - 1; i++)
            {
                dtGV_TaiKhoan.Rows[i].ReadOnly = false;
            }
            dtGV_TaiKhoan.Columns[0].ReadOnly = true;

            dtGV_TaiKhoan.AllowUserToAddRows = false;
        }

        private void btnLuuTK_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Lưu thay d?i tài kho?n?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                if (dtGV_TaiKhoan.Columns[0].ReadOnly != true)
                {

                    DataTable dt = listShopQuanAo.Tables["DANGNHAP"];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted) // Không xử lý dòng đã xóa
                        {
                            string passRaw = row["MATKHAU"].ToString();
                            // Luôn mã hóa lại giá trị hiện tại
                            row["MATKHAU"] = MaHoa.Encrypt(passRaw);
                        }
                    }

                    string selectStr = "select * from DANGNHAP";
                    OracleDataAdapter dataTK = new OracleDataAdapter(selectStr, connsql);
                    try
                    {
                        OracleCommandBuilder cmb = new OracleCommandBuilder(dataTK);
                        dataTK.Update(listShopQuanAo, "DANGNHAP");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Trùng Mã tài kho?n");
                        return;
                    }

                }
                else
                {
                    DataTable tempTable = ((DataTable)dtGV_TaiKhoan.DataSource).Copy();

                    foreach (DataRow row in tempTable.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            string passRaw = row["MATKHAU"].ToString();
                            row["MATKHAU"] = MaHoa.Encrypt(passRaw);
                        }
                    }

                    if (tempTable.PrimaryKey.Length > 0)
                    {
                        // Xoá ràng bu?c khóa chính
                        tempTable.Constraints.Remove("PrimaryKey");
                        tempTable.PrimaryKey = null;
                    }
                    string selectStr = "SELECT * FROM DANGNHAP";
                    OracleDataAdapter dataAdapter = new OracleDataAdapter(selectStr, connsql);
                    OracleCommandBuilder commandBuilder = new OracleCommandBuilder(dataAdapter);
                    dataAdapter.Update(tempTable);
                }
                    
                    dataBingDing(dtNhanVien.Tables["NHANVIEN"]);
                    MessageBox.Show("Thành công");
                    //loadDGV_NhanVien();
                    load_CBOMaTK();
                    resetDuLieu();
            }
        }

        private void dtGV_TaiKhoan_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dtGV_TaiKhoan.Rows[e.RowIndex];
    
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    MessageBox.Show("Nh?p d?y d? thông tin tài kho?n!!!");
                    break;
                }
            }
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("B?n có mu?n xoá tài kho?n này ch?!!!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
            {
                try
                {
                    if (dtGV_TaiKhoan.SelectedCells.Count > 0)
                    {
                        // L?y d?i tu?ng DataGridViewCell c?a ôô đang được chọn
                        DataGridViewCell selectedCell = dtGV_TaiKhoan.SelectedCells[0];

                        // Truy cập giá trị của ô đang được chọn
                        object cellValue = selectedCell.Value;

                        // Kiểm tra và sử dụng giá trị của ô đang được chọn
                        if (cellValue != null)
                        {
                            // Sử dụng giá trị của ô đang được chọn
                            string maTK = cellValue.ToString();
                            if (connsql.State == ConnectionState.Closed)
                                connsql.Open();

                            string selectString = "select count (*) from NHANVIEN where MATK = :MaTK";
                            OracleCommand cmd = new OracleCommand(selectString, connsql);
                            cmd.Parameters.Add(":MaTK", OracleDbType.Varchar2).Value = maTK;
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                
                            if (count >= 1)
                            {
                                connsql.Close();
                                MessageBox.Show("Không th? xoá do dang có nhân viên xài tài kho?n này!!!");
                                return;
                            }
                            else
                            {
                                string deleteStr = "delete from DANGNHAP where MATK = :MaTK";
                                cmd = new OracleCommand(deleteStr, connsql);
                                cmd.Parameters.Add(":MaTK", OracleDbType.Varchar2).Value = maTK;
                                cmd.ExecuteNonQuery();
                                connsql.Close();
                                MessageBox.Show("Thành công");
                                //dtGV_TaiKhoan.Refresh();
                                loadDGV_TaiKhoan();
                                load_CBOMaTK();
                                resetDuLieu();
                            } 
                        }
                        else
                        {
                            Console.WriteLine("Ô đang được chọn không có giá trị.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không có ô nào du?c ch?n trong b?ng Nhân Viên.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Th?t b?i");
                }
            }
        }

        // nut ma hoa all

        private void btnMaHoaALL_Click_1(object sender, EventArgs e)
        {
            string method = ShowEncryptionOption();
            if (string.IsNullOrEmpty(method)) return;

            DialogResult r = MessageBox.Show($"Bạn muốn mã hóa bằng {method}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No) return;

            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();

                // 1. Xử lý Nhân viên (SĐT, CCCD)
                OracleDataAdapter daNV = new OracleDataAdapter("Select * from NHANVIEN", connsql);
                OracleCommandBuilder cbNV = new OracleCommandBuilder(daNV);
                DataTable dtNV = new DataTable();
                daNV.Fill(dtNV);

                int count = 0;
                foreach (DataRow row in dtNV.Rows)
                {
                    bool updated = false;
                    // Mã hóa SĐT
                    if (row["SDTNV"] != DBNull.Value && IsNumber(row["SDTNV"].ToString()))
                    {
                        string plain = row["SDTNV"].ToString();
                        row["SDTNV"] = ApplyEncrypt(plain, method);
                        updated = true;
                    }
                    // Mã hóa CCCD 
                    if (row["CCCD"] != DBNull.Value && IsNumber(row["CCCD"].ToString()))
                    {
                        string plain = row["CCCD"].ToString();
                        row["CCCD"] = ApplyEncrypt(plain, method);
                        updated = true;
                    }
                    if (updated) count++;
                }
                if (count > 0) daNV.Update(dtNV);

                // 2. Xử lý Tài khoản (Mật khẩu)
                OracleDataAdapter daTK = new OracleDataAdapter("Select * from DANGNHAP", connsql);
                OracleCommandBuilder cbTK = new OracleCommandBuilder(daTK);
                DataTable dtTK = new DataTable();
                daTK.Fill(dtTK);

                int countTK = 0;
                foreach (DataRow row in dtTK.Rows)
                {
                    if (row["MATKHAU"] != DBNull.Value)
                    {
                        string plain = row["MATKHAU"].ToString();
                        if (plain.Length < 30)
                        {
                            row["MATKHAU"] = ApplyEncrypt(plain, method);
                            countTK++;
                        }
                    }
                }
                if (countTK > 0) daTK.Update(dtTK);
                // Ghi Log vào DB để bên Nhật ký xuất được
                LogActionToDB($"Mã hóa toàn bộ ({method})", $"Thành công: {count} NV, {countTK} TK");

                MessageBox.Show("Hoàn tất mã hóa!");
                loadDGV_NhanVien();
                loadDGV_TaiKhoan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi (có thể do cột DB quá ngắn): " + ex.Message);
            }
        }


        private void btnGiaiMaALL_Click(object sender, EventArgs e)
        {
            // Bước 1: Hỏi người dùng xem dữ liệu đang bị mã hóa bằng kiểu gì
            string method = ShowEncryptionOption();
            if (string.IsNullOrEmpty(method)) return;

            DialogResult r = MessageBox.Show(
                $"CẢNH BÁO: Bạn sắp giải mã toàn bộ bằng thuật toán {method}.\n\n" +
                "Nếu chọn sai thuật toán, dữ liệu sẽ bị lỗi hoặc không giải mã được.\n" +
                "Bạn có chắc chắn muốn tiếp tục?",
                "Xác nhận giải mã",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r == DialogResult.No) return;

            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();

                // --- XỬ LÝ BẢNG NHÂN VIÊN (SĐT, CCCD) ---
                string selectStr = "Select * from NHANVIEN";
                OracleDataAdapter da = new OracleDataAdapter(selectStr, connsql);
                OracleCommandBuilder cb = new OracleCommandBuilder(da); // Tự động sinh lệnh Update
                DataTable dtTemp = new DataTable();
                da.Fill(dtTemp);

                int countNV = 0;
                RSAEncryption rsa = new RSAEncryption(); // Khởi tạo sẵn để dùng

                foreach (DataRow row in dtTemp.Rows)
                {
                    bool isUpdated = false;

                    // Giải mã SĐT
                    if (row["SDTNV"] != DBNull.Value)
                    {
                        string val = row["SDTNV"].ToString();
                        string decrypted = ApplyDecrypt(val, method, rsa);
                        if (decrypted != val) // Nếu có sự thay đổi (giải mã thành công)
                        {
                            row["SDTNV"] = decrypted;
                            isUpdated = true;
                        }
                    }

                    // Giải mã CCCD
                    if (row["CCCD"] != DBNull.Value)
                    {
                        string val = row["CCCD"].ToString();
                        string decrypted = ApplyDecrypt(val, method, rsa);
                        if (decrypted != val)
                        {
                            row["CCCD"] = decrypted;
                            isUpdated = true;
                        }
                    }

                    if (isUpdated) countNV++;
                }

                if (countNV > 0) da.Update(dtTemp); // Lưu xuống DB

                // --- XỬ LÝ BẢNG TÀI KHOẢN (MẬT KHẨU) ---
                string selectTK = "Select * from DANGNHAP";
                OracleDataAdapter daTK = new OracleDataAdapter(selectTK, connsql);
                OracleCommandBuilder cbTK = new OracleCommandBuilder(daTK);
                DataTable dtTK = new DataTable();
                daTK.Fill(dtTK);

                int countTK = 0;
                foreach (DataRow row in dtTK.Rows)
                {
                    if (row["MATKHAU"] != DBNull.Value)
                    {
                        string val = row["MATKHAU"].ToString();
                        string decrypted = ApplyDecrypt(val, method, rsa);

                        if (decrypted != val)
                        {
                            row["MATKHAU"] = decrypted;
                            countTK++;
                        }
                    }
                }

                if (countTK > 0) daTK.Update(dtTK); // Lưu xuống DB

                // Ghi Log (để bên Nhật ký có thể xuất báo cáo)
                LogActionToDB($"Giải mã ({method})", $"Đã giải mã {countNV} nhân viên và {countTK} tài khoản.");

                if (connsql.State == ConnectionState.Open) connsql.Close();

                MessageBox.Show($"Hoàn tất!\n- Nhân viên đã cập nhật: {countNV}\n- Tài khoản đã cập nhật: {countTK}");

                // Tải lại dữ liệu lên lưới
                loadDGV_NhanVien();
                loadDGV_TaiKhoan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quá trình giải mã: " + ex.Message);
            }
        }


        // chon ham ma hoa theo phuong thuc
        private string ApplyEncrypt(string plain, string method)
        {
            if (method == "RSA")
            {
                byte[] encBytes = new RSAEncryption().Encrypt(plain);
                return Convert.ToBase64String(encBytes);
            }
            if (method == "HYBRID") return MaHoa.HybridEncryption.EncryptHybrid(plain);
            return MaHoa.Encrypt(plain);
        }



        // tao hop thoai de chon phuong thuc ma hoa

        private string ShowEncryptionOption()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Chọn phương thức mã hóa",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Chọn thuật toán:", AutoSize = true };
            RadioButton rbAES = new RadioButton() { Left = 20, Top = 50, Text = "Đối xứng (AES)", Checked = true, Width = 200 };
            RadioButton rbRSA = new RadioButton() { Left = 20, Top = 80, Text = "Bất đối xứng (RSA)", Width = 200 };
            RadioButton rbHybrid = new RadioButton() { Left = 20, Top = 110, Text = "Lai (Hybrid: RSA + AES)", Width = 200 };
            Button confirmation = new Button() { Text = "Thực hiện", Left = 220, Width = 100, Top = 120, DialogResult = DialogResult.OK };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(rbAES);
            prompt.Controls.Add(rbRSA);
            prompt.Controls.Add(rbHybrid);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ?
                   (rbAES.Checked ? "AES" : (rbRSA.Checked ? "RSA" : "HYBRID")) : "";
        }

        // ham ghi log vao db
        private void LogActionToDB(string action, string desc)
        {
            try
            {
                string sql = "INSERT INTO AUDIT_LOG (TABLE_NAME, OPERATION, USER_NAME, THOI_GIAN, DESCRIPTION) " +
                             "VALUES ('HE_THONG', :Op, :User, SYSDATE, :Desc)";
                OracleCommand cmd = new OracleCommand(sql, connsql);
                cmd.Parameters.Add(":Op", "MA_HOA_DATA"); // Đánh dấu để lọc
                cmd.Parameters.Add(":User", "ADMIN"); // Hoặc lấy user hiện tại
                cmd.Parameters.Add(":Desc", $"{action} - {desc}");
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private string ApplyDecrypt(string cipherText, string method, RSAEncryption rsaInstance)
        {
            if (string.IsNullOrEmpty(cipherText) || !IsBase64(cipherText))
                return cipherText;

            try
            {
                if (method == "AES")
                {
                    return MaHoa.Decrypt(cipherText);
                }
                else if (method == "RSA")
                {
                    // Code cũ RSA yêu cầu byte[] và xmlKey
                    // 1. Lấy Private Key
                    string privateKeyXml = rsaInstance.GetPrivateKey();
                    // 2. Chuyển chuỗi mã hóa trong DB (Base64) sang byte[]
                    byte[] dataBytes = Convert.FromBase64String(cipherText);
                    // 3. Gọi hàm Decrypt cũ
                    return rsaInstance.Decrypt(dataBytes, privateKeyXml);
                }
                else if (method == "HYBRID")
                {
                    return MaHoa.HybridEncryption.DecryptHybrid(cipherText);
                }
            }
            catch
            {
                // Nếu giải mã lỗi (do sai key, sai thuật toán), giữ nguyên giá trị cũ
                return cipherText;
            }
            return cipherText;
        }

        // Hàm kiểm tra chuỗi có phải là Base64 không
        private bool IsBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0 ||
                base64String.Contains(" ") || base64String.Contains("\t") ||
                base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Hàm kiểm tra chuỗi có phải là số không
        private bool IsNumber(string value)
        {
            return long.TryParse(value, out _);
        }

    }
}
