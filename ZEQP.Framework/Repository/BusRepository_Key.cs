using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class BusRepository<T, K> : EntityRepository<T, K>, IBusRepository<T, K>
        where T : BusEntity<K>
    {
        public BusRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
