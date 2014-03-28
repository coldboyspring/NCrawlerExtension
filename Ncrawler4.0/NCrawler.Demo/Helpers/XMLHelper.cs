using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NCrawler.Demo.Helpers {
    public static class XMLHelper {

        /// <summary>
        /// 将一个对象序列化成xml字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>返回的xml字符串</returns>
        public static string XmlSerialize(object obj, Encoding encoding) {
            using (MemoryStream stream = new MemoryStream()) {
                XmlSerializeInternal(stream, obj, encoding);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding)) {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按xml序列化的方式写入到一个文件中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public static void XmlSerializeToFile(object obj, string path, Encoding encoding) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentNullException("path");
            }
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
                XmlSerializeInternal(stream, obj, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding) {
            if (string.IsNullOrWhiteSpace(s)) {
                throw new ArgumentNullException("s");
            }
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(s))) {
                using (StreamReader reader = new StreamReader(stream, encoding)) {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding) {
            if (string.IsNullOrEmpty(path)) {
                throw new ArgumentNullException("path");
            }
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }

        /// <summary>
        /// 将对象序列化写入到流中
        /// </summary>
        /// <param name="stream">要写入的流</param>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        private static void XmlSerializeInternal(Stream stream, object obj, Encoding encoding) {
            if (obj == null) {
                throw new AggregateException("obj");
            }
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "   ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                serializer.Serialize(writer, obj);
                writer.Close();
            }
        }
    }
}
