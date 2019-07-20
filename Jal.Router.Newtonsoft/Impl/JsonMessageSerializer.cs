using System;
using Jal.Router.Interface;
using Jal.Router.Newtonsoft.Model;
using Newtonsoft.Json;

namespace Jal.Router.Newtonsoft.Impl
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly NewtonsoftSerializerParameter _parameter;
        public JsonMessageSerializer(IParameterProvider provider)
        {
            _parameter = provider.Get<NewtonsoftSerializerParameter>();
        }
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
                return JsonConvert.SerializeObject(content, new JsonSerializerSettings { NullValueHandling = _parameter.NullValueHandling, Formatting = _parameter.Formatting, DefaultValueHandling = _parameter.DefaultValueHandling });
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
                return JsonConvert.SerializeObject(content, new JsonSerializerSettings { NullValueHandling = _parameter.NullValueHandling, Formatting = _parameter.Formatting, DefaultValueHandling = _parameter.DefaultValueHandling });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}