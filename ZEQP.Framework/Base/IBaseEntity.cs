using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 基类实体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseEntity<T>
    {
        /// <summary>
        /// Id主键
        /// </summary>
        T Id { get; set; }
    }
    /// <summary>
    /// 自增基类实体接口
    /// </summary>
    public interface IBaseEntity : IBaseEntity<int>
    {
    }

    /// <summary>
    /// 基本类实体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBusEntity<T> : IBaseEntity<T>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        bool Deleted { get; set; }
        /// <summary>
        /// 修改时间/时间戳/行版本
        /// </summary>
        DateTime ModifyTime { get; set; }
    }
    /// <summary>
    /// 自增基本类实体接口
    /// </summary>
    public interface IBusEntity : IBusEntity<int>
    {
    }
}
