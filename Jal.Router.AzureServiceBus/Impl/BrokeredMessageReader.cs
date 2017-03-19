using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageReader : IBrokeredMessageReader
    {
        public TBody Read<TBody>(BrokeredMessage brokeredMessage)
        {
            return JsonConvert.DeserializeObject<TBody>(brokeredMessage.GetBody<string>());
        }
    }
}