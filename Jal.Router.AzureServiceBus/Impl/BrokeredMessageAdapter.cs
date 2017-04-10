using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageAdapter : IMessageAdapter<BrokeredMessage>
    {
        public InboundMessageContext<TContent> Read<TContent>(BrokeredMessage message)
        {
            var context = new InboundMessageContext<TContent>
            {
                Content = JsonConvert.DeserializeObject<TContent>(message.GetBody<string>()),
                Id = message.MessageId
            };


            if (message.Properties.ContainsKey("from"))
            {
                context.From = message.Properties["from"].ToString();
            }

            if (message.Properties.ContainsKey("version"))
            {
                context.Version = message.Properties["version"].ToString();
            }

            if (message.Properties.ContainsKey("origin"))
            {
                context.Origin = message.Properties["origin"].ToString();
            }

            if (message.Properties != null)
            {
                foreach (var property in message.Properties)
                {
                    context.Headers.Add(property.Key, property.Value?.ToString());
                }
            }

            return context;
        }

        public BrokeredMessage Write<TContent>(OutboundMessageContext<TContent> message)
        {
            var json = JsonConvert.SerializeObject(message.Content);

            var brokeredmessage = new BrokeredMessage(json) { ContentType = "application/json" };

            foreach (var header in message.Headers)
            {
                brokeredmessage.Properties.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(message.From))
            {
                brokeredmessage.Properties.Add("from", message.From);
            }

            if (!string.IsNullOrWhiteSpace(message.Version))
            {
                brokeredmessage.Properties.Add("version", message.Version);
            }

            if (message.ScheduledEnqueueDateTimeUtc!=null)
            {
                brokeredmessage.ScheduledEnqueueTimeUtc = message.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(message.Origin))
            {
                brokeredmessage.Properties.Add("origin", message.Origin);
            }

            if (!string.IsNullOrWhiteSpace(message.Id))
            {
                brokeredmessage.MessageId = message.Id;
            }

            return brokeredmessage;
        }
    }
}