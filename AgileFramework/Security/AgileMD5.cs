using AgileFramework.Text;
using System.Security.Cryptography;
using System.Text;

namespace AgileFramework.Security
{
    public static class AgileMD5
    {
        /// <summary>
        /// 计算MD5哈希值
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static string HashMD5(string input, Encoding encoding = null)
        {
            encoding = encoding ?? AgileEncoding.Default;

            var md5Hash = MD5.Create();

            byte[] datas = md5Hash.ComputeHash(encoding.GetBytes(input));

            var ret = new StringBuilder();

            for (var i = 0; i < datas.Length; i++)
            {
                ret.Append(datas[i].ToString("X2"));
            }
            return ret.ToString();
        }

        /// <summary>
        /// 计算MD5Cng哈希值
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static string HashMD5Cng(string input, Encoding encoding = null)
        {
            encoding = encoding ?? AgileEncoding.Default;

            var md5Hash = MD5Cng.Create();

            byte[] datas = md5Hash.ComputeHash(encoding.GetBytes(input));

            var ret = new StringBuilder();

            for (var i = 0; i < datas.Length; i++)
            {
                ret.Append(datas[i].ToString("X2"));
            }
            return ret.ToString();
        }
    }
}
