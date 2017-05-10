using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AgileFramework
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public static class AgileString
    {
        /// <summary>
        /// 截取字符串左半部分
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="length">长度</param>
        /// <param name="ex">扩展字符</param>
        /// <returns>结果</returns>
        public static string Left(string input, int length, string ex = "...")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            if (input.Length > length)
            {
                return input.Substring(0, length) + ex;
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// 截取右边指定长度的字符
        /// </summary>
        /// <param name="input">待截取的字符串</param>
        /// <param name="length">截取的长度</param>
        /// <returns>截取的结果</returns>
        public static string Right(string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(input.Length - length, length);
            }
        }

        /// <summary>
        /// 计算字符串的长度，一个中文算两个字符
        /// </summary>
        /// <param name="input">需要检查的字符串</param>
        /// <returns>长度</returns>
        public static int GetTrueLength(string input)
        {
            ASCIIEncoding ae = new ASCIIEncoding();
            byte[] datas = ae.GetBytes(input);
            int result = 0;
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i] == 63)
                {
                    result++;
                }
                result++;
            }
            return result;
        }

        /// <summary>
        /// 翻转字符串
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>翻转的结果</returns>
        public static string Reverse(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// 转换为全角或半角格式
        /// </summary>
        /// <param name="input">待转换的字符串</param>
        /// <param name="smartStringCaseType">字符格式类型</param>
        /// <returns>转换之后的结果</returns>
        public static string ChangeCase(string input, AgileStringCaseType caseType)
        {
            string result = string.Empty;
            switch (caseType)
            {
                case AgileStringCaseType.SBC:
                    {
                        // 转全角(SBC case)
                        // 全角空格为12288，半角空格为32
                        // 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
                        char[] chars = input.ToCharArray();
                        for (int i = 0; i < chars.Length; i++)
                        {
                            if (chars[i] == 32)
                            {
                                chars[i] = (char)12288;
                                continue;
                            }
                            if (chars[i] == 46)
                            {
                                chars[i] = (char)12290;
                                continue;
                            }
                            if (chars[i] < 127)
                            {
                                chars[i] = (char)(chars[i] + 65248);
                            }
                        }
                        result = new string(chars);
                        result = result.Replace(".", "。");
                    }
                    break;
                case AgileStringCaseType.DBC:
                    {
                        // 转半角(DBC case)
                        // 全角空格为12288，半角空格为32
                        // 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
                        char[] chars = input.ToCharArray();
                        for (int i = 0; i < chars.Length; i++)
                        {
                            if (chars[i] == 12288)
                            {
                                chars[i] = (char)32;
                                continue;
                            }
                            if (chars[i] == 12290)
                            {
                                chars[i] = (char)46;
                                continue;
                            }
                            if (chars[i] > 65280 && chars[i] < 65375)
                            {
                                chars[i] = (char)(chars[i] - 65248);
                            }
                        }
                        result = new string(chars);
                        result = result.Replace("。", ",");
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 去除所有Html标记
        /// </summary>
        /// <param name="input">待处理的文本</param>
        /// <returns>处理的结果</returns>
        public static string CleanHtmlTag(string input)
        {
            string pattern = @"\<.*?>";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return regex.Replace(input, string.Empty);
        }

        /// <summary>
        /// 获取32位哈希值
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>哈希值</returns>
        public static unsafe int GetHashCodeFor32(string input)
        {
            fixed (char* str = input.ToCharArray())
            {
                char* chPtr = str;
                int num = 0x15051505;
                int num2 = num;
                int* numPtr = (int*)chPtr;
                int length = input.Length;
                while (length > 2)
                {
                    num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                    num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                    numPtr += 2;
                    length -= 4;
                }
                if (length > 0)
                {
                    num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                }
                return (num + (num2 * 0x5d588b65));
            }
        }

        /// <summary>
        /// 获取64位哈希值
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static unsafe int GetHashCodeFor64(string input)
        {
            fixed (char* str = input.ToCharArray())
            {
                int num3;
                char* chPtr = str;
                int num = 0x1505;
                int num2 = num;
                for (char* chPtr2 = chPtr; (num3 = chPtr2[0]) != '\0'; chPtr2 += 2)
                {
                    num = ((num << 5) + num) ^ num3;
                    num3 = chPtr2[1];
                    if (num3 == 0)
                    {
                        break;
                    }
                    num2 = ((num2 << 5) + num2) ^ num3;
                }
                return (num + (num2 * 0x5d588b65));
            }
        }
    }

    /// <summary>
    /// 字符串格式类型枚举，全角或半角
    /// </summary>
    public enum AgileStringCaseType
    {
        /// <summary>
        /// 全角
        /// </summary>
        SBC,
        /// <summary>
        /// 半角
        /// </summary>
        DBC
    }
}
