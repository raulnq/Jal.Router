using Jal.Router.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageRouter
    {
        void Route<TContent>(BrokeredMessage brokeredMessage, string name = "");
    }
}