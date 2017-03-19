using Jal.Router.AzureServiceBus.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageContextBuilder
    {
        BrokeredMessageContext Build(BrokeredMessage brokeredMessage);

    }
}