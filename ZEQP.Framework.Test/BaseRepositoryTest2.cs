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
    public class BaseRepositoryTest2
    {
        //public BaseRepositoryTest1() { }
        public IMapper Mapper { get; set; }
        public BloggingContext Context { get; set; }

        public BaseRepository<BlogInt,int> Service { get; set; }
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
            this.Service = new BaseRepository<BlogInt, int>(this.Context, this.Mapper);
        }
        [TestCleanup]
        public void Dispose()
        {
            this.Context.Dispose();
        }
        #region Get
        [TestMethod]
        public void TestGet1()
        {
            var blog = this.Service.Get<Blog>(1);
            Console.WriteLine($@"
TestGet1
this.Service.Get<Blog>(1)
{blog.ToJson()}
");
            Assert.IsNotNull(blog);
        }
        [TestMethod]
        public async Task TestGetAsync1()
        {
            var blog = await this.Service.GetAsync<Blog>(1);
            Console.WriteLine($@"
TestGetAsync1
this.Service.GetAsync<Blog>(1);
{blog.ToJson()}
");
            Assert.IsNotNull(blog);
        }

        [TestMethod]
        public void TestGet2()
        {
            var blog = this.Service.Get<Blog>(w => w.BlogId == 1);
            Console.WriteLine($@"
TestGet2
this.Service.Get<Blog>(w => w.BlogId == 1);
{blog.ToJson()}
");
            Assert.IsNotNull(blog);
        }
        [TestMethod]
        public async Task TestGetAsync2()
        {
            var blog = await this.Service.GetAsync<Blog>(w => w.BlogId == 1);
            Console.WriteLine($@"
TestGetAsync2
this.Service.GetAsync<Blog>(w => w.BlogId == 1);
{blog.ToJson()}
");
            Assert.IsNotNull(blog);
        }
        #region GetModel
        [TestMethod]
        public void TestGetModel1()
        {
            var model = this.Service.GetModel<Blog, BlogModel>(1);
            Console.WriteLine($@"
TestGetModel1
this.Service.GetModel<Blog, BlogModel>(1);
{model.ToJson()}
");
            Assert.IsNotNull(model);
        }
        [TestMethod]
        public async Task TestGetModelAsync1()
        {
            var model = await this.Service.GetModelAsync<Blog, BlogModel>(1);
            Console.WriteLine($@"
TestGetModelAsync1
this.Service.GetModelAsync<Blog, BlogModel>(1);
{model.ToJson()}
");
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void TestGetModel2()
        {
            var model = this.Service.GetModel<Blog, BlogModel>(w => w.BlogId == 1);
            Console.WriteLine($@"
TestGetModel2
this.Service.GetModel<Blog, BlogModel>(w => w.BlogId == 1);
{model.ToJson()}
");
            Assert.IsNotNull(model);
        }
        [TestMethod]
        public async Task TestGetModelAsync2()
        {
            var model = await this.Service.GetModelAsync<Blog, BlogModel>(w => w.BlogId == 1);
            Console.WriteLine($@"
TestGetModelAsync2
this.Service.GetModelAsync<Blog, BlogModel>(w => w.BlogId == 1);
{model.ToJson()}
");
            Assert.IsNotNull(model);
        }
        #endregion
        #endregion

        #region GetList
        [TestMethod]
        public void GetAll1()
        {
            var list = this.Service.GetAll<Blog>();
            Console.WriteLine($@"
GetAll
this.Service.GetAll<Blog>();
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public async Task TestGetAllAsync1()
        {
            var list = await this.Service.GetAllAsync<Blog>();
            Console.WriteLine($@"
TestGetAllAsync1
this.Service.GetAllAsync<Blog>();
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestGetList1()
        {
            var list = this.Service.GetList<Blog, int>(new List<int>() { 1, 2 });
            Console.WriteLine($@"
TestGetList1
this.Service.GetList<Blog, int>(new List<int>() {{ 1,2 }});
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetListAsync1()
        {
            var list = await this.Service.GetListAsync<Blog, int>(new List<int>() { 1, 2 });
            Console.WriteLine($@"
TestGetListAsymc1
this.Service.GetListAsync<Blog, int>(new List<int>() {{ 1,2 }});
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestGetList2()
        {
            var list = this.Service.GetList<Blog>(w => w.Rating > 0);
            Console.WriteLine($@"
TestGetList2
this.Service.GetList<Blog>(w => w.Rating > 0);
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetListAsync2()
        {
            var list = await this.Service.GetListAsync<Blog>(w => w.Rating > 0);
            Console.WriteLine($@"
TestGetListAsymc1
this.Service.GetListAsync<Blog>(w => w.Rating > 0);
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        #region GetListModel
        [TestMethod]
        public void GetAllModel1()
        {
            var list = this.Service.GetAllModel<Blog, BlogModel>();
            Console.WriteLine($@"
GetAllModel1
this.Service.GetAllModel<Blog, BlogModel>();
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public async Task TestGetAllModelAsync1()
        {
            var list = await this.Service.GetAllModelAsync<Blog, BlogModel>();
            Console.WriteLine($@"
TestGetAllModelAsync1
this.Service.GetAllModelAsync<Blog, BlogModel>();
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListModel1()
        {
            var list = this.Service.GetListModel<Blog, BlogModel, int>(new List<int>() { 1, 2 });
            Console.WriteLine($@"
GetListModel1
this.Service.GetListModel<Blog, BlogModel, int>(new List<int>() {{ 1,2 }});
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetListModelAsync1()
        {
            var list = await this.Service.GetListModelAsync<Blog, BlogModel, int>(new List<int>() { 1, 2 });
            Console.WriteLine($@"
TestGetListModelAsync1
this.Service.GetListModelAsync<Blog, BlogModel,int>(new List<int>() {{ 1,2 }});
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetListModel2()
        {
            var list = this.Service.GetListModel<Blog, BlogModel>(w => w.Rating > 0);
            Console.WriteLine($@"
GetListModel2
this.Service.GetListModel<Blog, BlogModel>(w => w.Rating > 0);
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetListModelAsync2()
        {
            var list = await this.Service.GetListModelAsync<Blog, BlogModel>(w => w.Rating > 0);
            Console.WriteLine($@"
TestGetListModelAsync2
this.Service.GetListModelAsync<Blog, BlogModel>(w => w.Rating > 0);
{list.ToJson()}
");
            Assert.IsNotNull(list);
        }
        #endregion
        #endregion



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
