using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public interface IBaseRepository
    {
        #region Transaction
        IDbContextTransaction Transaction { get; }
        void BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        void Commit(bool rollback = false);
        #endregion
        DbContext Context { get; }
        DbSet<T> Set<T>() where T : class;

        #region SaveChanges
        int SaveChanges();
        Task<int> SaveChangesAsync();
        #endregion
    }
}
