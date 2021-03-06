﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public interface IEntityRepository<T, K> : IBaseRepository<T, K>
        where T : class, IBaseEntity<K>
    {

    }
}
