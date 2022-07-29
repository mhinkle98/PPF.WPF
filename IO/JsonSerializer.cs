﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PPF.WPF.IO
{
    public static class PPFlowJsonSerializer 
    {
        private static List<JsonConverter> _knownConverters;

#if DEBUG
        /// <summary>
        /// memory trace writer
        /// </summary>
        public static MemoryTraceWriter TraceWriter = new MemoryTraceWriter();
#endif

        /// <summary>
        /// method to write to json
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="myObject">object to write</param>
        /// <returns>written json</returns>
        public static string WriteToJson<T>(this T myObject)
        {
            var settings = new JsonSerializerSettings
            {
                //added temporarily to help serialize the sources that use interfaces
                TypeNameHandling = TypeNameHandling.None,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
#if DEBUG
            settings.TraceWriter = TraceWriter;
#endif
            settings.Converters.Add(new StringEnumConverter());
            string json = JsonConvert.SerializeObject(
                myObject,
                Formatting.Indented,
                settings);
#if DEBUG
            Console.WriteLine(TraceWriter);
#endif
            return json;
        }

        /// <summary>
        /// method to write json to file
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="myObject">object to write</param>
        /// <param name="filename">json file written to</param>
        public static void WriteToJsonFile<T>(this T myObject, string filename)
        {
            var settingsJson = WriteToJson(myObject);

            using (var stream = StreamFinder.GetFileStream(filename, FileMode.Create))
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(settingsJson);
            }
        }

        /// <summary>
        /// TO BE IMPLEMENTED!! List of converters that are known (from models, maybe)
        /// </summary>
        public static List<JsonConverter> KnownConverters
        {
            get
            {
                if (_knownConverters != null) return _knownConverters;
                _knownConverters = new List<JsonConverter>
                {
                    // add in here
                };
                return _knownConverters;
            }
            set => _knownConverters = value;
        }

        /// <summary>
        /// method to read form json
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="myString">string identifying json</param>
        /// <returns>deserialized object</returns>
        public static T ReadFromJson<T>(this string myString)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            foreach (var jsonConverter in KnownConverters)
            {
                serializer.Converters.Add(jsonConverter);
            }
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //added temporarily to help serialize the sources that use interfaces
            serializer.TypeNameHandling = TypeNameHandling.None;
            serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
#if DEBUG
            serializer.TraceWriter = TraceWriter;
#endif

            T deserializedProduct = default(T);
            using (var sr = new StringReader(myString))
            using (var reader = new JsonTextReader(sr))
            {
                deserializedProduct = serializer.Deserialize<T>(reader);
            }
#if DEBUG
            Console.WriteLine(TraceWriter);
#endif
            return deserializedProduct;
        }

        /// <summary>
        /// method to read json from file
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="filename">name of file to be read</param>
        /// <returns>json</returns>
        public static T ReadFromJsonFile<T>(string filename)
        {
            using (var stream = StreamFinder.GetFileStream(filename, FileMode.Open))
            using (var sr = new StreamReader(stream, false))
            {
                var json = sr.ReadToEnd();

                return ReadFromJson<T>(json);
            }
        }
    }
}
