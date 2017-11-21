using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAzureServiceBusRouter(this IServiceContainer container)
        {
            container.Register<IMessageAdapter, BrokeredMessageAdapter>(typeof(BrokeredMessageAdapter).FullName, new PerContainerLifetime());
            
            container.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName, new PerContainerLifetime());

            container.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName, new PerContainerLifetime());

            container.Register<IChannelManager, AzureServiceBusManager>(typeof(AzureServiceBusManager).FullName, new PerContainerLifetime());

            container.Register<IMessageBodySerializer, JsonMessageBodySerializer>(typeof(JsonMessageBodySerializer).FullName, new PerContainerLifetime());
        }
    }
}
