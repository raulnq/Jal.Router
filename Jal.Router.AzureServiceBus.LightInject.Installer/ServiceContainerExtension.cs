using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterBrokeredMessageRouter(this IServiceContainer container)
        {
            container.Register<IBrokeredMessageContentAdapter, BrokeredMessageContentAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageRouter, BrokeredMessageRouter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageFromAdapter, BrokeredMessageFromAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageIdAdapter, BrokeredMessageIdAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageReplyToAdapter, BrokeredMessageReplyToAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageContentAdapter, BrokeredMessageContentAdapter>(new PerContainerLifetime());

            container.Register<IQueue, AzureServiceBusQueue>(new PerContainerLifetime());

            container.Register<IBusInterceptor, LoggerBusInterceptor>(new PerContainerLifetime());
        }
    }
}
