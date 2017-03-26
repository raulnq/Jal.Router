using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageIdAdapter
    {
        string Read(BrokeredMessage message);

        void Write(string id, BrokeredMessage message);
    }
}