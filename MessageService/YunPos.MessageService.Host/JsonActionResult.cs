using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunPos.MessageService.Host
{
    /// <summary>
    /// 适用于WebApi的统一格式的ActionResult
    /// </summary>
    /// <typeparam name="T">返回结果数据类型</typeparam>
    public class JsonActionResult<T>
    {
        private bool _isSuccessful = true;
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccessful
        {
            get { return this._isSuccessful; }
            set { this._isSuccessful = value; }
        }

        /// <summary>
        /// 返回结果，如果<see cref="IsSuccessful"/>值为<see cref="true"/>时有值
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 错误信息，如果<see cref="IsSuccessful"/>值为<see cref="false"/>时有值
        /// </summary>
        public String ErrorMessage { get; set; }
    }
}
