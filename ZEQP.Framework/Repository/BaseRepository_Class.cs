using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public class BaseRepository<T, K> : BaseRepository, IBaseRepository<T, K>
        where T : class
    {
        public BaseRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }

        #region Queryable
        public DbSet<T> Set()
        {
            return base.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return base.GetQueryable<T>();
        }

        public IQueryable<T> GetQueryable(params Expression<Func<T, object>>[] propertySelectors)
        {
            return base.GetQueryable<T>(propertySelectors);
        }
        #endregion

        #region Get
        public T Get(object id, bool track = true)
        {
            return base.Get<T>(id, track);
        }

        public T Get(Expression<Func<T, bool>> predicate, bool track = true)
        {
            return base.Get<T>(predicate, track);
        }

        public Task<T> GetAsync(object id, bool track = true)
        {
            return base.GetAsync<T>(id, track);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = true)
        {
            return base.GetAsync<T>(predicate, track);
        }

        #region GetModel
        public M GetModel<M>(object id)
        {
            return base.GetModel<T, M>(id);
        }

        public M GetModel<M>(Expression<Func<T, bool>> predicate)
        {
            return base.GetModel<T, M>(predicate);
        }

        public Task<M> GetModelAsync<M>(object id)
        {
            return base.GetModelAsync<T, M>(id);
        }

        public Task<M> GetModelAsync<M>(Expression<Func<T, bool>> predicate)
        {
            return base.GetModelAsync<T, M>(predicate);
        }
        #endregion

        #endregion

        #region GetAll
        public List<T> GetAll(bool track = true)
        {
            return base.GetAll<T>(track);
        }

        public Task<List<T>> GetAllAsync(bool track = true)
        {
            return base.GetAllAsync<T>(track);
        }

        public List<M> GetAllModel<M>()
        {
            return base.GetAllModel<T, M>();
        }

        public Task<List<M>> GetAllModelAsync<M>()
        {
            return base.GetAllModelAsync<T, M>();
        }
        #endregion

        #region GetList
        public List<T> GetList(List<K> ids, bool track = true)
        {
            return base.GetList<T, K>(ids, track);
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate, bool track = true)
        {
            return base.GetList<T>(predicate, track);
        }

        public List<T> GetList(IQueryable<T> queryable)
        {
            return base.GetList<T>(queryable);
        }

        public Task<List<T>> GetListAsync(List<K> ids, bool track = true)
        {
            return base.GetListAsync<T, K>(ids, track);
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, bool track = true)
        {
            return base.GetListAsync<T>(predicate, track);
        }

        public Task<List<T>> GetListAsync(IQueryable<T> queryable)
        {
            return base.GetListAsync<T>(queryable);
        }

        #region GetListModel
        public List<M> GetListModel<M>(List<K> ids)
        {
            var listEntity = this.GetList(ids, false);
            return this.Map<M>(listEntity);
        }

        public List<M> GetListModel<M>(Expression<Func<T, bool>> predicate)
        {
            return base.GetListModel<T, M>(predicate);
        }

        public List<M> GetListModel<M>(IQueryable<T> queryable)
        {
            return base.GetListModel<T, M>(queryable);
        }

        public async Task<List<M>> GetListModelAsync<M>(List<K> ids)
        {
            var listEntity = await this.GetListAsync<T, K>(ids, false);
            return this.Map<M>(listEntity);
        }

        public Task<List<M>> GetListModelAsync<M>(Expression<Func<T, bool>> predicate)
        {
            return base.GetListModelAsync<T, M>(predicate);
        }

        public Task<List<M>> GetListModelAsync<M>(IQueryable<T> queryable)
        {
            return base.GetListModelAsync<T, M>(queryable);
        }
        #endregion

        #endregion

        #region GetPage
        public PageResult<T> GetPage<Q>(PageQuery<Q> query) where Q : class, new()
        {
            return base.GetPage<T, Q>(query);
        }

        public Task<PageResult<T>> GetPageAsync<Q>(PageQuery<Q> query) where Q : class, new()
        {
            return base.GetPageAsync<T, Q>(query);
        }
        #region GetPageModel
        public PageResult<M> GetPageModel<Q, M>(PageQuery<Q> query) where Q : class, new()
        {
            return base.GetPageModel<T, Q, M>(query);
        }

        public Task<PageResult<M>> GetPageModelAsync<Q, M>(PageQuery<Q> query) where Q : class, new()
        {
            return base.GetPageModelAsync<T, Q, M>(query);
        }
        #endregion

        #endregion

        #region Add
        public bool Add(T entity, bool save = true)
        {
            return base.Add<T>(entity, save);
        }

        public bool Add(List<T> list, bool save = true)
        {
            return base.Add<T>(list, save);
        }

        public Task<bool> AddAsync(T entity, bool save = true)
        {
            return base.AddAsync<T>(entity, save);
        }

        public Task<bool> AddAsync(List<T> list, bool save = true)
        {
            return base.AddAsync<T>(list, save);
        }

        public bool AddOrUpdate(T entity, bool save = true)
        {
            return base.AddOrUpdate<T>(entity, save);
        }

        public Task<bool> AddOrUpdateAsync(T entity, bool save = true)
        {
            return base.AddOrUpdateAsync<T>(entity, save);
        }
        #region AddMoel
        public bool AddModel<M>(M model)
        {
            return base.AddModel<T, M>(model);
        }

        public bool AddModel<M>(List<M> list)
        {
            return base.AddModel<T, M>(list);
        }

        public Task<bool> AddModelAsync<M>(M model)
        {
            return base.AddModelAsync<T, M>(model);
        }

        public Task<bool> AddModelAsync<M>(List<M> list)
        {
            return base.AddModelAsync<T, M>(list);
        }

        public bool AddOrUpdateModel<M>(M model)
        {
            return base.AddOrUpdateModel<T, M>(model);
        }

        public Task<bool> AddOrUpdateModelAsync<M>(M model)
        {
            return base.AddOrUpdateModelAsync<T, M>(model);
        }
        #endregion
        #endregion

        #region Delete
        public bool Delete(object id)
        {
            return base.Delete<T>(id);
        }

        public bool Delete(List<K> ids)
        {
            var list = this.GetList(ids);
            return this.Delete(list);
        }

        public bool Delete(T entity, bool save = true)
        {
            return base.Delete<T>(entity, save);
        }

        public bool Delete(List<T> list, bool save = true)
        {
            return base.Delete<T>(list, save);
        }

        public bool Delete(Expression<Func<T, bool>> where)
        {
            return base.Delete<T>(where);
        }

        public Task<bool> DeleteAsync(object id)
        {
            return base.DeleteAsync<T>(id);
        }

        public async Task<bool> DeleteAsync(List<K> ids)
        {
            var list = await this.GetListAsync(ids);
            return await this.DeleteAsync(list);
        }

        public Task<bool> DeleteAsync(T entity, bool save = true)
        {
            return base.DeleteAsync<T>(entity, save);
        }

        public Task<bool> DeleteAsync(List<T> list, bool save = true)
        {
            return base.DeleteAsync<T>(list, save);
        }

        public Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return base.DeleteAsync<T>(where);
        }
        #endregion

        #region Update
        public bool Update(T entity, bool save = true, List<string> props = null)
        {
            return base.Update<T>(entity, save, props);
        }

        public bool Update(List<T> list, bool save = true, List<string> props = null)
        {
            return base.Update<T>(list, save, props);
        }

        public bool Update(Expression<Func<T, bool>> where, Action<T> action)
        {
            return base.Update<T>(where, action);
        }
        public Task<bool> UpdateAsync(T entity, bool save = true, List<string> props = null)
        {
            return base.UpdateAsync<T>(entity, save, props);
        }

        public Task<bool> UpdateAsync(List<T> list, bool save = true, List<string> props = null)
        {
            return base.UpdateAsync<T>(list, save, props);
        }

        public Task<bool> UpdateAsync(Expression<Func<T, bool>> where, Action<T> action)
        {
            return base.UpdateAsync<T>(where, action);
        }
        #region UpdateModel
        public bool Update<M>(M model)
        {
            return base.Update<T, M>(model);
        }

        public bool Update<M>(List<M> list)
        {
            return base.Update<T, M>(list);
        }
        public Task<bool> UpdateAsync<M>(M model)
        {
            return base.UpdateAsync<T, M>(model);
        }

        public Task<bool> UpdateAsync<M>(List<M> list)
        {
            return base.UpdateAsync<T, M>(list);
        }
        #endregion
        #endregion

        #region Map
        public TOut Map<TOut>(T source)
        {
            return base.Map<T, TOut>(source);
        }

        public List<TOut> Map<TOut>(List<T> source)
        {
            return base.Map<T, TOut>(source);
        }

        public List<TOut> Map<TOut>(IQueryable<T> queryable)
        {
            return base.Map<T, TOut>(queryable);
        }
        #endregion
    }
}
