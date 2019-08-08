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

    public class BusEntityTypeConfig<T, K> : IEntityTypeConfiguration<T>
        where T : BusEntity<K>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.CreateTime).ValueGeneratedOnAdd();
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }
    public class BusEntityTypeConfig<T> : IEntityTypeConfiguration<T>
        where T : BusEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            //Id不会自动生成。请使用Snowflake生成ID
            builder.Property(k => k.Id).ValueGeneratedNever();
            builder.Property(p => p.CreateTime).ValueGeneratedOnAdd();
            builder.Property(p => p.Deleted).HasDefaultValue(false);
            builder.Property(p => p.ModifyTime).ValueGeneratedOnAddOrUpdate();
        }
    }


    public class TreeEntityTypeConfig<T, K> : IEntityTypeConfiguration<T>
        where T : TreeEntity<K>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(k => k.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ParentId).HasDefaultValue(default(T));
            builder.Property(p => p.IsLeaf).HasDefaultValue(true);
            builder.Property(p => p.Level).HasDefaultValue(0);
        }
    }
    public class TreeEntityTypeConfig<T> : IEntityTypeConfiguration<T>
        where T : TreeEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(k => k.Id);
            //Id不会自动生成。请使用Snowflake生成ID
            builder.Property(k => k.Id).ValueGeneratedNever();
            builder.Property(p => p.ParentId).HasDefaultValue(default(T));
            builder.Property(p => p.IsLeaf).HasDefaultValue(true);
            builder.Property(p => p.Level).HasDefaultValue(0);
        }
    }
}
