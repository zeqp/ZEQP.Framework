using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
    }
    /// <summary>
    /// 自增实体基类
    /// </summary>
    public class BaseEntity : BaseEntity<int>, IBaseEntity
    {
    }

    /// <summary>
    /// 基本实体类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusEntity<T> : BaseEntity<T>, IBusEntity<T>
    {
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime ModifyTime { get; set; }
    }
    /// <summary>
    /// 自增基本实体类
    /// </summary>
    public class BusEntity : BaseEntity, IBusEntity
    {
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
