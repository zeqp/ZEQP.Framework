using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public interface IBusRepository<T, K> : IEntityRepository<T, K>
        where T : class, IBusEntity<K>
    {

    }
}
