using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZEQP.Framework;
namespace ZEQP.Framework.Test
{
    [TestClass]
    public class SnowflakeTest1
    {
        [TestMethod]
        public void TestSnowflake1()
        {
            var listTask = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                var task= Task.Run(() => {
                    for (int j = 0; j < 100; j++)
                    {
                        var id = Snowflake.Instance.CreateId();
                        Console.WriteLine(id);
                    }
                });
                listTask.Add(task);
            }
            Task.WaitAll(listTask.ToArray());
        }
    }
}
