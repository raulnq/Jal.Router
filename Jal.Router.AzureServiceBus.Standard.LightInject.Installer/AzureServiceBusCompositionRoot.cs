using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer
{
    public class AzureServiceBusCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMessageAdapter, MessageAdapter>(typeof(MessageAdapter).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource, AzureServiceBusChannelResource>(typeof(AzureServiceBusChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromPointToPointChannel, AzureServiceBusRequestReplyFromPointToPointChannel>(typeof(AzureServiceBusRequestReplyFromPointToPointChannel).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>(typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName);

            serviceRegistry.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName);
        }
    }
}
