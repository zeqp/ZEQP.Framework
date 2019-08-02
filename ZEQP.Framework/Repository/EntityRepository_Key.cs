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

    }
}
