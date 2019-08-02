using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class EntityRepository<T> : EntityRepository<T, long>, IEntityRepository<T>
        where T : BaseEntity
    {
        public EntityRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
