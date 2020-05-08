using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Fluent.Interface;
using System.Collections.Generic;

namespace Jal.Router.AzureServiceBus.Standard
{

    public static class RouteChannelBuilderExtensions
    {
        public static IChannelIWhenBuilder AddQueue(this IRouteChannelBuilder builder, string path, string connectionstring, AzureServiceBusChannelConnection connection=null)
        {
            return builder.AddPointToPointChannel(path, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue), connection?.ToDictionary(), "Azure Service Bus");
        }

        public static IChannelIWhenBuilder AddSubscriptionToTopic(this IRouteChannelBuilder builder, string path, string subscription, string connectionstring, AzureServiceBusChannelConnection connection = null)
        {
            return builder.AddSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusSubscription), connection?.ToDictionary(), "Azure Service Bus");
        }
    }
}