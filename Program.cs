using QL_GiayTT.Class;
using QL_GiayTT.frm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_GiayTT
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin loginOracle = new frmLogin();

            // Chỉ mở frmDangNhap khi đăng nhập Oracle thành công (OK)
            if (loginOracle.ShowDialog() == DialogResult.OK)
            {
                KetNoi kn = loginOracle.conn;
                Application.Run(new frmDangNhap(kn));
            }
            else
            {

            }
        }
    }
}
