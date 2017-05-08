using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace AgileFramework.Messaging
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public static class AgileMessageQuene
    {
        /// <summary>
        /// 向消息队列发送一个消息
        /// </summary>
        /// <param name="address">地址，例如：FormatName:Direct=OS:.\private$\SKMQDEMO</param>
        /// <param name="smartMessageEntity">消息实体</param>
        public static void Send(string address, AgileMessageEntity agileMessageEntity)
        {
            //创建一个消息
            Message message = new Message();
            message.Label = agileMessageEntity.Label;
            message.Body = agileMessageEntity.Body;

            //保存消息到队列中
            using (MessageQueue messageQueue = new MessageQueue(address))
            {
                messageQueue.Send(message);
            }
        }
        /// <summary>
        /// 从消息队列取出一个消息，并移除消息
        /// </summary>
        /// <param name="address">地址，例如：FormatName:Direct=OS:.\private$\SKMQDEMO</param>
        /// <returns>返回消息实体</returns>
        public static AgileMessageEntity Receive(string address)
        {
            AgileMessageEntity agileMessageEntity = null;
            //取出一个消息进行处理
            using (MessageQueue messageQueue = new MessageQueue(address))
            {
                ((XmlMessageFormatter)messageQueue.Formatter).TargetTypeNames = new string[1] { typeof(string).ToString() };
                Message message = messageQueue.Receive();
                agileMessageEntity = new AgileMessageEntity(message.Label, message.Body.ToString());
            }
            return agileMessageEntity;
        }
        /// <summary>
        /// 从消息队列取出一个消息，不移除消息
        /// </summary>
        /// <param name="address">地址，例如：FormatName:Direct=OS:.\private$\SKMQDEMO</param>
        /// <returns>返回消息实体</returns>
        public static AgileMessageEntity Peek(string address)
        {
            AgileMessageEntity agileMessageEntity = null;
            //取出一个消息进行处理
            using (MessageQueue messageQueue = new MessageQueue(address))
            {
                ((XmlMessageFormatter)messageQueue.Formatter).TargetTypeNames = new string[1] { typeof(string).ToString() };
                Message message = messageQueue.Peek();
                agileMessageEntity = new AgileMessageEntity(message.Label, message.Body.ToString());
            }
            return agileMessageEntity;
        }
    }
}
