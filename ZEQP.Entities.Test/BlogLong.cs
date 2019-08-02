using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test
{
    public class BlogLong : BaseEntity
    {
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<PostLong> Posts { get; set; }
    }
}
