using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageAdapter
    {
        TBody Read<TBody>(BrokeredMessage brokeredMessage);

        BrokeredMessage Writer<TBody>(TBody body);
    }
}