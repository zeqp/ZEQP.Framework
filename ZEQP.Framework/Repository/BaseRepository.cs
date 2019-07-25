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
