using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 昨日营运报表通知服务
    /// </summary>
    public class SendDailyTurnoverReportTask : ITask
    {
        const string ScheduleJobIdentity = "SendDailyTurnoverReportTask";
        string _posApiDomain = string.Empty;
        log4net.ILog _logger;

        public SendDailyTurnoverReportTask()
        {
            _posApiDomain = AppContext.Settings.YunService;
            _logger = log4net.LogManager.GetLogger(ScheduleJobIdentity);
        }

        void SendDailyTurnoverReport()
        {
            try
            {
                _logger.Debug($"开始发送{ScheduleJobIdentity}请求");
                var requestUri = string.Format("{0}/api/DailyCashRecord/sendDailyTurnoverReport", _posApiDomain);
                var httpClient = new HttpClient();
                httpClient.GetStringAsync(requestUri);
                _logger.Debug($"发送{ScheduleJobIdentity} 请求 成功");
            }
            catch (Exception e)
            {
                _logger.Debug($"发送{ScheduleJobIdentity} 请求 出现异常");
            }
        }

        public void Start(string args)
        {
            _logger.Info($"开始启动{ScheduleJobIdentity} 服务");
            if (string.IsNullOrEmpty(_posApiDomain))
            {
                _logger.Error("未配置 YunPosService, 启动  发送昨日营运报表 失败.");
                return;
            }
            _logger.InfoFormat("YunPosService: [{0}]", _posApiDomain);
            if (string.IsNullOrEmpty(args))
            {
                _logger.Info($"未配置 执行时刻， 启动{ScheduleJobIdentity} 失败.");
                return;
            }
            _logger.InfoFormat("配置的执行时刻[{0}]", args);
            var timeSpanList = args.Split('|');
            foreach (var timeSpan in timeSpanList)
            {
                ConfigSchedule(timeSpan);
            }
        }

        void ConfigSchedule(string timeSpan)
        {
            var hourAndMins = timeSpan.Split(':');
            if (hourAndMins.Length != 2)
            {
                _logger.Info("参数配置错误,正确的格式为 HH:mm.");
                return;
            }
            int hour = 0, minute = 0;
            var parseSuccess = int.TryParse(hourAndMins[0], out hour);
            if (!parseSuccess)
            {
                _logger.Info("参数配置错误,正确的格式为 HH:mm.");
                return;
            }
            parseSuccess = int.TryParse(hourAndMins[1], out minute);
            if (!parseSuccess)
            {
                _logger.Info("参数配置错误,正确的格式为 HH:mm.");
                return;
            }
            var timeOfDay = new TimeSpan(hour, minute, 0);
            TimerService.Instance.AddScheduleJob(new TimerService.ScheduleJob(ScheduleJobIdentity, timeOfDay, () => { SendDailyTurnoverReport(); }));
        }

        public void Stop()
        {
            _logger.Info($"开始停止 {ScheduleJobIdentity}");
            try
            {
                TimerService.Instance.RemoveScheduleJob(ScheduleJobIdentity);
                _logger.Info($"停止 {ScheduleJobIdentity} 成功");
            }
            catch (Exception ex)
            {
                _logger.Error($"停止 {ScheduleJobIdentity} 失败, 出现异常.", ex);
            }
        }
    }
}
