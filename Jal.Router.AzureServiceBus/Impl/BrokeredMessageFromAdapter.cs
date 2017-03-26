using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageFromAdapter : IBrokeredMessageFromAdapter
    {
        public string Read(BrokeredMessage message)
        {
            if (message.Properties.ContainsKey("from"))
            {
                return message.Properties["from"].ToString();
            }

            return string.Empty;
        }

        public void Write(string @from, BrokeredMessage message)
        {
            if (!string.IsNullOrWhiteSpace(@from))
            {
                message.Properties.Add("from", @from);
            }
        }
    }
}