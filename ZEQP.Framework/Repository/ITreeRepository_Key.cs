using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public interface ITreeRepository<T, K> : IBaseRepository<T, K>
        where T : class, ITreeEntity<K>
    {
    }
}
