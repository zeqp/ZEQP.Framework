using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public class BaseEntityTypeConfig<T, K> : IEntityTypeConfiguration<T>
        where T : BaseEntity<K>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Id).ValueGeneratedOnAdd();
        }
    }
    public class BaseEntityTypeConfig<T> : BaseEntityTypeConfig<T, long>, IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            //Id不会自动生成。请使用Snowflake生成ID
            builder.Property(k => k.Id).ValueGeneratedNever();
        }
    }

    public class BusEntityTypeConfig<T, K> : BaseEntityTypeConfig<T, K>, IEntityTypeConfiguration<T>
        where T : BusEntity<K>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.CreateTime).ValueGeneratedOnAdd();
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }
    public class BusEntityTypeConfig<T> : BusEntityTypeConfig<T, long>, IEntityTypeConfiguration<T>
        where T : BusEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            //Id不会自动生成。请使用Snowflake生成ID
            builder.Property(k => k.Id).ValueGeneratedNever();
            builder.Property(p => p.CreateTime).ValueGeneratedOnAdd();
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }
}
