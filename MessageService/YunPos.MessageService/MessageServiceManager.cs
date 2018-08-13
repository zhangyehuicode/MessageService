using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunPos.MessageService.Contracts;
using YunPos.MessageService.Models;

namespace YunPos.MessageService
{
    /// <summary>
    /// MessageService管理器
    /// </summary>
    public static class MessageServiceManager
    {
        #region fields
        static log4net.ILog _logger;
        static readonly ConcurrentDictionary<string, IMessageCallBack> _Subscribers;

        static readonly List<DeliverMessageModel> _retryDeliverMessages;
        static readonly Timer _deliverMessageTimer;
        /// <summary>
        /// 重试任务是否在运行
        /// </summary>
        static bool _IsRetryTimerRunning;
        /// <summary>
        /// 重试间隔时间，单位（毫秒）
        /// </summary>
        const int RetryDueTime = 10 * 1000;
        /// <summary>
        /// 最大重试次数
        /// </summary>
        const int MaxRetryCount = 100;
        /// <summary>
        /// 所有订阅者
        /// </summary>
        public const string Receiver_AllSubscriber = "*";
        #endregion

        static MessageServiceManager()
        {
            _Subscribers = new ConcurrentDictionary<string, IMessageCallBack>();
            _retryDeliverMessages = new List<DeliverMessageModel>();
            _deliverMessageTimer = new Timer(new TimerCallback((state) =>
            {
                try
                {
                    _IsRetryTimerRunning = true;
                    var messages = _retryDeliverMessages.ToList();
                    foreach(var deliverMessage in messages)
                    {
                        TryDeliverMessage(deliverMessage);
                        if (deliverMessage.IsFinish)
                            _retryDeliverMessages.Remove(deliverMessage);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("尝试重发消息失败", ex);
                }
                finally
                {
                    if (_retryDeliverMessages.Count > 0)
                        _deliverMessageTimer.Change(RetryDueTime, Timeout.Infinite);
                    else
                        _IsRetryTimerRunning = false;
                }
            }), null, RetryDueTime, Timeout.Infinite);
            _logger = log4net.LogManager.GetLogger("MessageService");
        }

        #region properties

        public static log4net.ILog Logger
        {
            get
            {
                return _logger;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// 添加订阅者
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="callBack"></param>
        public static void AddSubscriber(string clientId, IMessageCallBack callBack)
        {
            string ipAddress = string.Empty;
            try
            {
                //获取传进的消息属性  
                var properties = OperationContext.Current.IncomingMessageProperties;
                //获取消息发送的远程终结点IP和端口  
                var endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                ipAddress = endpoint.Address;
            }
            catch (Exception ex)
            {
                Logger.Error("获取订阅客户端IP出错", ex);
            }
            Logger.InfoFormat("添加订阅 [{0}], IPAddress [{1}]", clientId, ipAddress);
            _Subscribers[clientId] = callBack;
        }

        /// <summary>
        /// 删除订阅者
        /// </summary>
        /// <param name="clientId"></param>
        public static void RemoveSubscriber(string clientId)
        {
            Logger.InfoFormat("取消订阅 [{0}]", clientId);
            if (_Subscribers.ContainsKey(clientId))
            {
                IMessageCallBack outObj = null;
                _Subscribers.TryRemove(clientId, out outObj);
            }
        }

        /// <summary>
        /// 删除订阅者
        /// </summary>
        /// <param name="callBack"></param>
        public static void RemoveSubscriber(IMessageCallBack callBack)
        {
            _Subscribers.ToList().ForEach(subscribe =>
            {
                if (subscribe.Value == callBack)
                {
                    Logger.InfoFormat("[{0}], 连接关闭", subscribe.Key);
                    RemoveSubscriber(subscribe.Key);
                }
            });
        }

        /// <summary>
        /// 获取订阅者列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSubscriberList()
        {
            return _Subscribers.Keys.ToArray();
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="message"></param>
        public static void PushMessage(MessageEntity message)
        {
            if (message == null || string.IsNullOrEmpty(message.Receiver))
            {
                Logger.Info("取消推送消息，消息或消息接收方为null，推送失败。");
                return;
            }
            DeliverMessageWithRetry(message);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="message"></param>
        public static string GetData(MessageEntity message)
        {
            Logger.InfoFormat("开始获取数据, Message:[Receiver:[{0}], MessageType:[{1}], Content:[{2}]]",
                message.Receiver, message.MessageType, message.Content);
            if (message == null || string.IsNullOrEmpty(message.Receiver))
                return string.Empty;

            if (!_Subscribers.ContainsKey(message.Receiver))
            {
                Logger.InfoFormat("取消获取数据，{0} 未订阅消息推送。", message.Receiver);
                return string.Empty;
            }
            var subscriber = _Subscribers[message.Receiver];

            var obj = (ICommunicationObject)subscriber;
            if ((obj).State != CommunicationState.Opened)
            {
                Logger.ErrorFormat("获取数据失败，[{0}] 连接状态异常，连接状态[{1}]。", message.Receiver, obj.State);
                RemoveSubscriber(message.Receiver);
                return string.Empty;
            }
            try
            {
                var result = subscriber.GetData(message);
                Logger.InfoFormat("获取数据成功, Receiver: [{0}], result:[{1}]", message.Receiver, result);
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("获取数据失败发生异常，异常原因：[{0}]]", ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// 投递消息，带重试功能
        /// </summary>
        /// <param name="message"></param>
        static void DeliverMessageWithRetry(MessageEntity message)
        {
            if (message == null)
                return;
            if (Receiver_AllSubscriber.Equals(message.Receiver))
            {
                var subscribers = _Subscribers.ToList();
                foreach (var subscriber in subscribers)
                {
                    TryDeliverMessage(new MessageEntity
                    {
                        Receiver = subscriber.Key,
                        MessageType = message.MessageType,
                        Content = message.Content,
                        NoRetry = message.NoRetry,
                    });
                }
            }
            else
            {
                TryDeliverMessage(message);
            }
        }

        /// <summary>
        /// 投递消息，带重试功能
        /// </summary>
        /// <param name="message"></param>
        static void TryDeliverMessage(MessageEntity message)
        {
            var deliverMessage = new DeliverMessageModel(message);
            TryDeliverMessage(deliverMessage);
            if (!deliverMessage.IsFinish)
            {
                _retryDeliverMessages.Add(deliverMessage);
                if (!_IsRetryTimerRunning)
                {
                    _deliverMessageTimer.Change(RetryDueTime, Timeout.Infinite);
                    _IsRetryTimerRunning = true;
                }
            }
        }

        /// <summary>
        /// 投递消息，带重试功能
        /// </summary>
        /// <param name="deliverMessage"></param>
        static void TryDeliverMessage(DeliverMessageModel deliverMessage)
        {
            if (deliverMessage == null || deliverMessage.Message == null)
                return;
            if (deliverMessage.DeliverCount == 0)
            {
                Logger.InfoFormat("开始推送消息, Message:[{0}]", deliverMessage.Message.ToString());
            }
            else
            {
                Logger.InfoFormat("开始尝试第 {0} 次推送消息, Message:[{1}]",
                    deliverMessage.DeliverCount, deliverMessage.Message.ToString());
            }
            var result = DeliverMessage(deliverMessage.Message);
            if (result)
            {
                deliverMessage.IsFinish = true;
            }
            else if (deliverMessage.Message.NoRetry)
            {
                deliverMessage.IsFinish = true;
            }
            else
            {
                if (deliverMessage.DeliverCount >= MaxRetryCount)
                {
                    deliverMessage.IsFinish = true;
                    Logger.Info("取消推送消息，重试次数超过最大重试上限");
                }
                else
                {
                    deliverMessage.DeliverCount++;
                }
            }
        }

        /// <summary>
        /// 投递消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        static bool DeliverMessage(MessageEntity message)
        {
            if (!_Subscribers.ContainsKey(message.Receiver))
            {
                Logger.InfoFormat("取消推送消息，{0} 未订阅消息推送。", message.Receiver);
                return false;
            }
            var subscriber = _Subscribers[message.Receiver];

            var obj = (ICommunicationObject)subscriber;
            if (obj.State != CommunicationState.Opened)
            {
                Logger.ErrorFormat("推送消息失败，[{0}] 连接状态异常，连接状态[{1}]。", message.Receiver, obj.State);
                RemoveSubscriber(message.Receiver);
                return false;
            }
            try
            {
                subscriber.OnMessageReceived(message);
                Logger.InfoFormat("推送消息成功, Message:[Receiver:[{0}], MessageType:[{1}], Content:[{2}]]",
                    message.Receiver, message.MessageType, message.Content);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("推送消息失败发生异常，异常原因：[{0}]]", ex.ToString());
                return false;
            }
        }

        #endregion
    }
}
