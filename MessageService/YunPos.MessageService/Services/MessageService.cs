using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using YunPos.MessageService.Contracts;

namespace YunPos.MessageService.Services
{
    /// <summary>
    /// Exchange服务实现
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessageService : IMessageService, IPushMessageService
    {
        #region methods

        public void Subscribe(string clientId)
        {
            var callBack = OperationContext.Current.GetCallbackChannel<IMessageCallBack>();
            MessageServiceManager.AddSubscriber(clientId, callBack);
            var obj = (ICommunicationObject)callBack;
            obj.Closing += Callback_Closing;
        }

        public void Unsubscribe(string clientId)
        {
            MessageServiceManager.RemoveSubscriber(clientId);
        }

        public void SendMessage(MessageEntity message)
        {
            MessageServiceManager.PushMessage(message);
        }

        public string SendMessageNeedBack(MessageEntity message)
        {
            return MessageServiceManager.GetData(message);
        }

        private void Callback_Closing(object sender, EventArgs e)
        {
            var callBack = sender as IMessageCallBack;
            if (callBack == null)
                return;
            MessageServiceManager.RemoveSubscriber(callBack);
        }

        public void PushMessage(MessageEntity message)
        {
            MessageServiceManager.PushMessage(message);
        }

        public string PushMessageNeedBack(MessageEntity message)
        {
            return MessageServiceManager.GetData(message);
        }

        public string Ping()
        {
            return DateTime.Now.ToString();
        }

        #endregion
    }
}
