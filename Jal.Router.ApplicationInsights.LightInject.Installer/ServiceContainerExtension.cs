using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.ApplicationInsights.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterApplicationInsights(this IServiceContainer container)
        {
            container.Register<ILogger<PointToPointChannelInfo>, ApplicationInsightsPointToPointChannelInfoLogger>(typeof(ApplicationInsightsPointToPointChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<SubscriptionToPublishSubscribeChannelInfo>, ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<PublishSubscribeChannelInfo>, ApplicationInsightsPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());
        }
    }
}
