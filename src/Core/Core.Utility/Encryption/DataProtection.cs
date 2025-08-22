using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class DataProtection
    {
        private static readonly SiasunLogger Logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly byte[] AesKey =
        {
            201, 219, 55, 183, 156, 64, 85, 204,
            201, 219, 55, 183, 156, 64, 85, 204,
            201, 219, 55, 183, 156, 64, 85, 204,
            201, 219, 55, 183, 156, 64, 85, 204
        };

        /// <summary>
        /// Encrypt data using AES
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Protect(string source)
        {
            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(AesKey, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(source);
                            }
                        }
                        var iv = aesAlg.IV;
                        var decryptedContent = msEncrypt.ToArray();
                        var result = new byte[iv.Length + decryptedContent.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt data using AES
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string UnProtect(string source)
        {
            var fullCipher = Convert.FromBase64String(source);
            var iv = new byte[16];
            var cipher = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(AesKey, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    return result;
                }
            }
        }
        public static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
        public static string UnprotectFromString(string protectedMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(protectedMsg))
                {
                    return protectedMsg;
                }
                string originalMsg = protectedMsg;
                if (protectedMsg.StartsWith("pt{") && protectedMsg.EndsWith("}"))
                {
                    originalMsg = protectedMsg.Replace("pt{", "").Replace("}", "");
                }
                else
                {
                    if (IsBase64String(protectedMsg))
                    {
                        originalMsg = UnProtect(protectedMsg);
                    }
                }
                return originalMsg;
            }
            catch (Exception ex)
            {
                Logger.Warn("An error occured while trying UnprotectFromString.", ex);
                return protectedMsg;
            }
        }

        private static string GetNodeString(string originalString, string endSplitFlag, string startNodeName, bool needTrimSpace = false)
        {
            try
            {
                if (string.IsNullOrEmpty(originalString))
                {
                    return originalString;
                }
                string nodeString = string.Empty;
                string[] nodeStrings = originalString.Split(new[] { endSplitFlag }, StringSplitOptions.None);
                foreach (var tmpString in nodeStrings)
                {
                    if (tmpString.IndexOf(startNodeName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        nodeString = tmpString;
                        if (needTrimSpace)
                        {
                            while (nodeString.Contains(" "))
                            {
                                nodeString = nodeString.Replace(" ", "");
                            }
                        }
                        return nodeString;
                    }
                }
                return nodeString;
            }
            catch (Exception ex)
            {
                Logger.Warn("An error occured while trying GetNodeString.", ex);
                return originalString;
            }
        }

        public static string GetUnprotectString(string originalString, string startNodeName, string endSplitFlag, bool needTrimSpace = false)
        {
            try
            {
                if (string.IsNullOrEmpty(originalString))
                {
                    return originalString;
                }
                string pwdSting = GetNodeString(originalString, endSplitFlag, startNodeName, needTrimSpace);
                string unprotectPwdString = UnprotectFromString(pwdSting.Substring((startNodeName + "=").Length));
                var unprotectString = originalString.Replace(pwdSting.Substring((startNodeName + "=").Length), unprotectPwdString);
                return unprotectString;
            }
            catch (Exception ex)
            {
                Logger.Warn("An error occured while trying GetUnprotectString.", ex);
                return originalString;
            }
        }

        public static string ResolveConnectionString(string connectionString)
        {
            return GetUnprotectString(connectionString, "Password", ";", true);
        }
    }
}
