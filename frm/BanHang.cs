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
using QL_GiayTT.frm;
using QL_GiayTT.Class;
using QL_GiayTT.ObjectClass;
using QL_GiayTT.frm.Cls;

namespace QL_GiayTT.frm
{
    public partial class frmBanHang : Form
    {
        OracleConnection connsql;
        DataSet listDMSP;
        public frmBanHang()
        {
            InitializeComponent();
            connsql = OracleSession.Connection;
        }

        private void load_FLP_DMSO(string selectStr)
        {
            //Xoá dữ liệu hiện tại
            fLP_DMSO.Controls.Clear();

            // Hiển thị dữ liệu sau khi lọc trong một DataGridView hoặc điều chỉnh dữ liệu hiển thị theo yêu cầu của bạn
            listDMSP = new DataSet();
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listDMSP, "DanhMuc_SP");

            foreach (DataRow item in listDMSP.Tables["DanhMuc_SP"].Rows)
            {
                //Tạo 1 panel để chứa hình và các thông tin
                Panel pn = new Panel();
                pn.Size = new Size(142, 200);
                pn.BorderStyle = BorderStyle.FixedSingle;

                //Tạo 1 PictureBox để chứa hình
                PictureBox pb = new PictureBox();
                pb.Size = new Size(pn.Width, 140);
                pb.BackColor = Color.LightGray;
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                pb.Click += Panel_Click;

                //Tạo tên sản phẩm và giá tiền
                Label lbTenSP = new Label();
                lbTenSP.Width = pn.Width;
                lbTenSP.Text = item["TenSP"].ToString();
                lbTenSP.Font = new Font("Constantia", 10, FontStyle.Bold);
                lbTenSP.AutoEllipsis = true;
                lbTenSP.Location = new Point(8, 150);
                lbTenSP.Click += Panel_Click;

                Label lbGiaTien = new Label();
                // Chuyển đổi item["GiaBan"] thành kiểu double
                double giaBanDouble = Convert.ToDouble(item["GiaBan"].ToString());
                lbGiaTien.Text = giaBanDouble.ToString("#,##0") + "đ";
                lbGiaTien.Font = new Font("Palatino Linotype", 8);
                lbGiaTien.AutoSize = true;
                lbGiaTien.Location = new Point(8, 176);
                lbGiaTien.Click += Panel_Click;

                Label lbSanCo = new Label();
                lbSanCo.Text = "Sẵn có: " + item["SoLuongTon"].ToString();
                lbSanCo.Font = new Font("Palatino Linotype", 8);
                lbSanCo.Location = new Point(75, 177);
                lbSanCo.Click += Panel_Click;

                //Thêm PictureBox vào panel
                pn.Controls.Add(pb);
                pn.Controls.Add(lbTenSP);
                pn.Controls.Add(lbGiaTien);
                pn.Controls.Add(lbSanCo);

                //item tương ứng cho select
                pn.Tag = item["MaSP"].ToString();

                //Thêm panel vào flowLayoutPanel1
                fLP_DMSO.Controls.Add(pn);
            }
        }

        private void load_cbo_DM()
        {
            listDMSP = new DataSet();
            string selectStr = "Select * from LOAISP";
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listDMSP, "LOAISP");
            cbo_DM.DataSource = listDMSP.Tables["LOAISP"];
            cbo_DM.DisplayMember = "TenL";
            cbo_DM.ValueMember = "MaL";
        }

        private void load_cbo_GiaTien()
        {
            string[] listStr = { "--Giá--", "Từ thấp đến cao", "Từ cao đến thấp" };
            cbo_GiaTien.DataSource = listStr;
        }

        private void load_cbo_NhanVien()
        {
            listDMSP = new DataSet();
            string selectStr = "Select MaNV, TenNV from NHANVIEN";
            OracleDataAdapter data = new OracleDataAdapter(selectStr, connsql);
            data.Fill(listDMSP, "NHANVIEN");
            cbo_NhanVien.DataSource = listDMSP.Tables["NHANVIEN"];
            cbo_NhanVien.DisplayMember = "TenNV";
            cbo_NhanVien.ValueMember = "MaNV";
        }

        private void load_cbo_HTGiamGia()
        {
            string[] listStr = { "--Hình Thức Giảm Giá--", "Giảm giá theo %", "GIảm giá trực tiếp" };
            cbo_HTGiamGia.DataSource = listStr;
        }

        private void load_cbo_HTThanhToan()
        {
            string[] listStr = { "--Hình Thức Thanh Toán--", "Tiền mặt", "Chuyển khoản" };
            cbo_HTThanhToan.DataSource = listStr;
        }

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            load_cbo_DM();
            load_cbo_GiaTien();
            load_cbo_NhanVien();
            load_cbo_HTGiamGia();
            load_cbo_HTThanhToan();
            string selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon from SANPHAM";
            load_FLP_DMSO(selectStr);
            dtGV_GioHang.AllowUserToAddRows = false;
            
            // Đảm bảo cột SoLuong có thể chỉnh sửa
            if (dtGV_GioHang.Columns["SoLuong"] != null)
            {
                dtGV_GioHang.Columns["SoLuong"].ReadOnly = false;
            }
            
            // Sau khi load xong, nếu có nhân viên trong danh sách thì chọn nhân viên đầu tiên
            // và enable các control thanh toán
            if (cbo_NhanVien.Items.Count > 0)
            {
                cbo_NhanVien.SelectedIndex = 0;
                // Gọi hàm enable các control vì SelectionChangeCommitted không tự động trigger khi set SelectedIndex
                if (cbo_NhanVien.SelectedIndex >= 0)
                {
                    cbo_HTThanhToan.Enabled = true;
                    cbo_HTGiamGia.Enabled = true;
                    btnThanhToan.Enabled = true;
                    chk_ChuaThanhToan.Enabled = true;
                    chk_DatCoc.Enabled = true;
                    txtGhiChu.Enabled = true;
                }
            }
        }

        // Xử lý sự kiện click trên Panel
        private void Panel_Click(object sender, EventArgs e)
        {

            //Biến sender đại diện cho đối tượng gửi sự kiện, ở đây là panel được click.
            Panel panel = (Panel)((Control)sender).Parent;

            //Gọi frmThongTinSP để lấy thông tin 
            string selectedMaSP = (string)panel.Tag;
            frmThongTinSP form = new frmThongTinSP(selectedMaSP);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();

            //Thêm dữ liệu vào dtGV_GioHang
            DataTable listGH = QuanLyGioHang.Instance.LayDuLieuGioHang();

            dtGV_GioHang.Columns[0].DataPropertyName = "MaSP";
            dtGV_GioHang.Columns[1].DataPropertyName = "TenSP";
            dtGV_GioHang.Columns[2].DataPropertyName = "KichCo";
            dtGV_GioHang.Columns[3].DataPropertyName = "SoLuong";
            dtGV_GioHang.Columns[4].DataPropertyName = "GiaBan";
            dtGV_GioHang.Columns[5].DataPropertyName = "ThanhTien";

            // Đảm bảo các cột không thể chỉnh sửa trừ cột SoLuong
            dtGV_GioHang.Columns["MaSP"].ReadOnly = true;
            dtGV_GioHang.Columns["TenSP"].ReadOnly = true;
            dtGV_GioHang.Columns["KichThuoc"].ReadOnly = true;
            dtGV_GioHang.Columns["SoLuong"].ReadOnly = false; // Cho phép chỉnh sửa số lượng
            dtGV_GioHang.Columns["GiaBan"].ReadOnly = true;
            dtGV_GioHang.Columns["ThanhTien"].ReadOnly = true;

            dtGV_GioHang.DataSource = listGH;

            cbo_NhanVien.Enabled = true;
            txtTenKH.Enabled = true;
            mtxt_SDTKH.Enabled = true;

            // Đảm bảo các control thanh toán được enable nếu đã chọn nhân viên
            if (cbo_NhanVien.SelectedIndex >= 0 && cbo_NhanVien.SelectedValue != null)
            {
                cbo_HTThanhToan.Enabled = true;
                cbo_HTGiamGia.Enabled = true;
                btnThanhToan.Enabled = true;
                chk_ChuaThanhToan.Enabled = true;
                chk_DatCoc.Enabled = true;
                txtGhiChu.Enabled = true;
            }

            capNhapThanhToan(QuanLyGioHang.Instance.TongThanhToan);
        }

        private void capNhapThanhToan(double tongTien)
        {
            lbTongTien.Text = tongTien.ToString("#,##0") + "đ";
            double giamGia;
            if (txtGiamGia.Text.Length == 0)
                giamGia = 0;
            else giamGia = double.Parse(txtGiamGia.Text);

            double tongThanhToan = 0;

            if (cbo_HTGiamGia.SelectedIndex == 1)
            {
                double phanTram = giamGia / 100;
                tongThanhToan = tongTien -= tongTien * phanTram;
            }
            else tongThanhToan = tongTien -= giamGia;
            lbTongThanhToan.Text = tongThanhToan.ToString("#,##0") + "đ";

            if (cbo_HTThanhToan.SelectedIndex == 1)
            {
                if (txtTienNhan.Text.Length > 0)
                {
                    double tienNhan = double.Parse(txtTienNhan.Text);
                    lbTienThua.Text = (tienNhan - tongThanhToan).ToString("#,##0") + "đ";
                }
            }
        }

        private void cbo_DM_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbo_GiaTien.SelectedItem = 0;
            string selectStr;
            if (cbo_DM.SelectedIndex != 0)
                selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon, SP.MaL from SANPHAM SP, LOAISP L where SP.MaL = L.MaL AND SP.MaL = '" + cbo_DM.SelectedValue.ToString() + "'";
            else selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon, SP.MaL from SANPHAM SP, LOAISP L where SP.MaL = L.MaL";
            load_FLP_DMSO(selectStr);
        }

        private void cbo_GiaTien_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbo_DM.SelectedItem = 0;
            string selectStr;
            if (cbo_GiaTien.SelectedIndex == 0)
                selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon, SP.MaL from SANPHAM SP, LOAISP L where SP.MaL = L.MaL";
            else if (cbo_GiaTien.SelectedIndex == 1)
                selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon, SP.MaL from SANPHAM SP, LOAISP L  where SP.MaL = L.MaL Order By SP.GiaBan ASC;";
            else selectStr = "Select MaSP, TenSP, GiaBan, SoLuongTon, SP.MaL from SANPHAM SP, LOAISP L  where SP.MaL = L.MaL Order By SP.GiaBan DESC;";
            load_FLP_DMSO(selectStr);
        }

        private void cbo_NhanVien_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbo_NhanVien.SelectedIndex < 0)
            {
                cbo_HTThanhToan.Enabled = false;
                cbo_HTGiamGia.Enabled = false;
                txtGiamGia.Enabled = false;
                txtTienNhan.Enabled = false;
                chk_ChuaThanhToan.Enabled = false;
                chk_DatCoc.Enabled = false;
                txtGhiChu.Enabled = false;
                btnThanhToan.Enabled = false;
            }
            else
            {
                cbo_HTThanhToan.Enabled = true;
                cbo_HTGiamGia.Enabled = true;
                btnThanhToan.Enabled = true;
                chk_ChuaThanhToan.Enabled = true;
                chk_DatCoc.Enabled = true;
                txtGhiChu.Enabled = true;
            }
        }

        private void cbo_HTThanhToan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtTienNhan.Text = "";
            string tongTienStr = lbTongTien.Text.Replace("đ", "").Replace(",", "").Trim();
            double tongTien = 0;
            if (double.TryParse(tongTienStr, out tongTien))
                capNhapThanhToan(tongTien);
            lbTienThua.Text = "0đ";

            if (cbo_HTThanhToan.SelectedIndex == 1)
                txtTienNhan.Enabled = true;
            else txtTienNhan.Enabled = false;
        }

        private void cbo_HTGiamGia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtGiamGia.Text = "";
            string tongTienStr = lbTongTien.Text.Replace("đ", "").Replace(",", "").Trim();
            double tongTien = 0;
            if (double.TryParse(tongTienStr, out tongTien))
                capNhapThanhToan(tongTien);
            if (cbo_HTGiamGia.SelectedIndex == 0)
            {
                txtGiamGia.Enabled = false;
                return;
            }
            txtGiamGia.Enabled = true;

            if (cbo_HTGiamGia.SelectedIndex == 1)
                lb_TTGG.Text = "%";
            else lb_TTGG.Text = "đ";

        }

        private void txtGiamGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra TextBox đang trống và ký tự đầu tiên là 0, ký tự không phải là số, TextBox đã có giá trị và ký tự đầu tiên là 0 thì không cho phép
            if ((txtGiamGia.Text.Length == 0 && e.KeyChar == '0') || (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) || (txtGiamGia.Text.Length > 0 && txtGiamGia.Text[0] == '0'))
            {
                e.Handled = true; // Hủy bỏ ký tự không hợp lệ
                return;
            }
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            double giamGia;
            if (txtGiamGia.Text.Length == 0)
                giamGia = 0;
            else 
            {
                if (!double.TryParse(txtGiamGia.Text, out giamGia))
                    return;
            }

            string tongTienStr = lbTongTien.Text.Replace("đ", "").Replace(",", "").Trim();
            double tongTien = 0;
            if (!double.TryParse(tongTienStr, out tongTien))
                return;
                
            if (cbo_HTGiamGia.SelectedIndex == 1)
            {
                if (giamGia > 100)
                {
                    giamGia = 100;
                    txtGiamGia.Text = "100";
                }
                double phanTram = giamGia / 100;
                lbTongThanhToan.Text = (tongTien - tongTien * phanTram).ToString("#,##0") + "đ";
            }
            else
                lbTongThanhToan.Text = (tongTien - giamGia).ToString("#,##0") + "đ";

            //đưa trỏ chuột nằm bên phải
            txtGiamGia.SelectionStart = txtGiamGia.TextLength;
            txtGiamGia.SelectionLength = 0;
        }

        private void txtTienNhan_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Nếu ký tự đầu là 0 hoặc ký tự không phải là số thì huỷ
            if ((txtTienNhan.Text.Length == 0 && e.KeyChar == '0') || (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) || (txtTienNhan.Text.Length > 0 && txtTienNhan.Text[0] == '0'))
            {
                e.Handled = true; // Hủy bỏ ký tự không hợp lệ
                return;
            }
        }

        private void txtTienNhan_TextChanged(object sender, EventArgs e)
        {
            if (txtTienNhan.Text.Length > 0)
            {
                double tienNhan = 0;
                if (!double.TryParse(txtTienNhan.Text, out tienNhan))
                    return;
                    
                string tongThanhToanStr = lbTongThanhToan.Text.Replace("đ", "").Replace(",", "").Trim();
                double tongThanhToan = 0;
                if (!double.TryParse(tongThanhToanStr, out tongThanhToan))
                    return;
                    
                lbTienThua.Text = (tienNhan - tongThanhToan).ToString("#,##0") + "đ";
            }
        }
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nhân viên đã được chọn chưa
                if (cbo_NhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!!!");
                    return;
                }
                
                // Kiểm tra SelectedValue
                object selectedValue = cbo_NhanVien.SelectedValue;
                if (selectedValue == null || selectedValue == DBNull.Value)
                {
                    MessageBox.Show("Lỗi: Không thể lấy thông tin nhân viên! Vui lòng chọn lại nhân viên.");
                    return;
                }

                // Kiểm tra giỏ hàng có sản phẩm không
                if (dtGV_GioHang.Rows.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng trống!!!");
                    return;
                }

                // Kiểm tra tiền thừa chỉ khi chọn hình thức thanh toán "Tiền mặt"
                if (cbo_HTThanhToan.SelectedIndex == 1) // Tiền mặt
                {
                    // Parse tiền thừa - xử lý format có dấu phẩy
                    string tienThuaStr = lbTienThua.Text.Replace("đ", "").Replace(",", "").Trim();
                    double tienThua = 0;
                    if (!string.IsNullOrEmpty(tienThuaStr))
                    {
                        double.TryParse(tienThuaStr, out tienThua);
                    }
                    
                    // Kiểm tra nếu chưa nhập tiền nhận
                    if (string.IsNullOrEmpty(txtTienNhan.Text.Trim()))
                    {
                        MessageBox.Show("Vui lòng nhập số tiền nhận!!!");
                        return;
                    }
                    
                    if (tienThua < 0)
                    {
                        MessageBox.Show("TIỀN NHẬN KHÔNG ĐỦ!!!");
                        return;
                    }
                }

                if (chk_DatCoc.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtGhiChu.Text))
                    {
                        MessageBox.Show("Vui lòng nhập số tiền đặt cọc vào ghi chú!!!");
                        return;
                    }
                }

                if (connsql.State == ConnectionState.Closed) 
                    connsql.Open();
                
                int newMaKH = 0;
                //Nếu có thông tin khách hàng thì tạo mới 1 khách hàng
                if (!string.IsNullOrEmpty(txtTenKH.Text.Trim()) || !string.IsNullOrEmpty(mtxt_SDTKH.Text.Trim()))
                {
                    string insertStrKH = "INSERT INTO KHACHHANG (TenKH, SDTKH) VALUES (:TenKH, :SDTKH) RETURNING MaKH INTO :NewMaKH";
                    OracleCommand cmdKH = new OracleCommand(insertStrKH, connsql);
                    cmdKH.Parameters.Add(":TenKH", OracleDbType.Varchar2).Value = txtTenKH.Text.Trim();
                    string sdtKH = mtxt_SDTKH.Text.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                    if (sdtKH.Length < 10)
                    {
                        MessageBox.Show("Số điện thoại phải đủ 10 số");
                        return;
                    }
                    cmdKH.Parameters.Add(":SDTKH", OracleDbType.Varchar2).Value = sdtKH;
                    OracleParameter maKhParameter = new OracleParameter(":NewMaKH", OracleDbType.Int32);
                    maKhParameter.Direction = ParameterDirection.Output;
                    cmdKH.Parameters.Add(maKhParameter);
                    cmdKH.ExecuteNonQuery();
                    newMaKH = Convert.ToInt32(maKhParameter.Value.ToString());
                }

                //Tạo mới 1 hoá đơn
                int maHD;
                string insertStrHD = "INSERT INTO HOADON (MaKH, MaNV, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan, GhiChu) " +
                    "VALUES (:MaKH, :MaNV, :NgayBan, :TongThanhToan, :TrangThai, :HinhThucThanhToan, :GhiChu) RETURNING MaHD INTO :NewMaHD";
                OracleCommand cmdHD = new OracleCommand(insertStrHD, connsql);
                //Nếu không nhập thông tin khách hàng thì không cần phải gán giá trị cho MaKH
                if (newMaKH != 0)
                    cmdHD.Parameters.Add(":MaKH", OracleDbType.Int32).Value = newMaKH;
                else
                    cmdHD.Parameters.Add(":MaKH", OracleDbType.Int32).Value = DBNull.Value;
                
                // Lấy mã nhân viên từ SelectedValue
                string maNV = selectedValue.ToString();
                if (string.IsNullOrWhiteSpace(maNV))
                {
                    MessageBox.Show("Lỗi: Mã nhân viên không hợp lệ!");
                    return;
                }
                cmdHD.Parameters.Add(":MaNV", OracleDbType.Varchar2).Value = maNV;
                cmdHD.Parameters.Add(":NgayBan", OracleDbType.Date).Value = DateTime.Now;
                
                // Parse tổng thanh toán - xử lý format có dấu phẩy
                string tongThanhToanStr = lbTongThanhToan.Text.Replace("đ", "").Replace(",", "").Trim();
                double tongThanhToan = 0;
                if (!double.TryParse(tongThanhToanStr, out tongThanhToan))
                {
                    MessageBox.Show("Lỗi: Không thể tính tổng thanh toán!");
                    return;
                }
                cmdHD.Parameters.Add(":TongThanhToan", OracleDbType.Decimal).Value = tongThanhToan;
                
                if (cbo_HTThanhToan.SelectedIndex == 0)
                    cmdHD.Parameters.Add(":HinhThucThanhToan", OracleDbType.Varchar2).Value = "";
                else 
                    cmdHD.Parameters.Add(":HinhThucThanhToan", OracleDbType.Varchar2).Value = cbo_HTThanhToan.SelectedItem?.ToString() ?? "";
                
                if (chk_ChuaThanhToan.Checked == true)
                    cmdHD.Parameters.Add(":TrangThai", OracleDbType.Varchar2).Value = "Chưa thanh toán";
                else if (chk_DatCoc.Checked == true)
                    cmdHD.Parameters.Add(":TrangThai", OracleDbType.Varchar2).Value = "Đặt cọc";
                else 
                    cmdHD.Parameters.Add(":TrangThai", OracleDbType.Varchar2).Value = "Đã thanh toán";

                if (cbo_HTGiamGia.SelectedIndex > 0)
                {
                    // Sửa bug: dùng lbTongTien.Text.Length thay vì lbTongThanhToan.Text.Length
                    string tongTienStr = lbTongTien.Text.Replace("đ", "").Replace(",", "").Trim();
                    double tongTien = 0;
                    double.TryParse(tongTienStr, out tongTien);
                    string ghiChu = "Tổng thanh toán: " + tongTien.ToString("N0") + " - Giảm: " + txtGiamGia.Text + ". " + txtGhiChu.Text;
                    cmdHD.Parameters.Add(":GhiChu", OracleDbType.Varchar2).Value = ghiChu;
                }
                else 
                    cmdHD.Parameters.Add(":GhiChu", OracleDbType.Varchar2).Value = txtGhiChu.Text ?? "";

                OracleParameter maHdParameter = new OracleParameter(":NewMaHD", OracleDbType.Int32);
                maHdParameter.Direction = ParameterDirection.Output;
                cmdHD.Parameters.Add(maHdParameter);
                cmdHD.ExecuteNonQuery();
                maHD = Convert.ToInt32(maHdParameter.Value.ToString());
                
                //Tạo mới CTHD
                foreach (DataGridViewRow item in dtGV_GioHang.Rows)
                {
                    // Bỏ qua row trống
                    if (item.IsNewRow || item.Cells["MaSP"].Value == null)
                        continue;
                        
                    string insertStrCTHD = "INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien) VALUES (:MaHD, :MaSP, :KichThuoc, :SoLuong, :GiaBan, :ThanhTien)";
                    OracleCommand cmdCTHD = new OracleCommand(insertStrCTHD, connsql);
                    cmdCTHD.Parameters.Add(":MaHD", OracleDbType.Int32).Value = maHD;
                    cmdCTHD.Parameters.Add(":MaSP", OracleDbType.Varchar2).Value = item.Cells["MaSP"].Value?.ToString() ?? "";
                    cmdCTHD.Parameters.Add(":KichThuoc", OracleDbType.Varchar2).Value = item.Cells["KichThuoc"].Value?.ToString() ?? "";
                    
                    int soLuong = 0;
                    if (item.Cells["SoLuong"].Value != null && int.TryParse(item.Cells["SoLuong"].Value.ToString(), out soLuong))
                        cmdCTHD.Parameters.Add(":SoLuong", OracleDbType.Int32).Value = soLuong;
                    else
                    {
                        MessageBox.Show("Lỗi: Số lượng không hợp lệ!");
                        return;
                    }
                    
                    double giaBan = 0;
                    string giaBanStr = item.Cells["GiaBan"].Value?.ToString()?.Replace(",", "") ?? "0";
                    if (double.TryParse(giaBanStr, out giaBan))
                        cmdCTHD.Parameters.Add(":GiaBan", OracleDbType.Decimal).Value = giaBan;
                    else
                    {
                        MessageBox.Show("Lỗi: Giá bán không hợp lệ!");
                        return;
                    }
                    
                    double thanhTien = 0;
                    string thanhTienStr = item.Cells["ThanhTien"].Value?.ToString()?.Replace(",", "") ?? "0";
                    if (double.TryParse(thanhTienStr, out thanhTien))
                        cmdCTHD.Parameters.Add(":ThanhTien", OracleDbType.Decimal).Value = thanhTien;
                    else
                    {
                        MessageBox.Show("Lỗi: Thành tiền không hợp lệ!");
                        return;
                    }
                    
                    cmdCTHD.ExecuteNonQuery();
                }
                connsql.Close();
                MessageBox.Show("Thành công");
                xoaThongTinBanHang(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connsql.State == ConnectionState.Open)
                    connsql.Close();
            }
        }

        private void xoaThongTinBanHang(object sender, EventArgs e)
        {
            ////btnThanhToan
            if (sender == btnThanhToan)
            {
                txtTenKH.Text = "";
                txtTenKH.Enabled = false;

                mtxt_SDTKH.Text = "";
                mtxt_SDTKH.Enabled = false;

                DataTable listGH = QuanLyGioHang.Instance.LayDuLieuGioHang();
                listGH.Clear();

                txtTimKiem.Text = "";
                cbo_DM.SelectedIndex = 0;
                cbo_GiaTien.SelectedIndex = 0;
            }

            cbo_NhanVien.SelectedIndex = 0;
            cbo_NhanVien.Enabled = false;

            lbTongTien.Text = "0đ";

            cbo_HTGiamGia.SelectedIndex = 0;
            cbo_HTGiamGia.Enabled = false;

            txtGiamGia.Text = "";
            txtGiamGia.Enabled = false;

            lbTongThanhToan.Text = "0đ";

            cbo_HTThanhToan.SelectedIndex = 0;
            cbo_HTThanhToan.Enabled = false;

            txtTienNhan.Text = "";
            txtTienNhan.Enabled = false;

            lbTienThua.Text = "0đ";

            chk_ChuaThanhToan.Checked = false;
            chk_ChuaThanhToan.Enabled = false;

            chk_DatCoc.Checked = false;
            chk_DatCoc.Enabled = false;

            txtGhiChu.Text = "";
            txtGhiChu.Enabled = false;

            btnThanhToan.Enabled = false;
        }

        private void dtGV_GioHang_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGV_GioHang.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                DataGridViewRow dongHienTai = dtGV_GioHang.Rows[e.RowIndex];
                
                // Kiểm tra nếu row không hợp lệ
                if (dongHienTai.IsNewRow || dongHienTai.Cells["MaSP"].Value == null)
                    return;
                
                string giaBanStr = dongHienTai.Cells["GiaBan"].Value?.ToString()?.Replace(",", "") ?? "0";
                if (!double.TryParse(giaBanStr, out double donGia))
                    return;
                
                // Lấy giá trị từ NumericUpDown
                int soLuong = 1;
                object soLuongValue = dongHienTai.Cells["SoLuong"].Value;
                if (soLuongValue != null)
                {
                    string soLuongStr = soLuongValue.ToString().Replace(",", "").Trim();
                    if (!int.TryParse(soLuongStr, out soLuong) || soLuong < 1)
                    {
                        soLuong = 1;
                        dongHienTai.Cells["SoLuong"].Value = "1";
                    }
                }
                else
                {
                    dongHienTai.Cells["SoLuong"].Value = "1";
                }
                    
                double thanhTien = soLuong * donGia;
                dongHienTai.Cells["ThanhTien"].Value = thanhTien.ToString("N0");
                
                // Cập nhật vào DataTable trong QuanLyGioHang
                DataTable listGH = QuanLyGioHang.Instance.LayDuLieuGioHang();
                string maSP = dongHienTai.Cells["MaSP"].Value?.ToString();
                string kichCo = dongHienTai.Cells["KichThuoc"].Value?.ToString();
                if (!string.IsNullOrEmpty(maSP) && !string.IsNullOrEmpty(kichCo))
                {
                    DataRow[] rows = listGH.Select("MaSP = '" + maSP + "' AND KichCo = '" + kichCo + "'");
                    foreach (DataRow row in rows)
                    {
                        row["SoLuong"] = soLuong.ToString();
                        row["ThanhTien"] = thanhTien.ToString("N0");
                    }
                }

                // Tính lại tổng tiền từ DataTable
                double tongTien = 0;
                foreach (DataRow row in listGH.Rows)
                {
                    string thanhTienStr = row["ThanhTien"].ToString().Replace(",", "");
                    if (double.TryParse(thanhTienStr, out double tt))
                    {
                        tongTien += tt;
                    }
                }
                
                // Cập nhật TongThanhToan trong QuanLyGioHang
                QuanLyGioHang.Instance.TongThanhToan = tongTien;

                capNhapThanhToan(tongTien);
            }
        }
        
        private void dtGV_GioHang_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý khi giá trị thay đổi (kể cả khi không edit)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dtGV_GioHang.Columns[e.ColumnIndex].Name == "SoLuong")
            {
                // Gọi lại logic tính toán
                dtGV_GioHang_CellEndEdit(sender, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
            }
        }

        private void dtGV_GioHang_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Kiểm tra có dữ liệu trong ô dữ liệu hay không
                if (dtGV_GioHang.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    e.ContextMenuStrip = cms_dtGVGioHang;
                    cms_dtGVGioHang.Tag = e.RowIndex;
                }
                else
                {
                    e.ContextMenuStrip = null; // Không hiển thị ContextMenuStrip
                }
            }
        }

        private void tsmi_Xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra Tag có giá trị không
            if (cms_dtGVGioHang.Tag == null)
            {
                MessageBox.Show("Không thể xác định sản phẩm cần xóa!");
                return;
            }

            int rowIndex = (int)cms_dtGVGioHang.Tag;
            
            // Kiểm tra rowIndex có hợp lệ không
            if (rowIndex < 0 || rowIndex >= dtGV_GioHang.Rows.Count)
            {
                MessageBox.Show("Chỉ số hàng không hợp lệ!");
                return;
            }

            // Lấy thông tin từ DataGridView trước khi xóa
            DataGridViewRow selectedRow = dtGV_GioHang.Rows[rowIndex];
            if (selectedRow == null || selectedRow.IsNewRow)
            {
                MessageBox.Show("Không thể xóa hàng này!");
                return;
            }

            string maSP = selectedRow.Cells["MaSP"].Value?.ToString();
            string kichCo = selectedRow.Cells["KichThuoc"].Value?.ToString();
            
            if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(kichCo))
            {
                MessageBox.Show("Không thể lấy thông tin sản phẩm!");
                return;
            }
            
            // Xóa khỏi DataTable trong QuanLyGioHang
            // Vì DataGridView đang bind với DataTable qua DataSource,
            // nên khi xóa từ DataTable, DataGridView sẽ tự động cập nhật
            DataTable listGH = QuanLyGioHang.Instance.LayDuLieuGioHang();
            DataRow[] rowsToDelete = listGH.Select("MaSP = '" + maSP + "' AND KichCo = '" + kichCo + "'");
            
            if (rowsToDelete.Length == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm trong giỏ hàng!");
                return;
            }
            
            // Xóa tất cả các dòng khớp (trường hợp có nhiều dòng cùng sản phẩm và size)
            foreach (DataRow row in rowsToDelete)
            {
                listGH.Rows.Remove(row);
            }
            
            // DataGridView sẽ tự động cập nhật vì nó bind với DataTable
            // Không cần gọi RemoveAt nữa
            
            // Tính lại tổng tiền từ DataTable
            double tongTien = 0;
            foreach (DataRow row in listGH.Rows)
            {
                string thanhTienStr = row["ThanhTien"].ToString().Replace(",", "");
                if (double.TryParse(thanhTienStr, out double thanhTien))
                {
                    tongTien += thanhTien;
                }
            }
            
            // Cập nhật TongThanhToan trong QuanLyGioHang
            QuanLyGioHang.Instance.TongThanhToan = tongTien;
            
            // Kiểm tra giỏ hàng còn sản phẩm không
            if (listGH.Rows.Count == 0)
            {
                xoaThongTinBanHang(sender, e);
            }
            else
            {
                capNhapThanhToan(tongTien);
            }
        }

        private void chk_ChuaThanhToan_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ChuaThanhToan.Checked == true)
            {
                chk_DatCoc.Enabled = false;
                cbo_HTThanhToan.SelectedIndex = 0;
            }
            else chk_DatCoc.Enabled = true;
        }

        private void chk_DatCoc_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DatCoc.Checked == true)
                chk_ChuaThanhToan.Enabled = false;
            else chk_ChuaThanhToan.Enabled = true;
        }
    }
}
