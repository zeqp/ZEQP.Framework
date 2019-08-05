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
    public class EntityRepository<T, K> : BaseRepository<T, K>, IEntityRepository<T, K>
        where T : BaseEntity<K>
    {
        public EntityRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }
        public override List<T> GetList(List<K> ids, bool track = true)
        {
            if (track)
                return this.Set().Where(w => ids.Contains(w.Id)).ToList();
            return this.Set().AsNoTracking().Where(w => ids.Contains(w.Id)).ToList();
        }
        public override Task<List<T>> GetListAsync(List<K> ids, bool track = true)
        {
            if (track)
                return this.Set().Where(w => ids.Contains(w.Id)).ToListAsync();
            return this.Set().AsNoTracking().Where(w => ids.Contains(w.Id)).ToListAsync();
        }
        public override bool AddOrUpdate(T entity, bool save = true)
        {
            var db = this.Get(entity.Id);
            if (db != null)
                return this.Update(entity, save);
            else
                return this.Add(entity, save);
        }
        public override async Task<bool> AddOrUpdateAsync(T entity, bool save = true)
        {
            var db = await this.GetAsync(entity.Id);
            if (db != null)
                return await this.UpdateAsync(entity, save);
            else
                return await this.AddAsync(entity, save);
        }
    }
}
