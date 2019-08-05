using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class BusRepository<T> : BusRepository<T, long>, IBusRepository<T>
        where T : BusEntity
    {
        public BusRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
