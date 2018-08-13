using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 推送服务任务
    /// </summary>
    public class PushTask : ITask
    {
        #region fields
        ServiceHost _WcfHost;
        log4net.ILog _logger;

        Thread _serviceThread;
        #endregion

        #region ctor.
        public PushTask()
        {
            _logger = log4net.LogManager.GetLogger("PushService");
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
            _logger.Info("开始启动Push服务");
            try
            {
                _WcfHost = new ServiceHost(typeof(Services.MessageService));
                _WcfHost.Open();
                _logger.Info("启动Push服务 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("启动Push服务 失败, 出现异常", ex);
            }
        }

        public void Stop()
        {
            _logger.Info("开始停止Push服务");
            try
            {
                if (_WcfHost != null)
                {
                    _WcfHost.Close();
                }
                _logger.Info("停止Push服务 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("停止Push服务 失败, 出现异常", ex);
            }
            finally
            {
                _serviceThread.Abort();
                _serviceThread = null;
            }
        }

        #endregion
    }
}
