using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Entities.Test
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<BlogInt> BlogInts { get; set; }
        public DbSet<PostInt> PostInts { get; set; }

        public DbSet<BlogLong> BlogLongs { get; set; }
        public DbSet<PostLong> PostLongs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=192.168.11.10;Initial Catalog=Blogging;Persist Security Info=True;User ID=sa;Password=Hncsie@2018");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
