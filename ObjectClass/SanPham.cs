using Oracle.ManagedDataAccess.Client;
using QL_GiayTT.Class;
using System;

namespace QL_GiayTT.ObjectClass
{
    public class SanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public double GiaBan { get; set; }
        public string GioiTinh { get; set; }
        public string ThongTinSP { get; set; }
        public string ChatLieu { get; set; }
        public string Form { get; set; }
        public int SoLuongTon { get; set; }
        public int DaBan { get; set; }
        public string TinhTrang { get; set; }
        public string MaL { get; set; }

        public void layThongTinSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                throw new ArgumentException("Mã sản phẩm không hợp lệ", nameof(maSP));

            var connection = OracleSession.Connection;
            string query = @"SELECT MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, 
                                     SoLuongTon, DaBan, TinhTrang, MaL
                              FROM SANPHAM
                              WHERE MaSP = :MaSP";

            using (OracleCommand cmd = new OracleCommand(query, connection))
            {
                cmd.Parameters.Add(":MaSP", OracleDbType.Varchar2).Value = maSP;
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MaSP = reader["MaSP"]?.ToString();
                        TenSP = reader["TenSP"]?.ToString();
                        GioiTinh = reader["GioiTinh"]?.ToString();
                        ThongTinSP = reader["ThongTinSP"]?.ToString();
                        ChatLieu = reader["ChatLieu"]?.ToString();
                        Form = reader["KichThuoc"]?.ToString();
                        TinhTrang = reader["TinhTrang"]?.ToString();
                        MaL = reader["MaL"]?.ToString();

                        GiaBan = reader["GiaBan"] == DBNull.Value ? 0 : Convert.ToDouble(reader["GiaBan"]);
                        SoLuongTon = reader["SoLuongTon"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SoLuongTon"]);
                        DaBan = reader["DaBan"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DaBan"]);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Không tìm thấy sản phẩm với mã {maSP}");
                    }
                }
            }
        }
    }
}

