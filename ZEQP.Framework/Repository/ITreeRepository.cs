using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public interface ITreeRepository<T> : ITreeRepository<T, long>
        where T : class, ITreeEntity
    {
    }
}
