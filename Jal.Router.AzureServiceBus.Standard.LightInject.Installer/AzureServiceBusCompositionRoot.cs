using System;
using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer
{
    public class AzureServiceBusCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMessageAdapter, MessageAdapter>(typeof(MessageAdapter).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelManager, AzureServiceBusManager>(typeof(AzureServiceBusManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMessageSerializer, JsonMessageSerializer>(typeof(JsonMessageSerializer).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IRequestReplyChannel, AzureServiceBusRequestReply>(typeof(AzureServiceBusRequestReply).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName, new PerContainerLifetime());
        }
    }
}
