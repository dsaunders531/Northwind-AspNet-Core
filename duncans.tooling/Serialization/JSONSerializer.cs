// <copyright file="JSONSerializer.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;

namespace duncans.Serialization
{
    /// <summary>
    /// Perform serialization operations using JSON data.
    /// </summary>
    public class JSONSerializer : Serializer
    {
        public JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateParseHandling = DateParseHandling.DateTime,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
            }
        }

        public override string[] SupportedContentTypes { get; } =
        {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public override DataFormat DataFormat { get; } = DataFormat.Json;

        public override string ContentType { get; set; } = "application/json";

        public object Deserialize(string data)
        {
            object result = null;

            if (data.IsNullOrEmpty() == false)
            {
                data = data.Minify();

                result = JsonConvert.DeserializeObject(data, this.Settings);
            }

            return result;
        }

        public T Deserialize<T>(string data)
        {
            T result = default(T);

            if (data.IsNullOrEmpty() == false)
            {
                // In case of problems when json is read from a file strip any line feeds.
                data = data.Minify();

                result = JsonConvert.DeserializeObject<T>(data, this.Settings);
            }

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

            if (File.Exists(JsonFilePath.AbsolutePath))
            {
                using (StreamReader sr = File.OpenText(JsonFilePath.LocalPath))
                {
                    // Note there are lots of settings for this. String formats, culture etc.
                    JsonSerializer jsonSerializer = new JsonSerializer()
                    {
                        DateFormatHandling = this.Settings.DateFormatHandling,
                        DateParseHandling = this.Settings.DateParseHandling,
                        DateTimeZoneHandling = this.Settings.DateTimeZoneHandling
                    };

                    result = (T)jsonSerializer.Deserialize(sr, typeof(T));
                    jsonSerializer = null;
                }
            }
            else
            {
                throw new FileNotFoundException("The file at '{0}' could not be found.", JsonFilePath.AbsolutePath);
            }

            return result;
        }

        public override T Deserialize<T>(IRestResponse response)
        {
            return this.Deserialize<T>(response.Content);
        }

        public override string Serialize(object o)
        {
            if (o == null)
            {
                return string.Empty;
            }
            else
            {
                return JsonConvert.SerializeObject(o, this.Settings);
            }
        }

        public override string Serialize(Parameter parameter)
        {
            if (this.SupportedContentTypes.Contains(parameter.ContentType))
            {
                return this.Serialize(parameter.Value);
            }
            else
            {
                throw new NotSupportedException(string.Format("The content type '0' is not supported by the serializer '{1}'", parameter.ContentType, this.GetType().ToString()));
            }
        }
    }
}
