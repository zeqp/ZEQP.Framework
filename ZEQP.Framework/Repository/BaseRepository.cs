using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
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
        public int SaveChanges() => this.Context.SaveChanges();
        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();
    }
}
