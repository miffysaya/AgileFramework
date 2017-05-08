using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AgileFramework.Text;

namespace AgileFramework.Security
{
    /// <summary>
    /// AES安全类
    /// </summary>
    public static class AgileAES
    {
        //默认密钥向量 
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="endcoding">编码格式</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string Encrypt(string plainText, string key, Encoding endcoding = null)
        {
            endcoding = endcoding ?? AgileEncoding.Default;
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = endcoding.GetBytes(plainText);//得到需要加密的字节数组 
            //设置密钥及密钥向量
            des.Key = endcoding.GetBytes(key);
            des.IV = _key1;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string Decrypt(string cipherText, string key, Encoding encoding = null)
        {
            encoding = encoding ?? AgileEncoding.Default;

            byte[] byteCipherText = Convert.FromBase64String(cipherText);
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = encoding.GetBytes(key);
            des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(byteCipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   //将字符串后尾的'\0'去掉
        }

    }
}
