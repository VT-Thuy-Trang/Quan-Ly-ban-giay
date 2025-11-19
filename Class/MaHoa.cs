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
        //tao key AES
        private static readonly string key = "b14ca5898a4e4133bbce2ea2315a1916";

        // mã hóa AES
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

        // giải mã AES
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
            catch { return cipherText; } 
        }


        // mã hóa lai 
        public static class HybridEncryption
        {
            public static string EncryptHybrid(string plainText)
            {
                try
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.GenerateKey();
                        aes.GenerateIV();

                        // 1. Mã hóa dữ liệu bằng AES
                        string aesEncryptedData;
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ms.Write(aes.IV, 0, aes.IV.Length);
                            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            aesEncryptedData = Convert.ToBase64String(ms.ToArray());
                        }

                        // 2. Mã hóa Key AES bằng RSA
                        RSAEncryption rsa = new RSAEncryption();
                        string aesKeyString = Convert.ToBase64String(aes.Key);

                        byte[] encryptedKeyBytes = rsa.Encrypt(aesKeyString);
                        string encryptedAesKey = Convert.ToBase64String(encryptedKeyBytes);

                        return $"{encryptedAesKey}||{aesEncryptedData}";
                    }
                }
                catch { return plainText; }
            }

            public static string DecryptHybrid(string hybridCipherText)
            {
                try
                {
                    string[] parts = hybridCipherText.Split(new[] { "||" }, StringSplitOptions.None);
                    if (parts.Length != 2) return hybridCipherText;

                    string encryptedAesKey = parts[0];
                    string aesEncryptedData = parts[1];

                    // 3. Giải mã Key AES bằng RSA
                    RSAEncryption rsa = new RSAEncryption();
                    string privateKeyXml = rsa.GetPrivateKey();

                    // Chuyển string Base64 về byte[] để giải mã
                    byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedAesKey);

                    // Gọi hàm Decrypt cũ
                    string aesKeyBase64 = rsa.Decrypt(encryptedKeyBytes, privateKeyXml);
                    byte[] aesKey = Convert.FromBase64String(aesKeyBase64);

                    // 4. Giải mã dữ liệu AES
                    byte[] fullCipher = Convert.FromBase64String(aesEncryptedData);
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        byte[] iv = new byte[16];
                        Array.Copy(fullCipher, 0, iv, 0, 16);
                        aes.IV = iv;

                        using (MemoryStream ms = new MemoryStream(fullCipher, 16, fullCipher.Length - 16))
                        using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
                catch { return hybridCipherText; }
            }
        }
    }

}
