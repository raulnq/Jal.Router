using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageFromAdapter
    {
        string Read(BrokeredMessage message);

        void Write(string from, BrokeredMessage message);

        string ReadOrigin(BrokeredMessage message);

        void WriteOrigin(string origin, BrokeredMessage message);
    }
}