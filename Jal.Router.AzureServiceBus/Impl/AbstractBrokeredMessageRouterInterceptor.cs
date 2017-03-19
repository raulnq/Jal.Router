using System;
using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public abstract class AbstractBrokeredMessageRouterInterceptor : IBrokeredMessageRouterInterceptor
    {
        public static IBrokeredMessageRouterInterceptor Instance = new NullBrokeredMessageRouterInterceptor();


        public virtual void OnEntry<TBody>(TBody body, BrokeredMessage message)
        {
            
        }

        public virtual void OnSuccess<TBody>(TBody body, BrokeredMessage message)
        {

        }

        public virtual void OnExit<TBody>(TBody body, BrokeredMessage message)
        {

        }

        public virtual void OnException<TBody>(TBody body, BrokeredMessage message, Exception exception)
        {

        }
    }
}