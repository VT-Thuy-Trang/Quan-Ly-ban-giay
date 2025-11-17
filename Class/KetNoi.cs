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
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi Oracle: " + e.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public OracleConnection GetConnection()
        {
            return Connection;
        }
    }
}