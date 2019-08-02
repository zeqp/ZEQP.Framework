using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZEQP.Framework;

namespace ZEQP.Entities.Test.Config
{
    public class PostLongConfig : BaseEntityTypeConfig<PostLong>
    {
        public override void Configure(EntityTypeBuilder<PostLong> builder)
        {
            base.Configure(builder);
        }
    }
}
