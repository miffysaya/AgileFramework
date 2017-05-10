using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AgileFramework.Net
{
    /// <summary>
    /// 基于字典的内存池，已加锁
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class AgileMemoryCache<TKey, TValue>
    {
        /// <summary>
        /// 缓存器
        /// </summary>
        private static Dictionary<AgileCacheKey<TKey>, AgileCacheValue<TValue>> cache = new Dictionary<AgileCacheKey<TKey>, AgileCacheValue<TValue>>();

        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer timer = new Timer(CleanCallback, null, 0, 60 * 1000);

        /// <summary>
        /// 定时清理方法
        /// </summary>
        /// <param name="o"></param>
        private static void CleanCallback(object o)
        {
            DateTime now = DateTime.Now;

            var list = cache.Where(one => one.Value.IsOverdue).Select(one => one.Key).ToList();

            lock (cache)
            {
                list.ForEach(item => cache.Remove(item));
            }
        }

        /// <summary>
        /// 加入缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="cacheTime"></param>
        /// <param name="refreshCreateTime"></param>
        /// <param name="minCounter"></param>
        /// <returns></returns>
        public bool Add(TKey key, TValue obj, int cacheTime = 0, bool refreshCreateTime = false, int minCounter = 0)
        {
            var cacheValue = new AgileCacheValue<TValue>(obj, cacheTime, refreshCreateTime, minCounter);
            var cacheKey = new AgileCacheKey<TKey>(key);
            lock (cache)
            {
                if (!cache.ContainsKey(cacheKey))
                {
                    cache.Add(cacheKey, cacheValue);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return cache.ContainsKey(new AgileCacheKey<TKey>(key));
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(TKey key)
        {
            var cacheKey = new AgileCacheKey<TKey>(key);

            lock (cache)
            {
                if (cache.ContainsKey(cacheKey))
                {
                    cache.Remove(cacheKey);

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 删除所有项
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            lock (cache)
            {
                cache.Clear();
            }
            return true;
        }

        /// <summary>
        /// 手动清理过期项
        /// </summary>
        /// <returns></returns>
        public bool Clean()
        {
            CleanCallback(new object());
            return true;
        }

        /// <summary>
        /// 获取指定项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(TKey key)
        {
            if (ContainsKey(key))
            {
                return cache[new AgileCacheKey<TKey>(key)];
            }
            return null;
        }

        /// <summary>
        /// 更新指定项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Set(TKey key, TValue obj, int cacheTime = 0, bool refreshCreateTime = false, int minCounter = 0)
        {
            if (!Add(key, obj, cacheTime, refreshCreateTime, minCounter))
            {
                var cacheValue = new AgileCacheValue<TValue>(obj, cacheTime, refreshCreateTime, minCounter);
                var cacheKey = new AgileCacheKey<TKey>(key);

                cache[cacheKey] = cacheValue;
            }
            return true;
        }
    }

    /// <summary>
    /// 缓存键
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class AgileCacheKey<TKey>
    {
        /// <summary>
        /// 只读键
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        public AgileCacheKey(TKey key)
        {
            Key = key;
        }

        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Key.Equals((obj as AgileCacheKey<TKey>).Key);
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }

    /// <summary>
    /// 缓存值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class AgileCacheValue<TValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tValue"></param>
        /// <param name="cacheTime"></param>
        /// <param name="isRefreshCreateTime"></param>
        /// <param name="minCounter"></param>
        public AgileCacheValue(TValue tValue, int cacheTime, bool isRefreshCreateTime, int minCounter)
        {
            if (value != null)
            {
                counter = 0;
                value = tValue;
                createTime = DateTime.Now;
                CacheTime = cacheTime;
                MinCounter = minCounter;
                IsRefreshCreateTime = isRefreshCreateTime;
            }
        }
        /// <summary>
        /// 是否刷新创建时间
        /// </summary>
        public bool IsRefreshCreateTime { get; }

        /// <summary>
        /// 缓存时间（单位：秒），小于等于0永久缓存
        /// </summary>
        public int CacheTime { get; }

        /// <summary>
        /// 最小调用次数，小于等于0无效
        /// </summary>
        /// <returns></returns>
        public int MinCounter { get; }

        private TValue value;
        /// <summary>
        /// 实际缓存项
        /// </summary>
        public TValue Value
        {
            get
            {
                Refresh();
                return value;
            }
        }

        private DateTime createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return createTime;
            }
        }

        private long counter;
        /// <summary>
        /// 调用次数计数器
        /// </summary>
        public long Counter
        {
            get
            {
                return counter;
            }
        }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsOverdue
        {
            get
            {
                var now = DateTime.Now;
                if (MinCounter > 0 && Counter < MinCounter && CacheTime > 0)
                {
                    //要求最低调用次数大于0，且调用次数小于最低调用次数，且缓存时间不是永久
                    return true;
                }
                else if (MinCounter <= 0 && CacheTime > 0 && CreateTime.AddSeconds(CacheTime) < now)
                {
                    //不受调用次数影响时，创建时间已经超时
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 刷新方法
        /// </summary>
        public void Refresh()
        {
            if (IsRefreshCreateTime)
            {
                createTime = DateTime.Now;
            }
            lock (this)
            {
                if (counter != long.MaxValue)
                {
                    counter++;
                }
            }
        }
    }
}
