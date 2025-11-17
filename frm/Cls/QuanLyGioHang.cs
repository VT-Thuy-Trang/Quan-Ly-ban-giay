using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QL_GiayTT.ObjectClass
{
    class QuanLyGioHang
    {
        private static QuanLyGioHang instance;
        private DataTable listGH;
        public double TongThanhToan { get; set; }

        private QuanLyGioHang()
        {
            listGH = new DataTable();
            listGH.Columns.Add("MaSP", typeof(string));
            listGH.Columns.Add("TenSP", typeof(string));
            listGH.Columns.Add("KichCo", typeof(string));
            listGH.Columns.Add("SoLuong", typeof(string));
            listGH.Columns.Add("GiaBan", typeof(string));
            listGH.Columns.Add("ThanhTien", typeof(string));
            TongThanhToan = 0;
        }

        public static QuanLyGioHang Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QuanLyGioHang();
                }
                return instance;
            }
        }

        public DataTable LayDuLieuGioHang()
        {
            return listGH;
        }

        public void ThemVaoGioHang(GioHang sp)
        {
            DataRow row = listGH.Select("MaSP = '" + sp.MaSP + "' AND KichCo = '" + sp.KichCo + "'").FirstOrDefault();
            if (row == null)
                listGH.Rows.Add(sp.MaSP, sp.TenSP, sp.KichCo, sp.SoLuong, sp.GiaBan.ToString("#,##0"), sp.ThanhTien.ToString("#,##0"));
            else
            {
                //Số lượng cũ + số lượng mới
                sp.SoLuong += int.Parse(row.Field<string>("SoLuong"));
                row.SetField<string>("SoLuong", sp.SoLuong.ToString());

                //Sau khi có số lượng mới
                sp.ThanhTien += double.Parse(row.Field<string>("ThanhTien"));
                row.SetField<string>("ThanhTien", sp.ThanhTien.ToString("#,##0"));
                //TongThanhToan += sp.ThanhTien;
            }
            TongThanhToan += sp.ThanhTien;
        }

        //public void XoaSP(DataTable gioHang)
        //{
        //    listGH.Remove(gioHang);
        //}
    }
}
