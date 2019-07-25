using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 把对象转换为Json字符串
        /// 不能用于Linq中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isCamelCase">是否自动将首字母转为小写</param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool isCamelCase = false)
        {
            if (isCamelCase)
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        /// <summary>
        /// 把Json字符串转换为相应对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
