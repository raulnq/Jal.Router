using Jal.Router.ApplicationInsights.Impl;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.ApplicationInsights.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseApplicationInsights(this IConfiguration configuration)
        {
            return configuration
                .AddOutboundMiddleware<BusLogger>()
                .AddInboundMiddleware<RouterLogger>()
                .AddLogger<BeatLogger, Beat>()
                .AddLogger<PointToPointChannelStatisticsLogger, PointToPointChannelStatistics>()
                .AddLogger<PublishSubscribeChannelStatisticsLogger, PublishSubscribeChannelStatistics>()
                .AddLogger<SubscriptionToPublishSubscribeChannelStatisticsLogger, SubscriptionToPublishSubscribeChannelStatistics>();
        }
    }
}