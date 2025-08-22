using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class AesEncryptionHelper
    {
        //private static readonly Byte[] IV = 
        //{
        //    75, 23, 155, 83, 16, 164, 135, 211,
        //    34, 245, 76, 21, 62, 171, 80, 104
        //};

        private static Byte[] oldAESKey =
        {
            201, 219, 55, 183, 156, 64, 85, 204,
            201, 219, 55, 183, 156, 64, 85, 204
        };

        private static readonly Byte[] oldRAWKey =
        {
            201, 219, 55, 183, 156, 64, 85, 204,
            201, 219, 55, 183, 156, 64, 85, 204
        };

        private static Byte[] aesKey =
{
            201, 219, 55, 183, 156, 64, 85, 204,
            139, 94, 73, 13, 222, 150, 39, 107,
            75, 23, 155, 83, 16, 164, 135, 211,
            34, 245, 76, 21, 62, 171, 80, 104
        };

        private static readonly Byte[] rawKey =
        {
            201, 219, 55, 183, 156, 64, 85, 204,
            139, 94, 73, 13, 222, 150, 39, 107,
            75, 23, 155, 83, 16, 164, 135, 211,
            34, 245, 76, 21, 62, 171, 80, 104
        };

        static Boolean isDefaultKey;

        public static String Key
        {
            get { return Convert.ToBase64String(aesKey); }
            set
            {
                aesKey = Convert.FromBase64String(value);
                isDefaultKey = false;
            }
        }

        public static bool IsDefaultKey
        {
            get { return isDefaultKey; }
        }

        public static Byte[] Encrypt(Byte[] plainData)
        {
            return InnerEncrypt(plainData, aesKey);
        }


        private static Byte[] InnerEncrypt(Byte[] plainData, Byte[] key)
        {
            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Mode = CipherMode.ECB;
                using (var stream = new MemoryStream())
                {
                    var IV = new byte[16];
                    RandomNumberGenerator.Create().GetBytes(IV);
                    using (var cryptoStream = new CryptoStream(stream, aesProvider.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainData, 0, plainData.Length);
                    }

                    var encryptMessageBytes = stream.ToArray();
                    var bts = new Byte[IV.Length + encryptMessageBytes.Length];
                    Array.Copy(IV, 0, bts, 0, IV.Length);
                    Array.Copy(encryptMessageBytes, 0, bts, IV.Length, encryptMessageBytes.Length);
                    return bts;
                }
            }
        }

        public static Byte[] Decrypt(Byte[] encryptionData)
        {
            return InnerDecrypt(encryptionData, aesKey);
        }

        private static Byte[] InnerDecrypt(Byte[] encryptionData, Byte[] key)
        {
            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Mode = CipherMode.ECB;
                using (var stream = new MemoryStream())
                {
                    var iv = new Byte[16];
                    Array.Copy(encryptionData, 0, iv, 0, iv.Length);
                    using (var cryptoStream = new CryptoStream(stream, aesProvider.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptionData, iv.Length, encryptionData.Length - iv.Length);
                    }
                    return stream.ToArray();
                }
            }
        }

        public static String EncryptString(String plainString)
        {
            var buffer = Encoding.UTF8.GetBytes(plainString);
            var encryptMessageBytes = Encrypt(buffer);
            return Convert.ToBase64String(encryptMessageBytes);
        }

        public static String DecryptString(String encryptedString)
        {
            var buffer = Convert.FromBase64String(encryptedString);
            var decryptMessageBytes = Decrypt(buffer);
            return Encoding.UTF8.GetString(decryptMessageBytes);
        }


        public static String EncryptStringWithRawKey(String plainString)
        {
            var buffer = Encoding.UTF8.GetBytes(plainString);
            var encryptMessageBytes = InnerEncrypt(buffer, rawKey);
            return Convert.ToBase64String(encryptMessageBytes);
        }

        public static String DecryptStringWithRawKey(String encryptedString)
        {
            var buffer = Convert.FromBase64String(encryptedString);
            var decryptMessageBytes = InnerDecrypt(buffer, rawKey);
            return Encoding.UTF8.GetString(decryptMessageBytes);
        }
    }

    public class AesDataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return this;
        }

        public byte[] Protect(byte[] plaintext)
        {
            return AesEncryptionHelper.Encrypt(plaintext);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return AesEncryptionHelper.Decrypt(protectedData);
        }
    }
}
