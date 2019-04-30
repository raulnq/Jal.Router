using System;
using Jal.Router.Interface;
using Newtonsoft.Json;

namespace Jal.Router.Newtonsoft.Impl
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public TContent Deserialize<TContent>(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<TContent>(content);
            }
            catch (Exception)
            {
                return default(TContent);
            }
        }

        public object Deserialize(string content, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(content, type);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Serialize<TContent>(TContent content)
        {
            try
            {
                return JsonConvert.SerializeObject(content, Formatting.None);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Serialize(object content)
        {
            try
            {
                return JsonConvert.SerializeObject(content, Formatting.None);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}