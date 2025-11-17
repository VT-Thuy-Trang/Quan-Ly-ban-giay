using QL_GiayTT.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_GiayTT
{
    public partial class TrangChu : Form
    {
        private KetNoi _ketNoi;

        public TrangChu() : this(OracleSession.CurrentKetNoi)
        {
        }

        // Constructor này chỉ nhận KetNoi và hiển thị
        // (Không có sự kiện _Load, không truy vấn CSDL)
        public TrangChu(KetNoi kn)
        {
           
            InitializeComponent();
            _ketNoi = kn;
            // Không làm gì thêm
        }
    }
}