using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService
{
    /// <summary>
    /// Exchange消息
    /// </summary>
    [DataContract]
    public class MessageEntity
    {
        /// <summary>
        /// 目标
        /// </summary>
        [DataMember]
        public string Receiver { get; set; }

        /// <summary>
        /// 消息类型,值取自 YunPos.MessageService.MessageTypes;
        /// </summary>
        [DataMember]
        public int MessageType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 不进行重试
        /// </summary>
        [DataMember]
        public bool NoRetry { get; set; }

        public override string ToString()
        {
            return $"Receiver:[{Receiver}], MessageType:[{MessageType}], Content:[{Content}], NoRetry:[{NoRetry}]";
        }
    }
}
