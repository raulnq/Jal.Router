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

            container.Register(Component.For<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<AzureManagementPointToPointChannelResourceManager>().Named(typeof(AzureManagementPointToPointChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<AzureManagementPublishSubscribeChannelResourceManager>().Named(typeof(AzureManagementPublishSubscribeChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<AzureManagementSubscriptionToPublishSubscribeChannelResourceManager>().Named(typeof(AzureManagementSubscriptionToPublishSubscribeChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>>().ImplementedBy<AzureServiceBusPointToPointChannelResourceManager>().Named(typeof(AzureServiceBusPointToPointChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>().ImplementedBy<AzureServiceBusPublishSubscribeChannelResourceManager>().Named(typeof(AzureServiceBusPublishSubscribeChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager>().Named(typeof(AzureServiceBusSubscriptionToPublishSubscribeChannelResourceManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<AzureServiceBusMessageAdapter>().Named(typeof(AzureServiceBusMessageAdapter).FullName).LifestyleSingleton());
        }
    }
}
