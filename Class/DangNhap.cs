using Oracle.ManagedDataAccess.Client;
using QL_GiayTT.Class;
using System.Data;
using System; // Thêm vào để dùng StringComparison

namespace QL_GiayTT.Class
{
    public class DangNhap
    {
        private OracleConnection conn;

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }

        public DangNhap(KetNoi kn)
        {
            // Lấy kết nối từ class KetNoi
            conn = kn.GetConnection();
        }

        public DangNhap(KetNoi kn, string tk, string mk)
        {
            conn = kn.GetConnection();
            TaiKhoan = tk;
            MatKhau = mk;
        }

        public int KiemTraDangNhap()
        {
            // Nếu người dùng không nhập gì
            if (string.IsNullOrEmpty(TaiKhoan) || string.IsNullOrEmpty(MatKhau))
                return -1; // Trả về -1 cho trường hợp rỗng

            // Chỉ cần 1 câu lệnh SQL để lấy thông tin
            string sql = @"
                SELECT MATKHAU, LOAITK 
                FROM DANGNHAP 
                WHERE TAIKHOAN = :tk";

            // Luôn sử dụng 'using' cho Command và Reader để đảm bảo chúng được đóng đúng cách
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(":tk", OracleDbType.Varchar2).Value = TaiKhoan;

                try
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        // 1. Kiểm tra xem tài khoản có tồn tại không
                        if (reader.Read())
                        {
                            // Tài khoản tồn tại, lấy mật khẩu và loại TK từ CSDL
                            string dbMatKhau = reader["MATKHAU"].ToString();
                            string dbLoaiTK = reader["LOAITK"].ToString();
                            
                            // 2. So sánh mật khẩu người dùng nhập với mật khẩu trong CSDL
                            bool matKhauDung = false;
                            
                            // Kiểm tra xem mật khẩu trong DB có phải Base64 (đã mã hóa) không
                            if (IsBase64(dbMatKhau))
                            {
                                // Mật khẩu trong DB đã mã hóa, cần mã hóa mật khẩu người dùng nhập để so sánh
                                string inputEncrypted = MaHoa.Encrypt(this.MatKhau);
                                matKhauDung = (dbMatKhau == inputEncrypted);
                            }
                            else
                            {
                                // Mật khẩu trong DB chưa mã hóa, so sánh trực tiếp
                                matKhauDung = (dbMatKhau == this.MatKhau);
                            }
                            
                            if (matKhauDung)
                            {
                                // Mật khẩu đúng, kiểm tra loại tài khoản
                                if (dbLoaiTK.Equals("admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    return 1; // Đăng nhập thành công với tư cách Admin
                                }
                                else
                                {
                                    return 2; // Đăng nhập thành công với tư cách User
                                }
                            }
                            else
                            {
                                // Mật khẩu sai
                                return 3; // Tài khoản tồn tại nhưng sai mật khẩu
                            }
                        }
                        else
                        {
                            // Không tìm thấy dòng nào, nghĩa là tài khoản không tồn tại
                            return 0; // Tài khoản không tồn tại
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi CSDL (ví dụ: mất kết nối)
                    System.Windows.Forms.MessageBox.Show("Lỗi CSDL: " + ex.Message);
                    return -2; // Trả về mã lỗi khác
                }
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
    }
}