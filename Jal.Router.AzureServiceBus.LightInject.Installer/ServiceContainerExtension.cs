using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterBrokeredMessageRouter(this IServiceContainer container)
        {
            container.Register<IBrokeredMessageAdapter, BrokeredMessageAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageRouter, BrokeredMessageRouter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageContextBuilder, BrokeredMessageContextBuilder>(new PerContainerLifetime());

        }
    }
}
