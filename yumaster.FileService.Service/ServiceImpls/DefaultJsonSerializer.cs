using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace yumaster.FileService.Service.ServiceImpls
{
    class DefaultJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializer _serializer;

        public DefaultJsonSerializer(IOptions<MvcJsonOptions> options)
        {
            var jss = options.Value.SerializerSettings;
            _serializer = JsonSerializer.Create(jss);
        }

        public void Serialize(TextWriter writer, object value, Type objectType = null)
        {
            using (var jtw = new JsonTextWriter(writer))
            {
                jtw.CloseOutput = false;
                if (objectType == null)
                    _serializer.Serialize(jtw, value);
                else
                    _serializer.Serialize(jtw, value, objectType);
            }
        }

        public object Deserialize(TextReader reader, Type objectType = null)
        {
            using (var jtr = new JsonTextReader(reader))
            {
                jtr.CloseInput = false;
                return objectType == null ? _serializer.Deserialize(jtr) : _serializer.Deserialize(jtr, objectType);
            }
        }
    }
}
