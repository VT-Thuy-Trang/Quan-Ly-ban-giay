-- Xóa user cũ để làm sạch môi trường
BEGIN
    EXECUTE IMMEDIATE 'DROP USER NgNam CASCADE';
EXCEPTION
    WHEN OTHERS THEN
        NULL; -- Bỏ qua nếu user không tồn tại
END;
/

-- Tạo user mới
CREATE USER NgNam IDENTIFIED BY 123;

GRANT CREATE SESSION TO NgNam;
GRANT CREATE TABLE TO NgNam;
GRANT CREATE SEQUENCE TO NgNam;
GRANT CREATE TRIGGER TO NgNam;
GRANT CREATE VIEW TO NgNam;
GRANT CREATE PROCEDURE TO NgNam;
GRANT UNLIMITED TABLESPACE TO NgNam;
GRANT EXECUTE ON SYS.DBMS_CRYPTO TO NgNam; -- Cần cho mã hóa AES
GRANT EXECUTE ON SYS.DBMS_RLS TO NgNam;    -- Cần cho VPD (nếu dùng)
GRANT EXECUTE ON SYS.DBMS_FGA TO NgNam;    -- Cần cho FGA (nếu dùng)
GRANT EXECUTE ON DBMS_CRYPTO TO NgNam;



-- Bước 1: Tạo PROFILE bảo mật
DROP PROFILE PROFILE_QLGIAYTT CASCADE;  -- Xóa PROFILE cũ nếu đã tồn tại (bỏ comment nếu cần)

CREATE PROFILE PROFILE_QLGIAYTT LIMIT 
  FAILED_LOGIN_ATTEMPTS 3          -- Khóa tài khoản sau 3 lần đăng nhập sai
  PASSWORD_LOCK_TIME 1              -- Khóa tài khoản 1 ngày (UNLIMITED = vĩnh viễn)
  CONNECT_TIME 60                   -- Giới hạn mỗi phiên kết nối tối đa 60 phút
  IDLE_TIME 30                      -- Tự động ngắt kết nối nếu không hoạt động 30 phút
  SESSIONS_PER_USER 2 ;              -- Mỗi user chỉ được mở tối đa 2 session đồng thời


-- Bước 2: Gán PROFILE cho các user cụ thể
-- Thay 'NgNam' bằng tên user/schema của bạn
ALTER USER NgNam PROFILE PROFILE_QLGIAYTT;

-- Bước 3: Kiểm tra PROFILE đã được tạo
SELECT 
    profile,
    resource_name,
    resource_type,
    limit
FROM dba_profiles 
WHERE profile = 'PROFILE_QLGIAYTT' 
ORDER BY resource_type, resource_name;

-- Bước 4: Kiểm tra user nào đang sử dụng PROFILE này
SELECT 
    username,
    profile,
    account_status,
    lock_date,
    expiry_date,
    created
FROM dba_users 
WHERE profile = 'PROFILE_QLGIAYTT'
ORDER BY username;

COMMIT;

-- 1. Mở khóa tài khoản nếu bị khóa do đăng nhập sai nhiều lần
-- ALTER USER NgNam ACCOUNT UNLOCK;
