using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Thêm thư viện này để dùng MessageBox
using System.Windows.Forms;

namespace QL_GiayTT.Class
{
    public class KetNoi
    {
        OracleConnection con = new OracleConnection();
        public string Host { get; set; }
        public string Port { get; set; }
        public string Sid { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DefaultSchema { get; set; } = "NGNAM";
        public KetNoi() { }
        public OracleConnection Connection { get; private set; }

        public KetNoi(string host, string port, string sid, string user, string password)
        {
            Host = host;
            Port = port;
            Sid = sid;
            User = user;
            Password = password;
        }

        public bool connect()
        {
            string connectionString = "Data Source=" + Host + ":" + Port + "/" + Sid + ";User Id=" + User + ";Password=" + Password + ";";

            // Cải tiến: Kiểm tra "sys" không phân biệt chữ hoa/thường
            if (User.Equals("sys", StringComparison.OrdinalIgnoreCase))
            {
                connectionString += "DBA Privilege=SYSDBA;";
            }

            Connection = new OracleConnection(connectionString);
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();

                // Đặt schema mặc định để các câu lệnh không cần prefix schema
                // Giúp user mới truy cập được bảng/proc của NgNam mà không cần quyền CREATE SYNONYM
                if (!string.IsNullOrWhiteSpace(DefaultSchema))
                {
                    try
                    {
                        using (var cmd = new OracleCommand($"ALTER SESSION SET CURRENT_SCHEMA={DefaultSchema}", Connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        // Bỏ qua nếu không đặt được schema, tránh làm hỏng kết nối
                    }
                }
                return true;
            }
            catch (OracleException oraEx)
            {
                // Xử lý các lỗi Oracle cụ thể
                string errorMessage = "";
                string errorTitle = "Lỗi Kết Nối Oracle";
                
                switch (oraEx.Number)
                {
                    case 28000: // Tài khoản bị khóa
                        errorMessage = $"Tài khoản '{User}' đã bị khóa!\n\n" +
                                      "Nguyên nhân có thể:\n" +
                                      "- Đăng nhập sai mật khẩu quá 3 lần\n" +
                                      "- Tài khoản đã hết hạn\n\n" +
                                      "Vui lòng liên hệ quản trị viên để mở khóa tài khoản.";
                        errorTitle = "Tài Khoản Bị Khóa";
                        break;
                    
                    case 1017: // Sai username hoặc password
                        errorMessage = "Sai tên đăng nhập hoặc mật khẩu!\n\n" +
                                      "Vui lòng kiểm tra lại thông tin đăng nhập.";
                        errorTitle = "Đăng Nhập Không Thành Công";
                        break;
                    
                    case 12514: // TNS: không thể giải quyết tên dịch vụ
                        errorMessage = $"Không thể kết nối đến server Oracle!\n\n" +
                                      $"Kiểm tra lại:\n" +
                                      $"- Host: {Host}\n" +
                                      $"- Port: {Port}\n" +
                                      $"- SID: {Sid}\n\n" +
                                      "Đảm bảo Oracle Database đang chạy.";
                        errorTitle = "Lỗi Kết Nối";
                        break;
                    
                    case 12541: // TNS: không có listener
                        errorMessage = "Không tìm thấy Oracle Listener!\n\n" +
                                      "Đảm bảo Oracle Listener đang chạy trên server.";
                        errorTitle = "Lỗi Listener";
                        break;
                    
                    case 12154: // TNS: không thể phân giải tên kết nối
                        errorMessage = "Không thể phân giải tên kết nối!\n\n" +
                                      "Kiểm tra lại thông tin kết nối (Host, Port, SID).";
                        errorTitle = "Lỗi Kết Nối";
                        break;
                    
                    default:
                        errorMessage = $"Lỗi Oracle (Code: {oraEx.Number}):\n{oraEx.Message}";
                        break;
                }
                
                MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối: " + e.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public OracleConnection GetConnection()
        {
            return Connection;
        }
    }
}