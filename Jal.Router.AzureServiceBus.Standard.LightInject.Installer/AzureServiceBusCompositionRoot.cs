using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer
{

    public class AzureServiceBusCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMessageAdapter, AzureServiceBusMessageAdapter>(typeof(AzureServiceBusMessageAdapter).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>, AzureManagementPointToPointChannelResourceManager>(typeof(AzureManagementPointToPointChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>, AzureServiceBusPublishSubscribeChannelResourceManager>(typeof(AzureServiceBusPublishSubscribeChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>, AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager>(typeof(AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>, AzureManagementPublishSubscribeChannelResourceManager>(typeof(AzureManagementPublishSubscribeChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>, AzureManagementSubscriptionToPublishSubscribeChannelResourceManager>(typeof(AzureManagementSubscriptionToPublishSubscribeChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>, AzureServiceBusPointToPointChannelResourceManager>(typeof(AzureServiceBusPointToPointChannelResourceManager).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromPointToPointChannel, AzureServiceBusRequestReplyFromPointToPointChannel>(typeof(AzureServiceBusRequestReplyFromPointToPointChannel).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>(typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName);

            serviceRegistry.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName);
        }
    }
}
