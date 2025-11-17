namespace QL_GiayTT.frm
{
    partial class frmThongTinSP
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbTTSP = new System.Windows.Forms.GroupBox();
            this.lbTTSP = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lb_SP_SC = new System.Windows.Forms.Label();
            this.btnThemVaoGioHang = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nud_SL = new System.Windows.Forms.NumericUpDown();
            this.gb_KichCo = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbTenSP = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbGiaSP = new System.Windows.Forms.Label();
            this.pBHinh = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.gbTTSP.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SL)).BeginInit();
            this.gb_KichCo.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBHinh)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.gbTTSP);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.pBHinh);
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(978, 908);
            this.panel1.TabIndex = 18;
            // 
            // gbTTSP
            // 
            this.gbTTSP.Controls.Add(this.lbTTSP);
            this.gbTTSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTTSP.Location = new System.Drawing.Point(10, 439);
            this.gbTTSP.Name = "gbTTSP";
            this.gbTTSP.Size = new System.Drawing.Size(958, 466);
            this.gbTTSP.TabIndex = 27;
            this.gbTTSP.TabStop = false;
            this.gbTTSP.Text = "Thông tin sản phẩm";
            // 
            // lbTTSP
            // 
            this.lbTTSP.AutoSize = true;
            this.lbTTSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTTSP.Location = new System.Drawing.Point(23, 45);
            this.lbTTSP.MaximumSize = new System.Drawing.Size(910, 428);
            this.lbTTSP.Name = "lbTTSP";
            this.lbTTSP.Size = new System.Drawing.Size(79, 29);
            this.lbTTSP.TabIndex = 0;
            this.lbTTSP.Text = "label3";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lb_SP_SC);
            this.panel3.Controls.Add(this.btnThemVaoGioHang);
            this.panel3.Controls.Add(this.btnHuy);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.nud_SL);
            this.panel3.Controls.Add(this.gb_KichCo);
            this.panel3.Location = new System.Drawing.Point(421, 181);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(547, 252);
            this.panel3.TabIndex = 26;
            // 
            // lb_SP_SC
            // 
            this.lb_SP_SC.AutoSize = true;
            this.lb_SP_SC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_SP_SC.ForeColor = System.Drawing.Color.Black;
            this.lb_SP_SC.Location = new System.Drawing.Point(235, 150);
            this.lb_SP_SC.Name = "lb_SP_SC";
            this.lb_SP_SC.Size = new System.Drawing.Size(137, 20);
            this.lb_SP_SC.TabIndex = 0;
            this.lb_SP_SC.Text = "Sản phẩm sẵn có:";
            // 
            // btnThemVaoGioHang
            // 
            this.btnThemVaoGioHang.BackColor = System.Drawing.Color.Bisque;
            this.btnThemVaoGioHang.Font = new System.Drawing.Font("Constantia", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemVaoGioHang.ForeColor = System.Drawing.Color.Tomato;
            this.btnThemVaoGioHang.Location = new System.Drawing.Point(17, 194);
            this.btnThemVaoGioHang.Name = "btnThemVaoGioHang";
            this.btnThemVaoGioHang.Size = new System.Drawing.Size(244, 55);
            this.btnThemVaoGioHang.TabIndex = 3;
            this.btnThemVaoGioHang.Text = "Thêm vào giỏ hàng";
            this.btnThemVaoGioHang.UseVisualStyleBackColor = false;
            this.btnThemVaoGioHang.Click += new System.EventHandler(this.btnThemVaoGioHang_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.Tomato;
            this.btnHuy.Font = new System.Drawing.Font("Sitka Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.SystemColors.Window;
            this.btnHuy.Location = new System.Drawing.Point(291, 194);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(239, 55);
            this.btnHuy.TabIndex = 3;
            this.btnHuy.Text = "Huỷ";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "SỐ LƯỢNG:";
            // 
            // nud_SL
            // 
            this.nud_SL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_SL.Location = new System.Drawing.Point(141, 144);
            this.nud_SL.Name = "nud_SL";
            this.nud_SL.Size = new System.Drawing.Size(69, 30);
            this.nud_SL.TabIndex = 1;
            // 
            // gb_KichCo
            // 
            this.gb_KichCo.Controls.Add(this.radioButton3);
            this.gb_KichCo.Controls.Add(this.radioButton5);
            this.gb_KichCo.Controls.Add(this.radioButton2);
            this.gb_KichCo.Controls.Add(this.radioButton4);
            this.gb_KichCo.Controls.Add(this.radioButton1);
            this.gb_KichCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_KichCo.Location = new System.Drawing.Point(17, 12);
            this.gb_KichCo.Name = "gb_KichCo";
            this.gb_KichCo.Size = new System.Drawing.Size(513, 113);
            this.gb_KichCo.TabIndex = 0;
            this.gb_KichCo.TabStop = false;
            this.gb_KichCo.Text = "KÍCH CỠ:";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(352, 34);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(48, 29);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Text = "41";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(195, 73);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(76, 29);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.Text = "42";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(195, 34);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(54, 29);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Text = "39";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(37, 73);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(62, 29);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.Text = "40";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(37, 34);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(51, 29);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "38";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbTenSP);
            this.panel2.Location = new System.Drawing.Point(421, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 122);
            this.panel2.TabIndex = 24;
            // 
            // lbTenSP
            // 
            this.lbTenSP.AutoSize = true;
            this.lbTenSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenSP.Location = new System.Drawing.Point(2, 0);
            this.lbTenSP.MaximumSize = new System.Drawing.Size(547, 122);
            this.lbTenSP.Name = "lbTenSP";
            this.lbTenSP.Size = new System.Drawing.Size(87, 30);
            this.lbTenSP.TabIndex = 0;
            this.lbTenSP.Text = "label1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lbGiaSP);
            this.panel4.Location = new System.Drawing.Point(421, 131);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(547, 40);
            this.panel4.TabIndex = 25;
            // 
            // lbGiaSP
            // 
            this.lbGiaSP.AutoSize = true;
            this.lbGiaSP.Font = new System.Drawing.Font("Franklin Gothic Medium", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGiaSP.ForeColor = System.Drawing.Color.Red;
            this.lbGiaSP.Location = new System.Drawing.Point(11, 4);
            this.lbGiaSP.Name = "lbGiaSP";
            this.lbGiaSP.Size = new System.Drawing.Size(90, 36);
            this.lbGiaSP.TabIndex = 0;
            this.lbGiaSP.Text = "label2";
            // 
            // pBHinh
            // 
            this.pBHinh.Location = new System.Drawing.Point(10, 3);
            this.pBHinh.Name = "pBHinh";
            this.pBHinh.Size = new System.Drawing.Size(400, 430);
            this.pBHinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBHinh.TabIndex = 23;
            this.pBHinh.TabStop = false;
            // 
            // frmThongTinSP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(978, 944);
            this.Controls.Add(this.panel1);
            this.Name = "frmThongTinSP";
            this.Text = "Thông tin sản phẩm";
            this.Load += new System.EventHandler(this.frmThongTinSP_Load);
            this.panel1.ResumeLayout(false);
            this.gbTTSP.ResumeLayout(false);
            this.gbTTSP.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SL)).EndInit();
            this.gb_KichCo.ResumeLayout(false);
            this.gb_KichCo.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBHinh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbTTSP;
        private System.Windows.Forms.Label lbTTSP;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnThemVaoGioHang;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_SL;
        private System.Windows.Forms.GroupBox gb_KichCo;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbTenSP;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbGiaSP;
        private System.Windows.Forms.PictureBox pBHinh;
        private System.Windows.Forms.Label lb_SP_SC;


    }
}