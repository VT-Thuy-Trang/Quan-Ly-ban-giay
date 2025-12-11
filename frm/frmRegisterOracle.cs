using QL_GiayTT.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_GiayTT.frm
{
    public partial class frmRegisterOracle : Form
    {
        public frmRegisterOracle()
        {
            InitializeComponent();
            // Thiết lập giá trị mặc định cho thông tin DBA
            txtDBAUser.Text = "SYS";
        }

        // Method để điền sẵn thông tin kết nối (tùy chọn)
        public void SetConnectionInfo(string host, string port, string sid)
        {
            txtHost.Text = host;
            txtPort.Text = port;
            txtSID.Text = sid;
        }

        private void btnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            // Validation thông tin kết nối
            if (string.IsNullOrWhiteSpace(txtHost.Text))
            {
                MessageBox.Show("Vui lòng nhập Host!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHost.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                MessageBox.Show("Vui lòng nhập Port!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSID.Text))
            {
                MessageBox.Show("Vui lòng nhập SID!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSID.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDBAUser.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản DBA (SYS hoặc SYSTEM)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDBAUser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDBAPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu DBA!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDBAPassword.Focus();
                return;
            }

            // Validation thông tin user mới
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                txtConfirmPassword.SelectAll();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Kiểm tra tên user hợp lệ (Oracle không cho phép một số ký tự đặc biệt)
            string username = txtUsername.Text.Trim().ToUpper();
            if (username.Contains(" ") || username.Contains("'") || username.Contains("\"") || username.Contains(";"))
            {
                MessageBox.Show("Tên đăng nhập không được chứa khoảng trắng hoặc ký tự đặc biệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            // Kết nối với DBA account
            KetNoi ketNoiDBA = new KetNoi(txtHost.Text.Trim(), txtPort.Text.Trim(), txtSID.Text.Trim(), 
                                          txtDBAUser.Text.Trim(), txtDBAPassword.Text);
            
            if (!ketNoiDBA.connect())
            {
                MessageBox.Show("Không thể kết nối đến Oracle với tài khoản DBA!\n\n" +
                              "Vui lòng kiểm tra lại:\n" +
                              "- Host, Port, SID\n" +
                              "- Tên đăng nhập và mật khẩu DBA (SYS hoặc SYSTEM)", 
                    "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OracleConnection conn = ketNoiDBA.GetConnection();

            try
            {
                // Kiểm tra user đã tồn tại chưa
                string checkUserSQL = "SELECT COUNT(*) FROM dba_users WHERE username = :username";
                using (OracleCommand cmdCheck = new OracleCommand(checkUserSQL, conn))
                {
                    cmdCheck.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    
                    if (count > 0)
                    {
                        MessageBox.Show($"Tài khoản '{username}' đã tồn tại!\n\nVui lòng chọn tên đăng nhập khác.", 
                            "Tài khoản đã tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                        return;
                    }
                }

                // Bắt đầu transaction
                OracleTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Tạo user mới
                    string createUserSQL = $"CREATE USER {username} IDENTIFIED BY \"{txtPassword.Text}\"";
                    using (OracleCommand cmd = new OracleCommand(createUserSQL, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Cấp quyền CONNECT và RESOURCE
                    string grantConnectSQL = $"GRANT CONNECT, RESOURCE TO {username}";
                    using (OracleCommand cmd = new OracleCommand(grantConnectSQL, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Cấp quyền CREATE SESSION (cần thiết để đăng nhập)
                    string grantSessionSQL = $"GRANT CREATE SESSION TO {username}";
                    using (OracleCommand cmd = new OracleCommand(grantSessionSQL, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 4. Tìm schema owner (ưu tiên NgNam, nếu không có thì tìm schema có chứa bảng DANGNHAP)
                    string schemaOwner = null;
                    
                    // Ưu tiên tìm schema NgNam trước
                    string checkNgNamSQL = "SELECT COUNT(*) FROM dba_users WHERE username = 'NGNAM'";
                    using (OracleCommand cmdCheck = new OracleCommand(checkNgNamSQL, conn))
                    {
                        cmdCheck.Transaction = transaction;
                        int ngNamExists = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (ngNamExists > 0)
                        {
                            schemaOwner = "NGNAM";
                        }
                    }
                    
                    // Nếu không tìm thấy NgNam, tìm schema có chứa bảng DANGNHAP
                    if (string.IsNullOrEmpty(schemaOwner))
                    {
                        string findSchemaSQL = @"
                            SELECT DISTINCT owner 
                            FROM dba_tables 
                            WHERE table_name = 'DANGNHAP' 
                            AND owner NOT IN ('SYS', 'SYSTEM', 'SYSMAN', 'DBSNMP', 'OUTLN', 'XS$NULL')
                            AND rownum = 1";
                        
                        using (OracleCommand cmdFind = new OracleCommand(findSchemaSQL, conn))
                        {
                            cmdFind.Transaction = transaction;
                            object result = cmdFind.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                schemaOwner = result.ToString().ToUpper();
                            }
                        }
                    }
                    
                    // Nếu vẫn không tìm thấy, sử dụng tên user DBA (trừ khi là SYS hoặc SYSTEM)
                    if (string.IsNullOrEmpty(schemaOwner))
                    {
                        string dbaUser = ketNoiDBA.User.ToUpper();
                        if (dbaUser != "SYS" && dbaUser != "SYSTEM")
                        {
                            schemaOwner = dbaUser;
                        }
                    }
                    
                    // Chỉ cấp quyền nếu tìm thấy schema owner
                    if (!string.IsNullOrEmpty(schemaOwner))
                    {
                        // 4.1. Cấp quyền trên TẤT CẢ các bảng trong schema
                        string getAllTablesSQL = @"
                            SELECT table_name 
                            FROM dba_tables 
                            WHERE owner = :owner 
                            AND table_name NOT LIKE 'BIN$%'  -- Bỏ qua các bảng đã drop (trong recycle bin)
                            ORDER BY table_name";
                        
                        using (OracleCommand cmdTables = new OracleCommand(getAllTablesSQL, conn))
                        {
                            cmdTables.Transaction = transaction;
                            cmdTables.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdTables.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string tableName = reader["table_name"].ToString();
                                    // Cấp quyền SELECT, INSERT, UPDATE, DELETE trên bảng
                                    string grantTableSQL = $"GRANT SELECT, INSERT, UPDATE, DELETE ON {schemaOwner}.{tableName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantTableSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // 4.2. Cấp quyền EXECUTE trên TẤT CẢ các Packages trong schema
                        string getAllPackagesSQL = @"
                            SELECT object_name 
                            FROM dba_objects 
                            WHERE owner = :owner 
                            AND object_type = 'PACKAGE'
                            AND object_name NOT LIKE 'BIN$%'
                            ORDER BY object_name";
                        
                        using (OracleCommand cmdPackages = new OracleCommand(getAllPackagesSQL, conn))
                        {
                            cmdPackages.Transaction = transaction;
                            cmdPackages.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdPackages.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string packageName = reader["object_name"].ToString();
                                    string grantPackageSQL = $"GRANT EXECUTE ON {schemaOwner}.{packageName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantPackageSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // 4.3. Cấp quyền EXECUTE trên TẤT CẢ các Procedures trong schema
                        string getAllProceduresSQL = @"
                            SELECT object_name 
                            FROM dba_procedures 
                            WHERE owner = :owner 
                            AND object_type = 'PROCEDURE'
                            AND procedure_name NOT LIKE 'BIN$%'
                            GROUP BY object_name
                            ORDER BY object_name";
                        
                        using (OracleCommand cmdProcedures = new OracleCommand(getAllProceduresSQL, conn))
                        {
                            cmdProcedures.Transaction = transaction;
                            cmdProcedures.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdProcedures.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string procedureName = reader["object_name"].ToString();
                                    string grantProcedureSQL = $"GRANT EXECUTE ON {schemaOwner}.{procedureName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantProcedureSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // 4.4. Cấp quyền EXECUTE trên TẤT CẢ các Functions trong schema
                        string getAllFunctionsSQL = @"
                            SELECT object_name 
                            FROM dba_procedures 
                            WHERE owner = :owner 
                            AND object_type = 'FUNCTION'
                            AND procedure_name NOT LIKE 'BIN$%'
                            GROUP BY object_name
                            ORDER BY object_name";
                        
                        using (OracleCommand cmdFunctions = new OracleCommand(getAllFunctionsSQL, conn))
                        {
                            cmdFunctions.Transaction = transaction;
                            cmdFunctions.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdFunctions.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string functionName = reader["object_name"].ToString();
                                    string grantFunctionSQL = $"GRANT EXECUTE ON {schemaOwner}.{functionName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantFunctionSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // 4.5. Cấp quyền SELECT trên TẤT CẢ các Sequences trong schema
                        string getAllSequencesSQL = @"
                            SELECT sequence_name 
                            FROM dba_sequences 
                            WHERE sequence_owner = :owner
                            ORDER BY sequence_name";
                        
                        using (OracleCommand cmdSequences = new OracleCommand(getAllSequencesSQL, conn))
                        {
                            cmdSequences.Transaction = transaction;
                            cmdSequences.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdSequences.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string sequenceName = reader["sequence_name"].ToString();
                                    string grantSequenceSQL = $"GRANT SELECT ON {schemaOwner}.{sequenceName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantSequenceSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // 4.6. Cấp quyền SELECT trên TẤT CẢ các Views trong schema
                        string getAllViewsSQL = @"
                            SELECT view_name 
                            FROM dba_views 
                            WHERE owner = :owner
                            AND view_name NOT LIKE 'BIN$%'
                            ORDER BY view_name";
                        
                        using (OracleCommand cmdViews = new OracleCommand(getAllViewsSQL, conn))
                        {
                            cmdViews.Transaction = transaction;
                            cmdViews.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            using (OracleDataReader reader = cmdViews.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string viewName = reader["view_name"].ToString();
                                    string grantViewSQL = $"GRANT SELECT ON {schemaOwner}.{viewName} TO {username}";
                                    using (OracleCommand cmdGrant = new OracleCommand(grantViewSQL, conn))
                                    {
                                        cmdGrant.Transaction = transaction;
                                        cmdGrant.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }

                    // 5. Cấp quyền truy cập các SYS packages cần thiết cho ứng dụng
                    // (Cần thiết cho mã hóa AES, VPD, FGA - như trong sys.sql)
                    try
                    {
                        // DBMS_CRYPTO - Cần cho mã hóa AES
                        string grantCryptoSQL = "GRANT EXECUTE ON SYS.DBMS_CRYPTO TO " + username;
                        using (OracleCommand cmdGrant = new OracleCommand(grantCryptoSQL, conn))
                        {
                            cmdGrant.Transaction = transaction;
                            cmdGrant.ExecuteNonQuery();
                        }
                    }
                    catch (OracleException)
                    {
                        // Bỏ qua nếu không có quyền cấp (không phải tất cả DBA đều có quyền này)
                    }

                    // 6. Gán PROFILE bảo mật (nếu có)
                    string checkProfileSQL = "SELECT COUNT(*) FROM dba_profiles WHERE profile = 'PROFILE_QLGIAYTT'";
                    using (OracleCommand cmdCheck = new OracleCommand(checkProfileSQL, conn))
                    {
                        cmdCheck.Transaction = transaction;
                        int profileCount = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        
                        if (profileCount > 0)
                        {
                            string alterProfileSQL = $"ALTER USER {username} PROFILE PROFILE_QLGIAYTT";
                            using (OracleCommand cmdProfile = new OracleCommand(alterProfileSQL, conn))
                            {
                                cmdProfile.Transaction = transaction;
                                cmdProfile.ExecuteNonQuery();
                            }
                        }
                    }

                    // Commit transaction
                    transaction.Commit();

                    // Đếm số lượng objects đã cấp quyền
                    int tableCount = 0, packageCount = 0, procedureCount = 0, functionCount = 0, sequenceCount = 0, viewCount = 0;
                    
                    if (!string.IsNullOrEmpty(schemaOwner))
                    {
                        // Đếm bảng
                        string countTablesSQL = "SELECT COUNT(*) FROM dba_tables WHERE owner = :owner AND table_name NOT LIKE 'BIN$%'";
                        using (OracleCommand cmd = new OracleCommand(countTablesSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // Đếm packages
                        string countPackagesSQL = "SELECT COUNT(*) FROM dba_objects WHERE owner = :owner AND object_type = 'PACKAGE' AND object_name NOT LIKE 'BIN$%'";
                        using (OracleCommand cmd = new OracleCommand(countPackagesSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            packageCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // Đếm procedures
                        string countProceduresSQL = "SELECT COUNT(DISTINCT object_name) FROM dba_procedures WHERE owner = :owner AND object_type = 'PROCEDURE' AND procedure_name NOT LIKE 'BIN$%'";
                        using (OracleCommand cmd = new OracleCommand(countProceduresSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            procedureCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // Đếm functions
                        string countFunctionsSQL = "SELECT COUNT(DISTINCT object_name) FROM dba_procedures WHERE owner = :owner AND object_type = 'FUNCTION' AND procedure_name NOT LIKE 'BIN$%'";
                        using (OracleCommand cmd = new OracleCommand(countFunctionsSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            functionCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // Đếm sequences
                        string countSequencesSQL = "SELECT COUNT(*) FROM dba_sequences WHERE sequence_owner = :owner";
                        using (OracleCommand cmd = new OracleCommand(countSequencesSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            sequenceCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // Đếm views
                        string countViewsSQL = "SELECT COUNT(*) FROM dba_views WHERE owner = :owner AND view_name NOT LIKE 'BIN$%'";
                        using (OracleCommand cmd = new OracleCommand(countViewsSQL, conn))
                        {
                            cmd.Parameters.Add(":owner", OracleDbType.Varchar2).Value = schemaOwner;
                            viewCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    
                    string quyenInfo = $"- CONNECT, RESOURCE, CREATE SESSION\n";
                    if (!string.IsNullOrEmpty(schemaOwner))
                    {
                        quyenInfo += $"- Schema: {schemaOwner}\n";
                        quyenInfo += $"- {tableCount} bảng (SELECT, INSERT, UPDATE, DELETE)\n";
                        quyenInfo += $"- {packageCount} packages (EXECUTE)\n";
                        quyenInfo += $"- {procedureCount} procedures (EXECUTE)\n";
                        quyenInfo += $"- {functionCount} functions (EXECUTE)\n";
                        quyenInfo += $"- {sequenceCount} sequences (SELECT)\n";
                        quyenInfo += $"- {viewCount} views (SELECT)";
                    }
                    
                    MessageBox.Show($"Tạo tài khoản '{username}' thành công!\n\n" +
                                  $"Thông tin đăng nhập:\n" +
                                  $"Host: {txtHost.Text}\n" +
                                  $"Port: {txtPort.Text}\n" +
                                  $"SID: {txtSID.Text}\n" +
                                  $"Username: {username}\n" +
                                  $"\nTài khoản đã được cấp các quyền:\n" +
                                  quyenInfo,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Đóng form sau khi tạo thành công
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (OracleException oraEx)
                {
                    transaction.Rollback();
                    
                    string errorMsg = "Lỗi khi tạo tài khoản Oracle:\n\n";
                    switch (oraEx.Number)
                    {
                        case 1920: // User đã tồn tại
                            errorMsg += "Tài khoản đã tồn tại!";
                            break;
                        case 1031: // Insufficient privileges
                            errorMsg += "Không đủ quyền để tạo user!\n\nVui lòng đăng nhập với tài khoản có quyền DBA (SYS hoặc SYSTEM).";
                            break;
                        default:
                            errorMsg += $"Mã lỗi: {oraEx.Number}\n{oraEx.Message}";
                            break;
                    }
                    
                    MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Đóng kết nối
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMatKhau.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtConfirmPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtConfirmPassword.PasswordChar = '*';
            }
        }

        private void chkHienDBAPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienDBAPassword.Checked)
                txtDBAPassword.PasswordChar = '\0';
            else
                txtDBAPassword.PasswordChar = '*';
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép chữ, số, và dấu gạch dưới
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép Enter để submit
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTaoTaiKhoan_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}

