using QL_GiayTT.Class;
using QL_GiayTT.frm;
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
    public partial class frmLogin : Form
    {
        public string Host => txtHost.Text;
        public string Port => txtPort.Text;
        public string Sid => txtSID.Text;
        public string User => txtUser.Text;
        public string Password => txtPassword.Text;

        public frmLogin()
        {
            CenterToScreen();
            InitializeComponent();
        }
        public frmLogin(string host, string port, string sid, string user)
        {
            CenterToScreen();
            InitializeComponent();

            txtHost.Text = host;
            txtPort.Text = port;
            txtSID.Text = sid;
            txtUser.Text = user;
        }
        public KetNoi conn;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            conn = new KetNoi(Host, Port, Sid, User, Password);

            if (conn.connect())
            {
                MessageBox.Show("Đăng nhập thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Kết nối thất bại!");
            }
        }

        private void chk_HienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_HienMK.Checked == true)
                txtPassword.PasswordChar = '\0';
            else txtPassword.PasswordChar = '*';
        }
    }
}
