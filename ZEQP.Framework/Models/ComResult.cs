using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 公共反回实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComResult<T>
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 公共反回实体
    /// </summary>
    public class ComResult : ComResult<string>
    { }
}
