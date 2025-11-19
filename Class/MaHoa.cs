using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QL_GiayTT.Class
{
    internal class MaHoa
    {
        private static readonly string key = "b14ca5898a4e4133bbce2ea2315a1916";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            try
            {
                byte[] iv = new byte[16];
                byte[] array;
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter((Stream)cs))
                            {
                                sw.Write(plainText);
                            }
                            array = ms.ToArray();
                        }
                    }
                }
                return Convert.ToBase64String(array);
            }
            catch { return plainText; }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        using (CryptoStream cs = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader((Stream)cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch { return cipherText; } // Nếu lỗi (do dữ liệu cũ chưa mã hóa) thì trả về gốc
        }
    }
}
