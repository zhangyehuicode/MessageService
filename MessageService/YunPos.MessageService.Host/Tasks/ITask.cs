using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 定义一个任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 执行
        /// </summary>
        void Start(string args);

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
    }
}
