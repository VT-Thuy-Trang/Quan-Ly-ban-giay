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
            string selectStr = "Select * from NHANVIEN";
            data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(dtNhanVien, "NHANVIEN");
            //
            foreach (DataRow row in dtNhanVien.Tables["NHANVIEN"].Rows)
            {
                row["SDTNV"] = MaHoa.Decrypt(row["SDTNV"].ToString());
                row["CCCD"] = MaHoa.Decrypt(row["CCCD"].ToString());
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
            listShopQuanAo = new DataSet();

            string selectStr = "Select * from DANGNHAP";
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listShopQuanAo, "DANGNHAP");
            //
            foreach (DataRow row in listShopQuanAo.Tables["DANGNHAP"].Rows)
            {
                row["MATKHAU"] = MaHoa.Decrypt(row["MATKHAU"].ToString());
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
                            insertRow["SDTNV"] = MaHoa.Encrypt(txtSDTNV.Text); // Mã hóa
                            
                            if (txtNgaySinh.Text != string.Empty)
                            {
                                DateTime NgaySinhNV = DateTime.Parse(txtNgaySinh.Text);
                                insertRow["NgaySinhNV"] = NgaySinhNV.ToString("yyyy-MM-dd");
                            }
                            else insertRow["NgaySinhNV"] = DBNull.Value;
                            insertRow["DiaChiNV"] = txtDiaChi.Text;
                            //insertRow["CCCD"] = txtCCCD.Text;
                            insertRow["CCCD"] = MaHoa.Encrypt(txtCCCD.Text);   // Mã hóa

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
                            //updateRow["CCCD"] = txtCCCD.Text;
                            updateRow["CCCD"] = MaHoa.Encrypt(txtCCCD.Text);   // Mã hóa

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

        private void btnMaHoaALL_Click_1(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
        "Bạn có muốn mã hóa toàn bộ dữ liệu (Lương, CCCD, SĐT) chưa được bảo mật không?",
        "Xác nhận mã hóa",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (r == DialogResult.No) return;

            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();

                // 2. Load toàn bộ dữ liệu lên để xử lý
                // Quan trọng: Phải có MissingSchemaAction.AddWithKey để update được dữ liệu
                string selectStr = "Select * from NHANVIEN";
                OracleDataAdapter da = new OracleDataAdapter(selectStr, connsql);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                DataTable dtTemp = new DataTable();
                da.Fill(dtTemp);

                int count = 0;

                // 3. Duyệt qua từng nhân viên
                foreach (DataRow row in dtTemp.Rows)
                {
                    bool isUpdated = false;
                    // --- Xử lý CCCD ---
                    if (row["CCCD"] != DBNull.Value)
                    {
                        string val = row["CCCD"].ToString();
                        if (IsNumber(val))
                        {
                            row["CCCD"] = MaHoa.Encrypt(val);
                            isUpdated = true;
                        }
                    }

                    // --- Xử lý SĐT ---
                    if (row["SDTNV"] != DBNull.Value)
                    {
                        string val = row["SDTNV"].ToString();
                        if (IsNumber(val))
                        {
                            row["SDTNV"] = MaHoa.Encrypt(val);
                            isUpdated = true;
                        }
                    }

                    if (isUpdated) count++;
                }

                // 4. Nếu có dữ liệu thay đổi thì cập nhật xuống Database
                if (count > 0)
                {
                    OracleCommandBuilder cb = new OracleCommandBuilder(da);
                    da.Update(dtTemp);
                    MessageBox.Show($"Đã mã hóa thành công cho {count} nhân viên!");

                    // Load lại lưới để hiển thị kết quả (lúc này code Load sẽ tự giải mã để hiển thị lại)
                    loadDGV_NhanVien();
                }
                else
                {
                    MessageBox.Show("Tất cả dữ liệu đã được mã hóa trước đó rồi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n(Hãy đảm bảo bạn đã mở rộng cột trong Oracle lên VARCHAR2(200))");
            }
        }


        // ham kiem tra 1 chuoi co phai base64 hay khong
        private bool IsBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0 || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
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

        private bool IsNumber(string value)
        {
            return long.TryParse(value, out _);
        }
    }
}
