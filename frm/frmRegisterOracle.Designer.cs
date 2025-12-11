namespace QL_GiayTT.frm
{
    partial class frmRegisterOracle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.gbThongTinKetNoi = new System.Windows.Forms.GroupBox();
            this.chkHienDBAPassword = new System.Windows.Forms.CheckBox();
            this.txtDBAPassword = new System.Windows.Forms.TextBox();
            this.lblDBAPassword = new System.Windows.Forms.Label();
            this.txtDBAUser = new System.Windows.Forms.TextBox();
            this.lblDBAUser = new System.Windows.Forms.Label();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.gbThongTinUser = new System.Windows.Forms.GroupBox();
            this.chkHienMatKhau = new System.Windows.Forms.CheckBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnTaoTaiKhoan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.lblThongTin = new System.Windows.Forms.Label();
            this.gbThongTinKetNoi.SuspendLayout();
            this.gbThongTinUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Silver;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(580, 70);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TẠO TÀI KHOẢN ORACLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbThongTinKetNoi
            // 
            this.gbThongTinKetNoi.Controls.Add(this.chkHienDBAPassword);
            this.gbThongTinKetNoi.Controls.Add(this.txtDBAPassword);
            this.gbThongTinKetNoi.Controls.Add(this.lblDBAPassword);
            this.gbThongTinKetNoi.Controls.Add(this.txtDBAUser);
            this.gbThongTinKetNoi.Controls.Add(this.lblDBAUser);
            this.gbThongTinKetNoi.Controls.Add(this.txtSID);
            this.gbThongTinKetNoi.Controls.Add(this.lblSID);
            this.gbThongTinKetNoi.Controls.Add(this.txtPort);
            this.gbThongTinKetNoi.Controls.Add(this.lblPort);
            this.gbThongTinKetNoi.Controls.Add(this.txtHost);
            this.gbThongTinKetNoi.Controls.Add(this.lblHost);
            this.gbThongTinKetNoi.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbThongTinKetNoi.Location = new System.Drawing.Point(14, 80);
            this.gbThongTinKetNoi.Margin = new System.Windows.Forms.Padding(5);
            this.gbThongTinKetNoi.Name = "gbThongTinKetNoi";
            this.gbThongTinKetNoi.Padding = new System.Windows.Forms.Padding(5);
            this.gbThongTinKetNoi.Size = new System.Drawing.Size(550, 200);
            this.gbThongTinKetNoi.TabIndex = 1;
            this.gbThongTinKetNoi.TabStop = false;
            this.gbThongTinKetNoi.Text = "Thông Tin Kết Nối Oracle";
            // 
            // chkHienDBAPassword
            // 
            this.chkHienDBAPassword.AutoSize = true;
            this.chkHienDBAPassword.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHienDBAPassword.Location = new System.Drawing.Point(350, 135);
            this.chkHienDBAPassword.Margin = new System.Windows.Forms.Padding(5);
            this.chkHienDBAPassword.Name = "chkHienDBAPassword";
            this.chkHienDBAPassword.Size = new System.Drawing.Size(107, 20);
            this.chkHienDBAPassword.TabIndex = 10;
            this.chkHienDBAPassword.Text = "Hiện mật khẩu";
            this.chkHienDBAPassword.UseVisualStyleBackColor = true;
            this.chkHienDBAPassword.CheckedChanged += new System.EventHandler(this.chkHienDBAPassword_CheckedChanged);
            // 
            // txtDBAPassword
            // 
            this.txtDBAPassword.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBAPassword.Location = new System.Drawing.Point(140, 133);
            this.txtDBAPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtDBAPassword.Name = "txtDBAPassword";
            this.txtDBAPassword.PasswordChar = '*';
            this.txtDBAPassword.Size = new System.Drawing.Size(200, 24);
            this.txtDBAPassword.TabIndex = 9;
            // 
            // lblDBAPassword
            // 
            this.lblDBAPassword.Location = new System.Drawing.Point(10, 135);
            this.lblDBAPassword.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDBAPassword.Name = "lblDBAPassword";
            this.lblDBAPassword.Size = new System.Drawing.Size(120, 25);
            this.lblDBAPassword.TabIndex = 8;
            this.lblDBAPassword.Text = "DBA Password:";
            this.lblDBAPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDBAUser
            // 
            this.txtDBAUser.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBAUser.Location = new System.Drawing.Point(140, 98);
            this.txtDBAUser.Margin = new System.Windows.Forms.Padding(5);
            this.txtDBAUser.Name = "txtDBAUser";
            this.txtDBAUser.Size = new System.Drawing.Size(200, 24);
            this.txtDBAUser.TabIndex = 7;
            // 
            // lblDBAUser
            // 
            this.lblDBAUser.Location = new System.Drawing.Point(10, 100);
            this.lblDBAUser.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDBAUser.Name = "lblDBAUser";
            this.lblDBAUser.Size = new System.Drawing.Size(120, 25);
            this.lblDBAUser.TabIndex = 6;
            this.lblDBAUser.Text = "DBA User:";
            this.lblDBAUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSID
            // 
            this.txtSID.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSID.Location = new System.Drawing.Point(140, 63);
            this.txtSID.Margin = new System.Windows.Forms.Padding(5);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(200, 24);
            this.txtSID.TabIndex = 5;
            // 
            // lblSID
            // 
            this.lblSID.Location = new System.Drawing.Point(10, 65);
            this.lblSID.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(120, 25);
            this.lblSID.TabIndex = 4;
            this.lblSID.Text = "SID:";
            this.lblSID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(440, 28);
            this.txtPort.Margin = new System.Windows.Forms.Padding(5);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 24);
            this.txtPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(350, 30);
            this.lblPort.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(80, 25);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHost
            // 
            this.txtHost.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost.Location = new System.Drawing.Point(140, 28);
            this.txtHost.Margin = new System.Windows.Forms.Padding(5);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(200, 24);
            this.txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(10, 30);
            this.lblHost.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(120, 25);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbThongTinUser
            // 
            this.gbThongTinUser.Controls.Add(this.chkHienMatKhau);
            this.gbThongTinUser.Controls.Add(this.txtConfirmPassword);
            this.gbThongTinUser.Controls.Add(this.lblConfirmPassword);
            this.gbThongTinUser.Controls.Add(this.txtPassword);
            this.gbThongTinUser.Controls.Add(this.lblPassword);
            this.gbThongTinUser.Controls.Add(this.txtUsername);
            this.gbThongTinUser.Controls.Add(this.lblUsername);
            this.gbThongTinUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbThongTinUser.Location = new System.Drawing.Point(14, 290);
            this.gbThongTinUser.Margin = new System.Windows.Forms.Padding(5);
            this.gbThongTinUser.Name = "gbThongTinUser";
            this.gbThongTinUser.Padding = new System.Windows.Forms.Padding(5);
            this.gbThongTinUser.Size = new System.Drawing.Size(550, 170);
            this.gbThongTinUser.TabIndex = 2;
            this.gbThongTinUser.TabStop = false;
            this.gbThongTinUser.Text = "Thông Tin Tài Khoản Mới";
            // 
            // chkHienMatKhau
            // 
            this.chkHienMatKhau.AutoSize = true;
            this.chkHienMatKhau.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHienMatKhau.Location = new System.Drawing.Point(350, 105);
            this.chkHienMatKhau.Margin = new System.Windows.Forms.Padding(5);
            this.chkHienMatKhau.Name = "chkHienMatKhau";
            this.chkHienMatKhau.Size = new System.Drawing.Size(107, 20);
            this.chkHienMatKhau.TabIndex = 6;
            this.chkHienMatKhau.Text = "Hiện mật khẩu";
            this.chkHienMatKhau.UseVisualStyleBackColor = true;
            this.chkHienMatKhau.CheckedChanged += new System.EventHandler(this.chkHienMatKhau_CheckedChanged);
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.Location = new System.Drawing.Point(140, 103);
            this.txtConfirmPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(200, 24);
            this.txtConfirmPassword.TabIndex = 5;
            this.txtConfirmPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmPassword_KeyPress);
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Location = new System.Drawing.Point(10, 105);
            this.lblConfirmPassword.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(120, 25);
            this.lblConfirmPassword.TabIndex = 4;
            this.lblConfirmPassword.Text = "Xác nhận MK:";
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(140, 68);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 24);
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(10, 70);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(120, 25);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Mật khẩu:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(140, 33);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 24);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUsername_KeyPress);
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(10, 35);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(120, 25);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Tên đăng nhập:";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTaoTaiKhoan
            // 
            this.btnTaoTaiKhoan.BackColor = System.Drawing.Color.LightGreen;
            this.btnTaoTaiKhoan.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoTaiKhoan.Location = new System.Drawing.Point(14, 470);
            this.btnTaoTaiKhoan.Margin = new System.Windows.Forms.Padding(5);
            this.btnTaoTaiKhoan.Name = "btnTaoTaiKhoan";
            this.btnTaoTaiKhoan.Size = new System.Drawing.Size(270, 50);
            this.btnTaoTaiKhoan.TabIndex = 3;
            this.btnTaoTaiKhoan.Text = "&Tạo Tài Khoản";
            this.btnTaoTaiKhoan.UseVisualStyleBackColor = false;
            this.btnTaoTaiKhoan.Click += new System.EventHandler(this.btnTaoTaiKhoan_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.LightCoral;
            this.btnHuy.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.Location = new System.Drawing.Point(294, 470);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(5);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(270, 50);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "&Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // lblThongTin
            // 
            this.lblThongTin.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongTin.ForeColor = System.Drawing.Color.DimGray;
            this.lblThongTin.Location = new System.Drawing.Point(14, 530);
            this.lblThongTin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblThongTin.Name = "lblThongTin";
            this.lblThongTin.Size = new System.Drawing.Size(550, 25);
            this.lblThongTin.TabIndex = 5;
            this.lblThongTin.Text = "Lưu ý: Cần đăng nhập với quyền DBA (SYS hoặc SYSTEM) để tạo tài khoản mới";
            this.lblThongTin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmRegisterOracle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 570);
            this.ControlBox = false;
            this.Controls.Add(this.lblThongTin);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnTaoTaiKhoan);
            this.Controls.Add(this.gbThongTinUser);
            this.Controls.Add(this.gbThongTinKetNoi);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRegisterOracle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đăng Ký Tài Khoản Oracle";
            this.gbThongTinKetNoi.ResumeLayout(false);
            this.gbThongTinKetNoi.PerformLayout();
            this.gbThongTinUser.ResumeLayout(false);
            this.gbThongTinUser.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox gbThongTinKetNoi;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblSID;
        private System.Windows.Forms.TextBox txtSID;
        private System.Windows.Forms.Label lblDBAUser;
        private System.Windows.Forms.TextBox txtDBAUser;
        private System.Windows.Forms.Label lblDBAPassword;
        private System.Windows.Forms.TextBox txtDBAPassword;
        private System.Windows.Forms.CheckBox chkHienDBAPassword;
        private System.Windows.Forms.GroupBox gbThongTinUser;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.CheckBox chkHienMatKhau;
        private System.Windows.Forms.Button btnTaoTaiKhoan;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label lblThongTin;
    }
}
