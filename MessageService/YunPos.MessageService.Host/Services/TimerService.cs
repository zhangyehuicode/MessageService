using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 提供公用的定时器服务
    /// </summary>
    public class TimerService
    {
        #region inner class
        /// <summary>
        /// 计划任务
        /// </summary>
        public class ScheduleJob
        {
            #region fields
            DateTime _nextExecuteTimeBegin;
            DateTime _nextExecuteTimeEnd;
            #endregion

            #region ctor.
            private ScheduleJob(string identity, Action action)
            {
                Identity = identity;
                Action = action;
            }

            public ScheduleJob(string identity, int fixedInterval, Action action)
                : this(identity, action)
            {
                FixedInterval = fixedInterval;
                CountNextExecuteTime();
            }

            public ScheduleJob(string identity, TimeSpan timeOfDay, Action action)
                : this(identity, action)
            {
                TimeOfDay = timeOfDay;
                CountNextExecuteTime();
            }
            #endregion

            #region properties
            /// <summary>
            /// 标识
            /// </summary>
            public string Identity { get; private set; }

            /// <summary>
            /// 固定间隔（分钟）
            /// </summary>
            int? FixedInterval { get; set; }

            /// <summary>
            /// 固定时刻
            /// </summary>
            TimeSpan? TimeOfDay { get; set; }

            /// <summary>
            /// 执行动作
            /// </summary>
            public Action Action { get; set; }
            #endregion

            #region methods
            /// <summary>
            /// 执行
            /// </summary>
            public void Execute()
            {
                if (Action == null) return;
                var now = DateTime.Now;
                if (now < _nextExecuteTimeBegin)
                {
                    return;
                }
                try
                {
                    Task.Run(Action);
                }
                catch (Exception ex)
                {
                    _logger.Debug($"执行 {Identity} 出现异常。", ex);
                }
                finally
                {
                    CountNextExecuteTime();
                }
            }

            /// <summary>
            /// 计算下一次执行时间
            /// </summary>
            private void CountNextExecuteTime()
            {
                var nextTime = DateTime.Now;
                if (FixedInterval.HasValue)
                {
                    nextTime = nextTime.AddMinutes(FixedInterval.Value);
                }
                else
                {
                    var startSpan = TimeOfDay.Value;
                    int days = nextTime.TimeOfDay < startSpan ? 0 : 1;
                    nextTime = nextTime.Date.AddDays(days).AddHours(startSpan.Hours).AddMinutes(startSpan.Minutes);
                }
                _nextExecuteTimeBegin = nextTime;
                _nextExecuteTimeEnd = nextTime.AddMinutes(AppContext.Settings.TimerInterval);
                _logger.Debug($"{Identity} 下一次执行时间,Start[{_nextExecuteTimeBegin}], End[{_nextExecuteTimeEnd}]");
            }
            #endregion
        }
        #endregion

        #region fields
        static readonly TimerService _instance;

        static log4net.ILog _logger;
        static int _timerInterval;

        System.Threading.Timer _innerTimer;
        List<ScheduleJob> _scheduleList;
        bool _isRunning;
        #endregion

        #region ctor.
        private TimerService()
        {
            _innerTimer = new System.Threading.Timer(new TimerCallback(DoJob), null, Timeout.Infinite, Timeout.Infinite);
            _scheduleList = new List<ScheduleJob>();
        }

        static TimerService()
        {
            _logger = log4net.LogManager.GetLogger("TimerService");
            _instance = new TimerService();
            if (AppContext.Settings.TimerInterval == 0)
            {
                _logger.Info("未配置 TimerInterval，系统默认使用[5]");
                AppContext.Settings.TimerInterval = 5;
            }
            else
            {
                _logger.InfoFormat("TimerInterval 间隔[{0}]", AppContext.Settings.TimerInterval);
            }
            _timerInterval = 1000 * 60 * AppContext.Settings.TimerInterval;
        }
        #endregion

        #region properties
        public static TimerService Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Methods
        void Start()
        {
            if (_isRunning)
                return;
            if (_scheduleList.Count < 1)
                return;
            _innerTimer.Change(1000, _timerInterval);
            _isRunning = true;
        }

        void Stop()
        {
            _isRunning = false;
            _innerTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void DoJob(object state)
        {
            if (_scheduleList.Count < 1)
            {
                Stop();
                return;
            }
            try
            {
                _scheduleList.AsParallel().ForAll(schedule =>
                {
                    schedule.Execute();
                });
            }
            catch (Exception ex)
            {
                _logger.Error("执行 TimerService.DoJob 出现异常", ex);
            }
            finally
            {
                //_innerTimer.Change(_timerInterval, Timeout.Infinite);
            }
        }

        public void AddScheduleJob(ScheduleJob job)
        {
            if (job == null)
            {
                _logger.Debug("添加计划任务失败，参数不正确");
                return;
            }
            _scheduleList.Add(job);
            Start();
        }

        public void RemoveScheduleJob(string identity)
        {
            var jobs = _scheduleList.Where(s => s.Identity == identity).ToList();
            if (jobs == null || jobs.Count() < 1) return;
            foreach (var job in jobs)
            {
                _scheduleList.Remove(job);
            }
        }

        #endregion
    }
}
