﻿using System;
using System.Net.Http;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 推送卡券服务
    /// </summary>
    public class CouponPushTask : ITask
    {
        #region fields
        const string ScheduleJobIdentity = "CouponPushTask";
        string _apiDomain = string.Empty;//memberApi地址
        log4net.ILog _logger;
        #endregion

        #region ctor.
        public CouponPushTask()
        {
            _apiDomain = AppContext.Settings.YunService;
            _logger = log4net.LogManager.GetLogger(ScheduleJobIdentity);
        }
        #endregion

        void RunSyncWeChatFans()
        {
            try
            {
                _logger.Debug($"开始发送 {ScheduleJobIdentity} 请求");
                var requestUri = string.Format("{0}/api/BackgroundTask/PushCoupon", _apiDomain);
                var httpClient = new HttpClient();
                httpClient.GetStringAsync(requestUri);
                _logger.Debug($"发送 {ScheduleJobIdentity} 请求 成功");
            }
            catch (Exception ex)
            {
                _logger.Error($"发送 {ScheduleJobIdentity} 请求 出现异常。", ex);
            }
        }

        public void Start(string args)
        {
            try
            {
                _logger.Info($"开始启动 {ScheduleJobIdentity} 服务");
                if (string.IsNullOrEmpty(_apiDomain))
                {
                    _logger.Error("未配置 YunService，启动 卡券推送服务 失败。");
                    return;
                }
                _logger.InfoFormat("YunService:[{0}]", _apiDomain);
                if (string.IsNullOrEmpty(args))
                {
                    _logger.Info($"未配置 执行时刻，启动 {ScheduleJobIdentity} 失败。");
                    return;
                }
                _logger.InfoFormat("配置的执行时刻[{0}]", args);
                var hourAndMins = args.Split(':');
                if (hourAndMins.Length != 2)
                {
                    _logger.Info("参数配置错误，正确的格式为 HH:mm，启动 卡券推送服务 失败。");
                    return;
                }
                int hour = 0, minute = 0;
                var parseSuccess = int.TryParse(hourAndMins[0], out hour);
                if (!parseSuccess)
                {
                    _logger.Info("参数配置错误，正确的格式为 HH:mm，启动 卡券推送服务 失败。");
                    return;
                }
                parseSuccess = int.TryParse(hourAndMins[1], out minute);
                if (!parseSuccess)
                {
                    _logger.Info("参数配置错误，正确的格式为 HH:mm，启动 卡券推送服务 失败。");
                    return;
                }
                var timeSpan = new TimeSpan(hour, minute, 0);
                TimerService.Instance.AddScheduleJob(new TimerService.ScheduleJob(ScheduleJobIdentity, timeSpan, () => { RunSyncWeChatFans(); }));
                _logger.Info($"启动 {ScheduleJobIdentity} 成功");
            }
            catch (Exception ex)
            {
                _logger.Error($"启动 {ScheduleJobIdentity} 失败, 出现异常", ex);
            }
        }

        public void Stop()
        {
            _logger.Info($"开始停止 {ScheduleJobIdentity} ");
            try
            {
                TimerService.Instance.RemoveScheduleJob(ScheduleJobIdentity);
                _logger.Info($"停止 {ScheduleJobIdentity} 成功");
            }
            catch (Exception ex)
            {
                _logger.Error($"停止 {ScheduleJobIdentity} 失败, 出现异常", ex);
            }
        }
    }
}
