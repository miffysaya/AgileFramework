using System.Text;

namespace AgileFramework.Text
{
    /// <summary>
    /// 编码帮助类
    /// </summary>
    public static class AgileEncoding
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding Default = Encoding.UTF8;

        /// <summary>
        /// 转换编码
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="from">源编码</param>
        /// <param name="to">转换编码</param>
        /// <returns>结果</returns>
        public static string ConvertIso8859ToGB2312(string input, Encoding from, Encoding to)
        {
            var bytes = from.GetBytes(input);

            return to.GetString(bytes);
        }
    }
}
