using AgileFramework.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace AgileFramework.Net
{
    /// <summary>
    /// 基于并行字典的对象池
    /// </summary>
    public class AgileConcurrentMemoryCache<TKey, TValue> : IDisposable where TValue : AgileConcurrentMemoryCacheValue
    {
        /// <summary>
        /// 基于并行字典的缓存器
        /// </summary>
        private ConcurrentDictionary<TKey, TValue> cache = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// 定时器
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 超时间隔（单位秒）
        /// </summary>
        private double OvertimeInterval;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="overtimeInterval">超时间隔（单位：秒）</param>
        /// <param name="cleanInterval">清除时间间隔（单位：秒）</param>
        public AgileConcurrentMemoryCache(double overtimeInterval, double clearInterval)
        {
            if (overtimeInterval > 0)
            {
                OvertimeInterval = overtimeInterval;
                timer = new Timer(clearInterval * 1000);
                timer.Elapsed += timeElapsed;
                timer.Start();
            }
        }

        /// <summary>
        /// 定时方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeElapsed(object sender, ElapsedEventArgs e)
        {
            var timestamp = DateTime.Now.AddSeconds(-OvertimeInterval);

            OverTimeClear(timestamp);
        }

        /// <summary>
        ///  超时清理方法
        /// </summary>
        /// <param name="timestamp"></param>
        public void OverTimeClear(DateTime timestamp)
        {
            var list = new List<TKey>();
            foreach (var kv in cache)
            {
                var value = kv.Value as AgileConcurrentMemoryCacheValue;

                if (value.Timestamp <= timestamp)
                {
                    list.Add(kv.Key);
                }
            }

            list.ForEach(key =>
            {
                Remove(key);
            });
        }

        /// <summary>
        /// 删除一项
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            TValue value = default(TValue);

            cache.TryRemove(key, out value);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            cache.Clear();
        }

        /// <summary>
        /// 增加一项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value)
        {
            cache.AddOrUpdate(key, value, (newKey, newValue) =>
            {
                return cache[newKey];
            });
        }

        /// <summary>
        /// 判断存在与否
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public bool Contains(TKey key)
        {
            return cache.ContainsKey(key);
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns>结果</returns>
        public List<KeyValuePair<TKey, TValue>> GetItems()
        {
            return cache.ToList();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
            cache.Clear();
        }
    }

    /// <summary>
    /// 基于并行字典的带缓存文件对象池
    /// </summary>
    public class AgileConcurrentFileMemoryCache<TKey, TValue> : IDisposable where TValue : AgileConcurrentMemoryCacheValue
    {
        /// <summary>
        /// 基于并行字典的缓存器
        /// </summary>
        private ConcurrentDictionary<TKey, TValue> cache = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// 定时器
        /// </summary>
        private Timer timer;

        /// <summary>
        /// 超时间隔（单位秒）
        /// </summary>
        private double OvertimeInterval;

        /// <summary>
        /// 缓存文件路径
        /// </summary>
        private string FilePath;

        /// <summary>
        /// 文件类型
        /// </summary>
        private AgileFileType FileType = AgileFileType.Text;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="overtimeInterval">超时间隔（单位：秒）</param>
        /// <param name="cleanInterval">清除时间间隔（单位：秒）</param>
        public AgileConcurrentFileMemoryCache(double overtimeInterval, double clearInterval,string filePath,AgileFileType fileType = AgileFileType.Text)
        {
            if (overtimeInterval > 0)
            {
                FilePath = filePath;
                FileType = fileType;
                cache = AgileFile.BinaryRead<ConcurrentDictionary<TKey, TValue>>(FilePath, FileType);
                OvertimeInterval = overtimeInterval;
                timer = new Timer(clearInterval * 1000);
                timer.Elapsed += timeElapsed;
                timer.Start();
            }
        }

        /// <summary>
        /// 定时方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeElapsed(object sender, ElapsedEventArgs e)
        {
            var timestamp = DateTime.Now.AddSeconds(-OvertimeInterval);

            OverTimeClear(timestamp);
        }

        /// <summary>
        ///  超时清理方法
        /// </summary>
        /// <param name="timestamp"></param>
        public void OverTimeClear(DateTime timestamp)
        {
            var list = new List<TKey>();
            foreach (var kv in cache)
            {
                var value = kv.Value as AgileConcurrentMemoryCacheValue;

                if (value.Timestamp <= timestamp)
                {
                    list.Add(kv.Key);
                }
            }

            list.ForEach(key =>
            {
                Remove(key);
            });
        }

        /// <summary>
        /// 删除一项
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            TValue value = default(TValue);

            cache.TryRemove(key, out value);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            cache.Clear();
        }

        /// <summary>
        /// 增加一项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value)
        {
            cache.AddOrUpdate(key, value, (newKey, newValue) =>
            {
                return cache[newKey];
            });
        }

        /// <summary>
        /// 判断存在与否
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果</returns>
        public bool Contains(TKey key)
        {
            return cache.ContainsKey(key);
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns>结果</returns>
        public List<KeyValuePair<TKey, TValue>> GetItems()
        {
            return cache.ToList();
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
            AgileFile.BinaryWrite(cache, FilePath, FileType);
            cache.Clear();
        }
    }

    /// <summary>
    /// 基于并行字典的对象池的值基类（必须继承此基类，才能正常清理）
    /// </summary>
    [Serializable]
    public abstract class AgileConcurrentMemoryCacheValue
    {
        /// <summary>
        /// 对象时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
