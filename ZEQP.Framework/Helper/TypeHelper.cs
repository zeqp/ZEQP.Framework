using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZEQP.Framework
{
    public static class TypeHelper
    {
        /// <summary>
        /// 拿到默认值
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetDefaultVal(this Type type)
        {
            if (!type.IsValueType) return null;
            switch (type.FullName)
            {
                case "System.Int32": return default(int);
                case "System.DateTime": return default(DateTime);
                case "System.Double": return default(double);
                case "System.Decimal": return default(decimal);
                case "System.Single": return default(float);
                case "System.Boolean": return default(bool);
                default: return Activator.CreateInstance(type);
            }
        }
        /// <summary>
        /// 是否默认值
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsDefault(this PropertyInfo prop, object model)
        {
            var val = prop.GetValue(model);
            if (val == null) return true;
            var propType = prop.PropertyType;
            if (propType == typeof(string))
                return val.Equals(String.Empty);
            var defaultVal = propType.GetDefaultVal();
            return val.Equals(defaultVal);
        }
    }
}
