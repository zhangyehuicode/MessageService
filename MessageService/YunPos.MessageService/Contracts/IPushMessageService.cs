using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService.Contracts
{
    /// <summary>
    /// 发送ExchangeMessage接口
    /// </summary>
    [ServiceContract]
    public interface IPushMessageService
    {
        /// <summary>
        /// 发送消息(异步)，不需要返回值时使用此方法。
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void PushMessage(MessageEntity message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        string PushMessageNeedBack(MessageEntity message);
    }
}
