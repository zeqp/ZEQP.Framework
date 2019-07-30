using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZEQP.Entities.Test;
using ZEQP.Framework;

namespace ZEQP.Framework.Test
{
    [TestClass]
    public class BaseRepositoryTest1
    {
        [TestMethod]
        public void TestGet1()
        {
            using (var context = new BloggingContext())
            {
                var svc = new BaseRepository(context);
                var blog = svc.Get<Blog>(1);
                Assert.IsNotNull(blog);
            }
        }
        [TestMethod]
        public async Task TestGetAsymc1()
        {
            using (var context = new BloggingContext())
            {
                var svc = new BaseRepository(context);
                var blog = await svc.GetAsync<Blog>(1);
                Assert.IsNotNull(blog);
            }
        }

        [TestMethod]
        public void TestGetList1()
        {
            using (var context = new BloggingContext())
            {
                var svc = new BaseRepository(context);
                var list = svc.GetList<Blog>(new List<object>() { 1, 2 });
                Assert.AreEqual<int>(2, list.Count);
            }
        }
        [TestMethod]
        public async Task TestGetListAsymc1()
        {
            using (var context = new BloggingContext())
            {
                var svc = new BaseRepository(context);
                var list = await svc.GetListAsync<Blog>(new List<object>() { 1, 2 });
                Assert.AreEqual<int>(2, list.Count);
            }
        }
    }
}
