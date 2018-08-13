using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace YunPos.MessageService.Host.Settings
{
    /// <summary>
    /// 服务设置
    /// </summary>
    public class ServiceSettings
    {
        #region fields

        const string _configFileName = "ServiceSettings.sets";

        #endregion

        #region ctor.

        private ServiceSettings()
        {
            ServiceName = "Yun.MessageService";
            YunService = "http://127.0.0.1:80";
            TimerInterval = 5;
            LauncherItems = new List<LauncherItem>();
        }

        #endregion

        #region properties
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 云服务地址
        /// </summary>
        public string YunService { get; set; }

        /// <summary>
        /// 统一定时器间隔，单位为分钟
        /// </summary>
        public int TimerInterval { get; set; }

        /// <summary>
        /// 启动项
        /// </summary>
        public IList<LauncherItem> LauncherItems { get; set; }
        #endregion

        #region methods

        /// <summary>
        /// 加载 ServiceSettings
        /// </summary>
        /// <returns></returns>
        public static ServiceSettings LoadSettings()
        {
            var settings = new ServiceSettings();
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configFileName);
                string jsonStr = File.ReadAllText(filePath, Encoding.UTF8);
                CoreService.Logger.InfoFormat("ServiceSettingsJsonData:[{0}]", jsonStr);
                if (string.IsNullOrEmpty(jsonStr))
                    return settings;
                return JsonConvert.DeserializeObject<ServiceSettings>(jsonStr);
            }
            catch (Exception ex)
            {
                CoreService.Logger.Error("记载配置文件出错。", ex);
                return settings;
            }
        }
        #endregion
    }

    /// <summary>
    /// 启动项
    /// </summary>
    public class LauncherItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Args { get; set; }
    }
}
