using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using LightInject;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAzureServiceBusRouter(this IServiceContainer container)
        {
            container.Register<IMessageBodyAdapter, BrokeredMessageBodyAdapter>(typeof(BrokeredMessageBodyAdapter).FullName, new PerContainerLifetime());
            
            container.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName, new PerContainerLifetime());

            container.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName, new PerContainerLifetime());

            container.Register<IChannelManager, AzureServiceBusManager>(typeof(AzureServiceBusManager).FullName, new PerContainerLifetime());

            container.Register<IMessageMetadataAdapter, BrokeredMessageMetadataAdapter>(typeof(BrokeredMessageMetadataAdapter).FullName, new PerContainerLifetime());

            container.Register<IMessageBodySerializer, JsonMessageBodySerializer>(typeof(JsonMessageBodySerializer).FullName, new PerContainerLifetime());
        }
    }
}
