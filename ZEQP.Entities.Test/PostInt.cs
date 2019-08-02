using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test
{
    public class PostInt : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public BlogInt Blog { get; set; }
    }
}
