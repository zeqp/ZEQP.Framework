using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework.Base
{
    public class BaseEntityTypeConfig<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            //Id不会自动生成。请使用Snowflake生成ID
            builder.Property(k => k.Id).ValueGeneratedNever();
        }
    }

    public class BusEntityTypeConfig<T> : BaseEntityTypeConfig<T>, IEntityTypeConfiguration<T>
        where T : BusEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.CreateTime).ValueGeneratedOnAdd();
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }
}
