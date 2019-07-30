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
        public BaseRepository(DbContext context)
        {
            this.Context = context;
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
        public List<T> GetAll<T>() 
            where T : class
        {
            return this.Set<T>().ToList();
        }
        public Task<List<T>> GetAllAsync<T>()
            where T : class
        {
            return this.Set<T>().ToListAsync();
        }
        public List<T> GetList<T>(List<object> ids)
            where T : class
        {
            var keyProp = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = keyProp.Select(x => x.Name).Single();
            var keyType = keyProp.Select(x => x.PropertyInfo).Single();
            var right = Expression.Constant(ids, ids.GetType());
            var typeName = keyType.PropertyType.FullName;
            var method = ids.GetType().GetMethod("Contains");
            switch (typeName)
            {
                case "System.Int32":
                    {
                        var values = ids.Select(s => (int)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.String":
                    {
                        var values = ids.Select(s => s.ToString()).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.Int64":
                    {
                        var values = ids.Select(s => (long)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.Guid":
                    {
                        var values = ids.Select(s => (Guid)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                default: break;
            }
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, method, left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetList<T>(pred);
        }
        public Task<List<T>> GetListAsync<T>(List<object> ids)
            where T : class
        {
            var keyProp = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            var keyName = keyProp.Select(x => x.Name).Single();
            var keyType = keyProp.Select(x => x.PropertyInfo).Single();

            var right = Expression.Constant(ids, ids.GetType());
            var typeName = keyType.PropertyType.FullName;
            var method = ids.GetType().GetMethod("Contains");
            switch (typeName)
            {
                case "System.Int32":
                    {
                        var values = ids.Select(s => (int)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.String":
                    {
                        var values = ids.Select(s => s.ToString()).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.Int64":
                    {
                        var values = ids.Select(s => (long)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                case "System.Guid":
                    {
                        var values = ids.Select(s => (Guid)s).ToList();
                        right = Expression.Constant(values, values.GetType());
                        method = values.GetType().GetMethod("Contains");
                    }; break;
                default: break;
            }
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var left = Expression.Property(param, keyName);
            Expression filter = Expression.Call(right, method, left);
            Expression<Func<T, bool>> pred = Expression.Lambda<Func<T, bool>>(filter, param);
            return this.GetListAsync<T>(pred);
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

        #endregion
    }
}
