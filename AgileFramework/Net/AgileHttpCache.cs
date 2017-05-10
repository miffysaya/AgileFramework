using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace AgileFramework.Net
{
    /// <summary>
    /// 基于.NET基础类的缓存池
    /// </summary>
    public class AgileHttpCache
    {
        /// <summary>
        /// 基于.NET的缓存器
        /// </summary>
        private static Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <returns>是否插入</returns>
        public bool Add(string key, object obj)
        {
            bool ret = false;

            if (obj != null && !string.IsNullOrEmpty(key))
            {
                cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.Zero);

                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 插入指定超时时间的缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="TimeOut">超时时间</param>
        /// <returns>是否插入</returns>
        public bool Add(string key, object obj, int timeOutSeconds)
        {
            bool ret = false;

            if (obj != null && !string.IsNullOrEmpty(key))
            {
                cache.Insert(key, obj, null, DateTime.Now.AddSeconds(timeOutSeconds), TimeSpan.Zero);

                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            bool isContainsKey = false;

            if (cache[key] != null)
            {
                isContainsKey = true;
            }
            if (!isContainsKey)
            {
                Delete(key);
            }
            return isContainsKey;
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(key))
            {
                cache.Remove(key);

                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 删除所有项
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            if (cache.Count == 0)
            {
                return true;
            }
            lock (cache)
            {
                var list = new List<string>();

                var enumerator = cache.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Key.ToString());
                }
                list.ForEach(item => cache.Remove(item));
            }
            return true;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return cache[key];
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)cache[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Set(string key, object obj)
        {
            Delete(key);
            return Add(key, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="TimeOut"></param>
        /// <returns></returns>
        public bool Set(string key, object obj, int timeOutSeconds)
        {
            Delete(key);
            return Add(key, obj, timeOutSeconds);
        }
    }
}
