using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageReader
    {
        TBody Read<TBody>(BrokeredMessage brokeredMessage);
    }
}