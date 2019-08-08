using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 基类实体接口
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public interface IBaseEntity<K>
    {
        /// <summary>
        /// Id主键
        /// </summary>
        K Id { get; set; }
    }
    /// <summary>
    /// 自增基类实体接口
    /// </summary>
    public interface IBaseEntity : IBaseEntity<long>
    {
    }

    /// <summary>
    /// 基本类实体接口
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public interface IBusEntity<K> : IBaseEntity<K>
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
    public interface IBusEntity : IBusEntity<long>
    {
    }

    /// <summary>
    /// 树结构基类实体
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public interface ITreeEntity<K> : IBaseEntity<K>
    {
        /// <summary>
        /// 父节点Id
        /// </summary>
        K ParentId { get; set; }
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        bool IsLeaf { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        int Level { get; set; }
        /// <summary>
        /// 全路径
        /// </summary>
        string Path { get; set; }
        void InitPath(ITreeEntity<K> parent);
    }

    /// <summary>
    /// 树结构基类实体
    /// </summary>
    public interface ITreeEntity : ITreeEntity<long>
    {

    }
}
