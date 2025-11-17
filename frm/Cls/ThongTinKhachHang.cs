using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using QL_GiayTT.Class;
using System.Data;

namespace QL_GiayTT.ObjectClass
{
    class ThongTinKhachHang
    {
        private static ThongTinKhachHang instance;
        private DangNhap TK { get; set; }
        public KhachHang kh;

        private ThongTinKhachHang()
        {
            kh = new KhachHang();
        }

        public static ThongTinKhachHang Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThongTinKhachHang();
                }
                return instance;
            }
        }

        OracleConnection connsql;

        public void SetDangNhap(DangNhap dangNhap)
        {
            TK = dangNhap;
            connsql = OracleSession.Connection;
            //Lấy mã tài khoản
            string selectStr = "select MaTK from DANGNHAP where TAIKHOAN = :TaiKhoan and MATKHAU = :MatKhau";
            if (connsql.State == ConnectionState.Closed)
                connsql.Open();
            OracleCommand cmd = new OracleCommand(selectStr, connsql);
            cmd.Parameters.Add(":TaiKhoan", OracleDbType.Varchar2).Value = TK.TaiKhoan;
            cmd.Parameters.Add(":MatKhau", OracleDbType.Varchar2).Value = TK.MatKhau;
            DataTable dtTable = new DataTable();
            dtTable.Load(cmd.ExecuteReader());
            DataRow row = dtTable.Rows[0];
            string maTK = row["MaTK"].ToString();

            //Lấy thông tin khách hàng từ mã tài khoản
            selectStr = "select * from KHACHHANG where MaTk = :MaTK";
            cmd = new OracleCommand(selectStr, connsql);
            cmd.Parameters.Add(":MaTK", OracleDbType.Varchar2).Value = maTK;
            dtTable = new DataTable();
            dtTable.Load(cmd.ExecuteReader());
            row = dtTable.Rows[0];
            kh.MaKH = row["MaKH"].ToString();
            kh.TenKH = row["TenKH"].ToString();
            //kh.DiaChiKH = row["DiaChiKH"].ToString();
            //kh.GioiTinhKH = row["GioiTinhKH"].ToString();
            kh.SDTKH = row["SDTKH"].ToString();
            //kh.NgaySinhKH = DateTime.Parse(row["NgaySinhKH"].ToString());
            //kh.MaTK = row["MaTK"].ToString();

            connsql.Close();
        }
    }
}
