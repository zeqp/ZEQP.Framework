using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public interface ITreeRepository<T, K> : IBaseRepository<T, K>
        where T : class, ITreeEntity<K>
    {
        /// <summary>
        /// 获取全部下级实体
        /// </summary>
        /// <param name="parent">父实体</param>
        List<T> GetAllChildren(T parent);
        /// <summary>
        /// 获取全部下级实体
        /// </summary>
        /// <param name="parent">父实体</param>
        Task<List<T>> GetAllChildrenAsync(T parent);
    }
}
