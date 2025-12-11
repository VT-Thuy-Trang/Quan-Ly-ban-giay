namespace QL_GiayTT.frm
{
    partial class frmDangNhap
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTK = new System.Windows.Forms.TextBox();
            this.btnDN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMK = new System.Windows.Forms.TextBox();
            this.chk_HienMK = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tài khoản:";
            // 
            // txtTK
            // 
            this.txtTK.Location = new System.Drawing.Point(71, 67);
            this.txtTK.Margin = new System.Windows.Forms.Padding(2);
            this.txtTK.Name = "txtTK";
            this.txtTK.Size = new System.Drawing.Size(200, 20);
            this.txtTK.TabIndex = 1;
            // 
            // btnDN
            // 
            this.btnDN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDN.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDN.ForeColor = System.Drawing.Color.Transparent;
            this.btnDN.Location = new System.Drawing.Point(99, 161);
            this.btnDN.Margin = new System.Windows.Forms.Padding(2);
            this.btnDN.Name = "btnDN";
            this.btnDN.Size = new System.Drawing.Size(106, 39);
            this.btnDN.TabIndex = 4;
            this.btnDN.Text = "Đăng nhập";
            this.btnDN.UseVisualStyleBackColor = false;
            this.btnDN.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mật khẩu:";
            // 
            // txtMK
            // 
            this.txtMK.Location = new System.Drawing.Point(71, 103);
            this.txtMK.Margin = new System.Windows.Forms.Padding(2);
            this.txtMK.Name = "txtMK";
            this.txtMK.PasswordChar = '*';
            this.txtMK.Size = new System.Drawing.Size(200, 20);
            this.txtMK.TabIndex = 3;
            // 
            // chk_HienMK
            // 
            this.chk_HienMK.AutoSize = true;
            this.chk_HienMK.Location = new System.Drawing.Point(71, 133);
            this.chk_HienMK.Margin = new System.Windows.Forms.Padding(2);
            this.chk_HienMK.Name = "chk_HienMK";
            this.chk_HienMK.Size = new System.Drawing.Size(95, 17);
            this.chk_HienMK.TabIndex = 6;
            this.chk_HienMK.Text = "Hiện mật khẩu";
            this.chk_HienMK.UseVisualStyleBackColor = true;
            this.chk_HienMK.CheckedChanged += new System.EventHandler(this.chk_HienMK_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Constantia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(87, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "Đăng Nhập";
            // 
            // frmDangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 206);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chk_HienMK);
            this.Controls.Add(this.btnDN);
            this.Controls.Add(this.txtMK);
            this.Controls.Add(this.txtTK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmDangNhap";
            this.Text = "Hệ Thống Quản Lý Giày Thể Thao";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTK;
        private System.Windows.Forms.Button btnDN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMK;
        private System.Windows.Forms.CheckBox chk_HienMK;
        private System.Windows.Forms.Label label3;
    }
}

