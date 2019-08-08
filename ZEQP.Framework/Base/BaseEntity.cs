using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class BaseEntity<K> : IBaseEntity<K>
    {
        public K Id { get; set; }
    }
    /// <summary>
    /// 实体基类
    /// </summary>
    public class BaseEntity : BaseEntity<long>, IBaseEntity
    {
    }

    /// <summary>
    /// 基本实体类
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class BusEntity<K> : BaseEntity<K>, IBusEntity<K>
    {
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime ModifyTime { get; set; }
    }
    /// <summary>
    /// 基本实体类
    /// </summary>
    public class BusEntity : BusEntity<long>, IBusEntity
    {

    }

    /// <summary>
    /// 树结构基类实体
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class TreeEntity<K> : BaseEntity<K>, ITreeEntity<K>
    {
        public K ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public int Level { get; set; }
        public string Path { get; set; }
    }
    /// <summary>
    /// 树结构基类实体
    /// </summary>
    public class TreeEntity : TreeEntity<long>, ITreeEntity
    {

    }
}
