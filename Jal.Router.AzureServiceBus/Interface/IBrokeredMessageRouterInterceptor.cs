using System;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageRouterInterceptor
    {
        void OnEntry<TBody>(TBody body, BrokeredMessage message);

        void OnSuccess<TBody>(TBody body, BrokeredMessage message);

        void OnExit<TBody>(TBody body, BrokeredMessage message);

        void OnException<TBody>(TBody body, BrokeredMessage message, Exception exception);
    }
}