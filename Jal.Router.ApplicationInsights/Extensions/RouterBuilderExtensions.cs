using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.ApplicationInsights
{
    public static class RouterBuilderExtensions
    {
        public static IRouterBuilder AddApplicationInsights(this IRouterBuilder builder)
        {
            builder.AddLogger<PointToPointChannelStatisticsLogger, PointToPointChannelStatistics>();

            builder.AddLogger<BeatLogger, Beat>();

            builder.AddLogger<SubscriptionToPublishSubscribeChannelStatisticsLogger, SubscriptionToPublishSubscribeChannelStatistics>();

            builder.AddLogger<PublishSubscribeChannelStatisticsLogger, PublishSubscribeChannelStatistics>();

            builder.AddMiddleware<RouterLogger>();

            builder.AddMiddleware<BusLogger>();

            return builder;
        }
    }
}
