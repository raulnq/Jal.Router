using System;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class ContextBuilder : IContextBuilder
    {
        public Context Build(BrokeredMessage brokeredMessage)
        {
            var context = new Context
            {
                Id = brokeredMessage.MessageId,
                ReplyToConnectionString = GetConnectionString(brokeredMessage.ReplyTo),//TODO delete
                ReplyToPath = GetEntity(brokeredMessage.ReplyTo),//TODO delete
                To = brokeredMessage.To,
                Correlation = brokeredMessage.CorrelationId
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
                context.ReplyToPath = brokeredMessage.Properties["replytoqueue"].ToString();
            }

            if (brokeredMessage.Properties.ContainsKey("replytotopic"))
            {
                context.ReplyToPublisherPath = brokeredMessage.Properties["replytotopic"].ToString();
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
            if(!string.IsNullOrWhiteSpace(connectionstringandqueuename))
            { 
                if (string.IsNullOrEmpty(connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7)))
                {
                    return string.Empty;
                }

                return connectionstringandqueuename.Substring(connectionstringandqueuename.IndexOf(";queue=", StringComparison.InvariantCultureIgnoreCase) + 7);
            }

            return string.Empty;
        }
    }

    
}