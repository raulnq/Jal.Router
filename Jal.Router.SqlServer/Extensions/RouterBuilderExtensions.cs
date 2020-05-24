using Jal.Router.SqlServer.Impl;
using Jal.Router.Interface;

namespace Jal.Router.SqlServer
{
    public static class RouterBuilderExtensions
    {
        public static void AddAzureServiceBus(this IRouterBuilder builder)
        {
            builder.AddPointToPointChannel<SqlServerQueue>();

            builder.AddPublishSubscribeChannel<SqlServerTopic>();

            builder.AddSubscriptionToPublishSubscribeChannel<SqlServerSubscription>();
        }
    }
}
