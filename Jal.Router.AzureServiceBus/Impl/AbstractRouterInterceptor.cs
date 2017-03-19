using System;
using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public abstract class AbstractRouterInterceptor : IBrokeredMessageRouterInterceptor
    {
        public static IBrokeredMessageRouterInterceptor Instance = new NullBrokeredMessageRouterInterceptor();


        public void OnEntry<TBody>(TBody body, BrokeredMessage message)
        {
            
        }

        public void OnSuccess<TBody>(TBody body, BrokeredMessage message)
        {

        }

        public void OnExit<TBody>(TBody body, BrokeredMessage message)
        {

        }

        public void OnException<TBody>(TBody body, BrokeredMessage message, Exception exception)
        {

        }
    }
}