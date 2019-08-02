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
    /// <summary>
    /// 基本仓储接口
    /// </summary>
    public interface IBaseRepository
    {
        #region Transaction
        /// <summary>
        /// 事务
        /// </summary>
        IDbContextTransaction Transaction { get; }
        /// <summary>
        /// 开始一个新事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 开始一个新事务
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync();
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="rollback"></param>
        void Commit(bool rollback = false);
        #endregion

        #region Queryable
        /// <summary>
        /// 当前数据库上下文
        /// </summary>
        DbContext Context { get; }
        /// <summary>
        /// 当前数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> Set<T>() where T : class;
        IQueryable<T> GetQueryable<T>() where T : class;
        IQueryable<T> GetQueryable<T>(params Expression<Func<T, object>>[] propertySelectors) where T : class;
        #endregion

        #region SaveChanges
        /// <summary>
        /// 保存数据变更
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 保存数据变更
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        #endregion

        #region Get
        /// <summary>
        /// 根据ID，拿到一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        T Get<T>(object id, bool track = true) where T : class;
        /// <summary>
        /// 根据ID，拿到一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键ID</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        Task<T> GetAsync<T>(object id, bool track = true) where T : class;
        /// <summary>
        /// 根据查询条件,拿到数据库中唯一的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        T Get<T>(Expression<Func<T, bool>> predicate, bool track = true) where T : class;
        /// <summary>
        /// 根据查询条件,拿到数据库中唯一的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据库实体</returns>
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, bool track = true) where T : class;

        #region GetModel
        M GetModel<T, M>(object id) where T : class;
        Task<M> GetModelAsync<T, M>(object id) where T : class;
        M GetModel<T, M>(Expression<Func<T, bool>> predicate) where T : class;
        Task<M> GetModelAsync<T, M>(Expression<Func<T, bool>> predicate) where T : class;
        #endregion

        #endregion

        #region GetList
        /// <summary>
        /// 拿到所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="track">是否跟踪</param>
        /// <returns>所有数据</returns>
        List<T> GetAll<T>(bool track = true) where T : class;
        /// <summary>
        /// 拿到所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="track">是否跟踪</param>
        /// <returns>所有数据</returns>
        Task<List<T>> GetAllAsync<T>(bool track = true) where T : class;
        /// <summary>
        /// 根据ID集合，拿到所有ID数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="K">主键类型</typeparam>
        /// <param name="ids"></param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        List<T> GetList<T, K>(List<K> ids, bool track = true) where T : class;
        /// <summary>
        /// 根据ID集合，拿到所有ID数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="K">主键类型</typeparam>
        /// <param name="ids"></param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync<T, K>(List<K> ids, bool track = true) where T : class;
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        List<T> GetList<T>(Expression<Func<T, bool>> predicate, bool track = true) where T : class;
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <param name="track">是否跟踪</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate, bool track = true) where T : class;
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <returns>数据集合</returns>
        List<T> GetList<T>(IQueryable<T> queryable) where T : class;
        /// <summary>
        /// 根据相询条件，拿到数据集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">相询条件</param>
        /// <returns>数据集合</returns>
        Task<List<T>> GetListAsync<T>(IQueryable<T> queryable) where T : class;

        #region GetListModel
        List<M> GetAllModel<T, M>() where T : class;
        Task<List<M>> GetAllModelAsync<T, M>() where T : class;
        List<M> GetListModel<T, M, K>(List<K> ids) where T : class;
        Task<List<M>> GetListModelAsync<T, M, K>(List<K> ids) where T : class;
        List<M> GetListModel<T, M>(Expression<Func<T, bool>> predicate) where T : class;
        Task<List<M>> GetListModelAsync<T, M>(Expression<Func<T, bool>> predicate) where T : class;
        List<M> GetListModel<T, M>(IQueryable<T> queryable) where T : class;
        Task<List<M>> GetListModelAsync<T, M>(IQueryable<T> queryable) where T : class;
        #endregion

        #endregion

        #region GetPage
        PageResult<T> GetPage<T, Q>(PageQuery<Q> query) where T : class where Q : class, new();
        Task<PageResult<T>> GetPageAsync<T, Q>(PageQuery<Q> query) where T : class where Q : class, new();

        #region GetPageModel
        PageResult<M> GetPageModel<T, Q, M>(PageQuery<Q> query) where T : class where Q : class, new();
        Task<PageResult<M>> GetPageModelAsync<T, Q, M>(PageQuery<Q> query) where T : class where Q : class, new();
        #endregion

        #endregion

        #region Add
        bool Add<T>(T entity, bool save = true) where T : class;
        bool Add<T>(List<T> list, bool save = true) where T : class;
        Task<bool> AddAsync<T>(T entity, bool save = true) where T : class;
        Task<bool> AddAsync<T>(List<T> list, bool save = true) where T : class;
        bool AddOrUpdate<T>(T entity, bool save = true) where T : class;
        Task<bool> AddOrUpdateAsync<T>(T entity, bool save = true) where T : class;

        #region AddModel
        bool AddModel<T, M>(M model) where T : class;
        bool AddModel<T, M>(List<M> list) where T : class;
        Task<bool> AddModelAsync<T, M>(M model) where T : class;
        Task<bool> AddModelAsync<T, M>(List<M> list) where T : class;
        bool AddOrUpdateModel<T, M>(M model) where T : class;
        Task<bool> AddOrUpdateModelAsync<T, M>(M model) where T : class;
        #endregion

        #endregion

        #region Update
        bool Update<T>(T entity, bool save = true, List<string> props = null) where T : class;
        bool Update<T>(List<T> list, bool save = true, List<string> props = null) where T : class;
        Task<bool> UpdateAsync<T>(T entity, bool save = true, List<string> props = null) where T : class;
        Task<bool> UpdateAsync<T>(List<T> list, bool save = true, List<string> props = null) where T : class;
        bool Update<T>(Expression<Func<T, bool>> where, Action<T> action) where T : class;
        Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> where, Action<T> action) where T : class;

        #region UpdateModel
        bool Update<T, M>(M model) where T : class;
        bool Update<T, M>(List<M> list) where T : class;
        Task<bool> UpdateAsync<T, M>(M model) where T : class;
        Task<bool> UpdateAsync<T, M>(List<M> list) where T : class;
        #endregion

        #endregion

        #region Delete
        bool Delete<T>(object id) where T : class;
        bool Delete<T, K>(List<K> ids) where T : class;
        Task<bool> DeleteAsync<T>(object id) where T : class;
        Task<bool> DeleteAsync<T, K>(List<K> ids) where T : class;
        bool Delete<T>(T entity, bool save = true) where T : class;
        bool Delete<T>(List<T> list, bool save = true) where T : class;
        Task<bool> DeleteAsync<T>(T entity, bool save = true) where T : class;
        Task<bool> DeleteAsync<T>(List<T> list, bool save = true) where T : class;
        bool Delete<T>(Expression<Func<T, bool>> where) where T : class;
        Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> where) where T : class;
        #endregion

        #region Map
        void Map<From, To>(From source, To model);
        TOut Map<TIn, TOut>(TIn source);
        List<TOut> Map<TIn, TOut>(List<TIn> source);
        List<TOut> Map<TIn, TOut>(IQueryable<TIn> queryable);
        #endregion
    }

    public interface IBaseRepository<T, K> : IBaseRepository
        where T : IBaseEntity<K>
    {

    }
    public interface IBaseRepository<T> : IBaseRepository<T, long>
        where T : IBaseEntity
    {
    }
}
