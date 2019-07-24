using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
    }
    public class BaseEntity : BaseEntity<int>, IBaseEntity
    {
    }

    public class BusEntity<T> : BaseEntity<T>, IBusEntity<T>
    {
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public byte[] ModifyTime { get; set; }
    }
    public class BusEntity : BaseEntity, IBusEntity
    {
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public byte[] ModifyTime { get; set; }
    }
}
