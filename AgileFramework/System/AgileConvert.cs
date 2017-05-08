using System;
using System.Text;
using System.Linq;

namespace AgileFramework
{
    /// <summary>
    /// 转换类
    /// </summary>
    public static class AgileConvert
    {
        /// <summary>
        /// 解析为数组
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="split">分割符</param>
        /// <returns>结果</returns>
        public static T[] ParseArray<T>(string value, char split = '|')
        {
            var type = typeof(T);
            T[] array = null;
            if (!string.IsNullOrEmpty(value))
            {
                array = value.Split(split).Select(item =>
                {
                    if (type.IsEnum)
                    {
                        return (T)Enum.Parse(type, item, true);
                    }
                    else
                    {
                        return (T)Convert.ChangeType(item, typeof(T));
                    }
                }).ToArray();
            }
            return array;
        }
        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为Int32类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static int TryToInt32(object input, int defaultValue = 0)
        {
            try
            {
                return Convert.ToInt32(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为Int16类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Int16 TryToInt16(object input, Int16 defaultValue = 0)
        {
            try
            {
                return Convert.ToInt16(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为Int64类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Int64 TryToInt64(object input, Int64 defaultValue = 0)
        {
            try
            {
                return Convert.ToInt64(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Decimal 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Decimal TryToDecimal(object input, Decimal defaultValue = 0)
        {
            try
            {
                return Convert.ToDecimal(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Double 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Double TryToDouble(object input, Double defaultValue = 0.00)
        {
            try
            {
                return Convert.ToDouble(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 String 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static String TryToString(object input, String defaultValue = "NULL")
        {
            try
            {
                return Convert.ToString(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Char 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Char TryToChar(object input, Char defaultValue = 'N')
        {
            try
            {
                return Convert.ToChar(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 DateTime类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static DateTime TryToDateTime(object input, DateTime defaultValue = default(DateTime))
        {
            try
            {
                return Convert.ToDateTime(input);
            }
            catch
            {
                return defaultValue;
                //return new DateTime((long)0);
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Boolean 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Boolean TryToBool(object input, Boolean defaultValue = false)
        {
            try
            {
                return Convert.ToBoolean(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Byte 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Byte TryToByte(object input, Byte defaultValue = 0)
        {
            try
            {
                return Convert.ToByte(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 SByte 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static SByte TryToSByte(object input, SByte defaultValue = 0)
        {
            try
            {
                return Convert.ToSByte(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 Single 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static Single TryToSingle(object input, Single defaultValue = 0)
        {
            try
            {
                return Convert.ToSingle(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 UInt32 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static UInt32 TryToUInt32(object input, UInt32 defaultValue = 0)
        {
            try
            {
                return Convert.ToUInt32(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 UInt16 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static UInt16 TryToUInt16(object input, UInt16 defaultValue = 0)
        {
            try
            {
                return Convert.ToUInt16(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试转换（失败则返回默认值）-- 转换为 UInt64 类型
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static UInt64 TryToUInt64(object input, UInt64 defaultValue = 0)
        {
            try
            {
                return Convert.ToUInt64(input);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            char[] original = input.ToCharArray();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < original.Length; i++)
            {
                if (i == 0) { sb.Append(original[i].ToString().ToUpper()); }
                else
                {
                    sb.Append(original[i].ToString());
                }
            }
            return sb.ToString();
        }
    }
}
