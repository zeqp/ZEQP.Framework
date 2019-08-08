using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZEQP.Framework
{
    /// <summary>
    /// XML帮助类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 把XML反序列化成相应实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>反序列化后的实体</returns>
        public static T XMLDeserialize<T>(string xmlString) where T : new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader textReader = new StringReader(xmlString))
            using (XmlReader reader = XmlReader.Create(textReader))
            {
                T model = (T)serializer.Deserialize(reader);
                return model;
            }
        }
        /// <summary>
        /// 把XML反序列化成相应实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>反序列化后的实体</returns>
        public static T ToXMLModel<T>(this string xmlString) where T : new()
        {
            return XmlHelper.XMLDeserialize<T>(xmlString);
        }
        /// <summary>
        /// 把实体转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="model">要序列化的实体</param>
        /// <returns>XML字符串</returns>
        public static string XMLSerialize<T>(T model) where T : new()
        {
            string xmlString = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                Encoding utf8EncodingWithNoByteOrderMark = new UTF8Encoding(false);
                XmlWriterSettings setting = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true,
                    Encoding = utf8EncodingWithNoByteOrderMark
                };
                //using (XmlTextWriter xtw = new XmlTextWriter(stream, utf8EncodingWithNoByteOrderMark))
                //{
                //    serializer.Serialize(xtw, model, ns);
                //    xmlString = Encoding.UTF8.GetString(stream.ToArray());
                //}
                using (XmlWriter xw = XmlWriter.Create(stream, setting))
                {
                    serializer.Serialize(xw, model, ns);
                    xmlString = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            return xmlString;
        }
        /// <summary>
        /// 把实体转换成XML
        /// </summary>
        /// <param name="model">要序列化的实体</param>
        /// <returns>XML字符串</returns>
        public static string XMLSerialize(object model)
        {
            string xmlString = string.Empty;
            XmlSerializer serializer = new XmlSerializer(model.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                Encoding utf8EncodingWithNoByteOrderMark = new UTF8Encoding(false);
                XmlWriterSettings setting = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true,
                    Encoding = utf8EncodingWithNoByteOrderMark
                };
                //using (XmlTextWriter xtw = new XmlTextWriter(stream, utf8EncodingWithNoByteOrderMark))
                //{
                //    serializer.Serialize(xtw, model, ns);
                //    xmlString = Encoding.UTF8.GetString(stream.ToArray());
                //}
                using (XmlWriter xw = XmlWriter.Create(stream, setting))
                {
                    serializer.Serialize(xw, model, ns);
                    xmlString = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            return xmlString;
        }
        /// <summary>
        /// 把实体转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="model">要序列化的实体</param>
        /// <returns>XML字符串</returns>
        public static string ToXML<T>(this T model) where T : new()
        {
            return XmlHelper.XMLSerialize<T>(model);
        }
        /// <summary>
        /// 把实体转换成XML
        /// </summary>
        /// <param name="model">要序列化的实体</param>
        /// <returns>XML字符串</returns>
        public static string ToXML(this object model)
        {
            return XmlHelper.XMLSerialize(model);
        }
    }
}
