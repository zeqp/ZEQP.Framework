using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Domain.Test
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
    }
}
