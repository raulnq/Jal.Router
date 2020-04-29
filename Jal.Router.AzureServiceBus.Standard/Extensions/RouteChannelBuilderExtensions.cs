using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class RouteChannelBuilderExtensions
    {
        public static IChannelIWhenBuilder AddQueue(this IRouteChannelBuilder builder, string path, string connectionstring)
        {
            return builder.AddPointToPointChannel(path, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue) );
        }
        public static IChannelIWhenBuilder AddSubscriptionToTopic(this IRouteChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            return builder.AddSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }
    }
}