using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AgileFramework
{
    /// <summary>
    /// 比较
    /// </summary>
    public static class AgileCompare
    {
        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool Equal(string field, string pattern)
        {
            return pattern == field;
        }

        /// <summary>
        /// NotEqual
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool NotEqual(string field, string pattern)
        {
            return !Equal(field, pattern);
        }

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool Contains(string field, string pattern)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }
            return field.Contains(pattern);
        }

        /// <summary>
        /// NotContains
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool NotContains(string field, string pattern)
        {
            return !Contains(field, pattern);
        }

        /// <summary>
        /// StartsWith
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool StartsWith(string field, string pattern)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }
            return field.StartsWith(pattern);
        }

        /// <summary>
        /// NotStartsWith
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool NotStartsWith(string field, string pattern)
        {
            return !StartsWith(field, pattern);
        }

        /// <summary>
        /// EndsWith
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool EndsWith(string field, string pattern)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }
            return field.EndsWith(pattern);
        }

        /// <summary>
        /// NotEndsWith
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="pattern">模式</param>
        /// <returns>结果</returns>
        public static bool NotEndsWith(string field, string pattern)
        {
            return !EndsWith(field, pattern);
        }

        /// <summary>
        /// RegexMatch
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>结果</returns>
        public static bool RegexMatch(string field, Regex regex)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }
            return regex.IsMatch(field);
        }

        /// <summary>
        /// NotRegexMatch
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>结果</returns>
        public static bool NotRegexMatch(string field, Regex regex)
        {
            return !RegexMatch(field, regex);
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool InScope(string field, string start, string end)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }
            if (field.Length != start.Length || field.Length != end.Length || start.Length != end.Length)
            {
                return false;
            }
            return field.CompareTo(start) >= 0 && field.CompareTo(end) <= 0;
        }

        /// <summary>
        /// 判断一个值是否不在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool NotInScope(string field, string start, string end)
        {
            return !InScope(field, start, end);
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool InScope(int field, int start, int end)
        {
            return field >= start && field <= end;
        }

        /// <summary>
        /// 判断一个值是否不在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool NotInScope(int field, int start, int end)
        {
            return !InScope(field, start, end);
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内（有一个在其范围内即为真）
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool InScope(List<string> fields, string start, string end)
        {
            if (fields == null || fields.Count == 0)
            {
                return false;
            }
            if (start.Length != end.Length)
            {
                return false;
            }
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field.Length == start.Length && field.Length == end.Length)
                {
                    if (field.CompareTo(start) >= 0 && field.CompareTo(end) <= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="fields">字段</param>
        /// <returns>结果</returns>
        public static bool InScope(string field, List<string> fields)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return false;
            }

            if (fields == null || fields.Count == 0)
            {
                return false;
            }

            foreach (var item in fields)
            {
                if (item == field)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="fields">字段</param>
        /// <returns>结果</returns>
        public static bool NotInScope(string field, List<string> fields)
        {
            return !InScope(field, fields);
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="fields">字段</param>
        /// <returns>结果</returns>
        public static bool InScope(int field, List<int> fields)
        {
            if (fields == null || fields.Count == 0)
            {
                return false;
            }

            foreach (var item in fields)
            {
                if (item == field)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="fields">字段</param>
        /// <returns>结果</returns>
        public static bool NotInScope(int field, List<int> fields)
        {
            return !InScope(field, fields);
        }

        /// <summary>
        /// 判断一个值是否不在一个值范围内（有一个在其范围内即为真）
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool NotInScope(List<string> fields, string start, string end)
        {
            return !InScope(fields, start, end);
        }

        /// <summary>
        /// 判断一个值是否在一个值范围内
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool InScope(List<int> fields, int start, int end)
        {
            if (fields == null || fields.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field >= start && field <= end)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个值是否不在一个值范围内
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns>结果</returns>
        public static bool NotInScope(List<int> fields, int start, int end)
        {
            return !InScope(fields, start, end);
        }

        /// <summary>
        /// 判断两个数组是否有交集
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>结果</returns>
        public static bool Intersect(List<string> array1, List<string> array2)
        {
            if (array1 == null || array2 == null || array1.Count == 0 || array2.Count == 0)
            {
                return false;
            }
            return array1.Intersect(array2).Count() > 0;
        }

        /// <summary>
        /// 判断两个数组是否无交集
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>结果</returns>
        public static bool NotIntersect(List<string> array1, List<string> array2)
        {
            return !Intersect(array1, array2);
        }

        /// <summary>
        /// 判断两个数组是否有交集
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>结果</returns>
        public static bool Intersect(List<int> array1, List<int> array2)
        {
            return array1.Intersect(array2).Count() > 0;
        }

        /// <summary>
        /// 判断两个数组是否无交集
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>结果</returns>
        public static bool NotIntersect(List<int> array1, List<int> array2)
        {
            return !Intersect(array1, array2);
        }
    }
}
