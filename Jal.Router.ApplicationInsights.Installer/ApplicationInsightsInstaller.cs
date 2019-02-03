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
    public class ApplicationInsightsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger<PointToPointChannelInfo>>().ImplementedBy<PointToPointChannelInfoLogger>().Named(typeof(PointToPointChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<Beat>>().ImplementedBy<BeatLogger>().Named(typeof(BeatLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<SubscriptionToPublishSubscribeChannelInfo>>().ImplementedBy<SubscriptionToPublishSubscribeChannelInfoLogger>().Named(typeof(SubscriptionToPublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<ILogger<PublishSubscribeChannelInfo>>().ImplementedBy<PublishSubscribeChannelInfoLogger>().Named(typeof(PublishSubscribeChannelInfoLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<RouterLogger>().Named(typeof(RouterLogger).FullName).LifestyleSingleton());

            container.Register(Component.For<IMiddleware<MessageContext>>().ImplementedBy<BusLogger>().Named(typeof(BusLogger).FullName).LifestyleSingleton());
        }
    }


}
