using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Interface;
using LightInject;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAzureServiceBusRouter(this IServiceContainer container)
        {
            container.Register<IMessageAdapter<BrokeredMessage>, BrokeredMessageAdapter>(new PerContainerLifetime());

            container.Register<IRouter<BrokeredMessage>, Jal.Router.Impl.Router<BrokeredMessage> >(new PerContainerLifetime());

            container.Register<IPublisher, AzureServiceBusTopic>(new PerContainerLifetime());

            container.Register<IQueue, AzureServiceBusQueue>(new PerContainerLifetime());
        }
    }
}
