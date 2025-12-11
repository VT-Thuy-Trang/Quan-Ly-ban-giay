---

--- BẢNG ĐĂNG NHẬP 
CREATE TABLE DANGNHAP (
    MATK VARCHAR2(10),
    TAIKHOAN VARCHAR2(20) UNIQUE NOT NULL,
    MATKHAU VARCHAR2(30) NOT NULL,
    LOAITK VARCHAR2(8) DEFAULT 'user',
    CONSTRAINT PK_DANGNHAP PRIMARY KEY(MATK),
    CONSTRAINT CHK_LOAITK CHECK (LOAITK IN ('admin', 'user'))
);

--- BẢNG NHÂN VIÊN
CREATE TABLE NHANVIEN (
    MaNV NCHAR(10),
    TenNV NVARCHAR2(30),
    SDTNV VARCHAR2(200),
    NgaySinhNV DATE,
    DiaChiNV NVARCHAR2(50),
    GioiTinhNV NVARCHAR2(10),
    CCCD VARCHAR2(15),
    NVL NUMBER(10),
    Luong NUMBER(10, 2),
    MATK VARCHAR2(10),
    CONSTRAINT PK_MaNV PRIMARY KEY(MaNV),
    CONSTRAINT FK_NHANVIEN_DANGNHAP FOREIGN KEY(MATK) REFERENCES DANGNHAP(MATK)
);

--- BẢNG KHÁCH HÀNG , sequence v  trigger
CREATE SEQUENCE SEQ_KHACHHANG_MAKH START WITH 1000 INCREMENT BY 1 NOCACHE;

CREATE TABLE KHACHHANG (
    MaKH NUMBER(10) PRIMARY KEY,
    TenKH NVARCHAR2(50),
    SDTKH VARCHAR2(200),
    DiaChiKH NVARCHAR2(200),
    GioiTinhKH NVARCHAR2(10),
    NamSinhKH NUMBER(4)
);

CREATE OR REPLACE TRIGGER TRG_KHACHHANG_MAKH
BEFORE INSERT ON KHACHHANG
FOR EACH ROW
BEGIN
    IF :NEW.MaKH IS NULL THEN
        SELECT SEQ_KHACHHANG_MAKH.NEXTVAL INTO :NEW.MaKH FROM DUAL;
    END IF;
END;
/

--- LOẠI GIÀY
CREATE TABLE LOAISP (
    MaL NCHAR(10),
    TenL NVARCHAR2(30),
    CONSTRAINT PK_MaL PRIMARY KEY(MaL)
);

--- BẢNG SẢN PHẨM
CREATE TABLE SANPHAM (
    MaSP NCHAR(10),
    TenSP NVARCHAR2(80),
    GiaBan NUMBER(10, 2) DEFAULT 0,
    GioiTinh NVARCHAR2(4),
    ThongTinSP NVARCHAR2(400),
    ChatLieu NVARCHAR2(100),
    KichThuoc NVARCHAR2(10),
    SoLuongTon NUMBER(10) DEFAULT 0,
    DaBan NUMBER(10) DEFAULT 0,
    TinhTrang NVARCHAR2(20),
    MaL NCHAR(10),
    CONSTRAINT PK_MaSP PRIMARY KEY (MaSP),
    CONSTRAINT FK_SANPHAM_LOAISP FOREIGN KEY (MaL) REFERENCES LOAISP(MaL)
);

--- HÓA ĐƠN, sequence v  trigger
CREATE SEQUENCE SEQ_HOADON_MAHD START WITH 1000 INCREMENT BY 1 NOCACHE;

CREATE TABLE HOADON (
    MaHD NUMBER(10) PRIMARY KEY,
    MaNV NCHAR(10),
    MaKH NUMBER(10),
    NgayBan DATE,
    TongThanhToan NUMBER(10, 2),
    TrangThai NVARCHAR2(20),
    HinhThucThanhToan NVARCHAR2(20),
    GhiChu NVARCHAR2(200),
    CONSTRAINT FK_HOADON_NHANVIEN FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV),
    CONSTRAINT FK_HOADON_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH)
);

CREATE OR REPLACE TRIGGER TRG_HOADON_MAHD
BEFORE INSERT ON HOADON
FOR EACH ROW
BEGIN
    IF :NEW.MaHD IS NULL THEN
        SELECT SEQ_HOADON_MAHD.NEXTVAL INTO :NEW.MaHD FROM DUAL;
    END IF;
END;
/

--- CHI TIẾT HÓA ĐƠN
CREATE TABLE CTHD (
    MaHD NUMBER(10),
    MaSP NCHAR(10),
    KichThuoc VARCHAR2(6),
    SoLuong NUMBER(10),
    GiaBan NUMBER(10, 2),
    ThanhTien NUMBER(10, 2),
    CONSTRAINT PK_CTHD PRIMARY KEY(MaHD, MaSP, KichThuoc),
    CONSTRAINT FK_CTHD_HOADON FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD),
    CONSTRAINT FK_CTHD_SANPHAM FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
);


--- Trigger CẬP NHẬT KHO
CREATE OR REPLACE TRIGGER TG_CAPNHAP_SANPHAM_KHITHEM
AFTER INSERT ON CTHD
FOR EACH ROW
DECLARE
    v_TT NVARCHAR2(20);
    v_SLCONLAI NUMBER(10);
BEGIN
    SELECT TrangThai INTO v_TT FROM HOADON WHERE MaHD = :NEW.MaHD;

    IF v_TT NOT IN ('Chưa thanh toán', '??t c?c') THEN
        SELECT SoLuongTon INTO v_SLCONLAI FROM SANPHAM WHERE MaSP = :NEW.MaSP;

        v_SLCONLAI := v_SLCONLAI - :NEW.SoLuong;

        IF v_SLCONLAI < 0 THEN
            UPDATE SANPHAM
            SET SoLuongTon = 0,
                TinhTrang = 'Hết hàng',
                DaBan = DaBan + :NEW.SoLuong
            WHERE MaSP = :NEW.MaSP;
        ELSE
            UPDATE SANPHAM
            SET SoLuongTon = v_SLCONLAI,
                DaBan = DaBan + :NEW.SoLuong
            WHERE MaSP = :NEW.MaSP;
        END IF;
    END IF;
END;
/
-- Insert ADMIN
INSERT INTO DANGNHAP (MATK, TAIKHOAN, MATKHAU, LOAITK)
VALUES ('ADMIN', 'admin', '123456', 'admin');

-- Insert other users
INSERT INTO DANGNHAP (MATK, TAIKHOAN, MATKHAU)
VALUES ('TK001', 'ngnam', '123456');
INSERT INTO DANGNHAP (MATK, TAIKHOAN, MATKHAU)
VALUES ('TK002', 'huy', '123456');
INSERT INTO DANGNHAP (MATK, TAIKHOAN, MATKHAU)
VALUES ('TK003', 'linh', '123456');

-- Insert NHANVIEN
INSERT INTO NHANVIEN (MaNV, TenNV, SDTNV, NgaySinhNV, DiaChiNV, GioiTinhNV, CCCD, NVL, Luong, MATK)
VALUES ('NV001', N'Nguyễn Nam', '0987654321', TO_DATE('10-10-1999', 'DD-MM-YYYY'), N'Hà Nội', 'Nam', '0123456789', 2020, 8000000, 'TK001');

INSERT INTO NHANVIEN (MaNV, TenNV, SDTNV, NgaySinhNV, DiaChiNV, GioiTinhNV, CCCD, NVL, Luong, MATK)
VALUES ('NV002', N'L  Huy', '0976543210', TO_DATE('20-05-2000', 'DD-MM-YYYY'), N'Hải Phòng', 'Nam', '0234567890', 2021, 7500000, 'TK002');

INSERT INTO NHANVIEN (MaNV, TenNV, SDTNV, NgaySinhNV, DiaChiNV, GioiTinhNV, CCCD, NVL, Luong, MATK)
VALUES ('NV003', N'Trần Linh', '0912345678', TO_DATE('15-03-1998', 'DD-MM-YYYY'), N'Hồ Chí Minh', 'Nữ', '0345678901', 2019, 8500000, 'TK003');

-- Insert LOAISP (LOAI GIÀY THỂ THAO)
INSERT INTO LOAISP (MaL, TenL) VALUES ('L001', N'Giày chạy bộ');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L002', N'Giày bóng rổ');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L003', N'Giày thời trang');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L004', N'Giày đá bóng');

-- Insert SANPHAM (GIÀY THỂ THAO)
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP001', N'Nike Air Zoom Pegasus 39', 2800000, N'Nam', N'Giày chạy bộ nhẹ, êm, hỗ trợ tốt', N'Vải lưới + Foam', N'42', 20, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP002', N'Adidas Ultraboost 22', 3500000, N'Nữ', N'Giày chạy bộ cao cấp với đệm Boost', N'Primeknit + Boost', N'38', 15, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP003', N'Jordan 1 Retro High OG', 5000000, N'Nam', N'Giày bóng rổ phong cách cổ điển', N'Da + Cao su', N'44', 10, N'Còn hàng', 'L002');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP004', N'Puma RS-X  Puzzle', 2400000, N'Nữ', N'Giày thời trang phong cách streetwear', N'Vải + Da tổng hợp', N'39', 25, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP005', N'Nike Mercurial Vapor 14', 4000000, N'Nam', N'Giày đá bóng dành cho tốc độ', N'Flyknit + Cao su', N'43', 12, N'Còn hàng', 'L004');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP006', N'Adidas Stan Smith', 2200000, N'Unisex', N'Mẫu giày iconic từ Adidas, phù hợp mọi phong cách', N'Da tổng hợp + Cao su', N'41', 30, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP007', N'New Balance 530', 2400000, N'Unisex', N'Giày thời trang phối màu retro, độ đệm êm ái', N'Da tổng hợp + Vải lưới', N'40', 20, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP008', N'Nike React Infinity Run 3', 3400000, N'Nữ', N'Giày chạy bộ nhẹ, hỗ trợ tốt cho chân', N'Vải Flyknit + React', N'39', 18, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP009', N'Anta KT7', 2800000, N'Nam', N'Giày bóng rổ Anta KT7 c?a Klay Thompson', N'Vải tổng hợp + EVA', N'44', 17, N'Còn hàng', 'L002');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP010', N'Li-Ning Speed 8', 2600000, N'Nam', N'Giày bóng rổ siêu nhẹ, hỗ trợ bật nhảy', N'Mesh + TPU', N'43', 16, N'Còn hàng', 'L002');


-- Insert KHACHHANG (SU DUNG trigger TINH tRAng MaKH)
INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Phạm Minh Hoàng', '0905123456', N'Đà Nẵng', N'Nam', 1995);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Nguyễn Thị Thu', '0887654321', N'Huế', N'Nữ', 1998);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Hoàng Văn Phúc', '0983112233', N'Đà Nẵng', N'Nam', 1997);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Lệ Thị', '0911223345', N'TP.HCM', N'Nữ', 1999);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Võ Thành Long', '0354789123', N'Hà Nội', N'Nam', 2000);


-- Insert HOADON (s? d?ng trigger từ ộng t?ng MaHD)
INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV002', 1002, TO_DATE('25-12-2023', 'DD-MM-YYYY'), 2400000, N'Đã thanh toán', N'Tiền mặt');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV003', 1003, TO_DATE('26-12-2023', 'DD-MM-YYYY'), 5000000, N'Đã thanh toán', N'Chuyển khoản');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV002', 1004, TO_DATE('27-12-2023', 'DD-MM-YYYY'), 2800000, N'Chưa thanh toán', NULL);

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV001', 1000, SYSDATE, 2800000, N'Đã thanh toán', N'Tiền mặt');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV003', 1001, SYSDATE, 5000000, N'Đã thanh toán', N'Chuyển khoản');

-- Insert CTHD (Chi tiết hóa đơn)
INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien)
VALUES (1002, 'SP006', '41', 1, 2400000, 2400000);

INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien)
VALUES (1003, 'SP003', '44', 1, 5000000, 5000000);

INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien)
VALUES (1004, 'SP001', '42', 1, 2800000, 2800000);

INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien)
VALUES (1000, 'SP001', '42', 1, 2800000, 2800000);

INSERT INTO CTHD (MaHD, MaSP, KichThuoc, SoLuong, GiaBan, ThanhTien)
VALUES (1001, 'SP003', '44', 1, 5000000, 5000000);

COMMIT;



select * from CTHD


GRANT EXECUTE ON SYS.DBMS_CRYPTO TO NgNam;

GRANT EXECUTE ON DBMS_CRYPTO TO NgNam;

----Giai Ma AES ----
--khai bao ham giai ma
CREATE OR REPLACE PACKAGE PKG_SECURITY IS
    FUNCTION Encrypt_AES(p_plainText VARCHAR2) RETURN VARCHAR2;
    FUNCTION Decrypt_AES(p_cipherText VARCHAR2) RETURN VARCHAR2;
END PKG_SECURITY;
/

CREATE OR REPLACE PACKAGE BODY PKG_SECURITY IS
    
    -- B?T BU?C KH?P V?I C#
    v_Key_String VARCHAR2(32) := 'b14ca5898a4e4133bbce2ea2315a1916';
    v_IV_String VARCHAR2(16) := '0000000000000000';
    
    v_IV_Raw RAW(16) := UTL_RAW.CAST_TO_RAW(v_IV_String);
    
    v_Algo NUMBER := DBMS_CRYPTO.ENCRYPT_AES256 
                   + DBMS_CRYPTO.CHAIN_CBC 
                   + DBMS_CRYPTO.PAD_PKCS5; 

    FUNCTION Encrypt_AES(p_plainText VARCHAR2) RETURN VARCHAR2 IS
        v_key_raw     RAW(32);
        v_data_raw    RAW(2000);
        v_encrypted   RAW(2000);
    BEGIN
        IF p_plainText IS NULL THEN RETURN NULL; END IF;
        
        v_key_raw  := UTL_I18N.STRING_TO_RAW(v_Key_String, 'AL32UTF8'); 
        v_data_raw := UTL_I18N.STRING_TO_RAW(p_plainText, 'AL32UTF8');

        v_encrypted := DBMS_CRYPTO.ENCRYPT(
            src => v_data_raw,
            typ => v_Algo,
            key => v_key_raw,
            iv  => v_IV_Raw
        );
        
        RETURN UTL_RAW.CAST_TO_VARCHAR2(UTL_ENCODE.BASE64_ENCODE(v_encrypted));
    EXCEPTION
        WHEN OTHERS THEN
            RETURN NULL;
    END Encrypt_AES;

    FUNCTION Decrypt_AES(p_cipherText VARCHAR2) RETURN VARCHAR2 IS
        v_key_raw     RAW(32);
        v_data_raw    RAW(2000);
        v_decrypted   RAW(2000);
    BEGIN
        IF p_cipherText IS NULL THEN RETURN NULL; END IF;
        
        v_key_raw := UTL_I18N.STRING_TO_RAW(v_Key_String, 'AL32UTF8');

        BEGIN
            v_data_raw := UTL_ENCODE.BASE64_DECODE(UTL_RAW.CAST_TO_RAW(p_cipherText));
        EXCEPTION
            WHEN OTHERS THEN
                RETURN p_cipherText;
        END;

        BEGIN
            v_decrypted := DBMS_CRYPTO.DECRYPT(
                src => v_data_raw,
                typ => v_Algo,
                key => v_key_raw,
                iv  => v_IV_Raw
            );
        EXCEPTION
             WHEN OTHERS THEN
                RETURN p_cipherText;
        END;
        
        RETURN UTL_I18N.RAW_TO_CHAR(v_decrypted, 'AL32UTF8');
    END Decrypt_AES;

END PKG_SECURITY;

/

-- ============================================
-- HỆ THỐNG GHI NHẬT KÝ VÀ GIẢI TRÌNH
-- Standard Auditing và Trigger
-- ============================================

-- 1. TẠO BẢNG AUDIT_LOG ?? LệU TR? NHẬT KÝ
CREATE TABLE AUDIT_LOG (
    AUDIT_ID NUMBER(10) PRIMARY KEY,
    TABLE_NAME VARCHAR2(50) NOT NULL,
    OPERATION VARCHAR2(10) NOT NULL, -- INSERT, UPDATE, DELETE
    OLD_VALUES CLOB,
    NEW_VALUES CLOB,
    PRIMARY_KEY_VALUE VARCHAR2(100),
    USER_NAME VARCHAR2(50),
    SESSION_USER VARCHAR2(50),
    IP_ADDRESS VARCHAR2(50),
    TIMESTAMP DATE DEFAULT SYSDATE,
    DESCRIPTION NVARCHAR2(500),
    CONSTRAINT CHK_OPERATION CHECK (OPERATION IN ('INSERT', 'UPDATE', 'DELETE', 'SELECT'))
);
/
-- Sequence cho AUDIT_ID
CREATE SEQUENCE SEQ_AUDIT_LOG
START WITH 1
INCREMENT BY 1
NOCACHE;
/
-- Trigger t? ??ng t?ng AUDIT_ID
CREATE OR REPLACE TRIGGER TRG_AUDIT_LOG_ID
BEFORE INSERT ON AUDIT_LOG
FOR EACH ROW
BEGIN
    IF :NEW.AUDIT_ID IS NULL THEN
        SELECT SEQ_AUDIT_LOG.NEXTVAL INTO :NEW.AUDIT_ID FROM DUAL;
    END IF;
END;
/

-- 2. TẠO FUNCTION/PROCEDURE H? TR? GHI NHẬT KÝ
CREATE OR REPLACE PACKAGE PKG_AUDIT IS
    PROCEDURE LogAudit(
        p_TableName VARCHAR2,
        p_Operation VARCHAR2,
        p_OldValues CLOB DEFAULT NULL,
        p_NewValues CLOB DEFAULT NULL,
        p_PrimaryKeyValue VARCHAR2 DEFAULT NULL,
        p_Description NVARCHAR2 DEFAULT NULL
    );
    
    FUNCTION GetOldValues(p_TableName VARCHAR2, p_PrimaryKey VARCHAR2, p_PrimaryKeyValue VARCHAR2) RETURN CLOB;
    FUNCTION GetNewValues(p_TableName VARCHAR2, p_PrimaryKey VARCHAR2, p_PrimaryKeyValue VARCHAR2) RETURN CLOB;
END PKG_AUDIT;
/

CREATE OR REPLACE PACKAGE BODY PKG_AUDIT IS
    
    PROCEDURE LogAudit(
        p_TableName VARCHAR2,
        p_Operation VARCHAR2,
        p_OldValues CLOB DEFAULT NULL,
        p_NewValues CLOB DEFAULT NULL,
        p_PrimaryKeyValue VARCHAR2 DEFAULT NULL,
        p_Description NVARCHAR2 DEFAULT NULL
    ) IS
        v_UserName VARCHAR2(50);
        v_SessionUser VARCHAR2(50);
        v_IPAddress VARCHAR2(50);
    BEGIN
        -- Lệy thông tin người dùng
        BEGIN
            SELECT USER INTO v_UserName FROM DUAL;
        EXCEPTION
            WHEN OTHERS THEN
                v_UserName := 'UNKNOWN';
        END;
        
        BEGIN
            SELECT SYS_CONTEXT('USERENV', 'SESSION_USER') INTO v_SessionUser FROM DUAL;
        EXCEPTION
            WHEN OTHERS THEN
                v_SessionUser := v_UserName;
        END;
        
        BEGIN
            SELECT SYS_CONTEXT('USERENV', 'IP_ADDRESS') INTO v_IPAddress FROM DUAL;
        EXCEPTION
            WHEN OTHERS THEN
                v_IPAddress := 'UNKNOWN';
        END;
        
        -- Ghi nh?t ký
        INSERT INTO AUDIT_LOG (
            TABLE_NAME,
            OPERATION,
            OLD_VALUES,
            NEW_VALUES,
            PRIMARY_KEY_VALUE,
            USER_NAME,
            SESSION_USER,
            IP_ADDRESS,
            TIMESTAMP,
            DESCRIPTION
        ) VALUES (
            p_TableName,
            p_Operation,
            p_OldValues,
            p_NewValues,
            p_PrimaryKeyValue,
            v_UserName,
            v_SessionUser,
            v_IPAddress,
            SYSDATE,
            p_Description
        );
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            -- Không throw exception ?? không ?nh h??ng ??n transaction chính
            NULL;
    END LogAudit;
    
    FUNCTION GetOldValues(p_TableName VARCHAR2, p_PrimaryKey VARCHAR2, p_PrimaryKeyValue VARCHAR2) RETURN CLOB IS
        v_Result CLOB;
    BEGIN
        -- Function này có th? ???c m? r?ng ?? l?y giá tr? c? t? b?ng
        RETURN NULL;
    END GetOldValues;
    
    FUNCTION GetNewValues(p_TableName VARCHAR2, p_PrimaryKey VARCHAR2, p_PrimaryKeyValue VARCHAR2) RETURN CLOB IS
        v_Result CLOB;
    BEGIN
        -- Function này có th? ???c m? r?ng ?? l?y giá tr? m?i t? b?ng
        RETURN NULL;
    END GetNewValues;
    
END PKG_AUDIT;
/

-- 3. TẠO TRIGGER CHO BẢNG DANGNHAP
CREATE OR REPLACE TRIGGER TRG_AUDIT_DANGNHAP
AFTER INSERT OR UPDATE OR DELETE ON DANGNHAP
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    -- Xác ??nh thao tác
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := :NEW.MATK;
        v_NewValues := 'MATK: ' || :NEW.MATK || ' | TAIKHOAN: ' || :NEW.TAIKHOAN || 
                      ' | LOAITK: ' || NVL(:NEW.LOAITK, 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := :NEW.MATK;
        v_OldValues := 'MATK: ' || :OLD.MATK || ' | TAIKHOAN: ' || :OLD.TAIKHOAN || 
                      ' | LOAITK: ' || NVL(:OLD.LOAITK, 'NULL');
        v_NewValues := 'MATK: ' || :NEW.MATK || ' | TAIKHOAN: ' || :NEW.TAIKHOAN || 
                      ' | LOAITK: ' || NVL(:NEW.LOAITK, 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := :OLD.MATK;
        v_OldValues := 'MATK: ' || :OLD.MATK || ' | TAIKHOAN: ' || :OLD.TAIKHOAN || 
                      ' | LOAITK: ' || NVL(:OLD.LOAITK, 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'DANGNHAP',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng đăng nhập'
    );
END;
/

-- 4. TẠO TRIGGER CHO BẢNG NHANVIEN
CREATE OR REPLACE TRIGGER TRG_AUDIT_NHANVIEN
AFTER INSERT OR UPDATE OR DELETE ON NHANVIEN
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := :NEW.MaNV;
        v_NewValues := 'MaNV: ' || :NEW.MaNV || ' | TenNV: ' || NVL(:NEW.TenNV, 'NULL') || 
                      ' | SDTNV: ' || NVL(:NEW.SDTNV, 'NULL') || ' | Luong: ' || NVL(TO_CHAR(:NEW.Luong), 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := :NEW.MaNV;
        v_OldValues := 'MaNV: ' || :OLD.MaNV || ' | TenNV: ' || NVL(:OLD.TenNV, 'NULL') || 
                      ' | SDTNV: ' || NVL(:OLD.SDTNV, 'NULL') || ' | Luong: ' || NVL(TO_CHAR(:OLD.Luong), 'NULL');
        v_NewValues := 'MaNV: ' || :NEW.MaNV || ' | TenNV: ' || NVL(:NEW.TenNV, 'NULL') || 
                      ' | SDTNV: ' || NVL(:NEW.SDTNV, 'NULL') || ' | Luong: ' || NVL(TO_CHAR(:NEW.Luong), 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := :OLD.MaNV;
        v_OldValues := 'MaNV: ' || :OLD.MaNV || ' | TenNV: ' || NVL(:OLD.TenNV, 'NULL') || 
                      ' | SDTNV: ' || NVL(:OLD.SDTNV, 'NULL') || ' | Luong: ' || NVL(TO_CHAR(:OLD.Luong), 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'NHANVIEN',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng nhân viên'
    );
END;
/

-- 5. TẠO TRIGGER CHO BẢNG KHACHHANG
CREATE OR REPLACE TRIGGER TRG_AUDIT_KHACHHANG
AFTER INSERT OR UPDATE OR DELETE ON KHACHHANG
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := TO_CHAR(:NEW.MaKH);
        v_NewValues := 'MaKH: ' || TO_CHAR(:NEW.MaKH) || ' | TenKH: ' || NVL(:NEW.TenKH, 'NULL') || 
                      ' | SDTKH: ' || NVL(:NEW.SDTKH, 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := TO_CHAR(:NEW.MaKH);
        v_OldValues := 'MaKH: ' || TO_CHAR(:OLD.MaKH) || ' | TenKH: ' || NVL(:OLD.TenKH, 'NULL') || 
                      ' | SDTKH: ' || NVL(:OLD.SDTKH, 'NULL');
        v_NewValues := 'MaKH: ' || TO_CHAR(:NEW.MaKH) || ' | TenKH: ' || NVL(:NEW.TenKH, 'NULL') || 
                      ' | SDTKH: ' || NVL(:NEW.SDTKH, 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := TO_CHAR(:OLD.MaKH);
        v_OldValues := 'MaKH: ' || TO_CHAR(:OLD.MaKH) || ' | TenKH: ' || NVL(:OLD.TenKH, 'NULL') || 
                      ' | SDTKH: ' || NVL(:OLD.SDTKH, 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'KHACHHANG',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng khách hàng'
    );
END;
/

-- 6. TẠO TRIGGER CHO BẢNG SANPHAM
CREATE OR REPLACE TRIGGER TRG_AUDIT_SANPHAM
AFTER INSERT OR UPDATE OR DELETE ON SANPHAM
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := :NEW.MaSP;
        v_NewValues := 'MaSP: ' || :NEW.MaSP || ' | TenSP: ' || NVL(:NEW.TenSP, 'NULL') || 
                      ' | GiaBan: ' || NVL(TO_CHAR(:NEW.GiaBan), 'NULL') || 
                      ' | SoLuongTon: ' || NVL(TO_CHAR(:NEW.SoLuongTon), 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := :NEW.MaSP;
        v_OldValues := 'MaSP: ' || :OLD.MaSP || ' | TenSP: ' || NVL(:OLD.TenSP, 'NULL') || 
                      ' | GiaBan: ' || NVL(TO_CHAR(:OLD.GiaBan), 'NULL') || 
                      ' | SoLuongTon: ' || NVL(TO_CHAR(:OLD.SoLuongTon), 'NULL');
        v_NewValues := 'MaSP: ' || :NEW.MaSP || ' | TenSP: ' || NVL(:NEW.TenSP, 'NULL') || 
                      ' | GiaBan: ' || NVL(TO_CHAR(:NEW.GiaBan), 'NULL') || 
                      ' | SoLuongTon: ' || NVL(TO_CHAR(:NEW.SoLuongTon), 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := :OLD.MaSP;
        v_OldValues := 'MaSP: ' || :OLD.MaSP || ' | TenSP: ' || NVL(:OLD.TenSP, 'NULL') || 
                      ' | GiaBan: ' || NVL(TO_CHAR(:OLD.GiaBan), 'NULL') || 
                      ' | SoLuongTon: ' || NVL(TO_CHAR(:OLD.SoLuongTon), 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'SANPHAM',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng sản phẩm'
    );
END;
/

-- 7. TẠO TRIGGER CHO BẢNG HOADON
CREATE OR REPLACE TRIGGER TRG_AUDIT_HOADON
AFTER INSERT OR UPDATE OR DELETE ON HOADON
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := TO_CHAR(:NEW.MaHD);
        v_NewValues := 'MaHD: ' || TO_CHAR(:NEW.MaHD) || ' | MaNV: ' || NVL(:NEW.MaNV, 'NULL') || 
                      ' | MaKH: ' || NVL(TO_CHAR(:NEW.MaKH), 'NULL') || 
                      ' | TongThanhToan: ' || NVL(TO_CHAR(:NEW.TongThanhToan), 'NULL') || 
                      ' | TrangThai: ' || NVL(:NEW.TrangThai, 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := TO_CHAR(:NEW.MaHD);
        v_OldValues := 'MaHD: ' || TO_CHAR(:OLD.MaHD) || ' | MaNV: ' || NVL(:OLD.MaNV, 'NULL') || 
                      ' | MaKH: ' || NVL(TO_CHAR(:OLD.MaKH), 'NULL') || 
                      ' | TongThanhToan: ' || NVL(TO_CHAR(:OLD.TongThanhToan), 'NULL') || 
                      ' | TrangThai: ' || NVL(:OLD.TrangThai, 'NULL');
        v_NewValues := 'MaHD: ' || TO_CHAR(:NEW.MaHD) || ' | MaNV: ' || NVL(:NEW.MaNV, 'NULL') || 
                      ' | MaKH: ' || NVL(TO_CHAR(:NEW.MaKH), 'NULL') || 
                      ' | TongThanhToan: ' || NVL(TO_CHAR(:NEW.TongThanhToan), 'NULL') || 
                      ' | TrangThai: ' || NVL(:NEW.TrangThai, 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := TO_CHAR(:OLD.MaHD);
        v_OldValues := 'MaHD: ' || TO_CHAR(:OLD.MaHD) || ' | MaNV: ' || NVL(:OLD.MaNV, 'NULL') || 
                      ' | MaKH: ' || NVL(TO_CHAR(:OLD.MaKH), 'NULL') || 
                      ' | TongThanhToan: ' || NVL(TO_CHAR(:OLD.TongThanhToan), 'NULL') || 
                      ' | TrangThai: ' || NVL(:OLD.TrangThai, 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'HOADON',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng hóa đơn'
    );
END;
/

-- 8. TẠO TRIGGER CHO BẢNG CTHD
CREATE OR REPLACE TRIGGER TRG_AUDIT_CTHD
AFTER INSERT OR UPDATE OR DELETE ON CTHD
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := TO_CHAR(:NEW.MaHD) || '-' || :NEW.MaSP || '-' || :NEW.KichThuoc;
        v_NewValues := 'MaHD: ' || TO_CHAR(:NEW.MaHD) || ' | MaSP: ' || :NEW.MaSP || 
                      ' | KichThuoc: ' || :NEW.KichThuoc || 
                      ' | SoLuong: ' || TO_CHAR(:NEW.SoLuong) || 
                      ' | ThanhTien: ' || TO_CHAR(:NEW.ThanhTien);
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := TO_CHAR(:NEW.MaHD) || '-' || :NEW.MaSP || '-' || :NEW.KichThuoc;
        v_OldValues := 'MaHD: ' || TO_CHAR(:OLD.MaHD) || ' | MaSP: ' || :OLD.MaSP || 
                      ' | KichThuoc: ' || :OLD.KichThuoc || 
                      ' | SoLuong: ' || TO_CHAR(:OLD.SoLuong) || 
                      ' | ThanhTien: ' || TO_CHAR(:OLD.ThanhTien);
        v_NewValues := 'MaHD: ' || TO_CHAR(:NEW.MaHD) || ' | MaSP: ' || :NEW.MaSP || 
                      ' | KichThuoc: ' || :NEW.KichThuoc || 
                      ' | SoLuong: ' || TO_CHAR(:NEW.SoLuong) || 
                      ' | ThanhTien: ' || TO_CHAR(:NEW.ThanhTien);
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := TO_CHAR(:OLD.MaHD) || '-' || :OLD.MaSP || '-' || :OLD.KichThuoc;
        v_OldValues := 'MaHD: ' || TO_CHAR(:OLD.MaHD) || ' | MaSP: ' || :OLD.MaSP || 
                      ' | KichThuoc: ' || :OLD.KichThuoc || 
                      ' | SoLuong: ' || TO_CHAR(:OLD.SoLuong) || 
                      ' | ThanhTien: ' || TO_CHAR(:OLD.ThanhTien);
    END IF;
    
    PKG_AUDIT.LogAudit(
        'CTHD',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng chi tiết hóa đơn'
    );
END;
/

-- 9. TẠO TRIGGER CHO BaNG LOAISP
CREATE OR REPLACE TRIGGER TRG_AUDIT_LOAISP
AFTER INSERT OR UPDATE OR DELETE ON LOAISP
FOR EACH ROW
DECLARE
    v_Operation VARCHAR2(10);
    v_OldValues CLOB;
    v_NewValues CLOB;
    v_PrimaryKey VARCHAR2(100);
BEGIN
    IF INSERTING THEN
        v_Operation := 'INSERT';
        v_PrimaryKey := :NEW.MaL;
        v_NewValues := 'MaL: ' || :NEW.MaL || ' | TenL: ' || NVL(:NEW.TenL, 'NULL');
    ELSIF UPDATING THEN
        v_Operation := 'UPDATE';
        v_PrimaryKey := :NEW.MaL;
        v_OldValues := 'MaL: ' || :OLD.MaL || ' | TenL: ' || NVL(:OLD.TenL, 'NULL');
        v_NewValues := 'MaL: ' || :NEW.MaL || ' | TenL: ' || NVL(:NEW.TenL, 'NULL');
    ELSIF DELETING THEN
        v_Operation := 'DELETE';
        v_PrimaryKey := :OLD.MaL;
        v_OldValues := 'MaL: ' || :OLD.MaL || ' | TenL: ' || NVL(:OLD.TenL, 'NULL');
    END IF;
    
    PKG_AUDIT.LogAudit(
        'LOAISP',
        v_Operation,
        v_OldValues,
        v_NewValues,
        v_PrimaryKey,
        'Thao tác trên bảng loại sản phẩm'
    );
END;
/

-- 10. Tap bang xem nhat ky
CREATE OR REPLACE VIEW VW_AUDIT_LOG AS
SELECT 
    AUDIT_ID,
    TABLE_NAME,
    OPERATION,
    PRIMARY_KEY_VALUE,
    USER_NAME,
    SESSION_USER,
    IP_ADDRESS,
    TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS THOI_GIAN,
    DESCRIPTION,
    CASE 
        WHEN OLD_VALUES IS NOT NULL THEN SUBSTR(OLD_VALUES, 1, 200)
        ELSE NULL
    END AS GIA_TRI_CU,
    CASE 
        WHEN NEW_VALUES IS NOT NULL THEN SUBSTR(NEW_VALUES, 1, 200)
        ELSE NULL
    END AS GIA_TRI_MOI
FROM AUDIT_LOG
ORDER BY TIMESTAMP DESC;

-- 11. STANDARD AUDITING
-- chay voi cac quyen DBA (SYS or SYSTEM)

-- Audit cho DANGNHAP
AUDIT INSERT, UPDATE, DELETE ON DANGNHAP BY ACCESS;

-- Audit cho NHANVIEN
AUDIT INSERT, UPDATE, DELETE ON NHANVIEN BY ACCESS;

-- Audit cho KHACHHANG
AUDIT INSERT, UPDATE, DELETE ON KHACHHANG BY ACCESS;

-- Audit cho SANPHAM
AUDIT INSERT, UPDATE, DELETE ON SANPHAM BY ACCESS;

-- Audit cho HOADON
AUDIT INSERT, UPDATE, DELETE ON HOADON BY ACCESS;

-- Audit cho CTHD
AUDIT INSERT, UPDATE, DELETE ON CTHD BY ACCESS;

-- Audit cho b?ng LOAISP
AUDIT INSERT, UPDATE, DELETE ON LOAISP BY ACCESS;

-- 12. TẠO CÁC PROCEDURE sp xem nhat ky
CREATE OR REPLACE PROCEDURE SP_XEM_NHAT_KY_THEO_BẢNG(
    p_TableName VARCHAR2,
    p_FromDate DATE DEFAULT NULL,
    p_ToDate DATE DEFAULT NULL
) IS
BEGIN
    FOR rec IN (
        SELECT 
            AUDIT_ID,
            TABLE_NAME,
            OPERATION,
            USER_NAME,
            TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS THOI_GIAN
        FROM AUDIT_LOG
        WHERE TABLE_NAME = p_TableName
        AND (p_FromDate IS NULL OR TIMESTAMP >= p_FromDate)
        AND (p_ToDate IS NULL OR TIMESTAMP <= p_ToDate + 1)
        ORDER BY TIMESTAMP DESC
    ) LOOP
        DBMS_OUTPUT.PUT_LINE('ID: ' || rec.AUDIT_ID || 
                            ' | Bang: ' || rec.TABLE_NAME || 
                            ' | Thao tác: ' || rec.OPERATION || 
                            ' | Nguoi dùng: ' || rec.USER_NAME || 
                            ' | Thoi gian: ' || rec.THOI_GIAN);
    END LOOP;
END;
/

-- 13. TẠO FUNCTION ham so luong THAO TÁC
CREATE OR REPLACE FUNCTION FN_DEM_THAO_TAC(
    p_TableName VARCHAR2 DEFAULT NULL,
    p_Operation VARCHAR2 DEFAULT NULL,
    p_FromDate DATE DEFAULT NULL,
    p_ToDate DATE DEFAULT NULL
) RETURN NUMBER IS
    v_Count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_Count
    FROM AUDIT_LOG
    WHERE (p_TableName IS NULL OR TABLE_NAME = p_TableName)
    AND (p_Operation IS NULL OR OPERATION = p_Operation)
    AND (p_FromDate IS NULL OR TIMESTAMP >= p_FromDate)
    AND (p_ToDate IS NULL OR TIMESTAMP <= p_ToDate + 1);
    
    RETURN v_Count;
END;


-- 14. TẠO INDEX tang hieu suat truy van
CREATE INDEX IDX_AUDIT_LOG_TABLE_NAME ON AUDIT_LOG(TABLE_NAME);
CREATE INDEX IDX_AUDIT_LOG_OPERATION ON AUDIT_LOG(OPERATION);
CREATE INDEX IDX_AUDIT_LOG_TIMESTAMP ON AUDIT_LOG(TIMESTAMP);
CREATE INDEX IDX_AUDIT_LOG_USER_NAME ON AUDIT_LOG(USER_NAME);

-- 15. GRANT QUY?N CHO NG??I DÙNG
-- Thay 'NgNam' b?ng tên user c?a b?n
GRANT SELECT, INSERT ON AUDIT_LOG TO NgNam;
GRANT SELECT ON VW_AUDIT_LOG TO NgNam;
GRANT EXECUTE ON PKG_AUDIT TO NgNam;
GRANT EXECUTE ON SP_XEM_NHAT_KY_THEO_BẢNG TO NgNam;
GRANT EXECUTE ON FN_DEM_THAO_TAC TO NgNam;

-- 16. TẠO PROCEDURE XÓA NHẬT KÝ C? (B?O TRÌ)
CREATE OR REPLACE PROCEDURE SP_XOA_NHAT_KY_CU(
    p_DaysOld NUMBER DEFAULT 90
) IS
    v_DeletedCount NUMBER;
BEGIN
    DELETE FROM AUDIT_LOG
    WHERE TIMESTAMP < SYSDATE - p_DaysOld;
    
    v_DeletedCount := SQL%ROWCOUNT;
    COMMIT;
    
    DBMS_OUTPUT.PUT_LINE('?ã xóa ' || v_DeletedCount || ' b?n ghi nh?t ký c? h?n ' || p_DaysOld || ' ngày.');
END;
/

GRANT EXECUTE ON SP_XOA_NHAT_KY_CU TO NgNam;

COMMIT;

-- ============================================
-- H??NG D?N S? D?NG
-- ============================================
-- 1. Xem tất cả nhật ký:
--    SELECT * FROM VW_AUDIT_LOG;
--
-- 2. Xem nhật ký theo b?ng:
--    SELECT * FROM VW_AUDIT_LOG WHERE TABLE_NAME = 'SANPHAM';
--
-- 3. Xem nhật ký theo thao tác:
--    SELECT * FROM VW_AUDIT_LOG WHERE OPERATION = 'UPDATE';
--
-- 4. Xem nhật ký theo người dùng:
--    SELECT * FROM VW_AUDIT_LOG WHERE USER_NAME = 'NGNAM';
--
-- 5. ??m s? l??ng thao tác:
--    SELECT FN_DEM_THAO_TAC('SANPHAM', 'UPDATE') FROM DUAL;
--
-- 6. Xem nhật ký trong kho?ng th?i gian:
--    SELECT * FROM VW_AUDIT_LOG 
--    WHERE TIMESTAMP BETWEEN TO_DATE('01/01/2024', 'DD/MM/YYYY') 
--                        AND TO_DATE('31/12/2024', 'DD/MM/YYYY');
--
-- 7. Xóa nh?t ký c? h?n 90 ngày:
--    EXEC SP_XOA_NHAT_KY_CU(90);

---------------------
-- 1. Tao TABLESPACE 
-- XEM DIA CHI FILE O DAU
--SELECT file_name FROM dba_data_files;
--
--CREATE TABLESPACE NgNam
--DATAFILE 'D:\APP\LENOVO\ORADATA\ORCL\NgNam.dbf' SIZE 100M AUTOEXTEND ON NEXT 10M;
--
---- 2. T?o PROFILE (Chính sách b?o m?t tài kho?n)
--CREATE PROFILE NgNam LIMIT 
--  FAILED_LOGIN_ATTEMPTS 3 
--  CONNECT_TIME 60
--  SESSIONS_PER_USER 2;
--
---- 3. T?o ROLE (Nhóm quy?n)
--CREATE ROLE ROLE_BANHANG;
--GRANT CONNECT, RESOURCE TO ROLE_BANHANG;
--GRANT SELECT, INSERT ON NgNam.HOADON TO ROLE_BANHANG; 
--
---- 4. T?o USER và gán các thành ph?n trên
--CREATE USER NgNam IDENTIFIED BY 123123 
--  DEFAULT TABLESPACE TBS_QLGIAY 
--  PROFILE PRO_NHANVIEN;
--
--GRANT ROLE_BANHANG TO NgNam;
--
----3.3. Thi?t l?p công c? FGA, cài đặt VPD, cài đặt OLS
----Thi?t l?p FGA
--
--BEGIN
--  DBMS_FGA.ADD_POLICY(
--    object_schema   => 'NgNam',
--    object_name     => 'NHANVIEN',
--    policy_name     => 'AUDIT_XEM_LUONG',
--    audit_column    => 'LUONG',
--    statement_types => 'SELECT'
--  );
--END;
--
---- Câu l?nh xem nh?t ký FGA:
--SELECT * FROM DBA_FGA_AUDIT_TRAIL;
-----Ki?m tra xem chính sách ?ã ?úng ch?a 
--
--SELECT POLICY_NAME, OBJECT_NAME, POLICY_COLUMN 
--FROM DBA_AUDIT_POLICIES 
--WHERE POLICY_NAME = 'AUDIT_XEM_LUONG';
--
--
---- Cài ??t VPD
---- 1. T?o hàm chính sách (Policy Function)
--CREATE OR REPLACE FUNCTION FUNC_XEM_TT_RIENG (p_schema IN VARCHAR2, p_object IN VARCHAR2) 
--  RETURN VARCHAR2 AS
--BEGIN
--  RETURN 'MATK = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';
--END;
--
--
---- 2. G?n chính sách vào b?ng NHANVIEN
--BEGIN
--  DBMS_RLS.ADD_POLICY(
--    object_schema   => 'NgNam',
--    object_name     => 'NHANVIEN',
--    policy_name     => 'VPD_NHANVIEN',
--    function_schema => 'SYS',
--    policy_function => 'FUNC_XEM_TT_RIENG',
--    statement_types => 'SELECT'
--  );
--END;
--
--
------ cai dat OLS
----- kiem tra trang thai
--SELECT VALUE FROM v$option WHERE PARAMETER = 'Oracle Label Security';
--
---- 1. đăng ký OLS voi Database
--EXEC LBACSYS.CONFIGURE_OLS;
--
---- 2.bat tinh nang ols
--EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;
--
--SHUTDOWN IMMEDIATE;
--STARTUP;
----chính sách nhãn
--BEGIN
--  SA_SYSDBA.CREATE_POLICY(policy_name => 'BAOMAT_SANPHAM', column_name => 'NHAN_BAOMAT');
--END;
--
---- CONG_KHAI (Public) và NOI_BO (Confidential)
--EXEC SA_COMPONENTS.CREATE_LEVEL('BAOMAT_SANPHAM', 1000, 'PUB', 'Cong Khai');
--EXEC SA_COMPONENTS.CREATE_LEVEL('BAOMAT_SANPHAM', 2000, 'CONF', 'Noi Bo');