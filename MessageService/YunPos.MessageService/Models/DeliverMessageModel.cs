using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService.Models
{
    /// <summary>
    /// 投递消息模型
    /// </summary>
    public class DeliverMessageModel
    {
        /// <summary>
        /// 投递消息模型
        /// </summary>
        /// <param name="message">需要投递的消息.</param>
        public DeliverMessageModel(MessageEntity message)
        {
            Message = message;
        }

        /// <summary>
        /// 需要发送的消息
        /// </summary>
        public MessageEntity Message { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 已投递次数
        /// </summary>
        public int DeliverCount { get; set; }
    }
}
