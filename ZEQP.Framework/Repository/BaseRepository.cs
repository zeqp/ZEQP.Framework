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
        public DbSet<T> Set<T>() where T : class => this.Context.Set<T>();

        #region Transaction
        public IDbContextTransaction Transaction => this.Context.Database.CurrentTransaction;
        public void BeginTransaction() => this.Context.Database.BeginTransaction();
        public Task<IDbContextTransaction> BeginTransactionAsync() => this.Context.Database.BeginTransactionAsync();
        public void Commit(bool rollback = false)
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
        public int SaveChanges() => this.Context.SaveChanges();
        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();
        #endregion

        #region Get
        public T Get<T>(object id, bool track = true)
            where T : class
        {
            if (!track)
                this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var entity = this.Set<T>().Find(id);
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return entity;
        }
        public async Task<T> GetAsync<T>(object id, bool track = true)
            where T : class
        {
            if (!track)
                this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var entity = await this.Set<T>().FindAsync(id);
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return entity;
        }
        public T Get<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).SingleOrDefault();
            return this.Set<T>().AsNoTracking().Where(predicate).SingleOrDefault();
        }
        public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).SingleOrDefaultAsync();
            return this.Set<T>().AsNoTracking().Where(predicate).SingleOrDefaultAsync();
        }
        #endregion

        #region GetList
        public List<T> GetAll<T>(bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().ToList();
            return this.Set<T>().AsNoTracking().ToList();
        }
        public Task<List<T>> GetAllAsync<T>(bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().ToListAsync();
            return this.Set<T>().AsNoTracking().ToListAsync();
        }
        public List<T> GetList<T, K>(List<K> ids, bool track = true)
            where T : class
        {
            var keyProp = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = keyProp.Select(x => x.Name).Single();
            var right = Expression.Constant(ids);
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, ids.GetType().GetMethod("Contains"), left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetList<T>(pred, track);
        }
        public Task<List<T>> GetListAsync<T, K>(List<K> ids, bool track = true)
            where T : class
        {
            var keyProp = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = keyProp.Select(x => x.Name).Single();
            var right = Expression.Constant(ids);

            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, ids.GetType().GetMethod("Contains"), left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetListAsync<T>(pred, track);
        }
        public List<T> GetList<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).ToList();
            return this.Set<T>().AsNoTracking().Where(predicate).ToList();
        }
        public Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate, bool track = true)
            where T : class
        {
            if (track)
                return this.Set<T>().Where(predicate).ToListAsync();
            return this.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }
        public List<T> GetList<T>(IQueryable<T> queryable)
            where T : class
        {
            return queryable.ToList();
        }
        public Task<List<T>> GetListAsync<T>(IQueryable<T> queryable)
            where T : class
        {
            return queryable.ToListAsync();
        }
        #endregion

        #region GetPage
        public PageResult<T> GetPage<T, M>(PageQuery<M> query)
            where T : class
            where M : class, new()
        {
            var queryable = this.Set<T>().AsNoTracking().AsQueryable();
            var model = query.Query;
            var outer = PredicateBuilder.True<T>();
            var entityProps = typeof(T).GetProperties();
            if (model != null)
            {
                var listProp = model.GetType().GetProperties();
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
            var result = new PageResult<T>();
            result.Count = queryable.Count();
            var totalPage = result.Count % query.Size == 0 ? (result.Count / query.Size) : (result.Count / query.Size + 1);
            if (query.Page > totalPage) query.Page = totalPage;
            if (query.Page <= 0) query.Page = 1;
            if (query.Size <= 0 || query.Size > 100) query.Size = 100;
            if (String.IsNullOrEmpty(query.Order)) query.Order = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(s => s.Name).Single();
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
        public async Task<PageResult<T>> GetPageAsync<T, M>(PageQuery<M> query)
            where T : class
            where M : class, new()
        {
            var queryable = this.Set<T>().AsNoTracking().AsQueryable();
            var model = query.Query;
            var outer = PredicateBuilder.True<T>();
            var entityProps = typeof(T).GetProperties();
            if (model != null)
            {
                var listProp = model.GetType().GetProperties();
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
            var result = new PageResult<T>();
            result.Count = await queryable.CountAsync();
            var totalPage = result.Count % query.Size == 0 ? (result.Count / query.Size) : (result.Count / query.Size + 1);
            if (query.Page > totalPage) query.Page = totalPage;
            if (query.Page <= 0) query.Page = 1;
            if (query.Size <= 0 || query.Size > 100) query.Size = 100;
            if (String.IsNullOrEmpty(query.Order)) query.Order = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(s => s.Name).Single();
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
        #endregion

        #region Add
        public bool Add<T>(T entity, bool save = true)
            where T : class
        {
            this.Set<T>().Add(entity);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public bool Add<T>(List<T> list, bool save = true)
            where T : class
        {
            this.Set<T>().AddRange(list);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public async Task<bool> AddAsync<T>(T entity, bool save = true)
            where T : class
        {
            await this.Set<T>().AddAsync(entity);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public async Task<bool> AddAsync<T>(List<T> list, bool save = true)
            where T : class
        {
            await this.Set<T>().AddRangeAsync(list);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        #endregion

        #region Update
        public bool Update<T>(T entity, bool save = true, List<string> props = null)
            where T : class
        {
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
            this.Set<T>().Update(entity);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public bool Update<T>(List<T> list, bool save = true, List<string> props = null)
            where T : class
        {
            foreach (var entity in list)
            {
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
            this.Set<T>().UpdateRange(list);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public async Task<bool> UpdateAsync<T>(T entity, bool save = true, List<string> props = null)
            where T : class
        {
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
            this.Set<T>().Update(entity);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public async Task<bool> UpdateAsync<T>(List<T> list, bool save = true, List<string> props = null)
            where T : class
        {
            foreach (var entity in list)
            {
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
            this.Set<T>().UpdateRange(list);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public bool Update<T>(Expression<Func<T, bool>> where, Action<T> action)
            where T : class
        {
            var list = this.GetList(where);
            foreach (var entity in list)
            {
                action(entity);
            }
            return this.Update(list);
        }
        public async Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> where, Action<T> action)
            where T : class
        {
            var list = await this.GetListAsync(where);
            foreach (var entity in list)
            {
                action(entity);
            }
            return await this.UpdateAsync(list);
        }
        #endregion

        #region Delete
        public bool Delete<T>(object id)
            where T : class
        {
            var entity = this.Get<T>(id);
            return this.Delete(entity);
        }
        public bool Delete<T, K>(List<K> ids)
            where T : class
        {
            var list = this.GetList<T, K>(ids);
            return this.Delete(list);
        }
        public async Task<bool> DeleteAsync<T>(object id)
            where T : class
        {
            var entity = await this.GetAsync<T>(id);
            return await this.DeleteAsync(entity);
        }
        public async Task<bool> DeleteAsync<T, K>(List<K> ids)
            where T : class
        {
            var list = await this.GetListAsync<T, K>(ids);
            return await this.DeleteAsync(list);
        }
        public bool Delete<T>(T entity, bool save = true)
            where T : class
        {
            this.Context.Entry<T>(entity).State = EntityState.Deleted;
            this.Set<T>().Remove(entity);
            if (save)
                return this.SaveChanges() > 0;
            return true;
        }
        public bool Delete<T>(List<T> list, bool save = true)
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
        public async Task<bool> DeleteAsync<T>(T entity, bool save = true)
            where T : class
        {
            this.Context.Entry<T>(entity).State = EntityState.Deleted;
            this.Set<T>().Remove(entity);
            if (save)
                return await this.SaveChangesAsync() > 0;
            return true;
        }
        public async Task<bool> DeleteAsync<T>(List<T> list, bool save = true)
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
        public bool Delete<T>(Expression<Func<T, bool>> where)
            where T : class
        {
            var list = this.GetList<T>(where);
            return this.Delete(list);
        }
        public async Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> where)
            where T : class
        {
            var list = await this.GetListAsync<T>(where);
            return await this.DeleteAsync(list);
        }
        #endregion

        #region Map
        public void Map<From, To>(From source, To model)
        {
            this.Mapper.Map<From, To>(source, model);
        }
        public TOut Map<TIn, TOut>(TIn source)
        {
            return this.Mapper.Map<TIn, TOut>(source);
        }
        public List<TOut> Map<TIn, TOut>(List<TIn> source)
        {
            return this.Mapper.Map<List<TIn>, List<TOut>>(source);
        }
        public List<TOut> Map<TIn, TOut>(IQueryable<TIn> queryable)
        {
            return this.Map<TIn, TOut>(queryable.ToList());
        }
        #endregion
    }
}
