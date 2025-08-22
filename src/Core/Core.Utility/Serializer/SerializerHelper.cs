using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Core.Utility
{/// <summary>
 /// Support class for serializing and deserializing workflow data such
 /// as association or task data. The method must use in pair.
 /// </summary>
    public static class SerializerHelper
    {
        static SiasunLogger logger = SiasunLogger.GetInstance(typeof(SerializerHelper));

        /// <summary>
        /// Serializes an object using the <see cref="BinaryFormatter" /> 
        /// into a Base64 encoded string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>A Base64 encoded string containing serialized 
        /// data.</returns>
        //public static String SerializeToBase64String<TData>(TData data)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, data);
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //}

        ///// <summary>
        ///// Serializes an object using the <see cref="BinaryFormatter" /> 
        ///// into a Base64 encoded string.
        ///// </summary>
        ///// <param name="data">The data to serialize.</param>
        ///// <typeparam name="TData">The type of data to process.</typeparam>
        ///// <returns>A Base64 encoded string containing serialized 
        ///// data.</returns>
        //public static String SerializeToBase64String(Object data)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, data);
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //}

        /// <summary>
        /// Deserializes an object from a Base64 encoded string 
        /// using the <see cref="BinaryFormatter" />.
        /// </summary>
        /// <param name="data">The Base64 encoded data to deserialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>The deserialized object.</returns>
        //public static TData DeserializeFromBase64String<TData>(String data)
        //{
        //    return (TData)DeserializeFromBase64String(data);
        //}

        /// <summary>
        /// Deserializes an object from a Base64 encoded string 
        /// using the <see cref="BinaryFormatter" />.
        /// </summary>
        /// <param name="data">The Base64 encoded data to deserialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>The deserialized object.</returns>
        //public static Object DeserializeFromBase64String(String data)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        byte[] content = Convert.FromBase64String(data);
        //        ms.Write(content, 0, content.Length);
        //        ms.Position = 0;
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        return formatter.Deserialize(ms);
        //    }
        //}

        /// <summary>
        /// Serializes an object using the <see cref="XmlSerializer" /> 
        /// into an XML string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>An XML string containing serialized 
        /// data.
        /// </returns>
        public static String SerializeToXmlString<TData>(TData data)
        {
            StringBuilder serializedData = new StringBuilder();
            using (StringWriter writer = new EncodingStringWriter(Encoding.UTF8, serializedData, CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TData));
                serializer.Serialize(writer, data);
            }
            return serializedData.ToString();
        }

        /// <summary>
        /// Serializes an object using the <see cref="XmlSerializer" /> 
        /// into an XML string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>An XML string containing serialized 
        /// data.
        /// </returns>
        public static String SerializeToXmlString(Object data)
        {
            StringBuilder serializedData = new StringBuilder();
            using (StringWriter writer = new EncodingStringWriter(Encoding.UTF8, serializedData, CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(writer, data);
            }
            return serializedData.ToString();
        }

        /// <summary>
        /// Deserializes an object from an XML string  
        /// using the <see cref="XmlSerializer" />.
        /// </summary>
        /// <param name="data">The XML data to Deserializes.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>The deserialized object.</returns>        
        public static TData DeserializeFromXmlString<TData>(String data)
        {
            using (StringReader reader = new StringReader(data))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TData));
                return (TData)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserializes an object from an XML string  
        /// using the <see cref="XmlSerializer" />.
        /// </summary>
        /// <param name="data">The XML data to Deserializes.</param>
        /// <param name="Type">The type of data to process.</typeparam>
        /// <returns>The deserialized object.</returns> 
        public static Object DeserializeFromXmlString(String data, Type type)
        {
            using (StringReader reader = new StringReader(data))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serializes an object using the <see cref="XmlSerializer" /> 
        /// into an XML string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>An XML string containing serialized 
        /// data.
        /// </returns>
        public static String SerializeToXmlStringWithoutDeclaring<TData>(TData data)
        {
            String result = SerializerHelper.SerializeToXmlString<TData>(data);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            return doc.DocumentElement.OuterXml;
        }

        /// <summary>
        /// Serializes an object using the <see cref="XmlSerializer" /> 
        /// into an XML string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>An XML string containing serialized 
        /// data.
        /// </returns>
        public static String SerializeToXmlStringWithoutDeclaring(Object data)
        {
            String result = SerializerHelper.SerializeToXmlString(data);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            return doc.DocumentElement.OuterXml;
        }

        /// <summary>
        /// Serializes an object using the <see cref="XmlSerializer" /> 
        /// into an XML string.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>An XML string containing serialized 
        /// data.
        /// </returns>
        public static Object DeserializeToXmlStringWithoutDeclaring(String data, Type type)
        {
            return DeserializeFromXmlString(data, type);
        }

        /// <summary>
        /// Deserializes an object from an XML string  
        /// using the <see cref="XmlSerializer" />.
        /// </summary>
        /// <param name="data">The XML data to Deserializes.</param>
        /// <typeparam name="TData">The type of data to process.</typeparam>
        /// <returns>The deserialized object.</returns>        
        public static TData DeserializeFromXmlStringWithoutDeclaring<TData>(String data)
        {
            return SerializerHelper.DeserializeFromXmlString<TData>(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String SerializeToBase64StringByDataContractSerializer(Object data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var dataContractSerializer = new DataContractSerializer(data.GetType());
                dataContractSerializer.WriteObject(ms, data);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// make sure the correct type of the data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String SerializeToBase64StringByDataContractSerializer(Object data, Type type)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var dataContractSerializer = new DataContractSerializer(type);
                dataContractSerializer.WriteObject(ms, data);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// 利用DataContractSerializer进行序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String SerializeByDataContractSerializer(Object data)
        {
            var serializedStringBuilder = new StringBuilder();
            using (var encodingStringWriter = new EncodingStringWriter(Encoding.UTF8, serializedStringBuilder, CultureInfo.InvariantCulture))
            {
                using (XmlTextWriter writer = new XmlTextWriter(encodingStringWriter))
                {
                    var dataContractSerializer = new DataContractSerializer(data.GetType());
                    dataContractSerializer.WriteObject(writer, data);
                    return serializedStringBuilder.ToString();
                }
            }
        }

        /// <summary>
        /// make the correct type of the data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String SerializeByDataContractSerializer(Object data, Type type)
        {
            var serializedStringBuilder = new StringBuilder();
            using (var encodingStringWriter = new EncodingStringWriter(Encoding.UTF8, serializedStringBuilder, CultureInfo.InvariantCulture))
            {
                using (XmlTextWriter writer = new XmlTextWriter(encodingStringWriter))
                {
                    var dataContractSerializer = new DataContractSerializer(type);
                    dataContractSerializer.WriteObject(writer, data);
                    return serializedStringBuilder.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object DeserializeFromBase64StringByDataContractSerializer(String data, Type type)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] content = Convert.FromBase64String(data);
                ms.Write(content, 0, content.Length);
                ms.Position = 0;
                var dataContractSerializer = new DataContractSerializer(type);
                return dataContractSerializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// 利用DataContractSerializer进行反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeByDataContractSerializer<T>(string data)
        {
            using (var stringReader = new StringReader(data))
            {
                using (XmlTextReader reader = new XmlTextReader(stringReader))
                {
                    var dataContractSerializer = new DataContractSerializer(typeof(T));
                    return (T)dataContractSerializer.ReadObject(reader);
                }
            }
        }

        /// <summary>
        /// 利用DataContractSerializer进行反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object DeserializeByDataContractSerializer(string data, Type type)
        {
            using (var stringReader = new StringReader(data))
            {
                using (var xmlTextReader = new XmlTextReader(stringReader))
                {
                    var dataContractSerializer = new DataContractSerializer(type);
                    return dataContractSerializer.ReadObject(xmlTextReader);
                }
            }
        }

        /// <summary>
        /// 利用JsonSerializer进行序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>
        public static string SerializeByJsonSerializer(Object data/*, bool ignoreDataMember = true*/)
        {
            try
            {
                var settings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                //if (ignoreDataMember) { settings.ContractResolver = AveContractResolver.Instance; }
                return JsonConvert.SerializeObject(data, settings);
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on json serialize data: {ex.Message}");
                return SerializeByDataContractSerializer(data);
            }
        }

        /// <summary>
        /// 利用JsonSerializer进行反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>
        public static T DeserializeByJsonSerializer<T>(string data/*, bool ignoreDataMember = true*/)
        {
            try
            {
                int intData = 0;
                if (data.StartsWith("{", StringComparison.Ordinal) || data.StartsWith("[", StringComparison.Ordinal)
                    || data.StartsWith("\"", StringComparison.Ordinal) || int.TryParse(data, out intData))
                {
                    var settings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                    //if (ignoreDataMember) { settings.ContractResolver = AveContractResolver.Instance; }
                    return JsonConvert.DeserializeObject<T>(data, settings);
                }
                else
                {
                    return DeserializeByDataContractSerializer<T>(data);
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on deserialize data: {ex.Message}");
                return DeserializeByDataContractSerializer<T>(data);
            }
        }

        /// <summary>
        /// 利用JsonSerializer进行序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SerializeByDataContractJsonSerializer(Object data)
        {
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(data.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, data);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on datacontract json serialize data: {ex.Message}");
                return SerializeByDataContractSerializer(data);
            }
        }

        /// <summary>
        /// 利用JsonSerializer进行反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeByDataContractJsonSerializer<T>(string data)
        {
            try
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(data)))
                {
                    return (T)json.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on deserialize data: {ex.Message}");
                return DeserializeByDataContractSerializer<T>(data);
            }
        }

        /// <summary>
        /// 利用Newtonsoft.Json.dll中JsonConvert进行序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>        
        public static string SerializeByJsonConvert(Object data)
        {
            return SerializeByJsonConvert(data, null);
        }

        /// <summary>
        /// 利用Newtonsoft.Json.dll中JsonConvert进行序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>        
        public static string SerializeByJsonConvert(Object data, IEnumerable<string> ignoreProps)
        {
            try
            {
                var settings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                if (ignoreProps != null) { settings.ContractResolver = new IgnorePropertiesContractResolver(ignoreProps); }
                //if (ignoreDataMember) { settings.ContractResolver = AveContractResolver.Instance; }
                return JsonConvert.SerializeObject(data, settings);
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on json convert serialize data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 利用Newtonsoft.Json.dll中JsonConvert进行反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>
        public static T DeserializeByJsonConvert<T>(string data/*, bool ignoreDataMember = true*/)
        {
            try
            {
                var settings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                //if (ignoreDataMember) { settings.ContractResolver = AveContractResolver.Instance; }
                return JsonConvert.DeserializeObject<T>(data, settings);
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on json convert deserialize data: {ex.Message}");
                throw;
            }
        }

        public static object DeserializeByJsonConvert(string data, Type type/*, bool ignoreDataMember = true*/)
        {
            try
            {
                var settings = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                return JsonConvert.DeserializeObject(data, type, settings);
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on json convert deserialize data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 利用Newtonsoft.Json.dll中JsonConvert进行序列化, Ignore null value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreDataMember"></param>
        /// <returns></returns>
        public static string SerializeByJsonConvertIgnoreNull(Object data, IEnumerable<string> ignoreProps = null)
        {
            try
            {
                var settings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
                if (ignoreProps != null) { settings.ContractResolver = new IgnorePropertiesContractResolver(ignoreProps); }
                return JsonConvert.SerializeObject(data, settings);
            }
            catch (Exception ex)
            {
                logger.Warn($"An exception occurred on json convert serialize data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 利用Newtonsoft.Json.dll中JsonConvert进行序列化(首字母小写)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SerializeByJsonConvertLowerCase(Object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// Deep Clone Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            //using (var ms = new MemoryStream())
            //{
            //    var formatter = new BinaryFormatter();
            //    formatter.Serialize(ms, obj);
            //    ms.Position = 0;
            //    return (T)formatter.Deserialize(ms);
            //}

            return DeepCloneNew<T>(obj);
        }

        public static T DeepCloneNew<T>(T obj)
        {
            string x = SerializeByJsonSerializer(obj);
            return DeserializeByJsonSerializer<T>(x);
        }
    }

    /// <summary>
    /// Use PropertyName to serialize or deserialize, not DataMemberAttribute's Name
    /// </summary>
    public class AveContractResolver : DefaultContractResolver
    {
        private static readonly IContractResolver _instance = new AveContractResolver();

        internal static IContractResolver Instance
        {
            get { return _instance; }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = property.UnderlyingName;
            return property;
        }

        protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
        {
            var property = base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
            property.PropertyName = property.UnderlyingName;
            return property;
        }
    }

    public class IgnorePropertiesContractResolver : DefaultContractResolver
    {
        private List<string> _props = new List<string>();

        public IgnorePropertiesContractResolver(IEnumerable<string> props)
        {
            if (props != null)
            {
                _props.AddRange(props);
            }
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperty(member, memberSerialization);
            if (_props.Contains(member.Name))
            {
                result.ShouldSerialize = i => false;
                result.ShouldDeserialize = i => false;
            }
            return result;
        }
    }
}
