using Jal.Router.Interface;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class JsonMessageBodySerializer : IMessageBodySerializer
    {
        public TContent Deserialize<TContent>(string content)
        {
            return JsonConvert.DeserializeObject<TContent>(content);
        }

        public string Serialize<TContent>(TContent content)
        {
            return JsonConvert.SerializeObject(content);
        }
    }
}