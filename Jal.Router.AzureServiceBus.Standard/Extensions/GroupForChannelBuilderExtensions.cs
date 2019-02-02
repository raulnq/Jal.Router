using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class GroupForChannelBuilderExtensions
    {
        public static IGroupUntilBuilder ForQueue(this IGroupForChannelBuilder builder, string path, string connectionstring)
        {
            return builder.ForPointToPointChannel(path, connectionstring);
        }

        public static IGroupUntilBuilder ForSubscriptionToTopic(this IGroupForChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            return builder.ForSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring);
        }

        public static void ForQueue<TValueFinder>(this IGroupForChannelBuilder builder, string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            builder.ForPointToPointChannel<TValueFinder>(path, connectionstringprovider);
        }
        public static void ForSubscriptionToTopic<TValueFinder>(this IGroupForChannelBuilder builder, string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            builder.ForSubscriptionToPublishSubscribeChannel<TValueFinder>(path, subscription, connectionstringprovider);
        }
    }
}