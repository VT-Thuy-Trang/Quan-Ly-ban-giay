using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QL_GiayTT.Class
{
    public class RSAEncryption
    {
        private RSACryptoServiceProvider rsa;

        public RSAEncryption()
        {
            rsa = new RSACryptoServiceProvider(2048); // 2048-bit key
        }

        // Lấy public key để mã hóa
        public string GetPublicKey()
        {
            return rsa.ToXmlString(false); // false = chỉ public key
        }

        // Lấy private key để giải mã
        public string GetPrivateKey()
        {
            return rsa.ToXmlString(true); // true = cả private và public key
        }

        // Mã hóa dữ liệu
        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            return rsa.Encrypt(dataToEncrypt, false); // false = OAEP padding
        }

        // Giải mã dữ liệu
        public string Decrypt(byte[] encryptedData, string privateKeyXml)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                return string.Empty;

            try
            {
                RSACryptoServiceProvider rsaDecrypt = new RSACryptoServiceProvider();
                rsaDecrypt.FromXmlString(privateKeyXml);
                byte[] decryptedBytes = rsaDecrypt.Decrypt(encryptedData, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        // Mã hóa file lớn (chia nhỏ)
        public byte[] EncryptLargeData(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            int maxBlockSize = 245; // RSA 2048-bit có thể mã hóa tối đa 245 bytes với PKCS#1 v1.5 padding
            
            using (MemoryStream ms = new MemoryStream())
            {
                int offset = 0;
                while (offset < dataToEncrypt.Length)
                {
                    int blockSize = Math.Min(maxBlockSize, dataToEncrypt.Length - offset);
                    byte[] block = new byte[blockSize];
                    Array.Copy(dataToEncrypt, offset, block, 0, blockSize);
                    
                    byte[] encryptedBlock = rsa.Encrypt(block, false);
                    ms.Write(BitConverter.GetBytes(encryptedBlock.Length), 0, 4);
                    ms.Write(encryptedBlock, 0, encryptedBlock.Length);
                    
                    offset += blockSize;
                }
                return ms.ToArray();
            }
        }

        // Giải mã file lớn
        public string DecryptLargeData(byte[] encryptedData, string privateKeyXml)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                return string.Empty;

            try
            {
                RSACryptoServiceProvider rsaDecrypt = new RSACryptoServiceProvider();
                rsaDecrypt.FromXmlString(privateKeyXml);
                
                using (MemoryStream ms = new MemoryStream(encryptedData))
                using (MemoryStream result = new MemoryStream())
                {
                    while (ms.Position < ms.Length)
                    {
                        byte[] lengthBytes = new byte[4];
                        ms.Read(lengthBytes, 0, 4);
                        int blockLength = BitConverter.ToInt32(lengthBytes, 0);
                        
                        byte[] encryptedBlock = new byte[blockLength];
                        ms.Read(encryptedBlock, 0, blockLength);
                        
                        byte[] decryptedBlock = rsaDecrypt.Decrypt(encryptedBlock, false);
                        result.Write(decryptedBlock, 0, decryptedBlock.Length);
                    }
                    
                    return Encoding.UTF8.GetString(result.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

