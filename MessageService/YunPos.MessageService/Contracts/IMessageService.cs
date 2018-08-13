using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace YunPos.MessageService.Contracts
{
    /// <summary>
    /// ExchangeService
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IMessageCallBack))]
    public interface IMessageService
    {

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="clientId">如果是门店服务订阅则填写门店Code，如果是POS客户端订阅则传入客户端ID</param>
        [OperationContract(IsOneWay = true)]
        void Subscribe(string clientId);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="clientId">如果是门店服务订阅则填写门店Code，如果是POS客户端订阅则传入客户端ID</param>
        [OperationContract(IsOneWay = true)]
        void Unsubscribe(string clientId);

        /// <summary>
        /// 发送消息(异步)，不需要返回值时使用此方法。
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(MessageEntity message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        string SendMessageNeedBack(MessageEntity message);

        /// <summary>
        /// 服务心跳测试
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        string Ping();
    }

    public interface IMessageCallBack
    {
        /// <summary>
        /// 服务像客户端发送信息(异步)
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void OnMessageReceived(MessageEntity message);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns>返回数据JSON</returns>
        [OperationContract]
        string GetData(MessageEntity message);
    }
}
