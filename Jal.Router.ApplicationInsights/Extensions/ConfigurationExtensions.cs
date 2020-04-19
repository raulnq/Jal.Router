using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.ApplicationInsights
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