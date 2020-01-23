using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{

    public static class ListenerChannelBuilderExtensions
    {
        public static void AddQueue(this IListenerChannelBuilder builder, string path, string connectionstring)
        {
            builder.AddPointToPointChannel(path, connectionstring);
        }
        public static void AddSubscriptionToTopic(this IListenerChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            builder.AddSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring);
        }
    }
}