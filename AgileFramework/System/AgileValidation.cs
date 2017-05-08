using System;
using System.Text.RegularExpressions;

namespace AgileFramework
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public static class AgileValidation
    {
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsDecimal(string input)
        {
            try
            {
                Convert.ToDecimal(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为Inte32类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsInt32(string input)
        {
            try
            {
                Convert.ToInt32(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为Inte16类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsInt16(string input)
        {
            try
            {
                Convert.ToInt16(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为Inte64类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsInt64(string input)
        {
            try
            {
                Convert.ToInt64(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsDouble(string input)
        {
            try
            {
                Convert.ToDouble(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为char类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsChar(string input)
        {
            try
            {
                Convert.ToChar(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为DateTime类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsDateTime(string input)
        {
            try
            {
                Convert.ToDateTime(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否为Single类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsSingle(string input)
        {
            try
            {
                Convert.ToSingle(input);
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 是否是邮箱地址
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是手机号码（中国地区）
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static bool IsMobile(string input)
        {
            return Regex.IsMatch(input, @"^1\d{10}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否有效的密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPassword(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]{6,16}$");
        }
    }
}
