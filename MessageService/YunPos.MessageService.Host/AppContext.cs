using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunPos.MessageService.Host.Settings;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 当前App运行上下文
    /// </summary>
    public static class AppContext
    {
        #region fields
        static ServiceSettings _settings;
        #endregion

        #region ctor.
        static AppContext()
        {
            _settings = ServiceSettings.LoadSettings();
        }
        #endregion

        #region properties
        /// <summary>
        /// 配置信息
        /// </summary>
        public static ServiceSettings Settings
        {
            get { return _settings; }
        }
        #endregion
    }
}
