using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageIdAdapter : IBrokeredMessageIdAdapter
    {
        public string Read(BrokeredMessage message)
        {
            return message.MessageId;
        }

        public void Write(string id, BrokeredMessage message)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                message.MessageId = id;
            }
        }
    }
}