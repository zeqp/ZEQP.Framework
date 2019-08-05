using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public interface IBusRepository<T> : IBusRepository<T, long>
        where T : class, IBusEntity
    {
    }
}
