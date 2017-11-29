using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.ApplicationInsights.Installer
{
    public class ApplicationInsightsRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger<PointToPointChannelInfo>>().ImplementedBy<ApplicationInsightsPointToPointChannelInfoLogger>().Named(typeof(ApplicationInsightsPointToPointChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<HeartBeat>> ().ImplementedBy<ApplicationInsightsHeartBeatLogger>().Named(typeof(ApplicationInsightsHeartBeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<SubscriptionToPublishSubscribeChannelInfo>>().ImplementedBy<ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger>().Named(typeof(ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<PublishSubscribeChannelInfo>>().ImplementedBy<ApplicationInsightsPublishSubscribeChannelInfoLogger>().Named(typeof(ApplicationInsightsPublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware>().ImplementedBy<ApplicationInsightsRouterLogger>().Named(typeof(ApplicationInsightsRouterLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<Interface.Outbound.IMiddleware>().ImplementedBy<ApplicationInsightsBusLogger>().Named(typeof(ApplicationInsightsBusLogger).FullName).LifestyleSingleton());
        }
    }


}
