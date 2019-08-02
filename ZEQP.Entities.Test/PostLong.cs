using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test
{
    public class PostLong : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public BlogLong Blog { get; set; }
    }
}
