using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileFramework.Algorithms
{
    /// <summary>
    /// 总结一致性哈希算法
    /// </summary>
    public class AgileConsistentHash
    {
        /// <summary>
        /// 根据hashKey和节点数确认是否需要更换到新节点
        /// </summary>
        /// <param name="beforeNodeCount">之前的节点数</param>
        /// <param name="afterNodeCount">之后的节点数</param>
        /// <param name="hashKey">进行hash操作的key</param>
        /// <param name="nextServer">返回key命中的节点</param>
        /// <returns>是否需要移动到新节点</returns>
        public static bool MoveToNextNode(int beforeNodeCount, int nextNodeCount, string hashKey, ref int next)
        {
            var pres = new List<int>();

            for (var i = 0; i < beforeNodeCount; i++)
            {
                pres.Add(i);
            }

            var nexts = new List<int>();

            for (var i = 0; i < nextNodeCount; i++)
            {
                nexts.Add(i);
            }

            var preHash = new ConsistentHash<int>();

            preHash.Initialize(pres);

            var nextHash = new ConsistentHash<int>();

            nextHash.Initialize(nexts);

            var pre = preHash.GetNode(hashKey);

            next = nextHash.GetNode(hashKey);

            return pre == next;
        }
    }

    /// <summary>
    /// 一致性哈希循环圈
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ConsistentHash<T>
    {
        /// <summary>
        /// 哈希循环圈
        /// </summary>
        SortedDictionary<int, T> circle = new SortedDictionary<int, T>();

        /// <summary>
        /// 默认重复次数
        /// </summary>
        int _replicate = 100;

        /// <summary>
        /// 有序键缓存器
        /// </summary>
        int[] arrKeys = null;

        /// <summary>
        /// 以默认重复次数初始化节点群
        /// </summary>
        /// <param name="nodes">节点群</param>
        public void Initialize(IEnumerable<T> nodes)
        {
            Initialize(nodes, _replicate);
        }

        /// <summary>
        /// 初始化节点群
        /// </summary>
        /// <param name="nodes">节点群</param>
        /// <param name="replicate">重复次数</param>
        public void Initialize(IEnumerable<T> nodes, int replicate)
        {
            _replicate = replicate;

            foreach (T node in nodes)
            {
                this.Add(node, false);
            }
            arrKeys = circle.Keys.ToArray();
        }

        /// <summary>
        /// 向循环圈中添加节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="updateKeyArray">是否更新缓存器</param>
        private void Add(T node, bool updateKeyArray)
        {
            for (var i = 0; i < _replicate; i++)
            {
                var hash = BetterHash(node.GetHashCode().ToString() + i);

                circle[hash] = node;
            }
            if (updateKeyArray)
            {
                arrKeys = circle.Keys.ToArray();
            }
        }

        /// <summary>
        /// 从循环圈中移除节点
        /// </summary>
        /// <param name="node">节点</param>
        public void Remove(T node)
        {
            for (int i = 0; i < _replicate; i++)
            {
                var hash = BetterHash(node.GetHashCode().ToString() + i);

                if (!circle.Remove(hash))
                {
                    throw new Exception("无法移除不存在的节点");
                }
            }
            arrKeys = circle.Keys.ToArray();
        }

        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>节点</returns>
        public T GetNode(string key)
        {
            int hash = BetterHash(key);

            int first = GetFirst(arrKeys, hash);

            return circle[arrKeys[first]];
        }

        /// <summary>
        /// 获取大于等于指定数值的第一个键的索引
        /// </summary>
        /// <param name="arr">有序键缓存器</param>
        /// <param name="val">指定数值</param>
        /// <returns>第一个键的索引，不存在则为0</returns>
        public int GetFirst(int[] arr, int val)
        {
            int begin = 0;

            int end = arr.Length - 1;

            if (arr[0] > val || arr[end] < val)
            {
                return 0;
            }

            int mid = begin;

            while (end - begin > 1)
            {
                mid = (end + begin) / 2;

                if (arr[mid] >= val)
                {
                    end = mid;
                }
                else
                {
                    begin = mid;
                }
            }
            if (arr[begin] > val || arr[end] < val)
            {
                throw new Exception("发生未知错误");
            }

            return end;
        }

        /// <summary>
        /// 获取合理的哈希值（此处用MurmurHash获取哈希值）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int BetterHash(String key)
        {
            var hash = MurmurHash32.Hash(Encoding.ASCII.GetBytes(key));

            return (int)hash;
        }
    }
}
