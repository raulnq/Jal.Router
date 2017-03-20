using System;
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
                ReplyToConnectionString = GetConnectionString(brokeredMessage.ReplyTo),
                ReplyToQueue = GetEntity(brokeredMessage.ReplyTo),
                To = brokeredMessage.To,
                CorrelationId = brokeredMessage.CorrelationId
            };

            if (brokeredMessage.Properties.ContainsKey("from"))
            {
                context.From = brokeredMessage.Properties["from"].ToString();
            }
            if (brokeredMessage.Properties.ContainsKey("replytoconnectionstring"))
            {
                context.ReplyToConnectionString = brokeredMessage.Properties["replytoconnectionstring"].ToString();
            }

            if (brokeredMessage.Properties.ContainsKey("replytoqueue"))
            {
                context.ReplyToQueue = brokeredMessage.Properties["replytoqueue"].ToString();
            }

            if (brokeredMessage.Properties.ContainsKey("replytotopic"))
            {
                context.ReplyToTopic = brokeredMessage.Properties["replytotopic"].ToString();
            }

            return context;
        }

        public string GetConnectionString(string connectionstringandqueuename)
        {
            if (string.IsNullOrEmpty(connectionstringandqueuename) || connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                return string.Empty;
            }

            return connectionstringandqueuename.Substring(0, connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetEntity(string connectionstringandqueuename)
        {
            if (string.IsNullOrEmpty(connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7)))
            {
                return string.Empty;
            }

            return connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7);
        }

        public void Convert(BrokeredMessageContext context, BrokeredMessage brokeredMessage)
        {
            
        }
    }

    
}