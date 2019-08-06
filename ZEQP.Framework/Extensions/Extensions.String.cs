using System;

namespace ZEQP.Framework
{
    /// <summary>
    /// 系统扩展 - 字符串
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="removeValue">要移除的值</param>
        public static string RemoveEnd(this string value, string removeValue)
        {
            return StringHelper.RemoveEnd(value, removeValue);
        }
    }
}
