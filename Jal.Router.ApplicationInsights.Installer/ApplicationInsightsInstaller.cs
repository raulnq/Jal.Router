using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability;
using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.ApplicationInsights.Installer
{
    public class ApplicationInsightsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger<PointToPointChannelStatistics>>().ImplementedBy<PointToPointChannelStatisticsLogger>().Named(typeof(PointToPointChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<Beat>>().ImplementedBy<BeatLogger>().Named(typeof(BeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<SubscriptionToPublishSubscribeChannelStatistics>>().ImplementedBy<SubscriptionToPublishSubscribeChannelStatisticsLogger>().Named(typeof(SubscriptionToPublishSubscribeChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<PublishSubscribeChannelStatistics>>().ImplementedBy<PublishSubscribeChannelStatisticsLogger>().Named(typeof(PublishSubscribeChannelStatisticsLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IAsyncMiddleware<MessageContext>>().ImplementedBy<RouterLogger>().Named(typeof(RouterLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IAsyncMiddleware<MessageContext>>().ImplementedBy<BusLogger>().Named(typeof(BusLogger).FullName).LifestyleSingleton());
        }
    }


}
