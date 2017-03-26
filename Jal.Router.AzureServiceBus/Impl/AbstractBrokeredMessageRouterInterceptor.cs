using System;
using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public abstract class AbstractBrokeredMessageRouterInterceptor : IBrokeredMessageRouterInterceptor
    {
        public static IBrokeredMessageRouterInterceptor Instance = new NullBrokeredMessageRouterInterceptor();


        public virtual void OnEntry(BrokeredMessage message)
        {
            
        }

        public virtual void OnSuccess<TBody>(TBody body, BrokeredMessage message)
        {

        }

        public virtual void OnExit(BrokeredMessage message)
        {

        }

        public virtual void OnException(BrokeredMessage message, Exception exception)
        {

        }
    }
}