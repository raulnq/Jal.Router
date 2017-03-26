using System;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageRouterInterceptor
    {
        void OnEntry(BrokeredMessage message);

        void OnSuccess<TBody>(TBody body, BrokeredMessage message);

        void OnExit(BrokeredMessage message);

        void OnException(BrokeredMessage message, Exception exception);
    }
}