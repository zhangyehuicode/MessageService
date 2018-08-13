using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public static class MessageTypes
    {
        /// <summary>
        /// 默认
        /// </summary>
        public const int Default = 0;

        /// <summary>
        /// 订单信息
        /// </summary>
        public const int Order = 100;

        /// <summary>
        /// 支付信息
        /// </summary>
        public const int Payment = 200;
    }
}
