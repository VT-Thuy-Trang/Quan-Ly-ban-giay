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

            // Thêm hướng dẫn xem lịch sử đăng nhập sai (ORA-1017) từ sys.sql
            AddAuditGuidePanel();
        }

        /// <summary>
        /// Hiển thị hướng dẫn truy vấn xem lịch sử đăng nhập sai (ORA-1017) như trong sys.sql (69-75)
        /// </summary>
        private void AddAuditGuidePanel()
        {
            const string guideText =
@"Xem lịch sử đăng nhập sai (ORA-1017):
SELECT username, timestamp, returncode, client_identifier, os_username
FROM dba_audit_trail
WHERE returncode = 1017  -- invalid username/password
ORDER BY timestamp DESC;";

            var txtGuide = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = guideText,
                BackColor = Color.AliceBlue,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Height = 80,
                Width = this.Width - 60,
                Left = 20,
                Top = this.Height - 140
            };

            // Đảm bảo khi resize form, textbox giữ vị trí hợp lý
            this.Controls.Add(txtGuide);
            this.Resize += (s, e) =>
            {
                txtGuide.Width = this.ClientSize.Width - 40;
                txtGuide.Top = this.ClientSize.Height - txtGuide.Height - 20;
            };
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
                // Bước 1: Chọn loại mã hóa
                string method = ShowEncryptionOption("Chọn loại mã hóa của file");
                if (string.IsNullOrEmpty(method))
                    return;

                // Bước 2: Chọn file mã hóa
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string filter = "";
                string title = "";
                
                if (method == "RSA")
                {
                    filter = "Encrypted files (*.rsa)|*.rsa|Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                    title = "Chọn file mã hóa RSA";
                }
                else if (method == "AES")
                {
                    filter = "Encrypted files (*.aes)|*.aes|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                    title = "Chọn file mã hóa AES";
                }
                else if (method == "HYBRID")
                {
                    filter = "Encrypted files (*.hybrid)|*.hybrid|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                    title = "Chọn file mã hóa Hybrid";
                }
                
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;
                
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string encryptedFilePath = openFileDialog.FileName;

                // Bước 3: Chọn file key (nếu cần - chỉ RSA và Hybrid cần key)
                string privateKey = null;
                if (method == "RSA" || method == "HYBRID")
                {
                    openFileDialog.Filter = "Key files (*.key)|*.key|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Title = "Chọn file key (private key)";
                    
                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    string keyFilePath = openFileDialog.FileName;
                    privateKey = File.ReadAllText(keyFilePath, Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(privateKey))
                    {
                        MessageBox.Show("File key không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Bước 4: Đọc và giải mã file theo loại mã hóa
                string decryptedText = "";
                
                if (method == "AES")
                {
                    // Đọc file text và giải mã bằng AES
                    string encryptedText = File.ReadAllText(encryptedFilePath, Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(encryptedText))
                    {
                        MessageBox.Show("File mã hóa không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    decryptedText = MaHoa.Decrypt(encryptedText);
                }
                else if (method == "RSA")
                {
                    // Đọc file binary và giải mã bằng RSA
                    byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
                    if (encryptedData == null || encryptedData.Length == 0)
                    {
                        MessageBox.Show("File mã hóa không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    RSAEncryption rsa = new RSAEncryption();
                    decryptedText = rsa.DecryptLargeData(encryptedData, privateKey);
                }
                else if (method == "HYBRID")
                {
                    // Đọc file text và giải mã bằng Hybrid
                    string encryptedText = File.ReadAllText(encryptedFilePath, Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(encryptedText))
                    {
                        MessageBox.Show("File mã hóa không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    // Hybrid cần private key để giải mã AES key
                    decryptedText = DecryptHybridWithKey(encryptedText, privateKey);
                }

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

        //Hàm hiển thị hộp thoại chọn phương thức mã hóa
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

        //Hàm lấy nội dung nhật ký từ DataGridView
        private string GetLogContent()
        {
            if (dtNhatKy == null || dtNhatKy.Rows.Count == 0) return string.Empty;

            StringBuilder content = new StringBuilder();
            content.AppendLine("NHẬT KÝ HỆ THỐNG");
            content.AppendLine("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            content.AppendLine("Tổng số bản ghi: " + dtNhatKy.Rows.Count);
            content.AppendLine();
            content.AppendLine("Mã\tBảng\tThao tác\tKhóa chính\tNgười dùng\tSession\tIP\tThời gian\tMô tả\tGiá trị cũ\tGiá trị mới");

            foreach (DataRow row in dtNhatKy.Rows)
            {
                // Làm sạch các ký tự xuống dòng để giữ format tab
                string desc = (row["DESCRIPTION"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ");
                string oldVal = (row["GIA_TRI_CU"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ");
                string newVal = (row["GIA_TRI_MOI"] ?? "").ToString().Replace("\n", " ").Replace("\r", " ");

                content.AppendLine(
                    $"{row["AUDIT_ID"]}\t{row["TABLE_NAME"]}\t{row["OPERATION"]}\t{row["PRIMARY_KEY_VALUE"]}\t" +
                    $"{row["USER_NAME"]}\t{row["SESSION_USER"]}\t{row["IP_ADDRESS"]}\t{row["THOI_GIAN"]}\t" +
                    $"{desc}\t{oldVal}\t{newVal}"
                );
            }
            return content.ToString();
        }

        private void btnXuatFileMaHoa_Click(object sender, EventArgs e)
        {
            string plainText = GetLogContent();
            if (string.IsNullOrEmpty(plainText))
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string method = ShowEncryptionOption("Chọn phương thức mã hóa file");
            if (string.IsNullOrEmpty(method)) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = $"Encrypted {method}|*.{method.ToLower()}|All files|*.*";
            saveFileDialog.FileName = $"NhatKy_{method}_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string encryptedData = "";
                    string extraInfo = ""; 

                    if (method == "AES")
                    {
                        encryptedData = MaHoa.Encrypt(plainText);
                    }
                    else if (method == "RSA")
                    {
                        RSAEncryption rsa = new RSAEncryption();
                        byte[] encBytes = rsa.EncryptLargeData(plainText);
                        File.WriteAllBytes(saveFileDialog.FileName, encBytes);

                        // Xuất thêm Key để người dùng có thể giải mã
                        File.WriteAllText(saveFileDialog.FileName + ".key", rsa.GetPrivateKey());
                        extraInfo = "\n\nĐã xuất kèm file Private Key (.key). Hãy giữ bí mật file này!";

                        MessageBox.Show("Xuất file RSA thành công!" + extraInfo);
                        return; // RSA lưu file binary nên return luôn
                    }
                    else if (method == "HYBRID")
                    {
                        // Hybrid cần RSA để mã hóa AES key, nên cần xuất private key
                        encryptedData = EncryptHybridWithKey(plainText, out string privateKey);
                        
                        // Xuất file mã hóa
                        File.WriteAllText(saveFileDialog.FileName, encryptedData, Encoding.UTF8);
                        
                        // Xuất file key (private key để giải mã)
                        File.WriteAllText(saveFileDialog.FileName + ".key", privateKey, Encoding.UTF8);
                        extraInfo = "\n\nĐã xuất kèm file Private Key (.key). Hãy giữ bí mật file này!";
                        
                        MessageBox.Show($"Xuất file Hybrid thành công!{extraInfo}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Hybrid đã xử lý xong nên return
                    }

                    File.WriteAllText(saveFileDialog.FileName, encryptedData, Encoding.UTF8);
                    MessageBox.Show($"Xuất file ({method}) thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Hàm mã hóa Hybrid và trả về private key
        private string EncryptHybridWithKey(string plainText, out string privateKey)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();

                    // 1. Mã hóa dữ liệu bằng AES
                    string aesEncryptedData;
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        aesEncryptedData = Convert.ToBase64String(ms.ToArray());
                    }

                    // 2. Mã hóa Key AES bằng RSA
                    RSAEncryption rsa = new RSAEncryption();
                    string aesKeyString = Convert.ToBase64String(aes.Key);

                    byte[] encryptedKeyBytes = rsa.Encrypt(aesKeyString);
                    string encryptedAesKey = Convert.ToBase64String(encryptedKeyBytes);

                    // Lấy private key để trả về
                    privateKey = rsa.GetPrivateKey();

                    return $"{encryptedAesKey}||{aesEncryptedData}";
                }
            }
            catch (Exception ex)
            {
                privateKey = "";
                MessageBox.Show("Lỗi khi mã hóa Hybrid: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return plainText;
            }
        }

        // Hàm giải mã Hybrid với private key từ file
        private string DecryptHybridWithKey(string hybridCipherText, string privateKeyXml)
        {
            try
            {
                string[] parts = hybridCipherText.Split(new[] { "||" }, StringSplitOptions.None);
                if (parts.Length != 2) return hybridCipherText;

                string encryptedAesKey = parts[0];
                string aesEncryptedData = parts[1];

                // Giải mã Key AES bằng RSA với private key từ file
                RSAEncryption rsa = new RSAEncryption();
                
                // Chuyển string Base64 về byte[] để giải mã
                byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedAesKey);
                string aesKeyBase64 = rsa.Decrypt(encryptedKeyBytes, privateKeyXml);
                byte[] aesKey = Convert.FromBase64String(aesKeyBase64);

                // Giải mã dữ liệu AES
                byte[] fullCipher = Convert.FromBase64String(aesEncryptedData);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKey;
                    byte[] iv = new byte[16];
                    Array.Copy(fullCipher, 0, iv, 0, 16);
                    aes.IV = iv;

                    using (MemoryStream ms = new MemoryStream(fullCipher, 16, fullCipher.Length - 16))
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi giải mã Hybrid: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return hybridCipherText;
            }
        }

        private void btnXuatFileGiaiMa_Click(object sender, EventArgs e)
        {
            string plainText = GetLogContent();
            if (string.IsNullOrEmpty(plainText))
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt|All files|*.*";
            saveFileDialog.FileName = $"NhatKy_Plain_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, plainText, Encoding.UTF8);
                    MessageBox.Show("Xuất file nhật ký (dạng thường) thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message);
                }
            }
        }
    }
}

