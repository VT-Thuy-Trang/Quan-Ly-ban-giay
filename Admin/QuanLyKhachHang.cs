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

            foreach (DataRow row in tb_KhachHang.Rows)
            {
                if (row["SDTKH"] != DBNull.Value)
                    row["SDTKH"] = MaHoa.Decrypt(row["SDTKH"].ToString());
            }

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
                        //cmdInsert.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = txtSDT.Text;
                        // Kiểm tra và mã hóa SDTKH
                        if (!string.IsNullOrEmpty(txtSDT.Text.Trim()))
                        {
                            string sdtMaHoa = MaHoa.Encrypt(txtSDT.Text.Trim());
                            cmdInsert.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = sdtMaHoa;
                        }
                        else
                        {
                            cmdInsert.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = DBNull.Value;
                        }

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
                        //cmdUpdate.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = txtSDT.Text;
                        // Kiểm tra và mã hóa SDTKH
                        if (!string.IsNullOrEmpty(txtSDT.Text.Trim()))
                        {
                            string sdtMaHoa = MaHoa.Encrypt(txtSDT.Text.Trim());
                            cmdUpdate.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = sdtMaHoa;
                        }
                        else
                        {
                            cmdUpdate.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = DBNull.Value;
                        }

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

        private void btnMaHoaKH_Click(object sender, EventArgs e)
        {
            string method = ShowEncryptionOption("Chọn phương thức mã hóa");
            if (string.IsNullOrEmpty(method)) return;

            DialogResult r = MessageBox.Show(
                $"Bạn có muốn mã hóa toàn bộ SĐT Khách hàng bằng {method} không?",
                "Xác nhận mã hóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r == DialogResult.No) return;

            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();

                string selectStr = "Select * from KHACHHANG";
                OracleDataAdapter da = new OracleDataAdapter(selectStr, connsql);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                DataTable dtTemp = new DataTable();
                da.Fill(dtTemp);

                int count = 0;
                foreach (DataRow row in dtTemp.Rows)
                {
                    if (row["SDTKH"] != DBNull.Value)
                    {
                        string val = row["SDTKH"].ToString();
                        if (IsNumber(val))
                        {
                            row["SDTKH"] = ApplyEncrypt(val, method);
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    OracleCommandBuilder cb = new OracleCommandBuilder(da);
                    da.Update(dtTemp);
                    LogActionToDB($"Mã hóa ({method})", $"Đã mã hóa {count} SĐT Khách hàng");

                    MessageBox.Show($"Đã mã hóa thành công {count} SĐT khách hàng!");
                    loadDGV_KhachHang();
                }
                else
                {
                    MessageBox.Show("Tất cả dữ liệu đã được mã hóa trước đó rồi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Kiểm tra chuỗi có phải là số hay không
        private bool IsNumber(string value)
        {
            return long.TryParse(value, out _);
        }

        private void btnGiaiMa_Click(object sender, EventArgs e)
        {
            // 1. Chọn thuật toán để giải mã
            string method = ShowEncryptionOption("Dữ liệu đang được mã hóa bằng gì?");
            if (string.IsNullOrEmpty(method)) return;

            DialogResult r = MessageBox.Show(
                $"CẢNH BÁO: Bạn có muốn GIẢI MÃ toàn bộ SĐT Khách hàng (đang dùng {method}) về dạng số thường không?",
                "Xác nhận giải mã",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r == DialogResult.No) return;

            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();

                string selectStr = "Select * from KHACHHANG";
                OracleDataAdapter da = new OracleDataAdapter(selectStr, connsql);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                DataTable dtTemp = new DataTable();
                da.Fill(dtTemp);

                int count = 0;
                RSAEncryption rsa = new RSAEncryption(); // Khởi tạo 1 lần để dùng lại

                foreach (DataRow row in dtTemp.Rows)
                {
                    if (row["SDTKH"] != DBNull.Value)
                    {
                        string val = row["SDTKH"].ToString();
                        // Kiểm tra nếu là chuỗi đã mã hóa (Base64) thì mới giải mã
                        if (IsBase64(val))
                        {
                            string decrypted = ApplyDecrypt(val, method, rsa);
                            // Nếu kết quả khác ban đầu (tức là giải mã được)
                            if (decrypted != val)
                            {
                                row["SDTKH"] = decrypted;
                                count++;
                            }
                        }
                    }
                }

                if (count > 0)
                {
                    OracleCommandBuilder cb = new OracleCommandBuilder(da);
                    da.Update(dtTemp);

                    // Ghi log
                    LogActionToDB($"Giải mã ({method})", $"Đã giải mã {count} SĐT Khách hàng");

                    MessageBox.Show($"Đã giải mã thành công cho {count} khách hàng!");
                    loadDGV_KhachHang();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được giải mã (có thể do chọn sai thuật toán hoặc dữ liệu đã là số thường).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        // Kiểm tra chuỗi có phải là Base64 (đã mã hóa) hay không
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


        //Hàm hiển thị hộp thoại chọn phương thức
        private string ShowEncryptionOption(string title)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 220,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Chọn thuật toán:", AutoSize = true };
            RadioButton rbAES = new RadioButton() { Left = 20, Top = 50, Text = "Đối xứng (AES) - Mặc định", Checked = true, Width = 250 };
            RadioButton rbRSA = new RadioButton() { Left = 20, Top = 80, Text = "Bất đối xứng (RSA)", Width = 250 };
            RadioButton rbHybrid = new RadioButton() { Left = 20, Top = 110, Text = "Lai (Hybrid: RSA + AES)", Width = 250 };
            Button confirmation = new Button() { Text = "Thực hiện", Left = 220, Width = 100, Top = 140, DialogResult = DialogResult.OK };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(rbAES);
            prompt.Controls.Add(rbRSA);
            prompt.Controls.Add(rbHybrid);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ?
                   (rbAES.Checked ? "AES" : (rbRSA.Checked ? "RSA" : "HYBRID")) : "";
        }

        //Hàm xử lý Mã hóa (Tương thích với RSA trả về byte[])
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

        //Hàm xử lý Giải mã
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
                    string privateKeyXml = rsaInstance.GetPrivateKey();
                    byte[] dataBytes = Convert.FromBase64String(cipherText);
                    return rsaInstance.Decrypt(dataBytes, privateKeyXml);
                }
                else if (method == "HYBRID")
                {
                    return MaHoa.HybridEncryption.DecryptHybrid(cipherText);
                }
            }
            catch
            {
                return cipherText; 
            }
            return cipherText;
        }

        //Hàm ghi log vào Database
        private void LogActionToDB(string action, string desc)
        {
            try
            {
                if (connsql.State == ConnectionState.Closed) connsql.Open();
                string sql = "INSERT INTO AUDIT_LOG (TABLE_NAME, OPERATION, USER_NAME, THOI_GIAN, DESCRIPTION) " +
                             "VALUES ('KHACHHANG', 'MA_HOA_DATA', :User, SYSDATE, :Desc)";
                OracleCommand cmd = new OracleCommand(sql, connsql);
                cmd.Parameters.Add(":User", "ADMIN");
                cmd.Parameters.Add(":Desc", $"{action} - {desc}");
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
    }
}
