using Newtonsoft.Json;
using System;
using System.IO;
using tools.Extensions;

namespace tools.Utility
{
    /// <summary>
    /// Perform serialization operations using JSON data.
    /// </summary>
    public class JSONSerialiser : Serializer
    {
        public T Deserialize<T>(string data)
        {
            T result = default(T);

            // In case of problems when json is read from a file strip any line feeds.
            data = data.Minify();

            result = JsonConvert.DeserializeObject<T>(data);

            return result;
        }

        /// <summary>
        /// Reads a local file (ie: one on the accessible filesystem of the machine) into a JSON object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JsonFilePath"></param>
        /// <returns></returns>
        public T Deserialize<T>(Uri JsonFilePath)
        {
            T result = default(T);

            using (StreamReader sr = File.OpenText(JsonFilePath.LocalPath))
            {
                JsonSerializer jsonSerializer = new JsonSerializer(); // Note there are lots of settings for this. String formats, culture etc.
                result = (T)jsonSerializer.Deserialize(sr, typeof(T));
                jsonSerializer = null;
            }

            return result;
        }

        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
