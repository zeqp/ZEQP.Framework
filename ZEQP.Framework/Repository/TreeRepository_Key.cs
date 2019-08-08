using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class TreeRepository<T, K> : BaseRepository<T, K>, ITreeRepository<T, K>
        where T : TreeEntity<K>
    {
        public TreeRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        { }
    }
}
