using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.AzureServiceBus.Standard.Installer
{
    public class AzureServiceBusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<AzureServiceBusQueue>().Named(typeof(AzureServiceBusQueue).FullName).LifestyleTransient());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<AzureServiceBusTopic>().Named(typeof(AzureServiceBusTopic).FullName).LifestyleTransient());

            container.Register(Component.For<IRequestReplyChannelFromPointToPointChannel>().ImplementedBy<AzureServiceBusRequestReplyFromPointToPointChannel>().Named(typeof(AzureServiceBusRequestReplyFromPointToPointChannel).FullName).LifestyleTransient());

            container.Register(Component.For<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>().ImplementedBy<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>().Named(typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName).LifestyleTransient());

            container.Register(Component.For<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<AzureManagementPointToPointChannelResource>().Named(typeof(AzureManagementPointToPointChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<AzureManagementPublishSubscribeChannelResource>().Named(typeof(AzureManagementPublishSubscribeChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<AzureManagementSubscriptionToPublishSubscribeChannelResource>().Named(typeof(AzureManagementSubscriptionToPublishSubscribeChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<AzureServiceBusPointToPointChannelResource>().Named(typeof(AzureServiceBusPointToPointChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<AzureServiceBusPublishSubscribeChannelResource>().Named(typeof(AzureServiceBusPublishSubscribeChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<AzureServiceBusSubscriptionToPublishSubscribeChannelResource>().Named(typeof(AzureServiceBusSubscriptionToPublishSubscribeChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<AzureServiceBusMessageAdapter>().Named(typeof(AzureServiceBusMessageAdapter).FullName).LifestyleSingleton());
        }
    }
}
