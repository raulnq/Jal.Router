using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.ApplicationInsights.Installer
{
    public class ApplicationInsightsRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger<PointToPointChannelInfo>>().ImplementedBy<ApplicationInsightsPointToPointChannelInfoLogger>().Named(typeof(ApplicationInsightsPointToPointChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<Beat>>().ImplementedBy<BeatLogger>().Named(typeof(BeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<SubscriptionToPublishSubscribeChannelInfo>>().ImplementedBy<ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger>().Named(typeof(ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<PublishSubscribeChannelInfo>>().ImplementedBy<ApplicationInsightsPublishSubscribeChannelInfoLogger>().Named(typeof(ApplicationInsightsPublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<RouterLogger>().Named(typeof(RouterLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<BusLogger>().Named(typeof(BusLogger).FullName).LifestyleSingleton());
        }
    }


}
