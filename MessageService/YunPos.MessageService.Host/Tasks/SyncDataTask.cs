using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 同步数据任务
    /// </summary>
    public class SyncDataTask : ITask
    {
        #region fields
        const string ScheduleJobIdentity = "SyncDataTask";
        string _posApiDomain = string.Empty;//posApi地址
        int _syncInterval;//间隔，分钟为单位
        log4net.ILog _logger;
        #endregion

        #region ctor.
        public SyncDataTask()
        {
            _posApiDomain = AppContext.Settings.YunService;
            _logger = log4net.LogManager.GetLogger("SyncDataService");
        }
        #endregion

        #region methods

        void DoSyncData()
        {
            try
            {
                _logger.Debug("开始发送 SyncData 请求");
                var requestUri = string.Format("{0}/api/SyncJob/StartSync", _posApiDomain);
                HttpClient httpClient = new HttpClient();
                httpClient.GetStringAsync(requestUri);
                _logger.Debug("发送 SyncData 请求 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("发送 SyncData 请求 出现异常。", ex);
            }
        }

        public void Start(string args)
        {
            try
            {
                _logger.Info("开始启动SyncData服务");
                if (string.IsNullOrEmpty(_posApiDomain))
                {
                    _logger.Error("未配置PosApiDomain，启动SyncData服务失败。");
                    return;
                }
                _logger.InfoFormat("PosApiDomain:[{0}]", _posApiDomain);
                int.TryParse(args, out _syncInterval);
                if (_syncInterval == 0)
                {
                    _logger.Info("未配置 SyncInterval 同步间隔，系统默认使用[10]");
                    _syncInterval = 10;
                }
                _logger.InfoFormat("SyncInterval 同步间隔[{0}]", _syncInterval);
                TimerService.Instance.AddScheduleJob(new TimerService.ScheduleJob(ScheduleJobIdentity, _syncInterval, () => { DoSyncData(); }));
                _logger.Info("启动SyncData服务 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("启动SyncData服务 失败, 出现异常", ex);
            }
        }

        public void Stop()
        {
            _logger.Info("开始停止SyncData服务");
            try
            {
                TimerService.Instance.RemoveScheduleJob(ScheduleJobIdentity);
                _logger.Info("停止SyncData服务 成功");
            }
            catch (Exception ex)
            {
                _logger.Error("停止SyncData服务 失败, 出现异常", ex);
            }
        }

        #endregion
    }
}
