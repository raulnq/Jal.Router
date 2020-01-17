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

            serviceRegistry.Register<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>, AzureManagementPointToPointChannelResource>(typeof(AzureManagementPointToPointChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>, AzureServiceBusPublishSubscribeChannelResource>(typeof(AzureServiceBusPublishSubscribeChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>, AzureServiceBusSubscriptionToPublishSubscribeChannelResource>(typeof(AzureServiceBusSubscriptionToPublishSubscribeChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>, AzureManagementPublishSubscribeChannelResource>(typeof(AzureManagementPublishSubscribeChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>, AzureManagementSubscriptionToPublishSubscribeChannelResource>(typeof(AzureManagementSubscriptionToPublishSubscribeChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>, AzureServiceBusPointToPointChannelResource>(typeof(AzureServiceBusPointToPointChannelResource).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IPointToPointChannel, AzureServiceBusQueue>(typeof(AzureServiceBusQueue).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromPointToPointChannel, AzureServiceBusRequestReplyFromPointToPointChannel>(typeof(AzureServiceBusRequestReplyFromPointToPointChannel).FullName);

            serviceRegistry.Register<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel, AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>(typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName);

            serviceRegistry.Register<IPublishSubscribeChannel, AzureServiceBusTopic>(typeof(AzureServiceBusTopic).FullName);
        }
    }
}
