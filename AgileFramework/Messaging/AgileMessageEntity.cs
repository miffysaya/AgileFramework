using System;

namespace AgileFramework.Messaging
{
    /// <summary>
    /// 消息实体
    /// </summary>
    [Serializable]
    public class AgileMessageEntity
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Body { get; }

        public AgileMessageEntity(string label, string body)
        {
            Label = label;
            Body = body;
        }

        /// <summary>
        /// 重写Equals方法，注意，标签相同就认为是同一个消息实体
        /// </summary>
        /// <param name="obj">比较的对象</param>
        /// <returns>比较的结果</returns>
        public override bool Equals(object obj)
        {
            var other = obj as AgileMessageEntity;
            if (other == null)
            {
                return false;
            }
            else
            {
                return other.Label == Label;
            }
        }
        /// <summary>
        /// 重写GetHashCode，注意，以标签的哈希码为消息实体的哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
}
