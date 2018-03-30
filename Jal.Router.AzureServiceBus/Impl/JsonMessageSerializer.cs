using System;
using Jal.Router.Interface;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public TContent Deserialize<TContent>(string content)
        {
            return JsonConvert.DeserializeObject<TContent>(content);
        }

        public object Deserialize(string content, Type type)
        {
            return JsonConvert.DeserializeObject(content, type);
        }

        public string Serialize<TContent>(TContent content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }

        public string Serialize(object content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }
    }
}