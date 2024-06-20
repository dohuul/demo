using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Org.BouncyCastle.Crypto.Paddings;
using QA.Common.Models;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace QA.Common.Utilities
{
    public static class SerializationUtilities
    {
        #region XML
        private static ConcurrentDictionary<Type, XmlSerializer> _xmlSerializers = new ConcurrentDictionary<Type, XmlSerializer>();
        private static XmlSerializer GetXMLSerializer(Type type)
        {
            return _xmlSerializers.GetOrAdd(type, (Type x) => new XmlSerializer(type));
        }   

        public static string SerializeXml<T>(T content, bool omitDeclaration)
        {
            string empty = string.Empty;
            using StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration
            };
            using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
            GetXMLSerializer(content.GetType()).Serialize(xmlWriter, content);
            return stringWriter.ToString();
        }

        public static string SerializeXml<T>(T content, bool omitDeclaration, bool omitNameSpace)
        {
            string empty = string.Empty;
            using StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration
            };

            using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
            XmlSerializer xmlSerializer = GetXMLSerializer(content.GetType());
            if(omitNameSpace)
            {
                xmlSerializer.Serialize(xmlWriter, content, new XmlSerializerNamespaces(new XmlQualifiedName[1] { XmlQualifiedName.Empty }));
            }
            else
            {
                xmlSerializer.Serialize(xmlWriter, content);
            }
            return stringWriter.ToString();

        }

        public static void SerializeXml<T>(T content, MemoryStream responseStream)
        {
            GetXMLSerializer(typeof(T)).Serialize(responseStream, content);
        }

        public static void SerializeXml<T>(T content, XmlWriter xmlWriter)
        {
            GetXMLSerializer(typeof(T)).Serialize(xmlWriter, content);
        }
        public static string SerializeXml<T>(T content)
        {
            return SerializeXml(content, omitDeclaration: true);
        }

        public static T DeserializeXml<T>(StreamReader requestStream)
        {
            return (T)GetXMLSerializer(typeof(T)).Deserialize(requestStream);
        }

        public static T DeserializeXml<T>(Stream requestStream)
        {
            return (T)GetXMLSerializer(typeof(T)).Deserialize(requestStream);
        }

        public static T DeserializeXml<T>(XmlReader xmlReader)
        {
            return (T)GetXMLSerializer(typeof(T)).Deserialize(xmlReader);
        }
        public static T DeserializeXml<T>(string xmlString)
        {
            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            return (T)GetXMLSerializer(typeof(T)).Deserialize(stream);
        }
        #endregion

        #region JSON
        private static JsonSerializer _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        });
        public static void SerializeJson<T>(T content, Stream responseStream)
        {
            StreamWriter streamWriter = new StreamWriter(responseStream);
            _jsonSerializer.Serialize(streamWriter, content);
            streamWriter.Flush();
        }

        public static string SerializeJson<T>(T content)
        {
            string empty = string.Empty;
            using StringWriter stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
            using JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter);
            _jsonSerializer.Serialize(jsonWriter, content, content.GetType());
            return stringWriter.ToString();
        }

        public static T DeserializeJson<T>(Stream requestStream)
        {
            requestStream.Position = 0L;
            JsonReader reader = new JsonTextReader(new StreamReader(requestStream));
            return _jsonSerializer.Deserialize<T>(reader);
        }

        public static T DeserializeJson<T>(StreamReader requestStream)
        {          
            return (T)_jsonSerializer.Deserialize(requestStream, typeof(T));
        }

        public static T DeserializeJson<T>(string responseString, bool isBase64Encoded)
        {
            if(isBase64Encoded)
            {
                byte[] bytes = Convert.FromBase64String(responseString);
            }
            using JsonTextReader reader = new JsonTextReader(new StringReader(responseString));
            return (T)_jsonSerializer.Deserialize(reader, typeof(T));
        }
        #endregion


        public static string SerializeContent<T> (string contentType, T content)
        {
            FileType fileType = GetContentType(contentType);
            string result = string.Empty;
            switch(fileType){
                case FileType.Unknown:
                    throw new Exception("Unable to serialize content - unkwon content type=" + contentType);
                case FileType.XML:
                    result = SerializeXml(content);
                    break;
                case FileType.JSON:
                    result = SerializeJson(content);
                    break;
            }
            return result;
        }

        public static FileType GetContentType(string contentType)
        {
            if (contentType.Contains("xml"))
            {
                return FileType.XML;
            }
            if (contentType.Contains("json"))
            {
                return FileType.JSON;
            }

            return FileType.Unknown;
        }
    }

   
}
