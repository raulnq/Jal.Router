using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.ApplicationInsights.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterApplicationInsights(this IServiceContainer container)
        {
            container.Register<ILogger<PointToPointChannelInfo>, ApplicationInsightsPointToPointChannelInfoLogger>(typeof(ApplicationInsightsPointToPointChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, ApplicationInsightsHeartBeatLogger>(typeof(ApplicationInsightsHeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<StartupBeat>, ApplicationInsightsStartupBeatLogger>(typeof(ApplicationInsightsStartupBeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<SubscriptionToPublishSubscribeChannelInfo>, ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<PublishSubscribeChannelInfo>, ApplicationInsightsPublishSubscribeChannelInfoLogger>(typeof(ApplicationInsightsPublishSubscribeChannelInfoLogger).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, ApplicationInsightsRouterLogger>(typeof(ApplicationInsightsRouterLogger).FullName, new PerContainerLifetime());

            container.Register<Interface.Outbound.IMiddleware, ApplicationInsightsBusLogger>(typeof(ApplicationInsightsBusLogger).FullName, new PerContainerLifetime());
        }
    }
}
