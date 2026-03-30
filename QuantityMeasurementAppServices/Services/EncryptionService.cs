using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementAppServices.Services
{
    // Class that Handles AES-256 encryption and decryption
    public class EncryptionService
    {
        // 32 byte key for AES-256
        private readonly byte[] encryptionKey;

        // Read key from config
        public EncryptionService(IConfiguration configuration)
        {
            string keyValue = configuration["Encryption:Key"]!;

            // Fill exactly 32 bytes
            encryptionKey = new byte[32];
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyValue);
            Array.Copy(keyBytes, encryptionKey, Math.Min(keyBytes.Length, encryptionKey.Length));
        }

        // Method to Encrypt plain text to Base64 cipher text
        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;

                // Generate random IV for each encryption
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    // Write IV first so we can extract it during decryption
                    ms.Write(iv, 0, iv.Length);

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Method to Decrypt Base64 cipher text back to plain text
        public string Decrypt(string cipherText)
        {
            byte[] allBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;

                // Extract IV from the front of the data
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(allBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;

                // Remaining bytes are the actual cipher text
                byte[] cipherBytes = new byte[allBytes.Length - iv.Length];
                Array.Copy(allBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}