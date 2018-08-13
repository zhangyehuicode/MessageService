using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YunPos.MessageService.Host
{
    public class ExchangeTask : ITask, Proxy.IMessageServiceCallback
    {
        #region fields

        Proxy.MessageServiceClient _client;
        string _posApiDomain = string.Empty;//posApi地址
        log4net.ILog _logger;
        /// <summary>
        /// 重试次数
        /// </summary>
        int _retryCount;
        /// <summary>
        /// 重试等待时间（毫秒）
        /// </summary>
        int _retryDueTime = 5000;
        /// <summary>
        /// 最大重试等待时间（毫秒）
        /// </summary>
        int _maxRetryDueTime = 1000 * 60 * 10;
        System.Threading.Timer _retryTimer;
        System.Threading.Timer _pingTimer;
        /// <summary>
        /// ping间隔时间（毫秒）
        /// </summary>
        int _pingDueTime = 10000;
        /// <summary>
        /// 是否正在启动中
        /// </summary>
        bool _isStarting = false;

        Thread _serviceThread;
        #endregion

        #region ctor.
        public ExchangeTask()
        {
            _posApiDomain = AppContext.Settings.YunService;
            _logger = log4net.LogManager.GetLogger("ExchangeService");
            _pingTimer = new Timer(PingServer, null, _pingDueTime, Timeout.Infinite);
        }
        #endregion

        #region properties
        string _clientId = null;
        string ClientId
        {
            get
            {
                if (_clientId == null)
                    _clientId = GetClientId();
                return _clientId;
            }
        }
        #endregion

        #region methods

        public void Start(string args)
        {
            if (_serviceThread != null)
                return;
            _serviceThread = new Thread(() =>
            {
                StartWork();
            });
            _serviceThread.Start();
        }

        void StartWork()
        {
            StartExchange();
            if (_retryCount == 0)//表示启动成功
                return;
            _retryTimer = new System.Threading.Timer(new TimerCallback((state) =>
            {
                _logger.InfoFormat("重启Exchange服务,第 {0} 次", _retryCount);
                StartExchange();
                if (_retryCount > 0)
                {
                    //如果超过最大等待时间，则取最大等待时间。
                    if (_retryDueTime > _maxRetryDueTime)
                        _retryDueTime = _maxRetryDueTime;
                    else if (_retryCount % 10 == 0)
                    {
                        //每重试10次，重试等待时间翻倍。
                        _retryDueTime = _retryDueTime * 2;
                    }
                    _logger.InfoFormat("重启Exchange服务失败，下次重试时间[{0}]", DateTime.Now.AddMilliseconds(_retryDueTime));
                    _retryTimer.Change(_retryDueTime, Timeout.Infinite);
                }
            }), null, 0, Timeout.Infinite);
        }

        void StartExchange()
        {
            if (_isStarting)
                return;
            _isStarting = true;
            try
            {
                _logger.Info("开始启动Exchange服务");
                if (string.IsNullOrEmpty(_posApiDomain))
                {
                    _logger.Error("未配置PosApiDomain，启动SyncData服务失败。");
                    return;
                }
                if (string.IsNullOrEmpty(ClientId))
                {
                    throw new Exception("未配置ClientId，启动Exchange服务失败。");
                }
                _logger.InfoFormat("PosApiDomain:[{0}]", _posApiDomain);
                _client = new Proxy.MessageServiceClient(new System.ServiceModel.InstanceContext(this));
                _client.Subscribe(ClientId);
                var commObj = _client as ICommunicationObject;
                commObj.Faulted += Client_Faulted;
                commObj.Closing += Client_Closing;
                commObj.Closed += Client_Closed;
                _logger.Info("启动Exchange服务 成功");
                _pingTimer.Change(_pingDueTime, Timeout.Infinite);
                //启动成功，重试数据重置。
                _retryCount = 0;
                _retryDueTime = 5000;
            }
            catch (Exception ex)
            {
                _logger.Error("启动Exchange服务 失败, 出现异常", ex);
                _retryCount++;
            }
            finally
            {
                _isStarting = false;
            }
        }

        public void Stop()
        {
            _logger.Info("开始停止Exchange服务");
            try
            {
                if (_retryTimer != null)
                {
                    _retryTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                _pingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                if (_client != null)
                {
                    _client.Unsubscribe(ClientId);
                    _client.Close();
                }
                _logger.Info("停止Exchange服务 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("停止Exchange服务 失败, 出现异常", ex);
                _client.Abort();
            }
            finally
            {
                _serviceThread.Abort();
                _serviceThread = null;
            }
        }

        void PingServer(object state)
        {
            try
            {
                if (_client == null)
                    return;
                _client.Ping();
                _pingTimer.Change(_pingDueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _logger.Error("Ping Push Service失败, 出现异常", ex);
                _pingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                StartWork();
            }
        }

        public void OnMessageReceived(Proxy.MessageEntity message)
        {
            _logger.DebugFormat("收到消息[Receiver:[{0}],MessageType:[{1}],Content:[{2}]]",
                message.Receiver, message.MessageType, message.Content);
            var requestUri = string.Format("{0}/api/Exchange/DealMessage", _posApiDomain);
            _logger.DebugFormat("开始调用{0} 处理消息", requestUri);
            try
            {
                var httpClient = new HttpClient();
                var jsonData = JsonConvert.SerializeObject(message);
                HttpContent httpContent = new StringContent(jsonData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpClient.PostAsync(requestUri, httpContent);
                _logger.DebugFormat("处理消息 成功", requestUri);
            }
            catch (Exception ex)
            {
                _logger.Error("消息处理 失败，发生异常", ex);
            }
        }

        public string GetData(Proxy.MessageEntity message)
        {
            _logger.DebugFormat("收到获取数据请求[Receiver:[{0}],MessageType:[{1}],Content:[{2}]]",
                message.Receiver, message.MessageType, message.Content);
            var requestUri = string.Format("{0}/api/Exchange/DealGetData", _posApiDomain);
            _logger.DebugFormat("开始调用{0} 处理获取数据请求", requestUri);
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                var jsonData = JsonConvert.SerializeObject(message);
                HttpContent httpContent = new StringContent(jsonData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = httpClient.PostAsync(requestUri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = response.Content.ReadAsStringAsync().Result;
                    _logger.DebugFormat("处理获取数据请求 成功,获得数据: [{0}].", responseStr);
                    return responseStr;
                }
                else
                {
                    _logger.ErrorFormat("处理获取数据请求 失败，得到错误的返回状态: [{0}].", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("处理获取数据请求 失败，发生异常", ex);
            }
            return result;
        }

        void Client_Faulted(object sender, EventArgs e)
        {
            _logger.Info("Channel Faulted.");
            _pingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            StartWork();
        }
        void Client_Closing(object sender, EventArgs e)
        {
            _logger.Info("Channel Closing.");
        }
        void Client_Closed(object sender, EventArgs e)
        {
            _logger.Info("Channel Closed.");
        }

        string GetClientId()
        {
            var requestUri = string.Format("{0}/api/Exchange/ClientId", _posApiDomain);
            _logger.DebugFormat("开始调用{0} 获取ClientId", requestUri);
            try
            {
                var httpClient = new HttpClient();
                var responseStr = httpClient.GetStringAsync(requestUri).Result;
                var tempObj = JsonConvert.DeserializeObject<JsonActionResult<string>>(responseStr);
                if (!tempObj.IsSuccessful)
                    throw new Exception(tempObj.ErrorMessage);
                if (".".Equals(tempObj.Data))
                    throw new Exception("服务器返回了空的数据");
                _clientId = tempObj.Data;
                _logger.DebugFormat("获取ClientId 成功, ClientId: [{0}]", _clientId);
            }
            catch (Exception ex)
            {
                _clientId = null;
                _logger.Error("获取ClientId 失败，发生异常", ex);
            }
            return _clientId;
        }

        #endregion
    }
}
