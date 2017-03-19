using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageContextBuilder : IBrokeredMessageContextBuilder
    {
        public BrokeredMessageContext Build(BrokeredMessage brokeredMessage)
        {
            var context = new BrokeredMessageContext
            {
                MessageId = brokeredMessage.MessageId,
                ReplyTo = brokeredMessage.ReplyTo,
                To = brokeredMessage.To,
                CorrelationId = brokeredMessage.CorrelationId
            };

            if (brokeredMessage.Properties.ContainsKey("from"))
            {
                context.From = brokeredMessage.Properties["from"].ToString();
            }

            return context;
        }
    }
}