using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ZEQP.Framework
{
    public class TreeRepository<T> : TreeRepository<T, long>, ITreeRepository<T>
        where T : TreeEntity
    {
        public TreeRepository(DbContext context, IMapper mapper) 
            : base(context, mapper)
        {
        }
    }
}
