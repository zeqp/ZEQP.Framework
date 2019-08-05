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
    public class BusRepository<T, K> : EntityRepository<T, K>, IBusRepository<T, K>
        where T : BusEntity<K>
    {
        public BusRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }

        public override IQueryable<T> GetQueryable(bool track = true)
        {
            return base.GetQueryable(track).Where(w => w.Deleted == false);
        }

        public override bool Delete(object id)
        {
            var entity = this.Get(id);
            entity.Deleted = true;
            return this.Update(entity, true, new List<string>() { nameof(entity.Deleted) });

        }
        public override async Task<bool> DeleteAsync(object id)
        {
            var entity = await this.GetAsync(id);
            entity.Deleted = true;
            return await this.UpdateAsync(entity, true, new List<string>() { nameof(entity.Deleted) });
        }
        public override bool Delete(List<K> ids)
        {
            var list = this.GetList(ids);
            list.ForEach((entity) =>
            {
                entity.Deleted = true;
            });
            return this.Update(list, true, new List<string>() { "Deleted" });
        }
        public override async Task<bool> DeleteAsync(List<K> ids)
        {
            var list = await this.GetListAsync(ids);
            list.ForEach((entity) => {
                entity.Deleted = true;
            });
            return await this.UpdateAsync(list, true, new List<string>() { "Deleted" });
        }
    }
}
