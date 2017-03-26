using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageContentAdapter : IBrokeredMessageContentAdapter
    {
        public TBody Read<TBody>(BrokeredMessage brokeredMessage)
        {
            return JsonConvert.DeserializeObject<TBody>(brokeredMessage.GetBody<string>());
        }

        public BrokeredMessage Writer<TBody>(TBody body)
        {
            var json = JsonConvert.SerializeObject(body);

            var message = new BrokeredMessage(json) { ContentType = "application/json" };

            return  message;
        }
    }
}