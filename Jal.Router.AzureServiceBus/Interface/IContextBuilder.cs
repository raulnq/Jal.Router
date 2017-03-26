using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IContextBuilder
    {
        Context Build(BrokeredMessage brokeredMessage);
    }
}