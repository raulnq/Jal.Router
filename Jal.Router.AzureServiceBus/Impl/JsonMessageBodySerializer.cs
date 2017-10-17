using Jal.Router.Interface;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class JsonMessageBodySerializer : IMessageBodySerializer
    {
        public TContent Deserialize<TContent>(string body)
        {
            return JsonConvert.DeserializeObject<TContent>(body);
        }

        public string Serialize<TContent>(TContent body)
        {
            return JsonConvert.SerializeObject(body);
        }
    }
}