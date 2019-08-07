using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZEQP.Framework;

namespace ZEQP.Framework.Test
{
    [TestClass]
    public class ObjectIdTest1
    {
        [TestMethod]
        public void CreateID()
        {
            Thread.ParallelExecute(() =>
            {
                Console.WriteLine($"{Task.CurrentId}\t{ObjectId.GenerateNewId()}");
            }, 10000);
        }
    }
}
