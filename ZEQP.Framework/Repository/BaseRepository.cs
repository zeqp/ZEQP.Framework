using AutoMapper;
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
    public class BaseRepository : IBaseRepository
    {
        public DbContext Context { get; }
        public IMapper Mapper { get; set; }
        public BaseRepository(DbContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        #region Queryable
        public virtual DbSet<T> Set<T>() where T : class => this.Context.Set<T>();
        public virtual IQueryable<T> GetQueryable<T>()
            where T : class
        {
            return this.Set<T>();
        }
        public virtual IQueryable<T> GetQueryable<T>(params Expression<Func<T, object>>[] propertySelectors)
            where T : class
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return this.GetQueryable<T>();
            }
            var query = this.GetQueryable<T>();
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
            return query;
        }
        #endregion

        #region Transaction
        public virtual IDbContextTransaction Transaction => this.Context.Database.CurrentTransaction;
        public virtual void BeginTransaction() => this.Context.Database.BeginTransaction();
        public virtual Task<IDbContextTransaction> BeginTransactionAsync() => this.Context.Database.BeginTransactionAsync();
        public virtual void Commit(bool rollback = false)
        {
            if (!rollback)
            {
                this.SaveChanges();
                this.Transaction?.Commit();
                return;
            }
            this.Transaction?.Rollback();
        }
        #endregion

        #region SaveChanges
        public virtual int SaveChanges() => this.Context.SaveChanges();
        public virtual Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();
        #endregion

        #region Get
        public virtual T Get<T>(object id, bool track = true)
            where T : class
        {
            if (!track)
                this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var entity = this.Set<T>().Find(id);
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return entity;
        }
        public virtual async Task<T> GetAsync<T>(object id, bool track = true)
            where T : class
        {
            if (!track)
                this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var entity = await this.Set<T>().FindAsync(id);
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return entity;
        }
        public virtual T Get<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).SingleOrDefault();
            return this.Set<T>().AsNoTracking().Where(predicate).SingleOrDefault();
        }
        public virtual Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).SingleOrDefaultAsync();
            return this.Set<T>().AsNoTracking().Where(predicate).SingleOrDefaultAsync();
        }

        #region GetModel
        public virtual M GetModel<T, M>(object id)
            where T : class
        {
            var entity = this.Get<T>(id, false);
            return this.Map<T, M>(entity);
        }
        public virtual async Task<M> GetModelAsync<T, M>(object id)
            where T : class
        {
            var entity = await this.GetAsync<T>(id, false);
            return this.Map<T, M>(entity);
        }
        public virtual M GetModel<T, M>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var entity = this.Get<T>(predicate, false);
            return this.Map<T, M>(entity);
        }
        public virtual async Task<M> GetModelAsync<T, M>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var entity = await this.GetAsync<T>(predicate, false);
            return this.Map<T, M>(entity);
        }
        #endregion

        #endregion

        #region GetList
        public virtual List<T> GetAll<T>(bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().ToList();
            return this.Set<T>().AsNoTracking().ToList();
        }
        public virtual Task<List<T>> GetAllAsync<T>(bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().ToListAsync();
            return this.Set<T>().AsNoTracking().ToListAsync();
        }
        public virtual List<T> GetList<T, K>(List<K> ids, bool track = true)
            where T : class
        {
            var keyProp = this.GetKeyProperty<T>();
            var keyName = keyProp.Select(x => x.Name).Single();
            var right = Expression.Constant(ids);
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, ids.GetType().GetMethod("Contains"), left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetList<T>(pred, track);
        }
        public virtual Task<List<T>> GetListAsync<T, K>(List<K> ids, bool track = true)
            where T : class
        {
            var keyProp = this.GetKeyProperty<T>();
            var keyName = keyProp.Select(x => x.Name).Single();
            var right = Expression.Constant(ids);

            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, ids.GetType().GetMethod("Contains"), left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetListAsync<T>(pred, track);
        }
        public virtual List<T> GetList<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).ToList();
            return this.Set<T>().AsNoTracking().Where(predicate).ToList();
        }
        public virtual Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).ToListAsync();
            return this.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual List<T> GetList<T>(IQueryable<T> queryable)
            where T : class
        {
            return queryable.ToList();
        }
        public virtual Task<List<T>> GetListAsync<T>(IQueryable<T> queryable)
            where T : class
        {
            return queryable.ToListAsync();
        }

        #region GetListModel
        public virtual List<M> GetAllModel<T, M>()
            where T : class
        {
            var list = this.GetAll<T>(false);
            return this.Map<T, M>(list);
        }
        public virtual async Task<List<M>> GetAllModelAsync<T, M>()
            where T : class
        {
            var list = await this.GetAllAsync<T>(false);
            return this.Map<T, M>(list);
        }
        public virtual List<M> GetListModel<T, M, K>(List<K> ids)
            where T : class
        {
            var list = this.GetList<T, K>(ids, false);
            return this.Map<T, M>(list);
        }
        public virtual async Task<List<M>> GetListModelAsync<T, M, K>(List<K> ids)
            where T : class
        {
            var list = await this.GetListAsync<T, K>(ids, false);
            return this.Map<T, M>(list);
        }
        public virtual List<M> GetListModel<T, M>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var list = this.GetList<T>(predicate, false);
            return this.Map<T, M>(list);
        }
        public virtual async Task<List<M>> GetListModelAsync<T, M>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var list = await this.GetListAsync<T>(predicate, false);
            return this.Map<T, M>(list);
        }
        public virtual List<M> GetListModel<T, M>(IQueryable<T> queryable)
            where T : class
        {
            var list = this.GetList<T>(queryable);
            return this.Map<T, M>(list);
        }
        public virtual async Task<List<M>> GetListModelAsync<T, M>(IQueryable<T> queryable)
            where T : class
        {
            var list = await this.GetListAsync<T>(queryable);
            return this.Map<T, M>(list);
        }
        #endregion

        #endregion

        #region GetPage
        protected virtual IQueryable<T> GetPageQueryable<T, Q>(PageQuery<Q> query)
            where T : class
            where Q : class, new()
        {
            var queryable = this.Set<T>().AsNoTracking().AsQueryable();
            var model = query.Query;
            var outer = PredicateBuilder.True<T>();
            var entityProps = typeof(T).GetProps();
            if (model != null)
            {
                var listProp = model.GetType().GetProps();
                var listEntityProp = entityProps.Select(s => s.Name).ToList();
                var param = Expression.Parameter(typeof(T), "x");
                if (!String.IsNullOrEmpty(query.Match))
                {
                    var nameOrCodeProp = listProp.Where(w => w.Name.EndsWith("Name") || w.Name.EndsWith("Code")).ToList();
                    var inner = PredicateBuilder.False<T>();
                    foreach (var prop in nameOrCodeProp)
                    {
                        //是否存再此字段，只有存再的字段才能做模糊查询
                        if (!listEntityProp.Contains(prop.Name)) continue;
                        //判断是否在查询实体里已经做了查询，如果查询实体里有值。就不用做模糊查询了
                        if (!prop.IsDefault(model)) continue;
                        Expression left = Expression.Property(param, prop.Name);
                        var right = Expression.Constant(query.Match, prop.PropertyType);
                        //如果是可空类型，要转成对应的非空类型
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            right = Expression.Constant(query.Match, Nullable.GetUnderlyingType(prop.PropertyType));
                        Expression filter = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
                        Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                        inner = inner.Or(pred);
                    }
                    outer = outer.And(inner);
                }
                foreach (var prop in listProp)
                {
                    if (prop.IsDefault(model)) continue;
                    var propVal = prop.GetValue(model);
                    var right = Expression.Constant(propVal, prop.PropertyType);
                    //如果是可空类型，要转成对应的非空类型
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        right = Expression.Constant(propVal, Nullable.GetUnderlyingType(prop.PropertyType));
                    var propType = prop.PropertyType;
                    if (propType == typeof(string))
                    {
                        if (!listEntityProp.Contains(prop.Name)) continue;
                        Expression left = Expression.Property(param, prop.Name);
                        var propValStr = propVal.ToString();
                        Expression filter = Expression.Equal(left, right);
                        if (propValStr.StartsWith("%") || propValStr.EndsWith("%"))
                        {
                            propValStr = propValStr.Replace("%", "");
                            right = Expression.Constant(propValStr, prop.PropertyType);
                            filter = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
                        }
                        Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                        outer = outer.And(pred);
                    }
                    else if (prop.Name.Contains("Start") || prop.Name.Contains("Begin"))
                    {
                        if (listEntityProp.Contains(prop.Name))
                        {
                            Expression left = Expression.Property(param, prop.Name);
                            Expression filter = Expression.GreaterThanOrEqual(left, right);
                            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                            outer = outer.And(pred);
                        }
                        else if (listEntityProp.Contains(prop.Name.Replace("Start", "")))
                        {
                            var left = Expression.Property(param, prop.Name.Replace("Start", ""));
                            Expression filter = Expression.GreaterThanOrEqual(left, right);
                            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                            outer = outer.And(pred);
                        }
                        else if (listEntityProp.Contains(prop.Name.Replace("Begin", "")))
                        {
                            var left = Expression.Property(param, prop.Name.Replace("Begin", ""));
                            Expression filter = Expression.GreaterThanOrEqual(left, right);
                            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                            outer = outer.And(pred);
                        }
                    }
                    else if (prop.Name.Contains("End"))
                    {
                        if (listEntityProp.Contains(prop.Name))
                        {
                            Expression left = Expression.Property(param, prop.Name);
                            Expression filter = Expression.LessThanOrEqual(left, right);
                            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                            outer = outer.And(pred);
                        }
                        else if (listEntityProp.Contains(prop.Name.Replace("End", "")))
                        {
                            var left = Expression.Property(param, prop.Name.Replace("End", ""));
                            Expression filter = Expression.LessThanOrEqual(left, right);
                            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                            outer = outer.And(pred);
                        }
                    }
                    else if (propType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && prop.Name.Contains("List"))
                    {
                        if (!listEntityProp.Contains(prop.Name.Replace("List", ""))) continue;
                        var left = Expression.Property(param, prop.Name.Replace("List", ""));
                        Expression filter = Expression.Call(right, propType.GetMethod("Contains"), left);
                        Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                        outer = outer.And(pred);
                    }
                    else
                    {
                        if (!listEntityProp.Contains(prop.Name)) continue;
                        Expression left = Expression.Property(param, prop.Name);
                        Expression filter = Expression.Equal(left, right);
                        Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
                        outer = outer.And(pred);
                    }
                }
            }
            queryable = queryable.Where(outer);
            return queryable;
        }
        public virtual PageResult<T> GetPage<T, Q>(PageQuery<Q> query)
            where T : class
            where Q : class, new()
        {
            var queryable = this.GetPageQueryable<T, Q>(query);
            var result = new PageResult<T>();
            result.Count = queryable.Count();
            var totalPage = result.Count % query.Size == 0 ? (result.Count / query.Size) : (result.Count / query.Size + 1);
            if (query.Page > totalPage) query.Page = totalPage;
            if (query.Page <= 0) query.Page = 1;
            if (query.Size <= 0 || query.Size > 100) query.Size = 100;
            if (String.IsNullOrEmpty(query.Order)) query.Order = this.GetKeyProperty<T>().Select(s => s.Name).Single();
            if (String.IsNullOrEmpty(query.Sort)) query.Sort = "AES";
            result.Page = query.Page;
            var sortingDir = "OrderBy";
            if (!query.Sort.Equals("AES", StringComparison.CurrentCultureIgnoreCase))
                sortingDir = "OrderByDescending";

            ParameterExpression param1 = Expression.Parameter(typeof(T), query.Order);
            var pi = typeof(T).GetProperty(query.Order);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, queryable.Expression, Expression.Lambda(Expression.Property(param1, query.Order), param1));
            queryable = queryable.Provider.CreateQuery<T>(expr);

            var skipCount = (query.Page - 1) * query.Size;
            queryable = queryable.Skip(skipCount).Take(query.Size);
            result.Data = queryable.ToList();
            return result;
        }
        public virtual async Task<PageResult<T>> GetPageAsync<T, Q>(PageQuery<Q> query)
            where T : class
            where Q : class, new()
        {
            var queryable = this.GetPageQueryable<T, Q>(query);
            var result = new PageResult<T>();
            result.Count = await queryable.CountAsync();
            var totalPage = result.Count % query.Size == 0 ? (result.Count / query.Size) : (result.Count / query.Size + 1);
            if (query.Page > totalPage) query.Page = totalPage;
            if (query.Page <= 0) query.Page = 1;
            if (query.Size <= 0 || query.Size > 100) query.Size = 100;
            if (String.IsNullOrEmpty(query.Order)) query.Order = this.GetKeyProperty<T>().Select(s => s.Name).Single();
            if (String.IsNullOrEmpty(query.Sort)) query.Sort = "AES";
            result.Page = query.Page;
            var sortingDir = "OrderBy";
            if (!query.Sort.Equals("AES", StringComparison.CurrentCultureIgnoreCase))
                sortingDir = "OrderByDescending";

            ParameterExpression param1 = Expression.Parameter(typeof(T), query.Order);
            var pi = typeof(T).GetProperty(query.Order);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, queryable.Expression, Expression.Lambda(Expression.Property(param1, query.Order), param1));
            queryable = queryable.Provider.CreateQuery<T>(expr);

            var skipCount = (query.Page - 1) * query.Size;
            queryable = queryable.Skip(skipCount).Take(query.Size);
            result.Data = await queryable.ToListAsync();
            return result;
        }

        #region GetPageModel
        public virtual PageResult<M> GetPageModel<T, Q, M>(PageQuery<Q> query)
            where T : class
            where Q : class, new()
        {
            var page = this.GetPage<T, Q>(query);
            var result = new PageResult<M>()
            {
                Count = page.Count,
                Page = page.Page,
                Data = this.Map<T, M>(page.Data)
            };
            return result;
        }
        public virtual async Task<PageResult<M>> GetPageModelAsync<T, Q, M>(PageQuery<Q> query)
            where T : class
            where Q : class, new()
        {
            var page = await this.GetPageAsync<T, Q>(query);
            var result = new PageResult<M>()
            {
                Count = page.Count,
                Page = page.Page,
                Data = this.Map<T, M>(page.Data)
            };
            return result;
        }
        #endregion

        #endregion

        #region Add
        public virtual bool Add<T>(T entity, bool save = true)
            where T : class
        {
            this.Set<T>().Add(entity);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual bool Add<T>(List<T> list, bool save = true)
            where T : class
        {
            this.Set<T>().AddRange(list);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual async Task<bool> AddAsync<T>(T entity, bool save = true)
            where T : class
        {
            await this.Set<T>().AddAsync(entity);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public virtual async Task<bool> AddAsync<T>(List<T> list, bool save = true)
            where T : class
        {
            await this.Set<T>().AddRangeAsync(list);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }

        public virtual bool AddOrUpdate<T>(T entity, bool save = true)
            where T : class
        {
            var keyProp = this.GetKeyProperty<T>();
            var keyType = keyProp.Select(s => s.PropertyInfo).Single();
            var id = keyType.GetValue(entity);
            var db = this.Get<T>(id);
            if (db != null)
                return this.Update(entity, save);
            else
                return this.Add(entity, save);
        }
        public virtual async Task<bool> AddOrUpdateAsync<T>(T entity, bool save = true)
            where T : class
        {
            var keyProp = this.GetKeyProperty<T>();
            var keyType = keyProp.Select(s => s.PropertyInfo).Single();
            var id = keyType.GetValue(entity);
            var db = await this.GetAsync<T>(id);
            if (db != null)
                return await this.UpdateAsync(entity, save);
            else
                return await this.AddAsync(entity, save);
        }

        #region AddModel
        public virtual bool AddModel<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            return this.Add<T>(entity);
        }
        public virtual bool AddModel<T, M>(List<M> list)
            where T : class
        {
            var listEntity = this.Map<M, T>(list);
            return this.Add<T>(listEntity);
        }
        public virtual Task<bool> AddModelAsync<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            return this.AddAsync<T>(entity);
        }
        public virtual Task<bool> AddModelAsync<T, M>(List<M> list)
            where T : class
        {
            var listEntity = this.Map<M, T>(list);
            return this.AddAsync<T>(listEntity);
        }
        public virtual bool AddOrUpdateModel<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            return this.AddOrUpdate<T>(entity);
        }
        public virtual Task<bool> AddOrUpdateModelAsync<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            return this.AddOrUpdateAsync<T>(entity);
        }
        #endregion

        #endregion

        #region Update
        public virtual bool Update<T>(T entity, bool save = true, List<string> props = null)
            where T : class
        {
            this.AttachIfNot<T>(entity);
            var entry = this.Context.Entry<T>(entity);
            if (props == null || props.Count == 0)
                entry.State = EntityState.Modified;
            else
            {
                foreach (var eProp in entry.Properties)
                {
                    eProp.IsModified = props.Contains(eProp.Metadata.Name);
                }
            }
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual bool Update<T>(List<T> list, bool save = true, List<string> props = null)
            where T : class
        {
            foreach (var entity in list)
            {
                this.AttachIfNot<T>(entity);
                var entry = this.Context.Entry<T>(entity);
                if (props == null || props.Count == 0)
                    entry.State = EntityState.Modified;
                else
                {
                    foreach (var eProp in entry.Properties)
                    {
                        eProp.IsModified = props.Contains(eProp.Metadata.Name);
                    }
                }
            }
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual async Task<bool> UpdateAsync<T>(T entity, bool save = true, List<string> props = null)
            where T : class
        {
            this.AttachIfNot<T>(entity);
            var entry = this.Context.Entry<T>(entity);
            if (props == null || props.Count == 0)
                entry.State = EntityState.Modified;
            else
            {
                foreach (var eProp in entry.Properties)
                {
                    eProp.IsModified = props.Contains(eProp.Metadata.Name);
                }
            }
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public virtual async Task<bool> UpdateAsync<T>(List<T> list, bool save = true, List<string> props = null)
            where T : class
        {
            foreach (var entity in list)
            {
                this.AttachIfNot<T>(entity);
                var entry = this.Context.Entry<T>(entity);
                if (props == null || props.Count == 0)
                    entry.State = EntityState.Modified;
                else
                {
                    foreach (var eProp in entry.Properties)
                    {
                        eProp.IsModified = props.Contains(eProp.Metadata.Name);
                    }
                }
            }
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public virtual bool Update<T>(Expression<Func<T, bool>> where, Action<T> action)
            where T : class
        {
            var list = this.GetList(where);
            foreach (var entity in list)
            {
                action(entity);
            }
            return this.Update(list);
        }
        public virtual async Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> where, Action<T> action)
            where T : class
        {
            var list = await this.GetListAsync(where);
            foreach (var entity in list)
            {
                action(entity);
            }
            return await this.UpdateAsync(list);
        }

        #region UpdateModel
        public virtual bool Update<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            var props = typeof(M).GetProps().Select(s => s.Name).ToList();
            return this.Update<T>(entity, true, props);
        }
        public virtual bool Update<T, M>(List<M> list)
            where T : class
        {
            var listEntity = this.Map<M, T>(list);
            var props = typeof(M).GetProps().Select(s => s.Name).ToList();
            return this.Update<T>(listEntity, true, props);
        }
        public virtual Task<bool> UpdateAsync<T, M>(M model)
            where T : class
        {
            var entity = this.Map<M, T>(model);
            var props = typeof(M).GetProps().Select(s => s.Name).ToList();
            return this.UpdateAsync<T>(entity, true, props);
        }
        public virtual Task<bool> UpdateAsync<T, M>(List<M> list)
            where T : class
        {
            var listEntity = this.Map<M, T>(list);
            var props = typeof(M).GetProps().Select(s => s.Name).ToList();
            return this.UpdateAsync<T>(listEntity, true, props);
        }
        #endregion

        #endregion

        #region Delete
        public virtual bool Delete<T>(object id)
            where T : class
        {
            var entity = this.Get<T>(id);
            return this.Delete(entity);
        }
        public virtual bool Delete<T, K>(List<K> ids)
            where T : class
        {
            var list = this.GetList<T, K>(ids);
            return this.Delete(list);
        }
        public virtual async Task<bool> DeleteAsync<T>(object id)
            where T : class
        {
            var entity = await this.GetAsync<T>(id);
            return await this.DeleteAsync(entity);
        }
        public virtual async Task<bool> DeleteAsync<T, K>(List<K> ids)
            where T : class
        {
            var list = await this.GetListAsync<T, K>(ids);
            return await this.DeleteAsync(list);
        }
        public virtual bool Delete<T>(T entity, bool save = true)
            where T : class
        {
            this.Context.Entry<T>(entity).State = EntityState.Deleted;
            this.Set<T>().Remove(entity);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual bool Delete<T>(List<T> list, bool save = true)
            where T : class
        {
            foreach (var entity in list)
            {
                this.Context.Entry<T>(entity).State = EntityState.Deleted;
            }
            this.Set<T>().RemoveRange(list);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public virtual async Task<bool> DeleteAsync<T>(T entity, bool save = true)
            where T : class
        {
            this.Context.Entry<T>(entity).State = EntityState.Deleted;
            this.Set<T>().Remove(entity);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public virtual async Task<bool> DeleteAsync<T>(List<T> list, bool save = true)
            where T : class
        {
            foreach (var entity in list)
            {
                this.Context.Entry<T>(entity).State = EntityState.Deleted;
            }
            this.Set<T>().RemoveRange(list);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public virtual bool Delete<T>(Expression<Func<T, bool>> where)
            where T : class
        {
            var list = this.GetList<T>(where);
            return this.Delete(list);
        }
        public virtual async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> where)
            where T : class
        {
            var list = await this.GetListAsync<T>(where);
            return await this.DeleteAsync(list);
        }
        #endregion

        #region Map
        public virtual void Map<From, To>(From source, To model)
        {
            this.Mapper.Map<From, To>(source, model);
        }
        public virtual TOut Map<TIn, TOut>(TIn source)
        {
            return this.Mapper.Map<TIn, TOut>(source);
        }
        public virtual List<TOut> Map<TIn, TOut>(List<TIn> source)
        {
            return this.Mapper.Map<List<TIn>, List<TOut>>(source);
        }
        public virtual List<TOut> Map<TIn, TOut>(IQueryable<TIn> queryable)
        {
            return this.Map<TIn, TOut>(queryable.ToList());
        }
        #endregion

        #region Helper
        protected Expression<Func<T, K>> CreateKeySelector<T, K>(string key)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));
            var left = Expression.Property(param, typeof(K), key);
            return Expression.Lambda<Func<T, K>>(left);
        }
        protected Expression<Func<T, bool>> CreatePredicate<T, K>(string key, K value, Func<Expression, Expression, BinaryExpression> func)
        {
            var lambdaParam = Expression.Parameter(typeof(T));
            var leftExpression = Expression.PropertyOrField(lambdaParam, key);
            Expression<Func<K>> closure = () => value;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);
            var lambdaBody = func(leftExpression, rightExpression);
            return Expression.Lambda<Func<T, bool>>(lambdaBody, lambdaParam);
        }
        //public virtual Expression<Func<T, bool>> CreatePredicate<T, K>(string key, List<K> value, Func<Expression, Expression, BinaryExpression> fun)
        //{
        //    var keySelector = this.CreateKeySelector<T, K>(key);
        //    var right = Expression.Constant(value, typeof(K));
        //    Expression filter = fun(keySelector.Body, right);
        //    return Expression.Lambda<Func<T, bool>>(filter, keySelector.Parameters);
        //}
        protected void AttachIfNot<T>(T entity)
            where T : class
        {
            if (!this.Set<T>().Local.Contains(entity))
            {
                this.Set<T>().Attach(entity);
            }
        }
        protected IReadOnlyList<Microsoft.EntityFrameworkCore.Metadata.IProperty> GetKeyProperty<T>()
        {
            var cacheKey = "_KeyProperty:" + typeof(T).FullName;
            var cacheIntance = SingletonCache<IReadOnlyList<Microsoft.EntityFrameworkCore.Metadata.IProperty>>.Instance;
            var keyProp = cacheIntance.Get(cacheKey);
            if (keyProp == null)
            {
                keyProp = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
                cacheIntance.Add(cacheKey, keyProp);
            }
            return keyProp;
        }
        #endregion
    }
}
