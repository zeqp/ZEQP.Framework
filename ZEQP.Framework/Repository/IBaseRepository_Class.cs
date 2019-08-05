using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public interface IBaseRepository<T, K> : IBaseRepository
        where T : class
    {
        #region Queryable
        /// <summary>
        /// 当前数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> Set();
        IQueryable<T> GetQueryable(bool track = true);
        IQueryable<T> GetQueryable(bool track = true,params Expression<Func<T, object>>[] propertySelectors);
        #endregion

        #region Get
        /// <summary>
        /// 根据ID，拿到一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        T Get(object id, bool track = true);
        /// <summary>
        /// 根据ID，拿到一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        Task<T> GetAsync(object id, bool track = true);
        /// <summary>
        /// 根据查询条件,拿到数据库中唯一的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        T Get(Expression<Func<T, bool>> predicate, bool track = true);
        /// <summary>
        /// 根据查询条件,拿到数据库中唯一的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = true);

        #region GetModel
        M GetModel<M>(object id);
        Task<M> GetModelAsync<M>(object id);
        M GetModel<M>(Expression<Func<T, bool>> predicate);
        Task<M> GetModelAsync<M>(Expression<Func<T, bool>> predicate);
        #endregion

        #endregion

        #region GetList
        /// <summary>
        /// 拿到所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="track">是否跟踪</param>
        /// <returns>所有数据</returns>
        List<T> GetAll(bool track = true);
        /// <summary>
        /// 拿到所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="track">是否跟踪</param>
        /// <returns>所有数据</returns>
        Task<List<T>> GetAllAsync(bool track = true);
        /// <summary>
        /// 根据ID集合，拿到所有ID数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="K">主键类型</typeparam>
        /// <param name="ids"></param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        List<T> GetList(List<K> ids, bool track = true);
        /// <summary>
        /// 根据ID集合，拿到所有ID数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="K">主键类型</typeparam>
        /// <param name="ids"></param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync(List<K> ids, bool track = true);
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        List<T> GetList(Expression<Func<T, bool>> predicate, bool track = true);
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, bool track = true);
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <returns>数据集合</returns>
        List<T> GetList(IQueryable<T> queryable);
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync(IQueryable<T> queryable);

        #region GetListModel
        List<M> GetAllModel<M>();
        Task<List<M>> GetAllModelAsync<M>();
        List<M> GetListModel<M>(List<K> ids);
        Task<List<M>> GetListModelAsync<M>(List<K> ids);
        List<M> GetListModel<M>(Expression<Func<T, bool>> predicate);
        Task<List<M>> GetListModelAsync<M>(Expression<Func<T, bool>> predicate);
        List<M> GetListModel<M>(IQueryable<T> queryable);
        Task<List<M>> GetListModelAsync<M>(IQueryable<T> queryable);
        #endregion

        #endregion

        #region GetPage
        PageResult<T> GetPage<Q>(PageQuery<Q> query) where Q : class, new();
        Task<PageResult<T>> GetPageAsync<Q>(PageQuery<Q> query) where Q : class, new();

        #region GetPageModel
        PageResult<M> GetPageModel<Q, M>(PageQuery<Q> query) where Q : class, new();
        Task<PageResult<M>> GetPageModelAsync<Q, M>(PageQuery<Q> query) where Q : class, new();
        #endregion

        #endregion

        #region Add
        bool Add(T entity, bool save = true);
        bool Add(List<T> list, bool save = true);
        Task<bool> AddAsync(T entity, bool save = true);
        Task<bool> AddAsync(List<T> list, bool save = true);
        bool AddOrUpdate(T entity, bool save = true);
        Task<bool> AddOrUpdateAsync(T entity, bool save = true);

        #region AddModel
        bool AddModel<M>(M model);
        bool AddModel<M>(List<M> list);
        Task<bool> AddModelAsync<M>(M model);
        Task<bool> AddModelAsync<M>(List<M> list);
        bool AddOrUpdateModel<M>(M model);
        Task<bool> AddOrUpdateModelAsync<M>(M model);
        #endregion

        #endregion

        #region Update
        bool Update(T entity, bool save = true, List<string> props = null);
        bool Update(List<T> list, bool save = true, List<string> props = null);
        Task<bool> UpdateAsync(T entity, bool save = true, List<string> props = null);
        Task<bool> UpdateAsync(List<T> list, bool save = true, List<string> props = null);
        bool Update(Expression<Func<T, bool>> where, Action<T> action);
        Task<bool> UpdateAsync(Expression<Func<T, bool>> where, Action<T> action);

        #region UpdateModel
        bool Update<M>(M model);
        bool Update<M>(List<M> list);
        Task<bool> UpdateAsync<M>(M model);
        Task<bool> UpdateAsync<M>(List<M> list);
        #endregion

        #endregion

        #region Delete
        bool Delete(object id);
        bool Delete(List<K> ids);
        Task<bool> DeleteAsync(object id);
        Task<bool> DeleteAsync(List<K> ids);
        bool Delete(T entity, bool save = true);
        bool Delete(List<T> list, bool save = true);
        Task<bool> DeleteAsync(T entity, bool save = true);
        Task<bool> DeleteAsync(List<T> list, bool save = true);
        bool Delete(Expression<Func<T, bool>> where);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);
        #endregion

        #region Map
        TOut Map<TOut>(T source);
        List<TOut> Map<TOut>(List<T> source);
        List<TOut> Map<TOut>(IQueryable<T> queryable);
        #endregion
    }
}
