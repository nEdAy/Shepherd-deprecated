using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MvcApplication1.Utility
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// JSON操作助手类
    /// </summary>
    /// <remarks>
    ///  2013-11-18 18:56 Created By iceStone
    /// </remarks>
    public static class JsonHelper
    {
        static JsonHelper(){
            //JsonSerializer.DateFormatHandling
        }
        /// <summary>
        /// The json serializer
        /// </summary>
        private static JsonSerializer JsonSerializer = new JsonSerializer();
        /// <summary>
        /// 将一个对象序列化JSON字符串
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:56 Created By iceStone
        /// </remarks>
        /// <param name="obj">待序列化的对象</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize(object obj)
        {
            //var sw = new StringWriter();
            //JsonSerializer.Serialize(new JsonTextWriter(sw), obj);
            //return sw.GetStringBuilder().ToString();

            //Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //timeConverter.DateTimeFormat = "yyyy-mm-dd HH:mm:ss";
            //return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeConverter);

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });
            return JsonConvert.SerializeObject(obj, jsSettings);

            
        }
        /// <summary>
        /// 将JSON字符串反序列化为一个Object对象
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:56 Created By iceStone
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>Object对象</returns>
        public static object Deserialize(string json)
        {
            //var sr = new StringReader(json);
            //return JsonSerializer.Deserialize(new JsonTextReader(sr));

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });
            return JsonConvert.DeserializeObject(json, jsSettings);
        }
        /// <summary>
        /// 将JSON字符串反序列化为一个指定类型对象
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:56 Created By iceStone
        /// </remarks>
        /// <typeparam name="TObj">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>指定类型对象</returns>
        public static TObj Deserialize<TObj>(string json) where TObj : class
        {
            var sr = new StringReader(json);
            return JsonSerializer.Deserialize(new JsonTextReader(sr), typeof(TObj)) as TObj;
        }
        /// <summary>
        /// 将JSON字符串反序列化为一个JObject对象
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:56 Created By iceStone
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>JObject对象</returns>
        public static JObject DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject(json) as JObject;
        }
        /// <summary>
        /// 将JSON字符串反序列化为一个JArray数组
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:56 Created By iceStone
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>JArray对象</returns>
        public static JArray DeserializeArray(string json)
        {
            return JsonConvert.DeserializeObject(json) as JArray;
        }

        public  static int[] GetRandomArray(int Number, int minNum, int maxNum)
        {
            int j;
            int[] b = new int[Number];
            Random r = new Random();
            for (j = 0; j < Number; j++)
            {
                int i = r.Next(minNum, maxNum + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (b[k] == i)
                    {
                        num = num + 1;
                    }
                }
                if (num == 0)
                {
                    b[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }
            string[] result = new string[Number];
            //for(int i = 0; i< Number + 1; )
            return b;
        }

        public static string[] readCatsFromLocal()
        {
            FileStream fs  = File.OpenRead("C:\\cache\\cats");
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string item = "";
            List<string> items = new List<string>();
            while((item = sr.ReadLine()) != null)
            {
                items.Add(item);
            }
            return items.ToArray();
        }

    }

}
