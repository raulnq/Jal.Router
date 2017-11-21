using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.ApplicationInsights.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UsingApplicationInsights(this IConfiguration configuration)
        {
            configuration.AddOutboundMiddleware<ApplicationInsightsBusLogger>();

            configuration.AddInboundMiddleware<ApplicationInsightsRouterLogger>();

            configuration.AddLogger<ApplicationInsightsHeartBeatLogger, HeartBeat>();

            configuration.AddLogger<ApplicationInsightsPointToPointChannelInfoLogger, PointToPointChannelInfo>();

            configuration.AddLogger<ApplicationInsightsPublishSubscribeChannelInfoLogger, PublishSubscribeChannelInfo>();

            configuration.AddLogger<ApplicationInsightsSubscriptionToPublishSubscribeChannelInfoLogger, SubscriptionToPublishSubscribeChannelInfo>();
        }
    }
}