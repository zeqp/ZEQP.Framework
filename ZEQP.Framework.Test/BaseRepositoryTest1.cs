using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZEQP.Domain.Test;
using ZEQP.Entities.Test;
using ZEQP.Framework;

namespace ZEQP.Framework.Test
{
    [TestClass]
    public class BaseRepositoryTest1
    {
        //public BaseRepositoryTest1() { }
        public IMapper Mapper { get; set; }
        public BloggingContext Context { get; set; }

        public BaseRepository Service { get; set; }
        [TestInitialize]
        public void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogModel>(MemberList.None);
                cfg.CreateMap<Post, PostModel>(MemberList.None);
                cfg.CreateMap<BlogModel, Blog>(MemberList.None);
                cfg.CreateMap<PostModel, Post>(MemberList.None);
            });
            this.Mapper = config.CreateMapper();
            this.Context = new BloggingContext();
            this.Service = new BaseRepository(this.Context, this.Mapper);
        }
        [TestCleanup]
        public void Dispose()
        {
            this.Context.Dispose();
        }

        [TestMethod]
        public void TestGet1()
        {
            var blog = this.Service.Get<Blog>(1);
            Assert.IsNotNull(blog);
        }
        [TestMethod]
        public async Task TestGetAsymc1()
        {
            var blog = await this.Service.GetAsync<Blog>(1);
            Assert.IsNotNull(blog);
        }

        [TestMethod]
        public void TestGetList1()
        {
            var list = this.Service.GetList<Blog, int>(new List<int>() { 1, 2 });
            Assert.AreEqual<int>(2, list.Count);
        }
        [TestMethod]
        public async Task TestGetListAsymc1()
        {
            var list = await this.Service.GetListAsync<Blog, int>(new List<int>() { 1, 2 });
            Assert.AreEqual<int>(2, list.Count);
        }

        [TestMethod]
        public void TestGetPage1()
        {
            var result = this.Service.GetPage<Blog, dynamic>(new PageQuery<dynamic>() { Page = 1, Size = 10, Order = "Url", Sort = "AES", Query = new { Url = "http://a." } });
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestGetPage2()
        {
            var result = this.Service.GetPage<Blog, dynamic>(new PageQuery<dynamic>() { Page = 1, Size = 10, Order = "Url", Sort = "AES", Query = new { Url = "%http://a." } });
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestGetPage3()
        {
            var result = this.Service.GetPage<Blog, dynamic>(new PageQuery<dynamic>() { Page = 1, Size = 10, Order = "Rating", Sort = "DESC" });
            Assert.IsNotNull(result);
        }
    }
}
