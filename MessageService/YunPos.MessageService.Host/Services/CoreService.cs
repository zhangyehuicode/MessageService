using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunPos.MessageService.Host.Settings;
using YunPos.MessageService.Host.Tasks;

namespace YunPos.MessageService.Host
{
    public class CoreService : ServiceBase
    {
        #region fields
        static log4net.ILog _logger;
        bool _isRunning = false;
        #endregion

        #region ctor.
        static CoreService()
        {
            _logger = log4net.LogManager.GetLogger("CoreService");
            _runningTasks = new List<ITask>();
        }
        #endregion

        #region properties

        public static log4net.ILog Logger
        {
            get
            {
                return _logger;
            }
        }

        static IList<ITask> _runningTasks;
        #endregion

        #region methods

        public void Start()
        {
            if (_isRunning)
            {
                _logger.Info("已在运行中");
                return;
            }
            if (AppContext.Settings.LauncherItems == null || AppContext.Settings.LauncherItems.Count < 1)
            {
                _logger.Info("未配置LauncherItems");
                return;
            }
            _isRunning = true;
            var launcherItems = AppContext.Settings.LauncherItems.OrderBy(item => item.Index);
            ITask task;
            foreach (var item in launcherItems)
            {
                task = GetTaskByRole(item.TaskName);
                if (task == null)
                    continue;
                _runningTasks.Add(task);
                task.Start(item.Args);
            }
        }

        /// <summary>
        /// 根据角色获取Task
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        ITask GetTaskByRole(string role)
        {
            ITask task = null;
            switch (role)
            {
                case "Push":
                    task = new PushTask();
                    break;
                case "Exchange":
                    task = new ExchangeTask();
                    break;
                case "SyncData":
                    task = new SyncDataTask();
                    break;
                case "CouponReminder":
                    task = new CouponReminderTask();
                    break;
                case "SyncWeChatFans":
                    task = new SyncWeChatFansTask();
                    break;
                case "CouponPush":
                    task = new CouponPushTask();
                    break;
                case "RealOperatingReportNotify":
                    task = new RealOperatingReportNotifyTask();
                    break;
                case "SendDailyTurnoverReportTask":
                    task = new SendDailyTurnoverReportTask();
                    break;
                case "ServicePeriodReminderTask":
                    task = new ServicePeriodReminderTask();
                    break;
                case "CarBitCoinDistributionTask":
                    task = new CarBitCoinDistributionTask();
                    break;
            }
            return task;
        }

        public void StopService()
        {
            if (_runningTasks == null || _runningTasks.Count < 0)
                return;
            foreach (var task in _runningTasks)
            {
                task.Stop();
            }
            _runningTasks.Clear();
            _isRunning = false;
        }


        protected override void OnStart(string[] args)
        {
            _logger.Info("MessageService Start. ");
            Start();
        }

        protected override void OnShutdown()
        {
            _logger.Info("MessageService Shutingdown. ");
            StopService();
        }

        protected override void OnStop()
        {
            _logger.Info("MessageService Stopped. ");
            StopService();
        }

        #endregion
    }
}
