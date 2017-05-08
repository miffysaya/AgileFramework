using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace AgileFramework.Web
{
    /// <summary>
    /// Json格式数据处理帮助类
    /// </summary>
    public static class AgileJson
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private static readonly JavaScriptSerializer serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        static AgileJson()
        {
            serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            serializer.RecursionLimit = int.MaxValue;
        }

        /// <summary>
        /// 将对象序列化为Json格式的字符串
        /// </summary>
        /// <param name="source">源类型对象</param>
        /// <returns>Json格式的字符串</returns>
        public static string ToJson(object source)
        {
            return serializer.Serialize(source);
        }

        /// <summary>
        /// 将Json格式的数据转换为对象
        /// </summary>
        /// <typeparam name="T">源类型</typeparam>
        /// <param name="json">json格式的字符串</param>
        /// <returns>序列化之后的格式</returns>
        public static T FromJson<T>(string json)
        {
            return serializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 将Json格式字符串反序列化为动态对象
        /// </summary>
        /// <param name="json">Json格式字符串</param>
        /// <returns>反序列化之后的对象</returns>
        public static object FromJson(string json)
        {
            serializer.RegisterConverters(new[] { new DynamicCoverter() });

            return serializer.Deserialize(json, typeof(object));
        }

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>结果</returns>
        public static bool JsonEquals(object a, object b)
        {
            return ToJson(a) == ToJson(b);
        }

        /// <summary>
        /// 获得安全字符串
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>结果</returns>
        public static string GetSafeJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return ToJson(input).TrimStart('"').TrimEnd('"');
        }
    }

    /// <summary>
    /// 动态反序列化用转换器
    /// </summary>
    internal sealed class DynamicCoverter : JavaScriptConverter
    {
        /// <summary>
        /// 重写反序列化方法
        /// </summary>
        /// <param name="dictionary">序列化字典</param>
        /// <param name="type">序列化类型</param>
        /// <param name="serializer">序列化器</param>
        /// <returns>对象</returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("序列化字典为NULL");
            }
            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        /// <summary>
        /// 重写序列化方法（由于动态类可序列化，实际此方法无用）
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="serializer">序列化器</param>
        /// <returns>序列化字典</returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 可支持的类型
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) })); }
        }

        /// <summary>
        /// 动态对象
        /// </summary>
        private sealed class DynamicJsonObject : DynamicObject
        {
            /// <summary>
            /// 序列化字典
            /// </summary>
            private readonly IDictionary<string, object> _dictionary;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="dictionary">序列化字典</param>
            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("序列化字典为NULL");
                }
                _dictionary = dictionary;
            }

            /// <summary>
            /// 重写ToString()方法
            /// </summary>
            /// <returns>反序列化后的字符串</returns>
            public override string ToString()
            {
                var stringBuilder = new StringBuilder();

                Format(stringBuilder, _dictionary);

                return stringBuilder.ToString();
            }

            /// <summary>
            /// 字典格式化为字符串的方法
            /// </summary>
            /// <param name="stringBuilder"></param>
            /// <param name="dictionary"></param>
            private void Format(StringBuilder stringBuilder, IDictionary<string, object> dictionary)
            {
                stringBuilder.Append("{");

                var firstInDictionary = false;

                foreach (var keyValuePair in dictionary)
                {
                    if (firstInDictionary)
                    {
                        stringBuilder.Append(",");
                    }
                    firstInDictionary = true;

                    var value = keyValuePair.Value;

                    var key = keyValuePair.Key;

                    if (value is string)
                    {
                        stringBuilder.AppendFormat("{0}:\"{1}\"", key, value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        Format(stringBuilder, value as IDictionary<string, object>);
                    }
                    else if (value is ArrayList)
                    {
                        stringBuilder.Append(key + ":");

                        FormatArray(stringBuilder, value as ArrayList);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("{0}:{1}", key, value);
                    }
                }
                stringBuilder.Append("}");
            }

            /// <summary>
            /// 链表格式化为字符串的方法
            /// </summary>
            /// <param name="stringBuilder"></param>
            /// <param name="list"></param>
            private void FormatArray(StringBuilder stringBuilder, ArrayList list)
            {
                stringBuilder.Append("[");

                var firstInArray = false;

                foreach (var value in list)
                {
                    if (firstInArray)
                    {
                        stringBuilder.Append(",");
                    }
                    firstInArray = true;

                    if (value is string)
                    {
                        stringBuilder.AppendFormat("\"{0}\"", value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        Format(stringBuilder, value as IDictionary<string, object>);
                    }
                    else if (value is ArrayList)
                    {
                        stringBuilder.Append("[");

                        FormatArray(stringBuilder, value as ArrayList);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("{0}", value);
                    }
                }
                stringBuilder.Append("]");
            }

            /// <summary>
            /// 动态获取成员的方法
            /// </summary>
            /// <param name="binder">获取动态成员的操作</param>
            /// <param name="result">结果</param>
            /// <returns>是否获取成员</returns>
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!_dictionary.TryGetValue(binder.Name, out result))
                {
                    result = null;

                    return true;
                }
                var dictionary = result as IDictionary<string, object>;

                if (dictionary != null)
                {
                    result = new DynamicJsonObject(dictionary);

                    return true;
                }
                var arrayList = result as ArrayList;

                if (arrayList != null && arrayList.Count > 0)
                {
                    if (arrayList[0] is IDictionary<string, object>)
                    {
                        result = new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
                    }
                    else
                    {
                        result = new List<object>(arrayList.Cast<object>());
                    }
                }
                return true;
            }
        }
    }
}
