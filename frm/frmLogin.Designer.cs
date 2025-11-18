namespace QL_GiayTT
{
    partial class frmLogin
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

        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.chk_HienMK = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Moccasin;
            this.btnLogin.Location = new System.Drawing.Point(54, 394);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(295, 66);
            this.btnLogin.TabIndex = 41;
            this.btnLogin.Text = "&LOGIN";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(136, 326);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(236, 29);
            this.txtPassword.TabIndex = 40;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(-67, 326);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(193, 42);
            this.lblPassword.TabIndex = 46;
            this.lblPassword.Text = "PASSWORD";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(136, 275);
            this.txtUser.Margin = new System.Windows.Forms.Padding(5);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(236, 29);
            this.txtUser.TabIndex = 39;
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(-61, 275);
            this.lblUser.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(187, 42);
            this.lblUser.TabIndex = 45;
            this.lblUser.Text = "USER";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSID
            // 
            this.txtSID.Location = new System.Drawing.Point(136, 223);
            this.txtSID.Margin = new System.Windows.Forms.Padding(5);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(236, 29);
            this.txtSID.TabIndex = 38;
            // 
            // lblSID
            // 
            this.lblSID.Location = new System.Drawing.Point(-61, 223);
            this.lblSID.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(187, 42);
            this.lblSID.TabIndex = 44;
            this.lblSID.Text = "SID";
            this.lblSID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPort
            // 
            this.txtPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPort.Location = new System.Drawing.Point(136, 171);
            this.txtPort.Margin = new System.Windows.Forms.Padding(5);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(236, 29);
            this.txtPort.TabIndex = 37;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(-61, 171);
            this.lblPort.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(187, 42);
            this.lblPort.TabIndex = 43;
            this.lblPort.Text = "PORT";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(136, 120);
            this.txtHost.Margin = new System.Windows.Forms.Padding(5);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(236, 29);
            this.txtHost.TabIndex = 36;
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(-61, 120);
            this.lblHost.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(187, 42);
            this.lblHost.TabIndex = 42;
            this.lblHost.Text = "HOST";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogin
            // 
            this.lblLogin.BackColor = System.Drawing.Color.Silver;
            this.lblLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(0, 0);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(394, 102);
            this.lblLogin.TabIndex = 47;
            this.lblLogin.Text = "LOGIN";
            this.lblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chk_HienMK
            // 
            this.chk_HienMK.AutoSize = true;
            this.chk_HienMK.Location = new System.Drawing.Point(136, 362);
            this.chk_HienMK.Margin = new System.Windows.Forms.Padding(2);
            this.chk_HienMK.Name = "chk_HienMK";
            this.chk_HienMK.Size = new System.Drawing.Size(135, 25);
            this.chk_HienMK.TabIndex = 48;
            this.chk_HienMK.Text = "Hiện mật khẩu";
            this.chk_HienMK.UseVisualStyleBackColor = true;
            this.chk_HienMK.CheckedChanged += new System.EventHandler(this.chk_HienMK_CheckedChanged);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 465);
            this.Controls.Add(this.chk_HienMK);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtSID);
            this.Controls.Add(this.lblSID);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.lblLogin);
            this.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmLogin";
            this.Text = "Ket Noi Oracle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnLogin;
        protected internal System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        public System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblUser;
        public System.Windows.Forms.TextBox txtSID;
        private System.Windows.Forms.Label lblSID;
        public System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        public System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.CheckBox chk_HienMK;
    }
}


