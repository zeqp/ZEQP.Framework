using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test
{
    public class BlogInt : BaseEntity<int>
    {
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<PostInt> Posts { get; set; }
    }
}
