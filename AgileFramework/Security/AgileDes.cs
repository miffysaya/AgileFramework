using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AgileFramework.Text;

namespace AgileFramework.Security
{
    /// <summary>
    /// DES安全
    /// </summary>
    public static class AgileDES
    {
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="input">待加密的字符串</param>
        /// <param name="password">加密的密码（只能为8位长）</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="cipherMode">加密模式（C语言常用为ECB）</param>
        /// <param name="paddingMode">填充模式（C语言常用为None）</param>
        /// <returns>加密之后的文本</returns>
        public static string Encrypt(string input, string password, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.ANSIX923)
        {
            encoding = encoding ?? AgileEncoding.Default;
            //注意iv的长度，必须和key中的密码长度相同
            var iv = encoding.GetBytes(password);
            var key = encoding.GetBytes(password);
            var datas = encoding.GetBytes(input);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            desCryptoServiceProvider.Mode = cipherMode;
            desCryptoServiceProvider.Padding = paddingMode;
            using (var memoryStream = new MemoryStream())
            {
                using (
                    var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(iv, key),
                                                        CryptoStreamMode.Write))
                {
                    cryptoStream.Write(datas, 0, datas.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// 通用加密方法
        /// </summary>
        /// <param name="input">待加密的字符串</param>
        /// <param name="password">加密的密码（只能为8位长）</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="cipherMode">加密模式（C语言常用为ECB）</param>
        /// <param name="paddingMode">填充模式（C语言常用为None）</param>
        /// <returns>加密之后的文本（Passpord返回16进制编码后的加密串）</returns>
        public static string CommonEncrypt(string input, string password, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.ANSIX923)
        {
            encoding = encoding ?? AgileEncoding.Default;
            //注意iv的长度，必须和key中的密码长度相同
            var iv = Encoding.ASCII.GetBytes(password);
            var key = Encoding.ASCII.GetBytes(password);
            var datas = encoding.GetBytes(input);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            using (var memoryStream = new MemoryStream())
            {
                using (
                    var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(iv, key),
                                                        CryptoStreamMode.Write))
                {
                    cryptoStream.Write(datas, 0, datas.Length);
                    cryptoStream.FlushFinalBlock();

                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in memoryStream.ToArray())
                    {
                        //十六进制编码
                        ret.AppendFormat("{0:X2}", b);
                    }
                    return ret.ToString();
                }
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="input">待解密的字符串</param>
        /// <param name="password">加密时用的密码（只能为8位长）</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="cipherMode">加密模式（C语言常用为ECB）</param>
        /// <param name="paddingMode">填充模式（C语言常用为None）</param>
        /// <returns>解密之后的文本</returns>
        public static string Decrypt(string input, string password, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.ANSIX923)
        {
            encoding = encoding ?? AgileEncoding.Default;
            var iv = encoding.GetBytes(password);
            var key = encoding.GetBytes(password);
            var datas = Convert.FromBase64String(input);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            using (var memoryStream = new MemoryStream())
            {
                using (
                    var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(iv, key),
                                                        CryptoStreamMode.Write))
                {
                    cryptoStream.Write(datas, 0, datas.Length);
                    cryptoStream.FlushFinalBlock();
                    return encoding.GetString(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// 多语言通用解密
        /// </summary>
        /// <param name="input">待解密的字符串</param>
        /// <param name="password">加密时用的密码（只能为8位长）</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="cipherMode">加密模式（C语言常用为ECB）</param>
        /// <param name="paddingMode">填充模式（C语言常用为None）</param>
        /// <returns>解密之后的文本</returns>
        public static string CommonDecrypt(string input, string password, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.ANSIX923)
        {
            encoding = encoding ?? AgileEncoding.Default;
            var iv = Encoding.ASCII.GetBytes(password);
            var key = Encoding.ASCII.GetBytes(password);
            //把字符串放到byte数组中
            byte[] datas = new byte[input.Length / 2];
            for (int x = 0; x < input.Length / 2; x++)
            {
                int i = (Convert.ToInt32(input.Substring(x * 2, 2), 16));
                datas[x] = (byte)i;
            }
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            using (var memoryStream = new MemoryStream())
            {
                using (
                    var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(iv, key),
                                                        CryptoStreamMode.Write))
                {
                    cryptoStream.Write(datas, 0, datas.Length);
                    cryptoStream.FlushFinalBlock();
                    return encoding.GetString(memoryStream.ToArray());
                }
            }
        }
    }

}



