using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;
using LightInject;
using IMiddleware = Jal.Router.Interface.Inbound.IMiddleware;

namespace Jal.Router.ApplicationInsights.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterApplicationInsights(this IServiceContainer container)
        {
            container.Register<ILogger<PointToPointChannelInfo>, ApplicationInsightsPointToPointChannelInfoLogger>(typeof(ApplicationInsightsPointToPointChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, ApplicationInsightsHeartBeatLogger>(typeof(ApplicationInsightsHeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<SubscriptionToPublishSubscribeChannelInfo>, ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<PublishSubscribeChannelInfo>, ApplicationInsightsPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, ApplicationInsightsRouterLogger>(typeof(ApplicationInsightsRouterLogger).FullName, new PerContainerLifetime());

            container.Register<Jal.Router.Interface.Outbound.IMiddleware, ApplicationInsightsBusLogger>(typeof(ApplicationInsightsBusLogger).FullName, new PerContainerLifetime());
        }
    }
}
