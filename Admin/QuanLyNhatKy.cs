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
using System.IO;
using System.Security.Cryptography;

namespace QL_GiayTT.Admin
{
    public partial class QuanLyNhatKy : Form
    {
        OracleConnection connsql;
        OracleDataAdapter data;
        DataTable dtNhatKy;

        public QuanLyNhatKy()
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
            LoadComboBoxes();
            LoadNhatKy();
            
            // Thiết lập DataGridView giống các form khác
            dgvNhatKy.ReadOnly = true;
            dgvNhatKy.AllowUserToAddRows = false;
        }

        private void LoadComboBoxes()
        {
            // Load danh sách bảng
            string[] tables = { "Tất cả", "DANGNHAP", "NHANVIEN", "KHACHHANG", "SANPHAM", "HOADON", "CTHD", "LOAISP" };
            cboBang.DataSource = tables;

            // Load danh sách thao tác
            string[] operations = { "Tất cả", "INSERT", "UPDATE", "DELETE" };
            cboThaoTac.DataSource = operations;
        }

        private void LoadNhatKy()
        {
            try
            {
                // Clear dữ liệu cũ trước khi load lại
                if (dtNhatKy != null)
                {
                    dtNhatKy.Clear();
                }
                else
                {
                    dtNhatKy = new DataTable();
                }
                
                string selectStr = "SELECT * FROM VW_AUDIT_LOG ORDER BY TO_DATE(THOI_GIAN, 'DD/MM/YYYY HH24:MI:SS') DESC";
                data = new OracleDataAdapter(selectStr, connsql);
                data.Fill(dtNhatKy);

                dgvNhatKy.DataSource = dtNhatKy;

                // Định dạng cột
                if (dgvNhatKy.Columns.Count > 0)
                {
                    dgvNhatKy.Columns["AUDIT_ID"].HeaderText = "Mã";
                    dgvNhatKy.Columns["TABLE_NAME"].HeaderText = "Bảng";
                    dgvNhatKy.Columns["OPERATION"].HeaderText = "Thao tác";
                    dgvNhatKy.Columns["PRIMARY_KEY_VALUE"].HeaderText = "Khóa chính";
                    dgvNhatKy.Columns["USER_NAME"].HeaderText = "Người dùng";
                    dgvNhatKy.Columns["SESSION_USER"].HeaderText = "Session";
                    dgvNhatKy.Columns["IP_ADDRESS"].HeaderText = "IP";
                    dgvNhatKy.Columns["THOI_GIAN"].HeaderText = "Thời gian";
                    dgvNhatKy.Columns["DESCRIPTION"].HeaderText = "Mô tả";
                    dgvNhatKy.Columns["GIA_TRI_CU"].HeaderText = "Giá trị cũ";
                    dgvNhatKy.Columns["GIA_TRI_MOI"].HeaderText = "Giá trị mới";

                    // Điều chỉnh độ rộng cột
                    dgvNhatKy.Columns["AUDIT_ID"].Width = 60;
                    dgvNhatKy.Columns["TABLE_NAME"].Width = 100;
                    dgvNhatKy.Columns["OPERATION"].Width = 80;
                    dgvNhatKy.Columns["PRIMARY_KEY_VALUE"].Width = 120;
                    dgvNhatKy.Columns["USER_NAME"].Width = 100;
                    dgvNhatKy.Columns["SESSION_USER"].Width = 100;
                    dgvNhatKy.Columns["IP_ADDRESS"].Width = 120;
                    dgvNhatKy.Columns["THOI_GIAN"].Width = 150;
                    dgvNhatKy.Columns["DESCRIPTION"].Width = 200;
                    dgvNhatKy.Columns["GIA_TRI_CU"].Width = 200;
                    dgvNhatKy.Columns["GIA_TRI_MOI"].Width = 200;
                }

                lblTongSo.Text = "Tổng số: " + dtNhatKy.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nhật ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string whereClause = "1=1";

                // Lọc theo bảng
                if (cboBang.SelectedItem != null && cboBang.SelectedItem.ToString() != "Tất cả")
                {
                    whereClause += " AND TABLE_NAME = '" + cboBang.SelectedItem.ToString() + "'";
                }

                // Lọc theo thao tác
                if (cboThaoTac.SelectedItem != null && cboThaoTac.SelectedItem.ToString() != "Tất cả")
                {
                    whereClause += " AND OPERATION = '" + cboThaoTac.SelectedItem.ToString() + "'";
                }

                // Lọc theo người dùng
                if (!string.IsNullOrWhiteSpace(txtNguoiDung.Text))
                {
                    whereClause += " AND UPPER(USER_NAME) LIKE '%" + txtNguoiDung.Text.ToUpper() + "%'";
                }

                // Lọc theo thời gian
                if (dtpTuNgay.Checked)
                {
                    whereClause += " AND TO_DATE(THOI_GIAN, 'DD/MM/YYYY HH24:MI:SS') >= TO_DATE('" + 
                        dtpTuNgay.Value.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY')";
                }

                if (dtpDenNgay.Checked)
                {
                    whereClause += " AND TO_DATE(THOI_GIAN, 'DD/MM/YYYY HH24:MI:SS') <= TO_DATE('" + 
                        dtpDenNgay.Value.ToString("dd/MM/yyyy") + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS')";
                }

                string selectStr = "SELECT * FROM VW_AUDIT_LOG WHERE " + whereClause + 
                    " ORDER BY TO_DATE(THOI_GIAN, 'DD/MM/YYYY HH24:MI:SS') DESC";

                dtNhatKy = new DataTable();
                data = new OracleDataAdapter(selectStr, connsql);
                data.Fill(dtNhatKy);

                dgvNhatKy.DataSource = dtNhatKy;
                lblTongSo.Text = "Tổng số: " + dtNhatKy.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboBang.SelectedIndex = 0;
            cboThaoTac.SelectedIndex = 0;
            txtNguoiDung.Clear();
            dtpTuNgay.Checked = false;
            dtpDenNgay.Checked = false;
            LoadNhatKy();
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvNhatKy.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvNhatKy.SelectedRows[0];
                string chiTiet = "Mã nhật ký: " + row.Cells["AUDIT_ID"].Value + "\n\n";
                chiTiet += "Bảng: " + row.Cells["TABLE_NAME"].Value + "\n";
                chiTiet += "Thao tác: " + row.Cells["OPERATION"].Value + "\n";
                chiTiet += "Khóa chính: " + row.Cells["PRIMARY_KEY_VALUE"].Value + "\n";
                chiTiet += "Người dùng: " + row.Cells["USER_NAME"].Value + "\n";
                chiTiet += "Session: " + row.Cells["SESSION_USER"].Value + "\n";
                chiTiet += "IP: " + row.Cells["IP_ADDRESS"].Value + "\n";
                chiTiet += "Thời gian: " + row.Cells["THOI_GIAN"].Value + "\n";
                chiTiet += "Mô tả: " + row.Cells["DESCRIPTION"].Value + "\n\n";
                
                if (row.Cells["GIA_TRI_CU"].Value != DBNull.Value && !string.IsNullOrEmpty(row.Cells["GIA_TRI_CU"].Value.ToString()))
                {
                    chiTiet += "Giá trị cũ:\n" + row.Cells["GIA_TRI_CU"].Value + "\n\n";
                }
                
                if (row.Cells["GIA_TRI_MOI"].Value != DBNull.Value && !string.IsNullOrEmpty(row.Cells["GIA_TRI_MOI"].Value.ToString()))
                {
                    chiTiet += "Giá trị mới:\n" + row.Cells["GIA_TRI_MOI"].Value;
                }

                MessageBox.Show(chiTiet, "Chi tiết nhật ký", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtNhatKy == null || dtNhatKy.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FileName = "NhatKy_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string baseFileName = saveFileDialog.FileName;
                    if (baseFileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        baseFileName = baseFileName.Substring(0, baseFileName.Length - 4);
                    }

                    // Tạo đối tượng RSA để mã hóa
                    RSAEncryption rsa = new RSAEncryption();
                    string privateKey = rsa.GetPrivateKey();
                    string publicKey = rsa.GetPublicKey();

                    // Tạo nội dung file TXT
                    StringBuilder content = new StringBuilder();
                    content.AppendLine("NHẬT KÝ HỆ THỐNG");
                    content.AppendLine("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    content.AppendLine("Tổng số bản ghi: " + dtNhatKy.Rows.Count);
                    content.AppendLine();
                    content.AppendLine("Mã\tBảng\tThao tác\tKhóa chính\tNgười dùng\tSession\tIP\tThời gian\tMô tả\tGiá trị cũ\tGiá trị mới");

                    // Ghi dữ liệu
                    foreach (DataRow row in dtNhatKy.Rows)
                    {
                        content.AppendLine(
                            (row["AUDIT_ID"] ?? "").ToString() + "\t" +
                            (row["TABLE_NAME"] ?? "").ToString() + "\t" +
                            (row["OPERATION"] ?? "").ToString() + "\t" +
                            (row["PRIMARY_KEY_VALUE"] ?? "").ToString() + "\t" +
                            (row["USER_NAME"] ?? "").ToString() + "\t" +
                            (row["SESSION_USER"] ?? "").ToString() + "\t" +
                            (row["IP_ADDRESS"] ?? "").ToString() + "\t" +
                            (row["THOI_GIAN"] ?? "").ToString() + "\t" +
                            (row["DESCRIPTION"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ") + "\t" +
                            (row["GIA_TRI_CU"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ") + "\t" +
                            (row["GIA_TRI_MOI"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ")
                        );
                    }

                    string plainText = content.ToString();

                    // Mã hóa dữ liệu bằng RSA
                    byte[] encryptedData = rsa.EncryptLargeData(plainText);

                    // Lưu file mã hóa
                    string encryptedFileName = baseFileName + "_encrypted.txt";
                    File.WriteAllBytes(encryptedFileName, encryptedData);

                    // Lưu file key (private key để giải mã)
                    string keyFileName = baseFileName + "_key.txt";
                    File.WriteAllText(keyFileName, privateKey, Encoding.UTF8);

                    // Lưu file public key (để chia sẻ cho người khác mã hóa dữ liệu)
                    string publicKeyFileName = baseFileName + "_publickey.txt";
                    File.WriteAllText(publicKeyFileName, publicKey, Encoding.UTF8);

                    MessageBox.Show($"Xuất dữ liệu thành công!\n\n" +
                        $"File mã hóa: {Path.GetFileName(encryptedFileName)}\n" +
                        $"File key (private): {Path.GetFileName(keyFileName)}\n" +
                        $"File public key: {Path.GetFileName(publicKeyFileName)}\n\n" +
                        $"Lưu ý:\n" +
                        $"- Giữ an toàn file key (private) để có thể giải mã dữ liệu!\n" +
                        $"- File public key có thể chia sẻ để người khác mã hóa dữ liệu.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất dữ liệu: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace, 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"
                    SELECT 
                        TABLE_NAME AS BANG,
                        OPERATION AS THAO_TAC,
                        COUNT(*) AS SO_LUONG
                    FROM AUDIT_LOG
                    GROUP BY TABLE_NAME, OPERATION
                    ORDER BY TABLE_NAME, OPERATION";

                DataTable dtThongKe = new DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, connsql);
                adapter.Fill(dtThongKe);

                string thongKe = "THỐNG KÊ NHẬT KÝ\n\n";
                thongKe += "Bảng\t\tThao tác\tSố lượng\n";
                thongKe += "----------------------------------------\n";

                foreach (DataRow row in dtThongKe.Rows)
                {
                    thongKe += row["BANG"].ToString().PadRight(20) + "\t" +
                               row["THAO_TAC"].ToString().PadRight(10) + "\t" +
                               row["SO_LUONG"].ToString() + "\n";
                }

                MessageBox.Show(thongKe, "Thống kê", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaTatCa_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
                "CẢNH BÁO: Bạn có chắc chắn muốn XÓA TOÀN BỘ nhật ký không?\n\nHành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (r == DialogResult.No)
                return;

            try
            {
                if (connsql.State == ConnectionState.Closed)
                    connsql.Open();

                // Xóa toàn bộ dữ liệu trong bảng AUDIT_LOG
                string deleteStr = "DELETE FROM AUDIT_LOG";
                OracleCommand cmd = new OracleCommand(deleteStr, connsql);
                int rowsDeleted = cmd.ExecuteNonQuery();

                if (connsql.State == ConnectionState.Open)
                    connsql.Close();

                MessageBox.Show($"Đã xóa thành công {rowsDeleted} bản ghi nhật ký!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload lại dữ liệu
                LoadNhatKy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhật ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connsql.State == ConnectionState.Open)
                    connsql.Close();
            }
        }

        private void btnNhapTXT_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Chọn file mã hóa
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "TXT files (*_encrypted.txt)|*_encrypted.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file mã hóa (encrypted)";
                
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string encryptedFilePath = openFileDialog.FileName;

                // Bước 2: Chọn file key
                openFileDialog.Filter = "TXT files (*_key.txt)|*_key.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file key (private key)";
                
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string keyFilePath = openFileDialog.FileName;

                // Bước 3: Đọc file key
                string privateKey = File.ReadAllText(keyFilePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(privateKey))
                {
                    MessageBox.Show("File key không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Bước 4: Đọc và giải mã file encrypted
                byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
                if (encryptedData == null || encryptedData.Length == 0)
                {
                    MessageBox.Show("File mã hóa không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RSAEncryption rsa = new RSAEncryption();
                string decryptedText = rsa.DecryptLargeData(encryptedData, privateKey);

                if (string.IsNullOrWhiteSpace(decryptedText))
                {
                    MessageBox.Show("Không thể giải mã file! Vui lòng kiểm tra lại file key và file mã hóa.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Bước 5: Parse dữ liệu và insert vào database
                string[] lines = decryptedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                
                if (lines.Length < 5)
                {
                    MessageBox.Show("File không đúng định dạng! Thiếu dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Bỏ qua các dòng header (dòng 1-4: "NHẬT KÝ HỆ THỐNG", "Ngày xuất: ...", "Tổng số bản ghi: ...", dòng trống)
                // Dòng 5 là header row: "Mã\tBảng\tThao tác\t..."
                // Từ dòng 6 trở đi là dữ liệu

                int startDataRow = 5; // Bắt đầu từ dòng 6 (index 5)
                int successCount = 0;
                int errorCount = 0;
                List<string> errorMessages = new List<string>();

                if (connsql.State == ConnectionState.Closed)
                    connsql.Open();

                for (int i = startDataRow; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        // Parse dữ liệu theo format: Mã\tBảng\tThao tác\tKhóa chính\tNgười dùng\tSession\tIP\tThời gian\tMô tả\tGiá trị cũ\tGiá trị mới
                        string[] fields = line.Split('\t');
                        
                        if (fields.Length < 8) // Ít nhất phải có 8 trường
                        {
                            errorCount++;
                            errorMessages.Add($"Dòng {i + 1}: Thiếu dữ liệu (chỉ có {fields.Length} trường)");
                            continue;
                        }

                        // Lấy các giá trị
                        string tableName = fields.Length > 1 ? fields[1].Trim() : "";
                        string operation = fields.Length > 2 ? fields[2].Trim() : "";
                        string primaryKeyValue = fields.Length > 3 ? fields[3].Trim() : "";
                        string userName = fields.Length > 4 ? fields[4].Trim() : "";
                        string sessionUser = fields.Length > 5 ? fields[5].Trim() : "";
                        string ipAddress = fields.Length > 6 ? fields[6].Trim() : "";
                        string thoiGian = fields.Length > 7 ? fields[7].Trim() : "";
                        string description = fields.Length > 8 ? fields[8].Trim() : "";
                        string giaTriCu = fields.Length > 9 ? fields[9].Trim() : "";
                        string giaTriMoi = fields.Length > 10 ? fields[10].Trim() : "";

                        // Validate dữ liệu
                        if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(operation))
                        {
                            errorCount++;
                            errorMessages.Add($"Dòng {i + 1}: Thiếu tên bảng hoặc thao tác");
                            continue;
                        }

                        // Parse thời gian
                        DateTime timestamp;
                        if (!DateTime.TryParseExact(thoiGian, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out timestamp))
                        {
                            // Thử format khác nếu không được
                            if (!DateTime.TryParse(thoiGian, out timestamp))
                            {
                                timestamp = DateTime.Now; // Mặc định là thời gian hiện tại
                            }
                        }

                        // Insert vào database
                        string insertSql = @"INSERT INTO AUDIT_LOG (
                            TABLE_NAME, OPERATION, OLD_VALUES, NEW_VALUES, 
                            PRIMARY_KEY_VALUE, USER_NAME, SESSION_USER, IP_ADDRESS, 
                            TIMESTAMP, DESCRIPTION
                        ) VALUES (
                            :TABLE_NAME, :OPERATION, :OLD_VALUES, :NEW_VALUES,
                            :PRIMARY_KEY_VALUE, :USER_NAME, :SESSION_USER, :IP_ADDRESS,
                            :TIMESTAMP, :DESCRIPTION
                        )";

                        OracleCommand cmd = new OracleCommand(insertSql, connsql);
                        cmd.Parameters.Add(":TABLE_NAME", OracleDbType.Varchar2).Value = tableName;
                        cmd.Parameters.Add(":OPERATION", OracleDbType.Varchar2).Value = operation;
                        cmd.Parameters.Add(":OLD_VALUES", OracleDbType.Clob).Value = string.IsNullOrWhiteSpace(giaTriCu) ? (object)DBNull.Value : giaTriCu;
                        cmd.Parameters.Add(":NEW_VALUES", OracleDbType.Clob).Value = string.IsNullOrWhiteSpace(giaTriMoi) ? (object)DBNull.Value : giaTriMoi;
                        cmd.Parameters.Add(":PRIMARY_KEY_VALUE", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(primaryKeyValue) ? (object)DBNull.Value : primaryKeyValue;
                        cmd.Parameters.Add(":USER_NAME", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(userName) ? (object)DBNull.Value : userName;
                        cmd.Parameters.Add(":SESSION_USER", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(sessionUser) ? (object)DBNull.Value : sessionUser;
                        cmd.Parameters.Add(":IP_ADDRESS", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(ipAddress) ? (object)DBNull.Value : ipAddress;
                        cmd.Parameters.Add(":TIMESTAMP", OracleDbType.Date).Value = timestamp;
                        cmd.Parameters.Add(":DESCRIPTION", OracleDbType.NVarchar2).Value = string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description;

                        cmd.ExecuteNonQuery();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorMessages.Add($"Dòng {i + 1}: {ex.Message}");
                    }
                }

                if (connsql.State == ConnectionState.Open)
                    connsql.Close();

                // Hiển thị kết quả
                string resultMessage = $"Nhập dữ liệu hoàn tất!\n\n";
                resultMessage += $"Thành công: {successCount} bản ghi\n";
                resultMessage += $"Lỗi: {errorCount} bản ghi";

                if (errorCount > 0 && errorMessages.Count > 0)
                {
                    resultMessage += $"\n\nChi tiết lỗi (10 lỗi đầu tiên):\n";
                    int maxErrors = Math.Min(10, errorMessages.Count);
                    for (int i = 0; i < maxErrors; i++)
                    {
                        resultMessage += $"- {errorMessages[i]}\n";
                    }
                    if (errorMessages.Count > 10)
                    {
                        resultMessage += $"... và {errorMessages.Count - 10} lỗi khác";
                    }
                }

                MessageBox.Show(resultMessage, "Kết quả nhập dữ liệu", 
                    MessageBoxButtons.OK, 
                    successCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                // Reload lại dữ liệu
                if (successCount > 0)
                {
                    LoadNhatKy();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhập dữ liệu: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace, 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connsql.State == ConnectionState.Open)
                    connsql.Close();
            }
        }
    }
}

