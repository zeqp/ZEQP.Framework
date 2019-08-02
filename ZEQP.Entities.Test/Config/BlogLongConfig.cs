using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test.Config
{
    public class BlogLongConfig : BaseEntityTypeConfig<BlogLong>
    {
        public override void Configure(EntityTypeBuilder<BlogLong> builder)
        {
            base.Configure(builder);
        }
    }
}
