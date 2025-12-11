--- T?o ng??i dùng
CREATE USER NgNam IDENTIFIED BY 123123;
GRANT CREATE SESSION TO NgNam;
GRANT CREATE TABLE TO NgNam;
ALTER USER NgNam QUOTA 1000M ON USERS;
GRANT CREATE PROCEDURE TO NgNam;
GRANT EXECUTE ON SYS.DBMS_CRYPTO TO NgNam;
GRANT EXECUTE ON DBMS_CRYPTO TO NgNam;


--- B?ng ??ng nh?p
CREATE TABLE DANGNHAP (
    MATK VARCHAR2(10),
    TAIKHOAN VARCHAR2(20) UNIQUE NOT NULL,
    MATKHAU VARCHAR2(30) NOT NULL,
    LOAITK VARCHAR2(8) DEFAULT 'user',
    CONSTRAINT PK_DANGNHAP PRIMARY KEY(MATK),
    CONSTRAINT CHK_LOAITK CHECK (LOAITK IN ('admin', 'user'))
);

--- B?ng nhân viên
CREATE TABLE NHANVIEN (
    MaNV NCHAR(10),
    TenNV NVARCHAR2(30),
    SDTNV VARCHAR2(15),
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

--- B?ng khách hàng, sequence và trigger
CREATE SEQUENCE SEQ_KHACHHANG_MAKH START WITH 1000 INCREMENT BY 1 NOCACHE;

CREATE TABLE KHACHHANG (
    MaKH NUMBER(10) PRIMARY KEY,
    TenKH NVARCHAR2(50),
    SDTKH VARCHAR2(10),
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

--- Lo?i giày
CREATE TABLE LOAISP (
    MaL NCHAR(10),
    TenL NVARCHAR2(30),
    CONSTRAINT PK_MaL PRIMARY KEY(MaL)
);

--- B?ng s?n ph?m (giày)
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

--- Hóa ??n, sequence và trigger
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

--- Chi ti?t hóa ??n
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


--- Trigger c?p nh?t t?n kho
CREATE OR REPLACE TRIGGER TG_CAPNHAP_SANPHAM_KHITHEM
AFTER INSERT ON CTHD
FOR EACH ROW
DECLARE
    v_TT NVARCHAR2(20);
    v_SLCONLAI NUMBER(10);
BEGIN
    SELECT TrangThai INTO v_TT FROM HOADON WHERE MaHD = :NEW.MaHD;

    IF v_TT NOT IN ('Ch?a thanh toán', '??t c?c') THEN
        SELECT SoLuongTon INTO v_SLCONLAI FROM SANPHAM WHERE MaSP = :NEW.MaSP;

        v_SLCONLAI := v_SLCONLAI - :NEW.SoLuong;

        IF v_SLCONLAI < 0 THEN
            UPDATE SANPHAM
            SET SoLuongTon = 0,
                TinhTrang = 'H?t hàng',
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
VALUES ('NV001', N'Nguy?n Nam', '0987654321', TO_DATE('10-10-1999', 'DD-MM-YYYY'), N'Hà N?i', 'Nam', '0123456789', 2020, 8000000, 'TK001');

INSERT INTO NHANVIEN (MaNV, TenNV, SDTNV, NgaySinhNV, DiaChiNV, GioiTinhNV, CCCD, NVL, Luong, MATK)
VALUES ('NV002', N'Lê Huy', '0976543210', TO_DATE('20-05-2000', 'DD-MM-YYYY'), N'H?i Phòng', 'Nam', '0234567890', 2021, 7500000, 'TK002');

INSERT INTO NHANVIEN (MaNV, TenNV, SDTNV, NgaySinhNV, DiaChiNV, GioiTinhNV, CCCD, NVL, Luong, MATK)
VALUES ('NV003', N'Tr?n Linh', '0912345678', TO_DATE('15-03-1998', 'DD-MM-YYYY'), N'H? Chí Minh', 'N?', '0345678901', 2019, 8500000, 'TK003');

-- Insert LOAISP (Lo?i giày th? thao)
INSERT INTO LOAISP (MaL, TenL) VALUES ('L001', N'Giày ch?y b?');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L002', N'Giày bóng r?');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L003', N'Giày th?i trang');
INSERT INTO LOAISP (MaL, TenL) VALUES ('L004', N'Giày ?á bóng');

-- Insert SANPHAM (Giày th? thao)
INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP001', N'Nike Air Zoom Pegasus 39', 2800000, N'Nam', N'Giày ch?y b? nh?, ?àn h?i t?t', N'V?i l??i + Foam', N'42', 20, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP002', N'Adidas Ultraboost 22', 3500000, N'N?', N'Giày ch?y b? cao c?p v?i ??m Boost', N'Primeknit + Boost', N'38', 15, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP003', N'Jordan 1 Retro High OG', 5000000, N'Nam', N'Giày bóng r? phong cách c? ?i?n', N'Da + Cao su', N'44', 10, N'Còn hàng', 'L002');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP004', N'Puma RS-X³ Puzzle', 2400000, N'N?', N'Giày th?i trang phong cách streetwear', N'V?i + Da t?ng h?p', N'39', 25, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP005', N'Nike Mercurial Vapor 14', 4000000, N'Nam', N'Giày ?á bóng dành cho t?c ??', N'Flyknit + Cao su', N'43', 12, N'Còn hàng', 'L004');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP006', N'Adidas Stan Smith', 2200000, N'Unisex', N'M?u giày iconic t? Adidas, phù h?p m?i phong cách', N'Da t?ng h?p + Cao su', N'41', 30, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP007', N'New Balance 530', 2400000, N'Unisex', N'Giày th?i trang ph?i màu retro, ?? dày êm ái', N'Da t?ng h?p + V?i l??i', N'40', 20, N'Còn hàng', 'L003');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP008', N'Nike React Infinity Run 3', 3400000, N'N?', N'Giày ch?y b? nh?, h? tr? t?t cho chân', N'V?i Flyknit + React', N'39', 18, N'Còn hàng', 'L001');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP009', N'Anta KT7', 2800000, N'Nam', N'Giày bóng r? Anta KT7 c?a Klay Thompson', N'V?i t?ng h?p + EVA', N'44', 17, N'Còn hàng', 'L002');

INSERT INTO SANPHAM (MaSP, TenSP, GiaBan, GioiTinh, ThongTinSP, ChatLieu, KichThuoc, SoLuongTon, TinhTrang, MaL)
VALUES ('SP010', N'Li-Ning Speed 8', 2600000, N'Nam', N'Giày bóng r? siêu nh?, h? tr? b??c nh?y', N'Mesh + TPU', N'43', 16, N'Còn hàng', 'L002');


-- Insert KHACHHANG (s? d?ng trigger t? ??ng t?ng MaKH)
INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Ph?m Minh Hoàng', '0905123456', N'?à N?ng', N'Nam', 1995);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Nguy?n Th? Thu', '0887654321', N'Hu?', N'N?', 1998);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Hoàng V?n Phúc', '0983112233', N'?à N?ng', N'Nam', 1997);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'L? Th?', '0911223345', N'TP.HCM', N'N?', 1999);

INSERT INTO KHACHHANG (TenKH, SDTKH, DiaChiKH, GioiTinhKH, NamSinhKH)
VALUES (N'Võ Thành Long', '0354789123', N'Hà N?i', N'Nam', 2000);


-- Insert HOADON (s? d?ng trigger t? ??ng t?ng MaHD)
INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV002', 1002, TO_DATE('25-12-2023', 'DD-MM-YYYY'), 2400000, N'?ã thanh toán', N'Ti?n m?t');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV003', 1003, TO_DATE('26-12-2023', 'DD-MM-YYYY'), 5000000, N'?ã thanh toán', N'Chuy?n kho?n');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV002', 1004, TO_DATE('27-12-2023', 'DD-MM-YYYY'), 2800000, N'Ch?a thanh toán', NULL);

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV001', 1000, SYSDATE, 2800000, N'?ã thanh toán', N'Ti?n m?t');

INSERT INTO HOADON (MaNV, MaKH, NgayBan, TongThanhToan, TrangThai, HinhThucThanhToan)
VALUES ('NV003', 1001, SYSDATE, 5000000, N'?ã thanh toán', N'Chuy?n kho?n');

-- Insert CTHD (Chi ti?t hóa ??n)
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


----Giai Ma AES ----
--khai bao ham giai ma
CREATE OR REPLACE PACKAGE PKG_SECURITY IS
    FUNCTION Encrypt_AES(p_plainText VARCHAR2) RETURN VARCHAR2;
    FUNCTION Decrypt_AES(p_cipherText VARCHAR2) RETURN VARCHAR2;
END PKG_SECURITY;


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



SELECT TAIKHOAN, MATKHAU FROM DANGNHAP;
SELECT MaKH, TenKH, SDTKH FROM KHACHHANG;
SELECT MaNV, TenNV, SDTNV, CCCD FROM NHANVIEN;
