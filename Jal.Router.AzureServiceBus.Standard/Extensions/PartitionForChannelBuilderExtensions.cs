using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Extensions;

namespace Jal.Router.AzureServiceBus.Standard.Extensions
{
    public static class PartitionForChannelBuilderExtensions
    {
        public static IPartitionUntilBuilder ForQueue(this IPartitionForChannelBuilder builder, string path, string connectionstring)
        {
            return builder.ForPointToPointChannel(path, connectionstring);
        }

        public static IPartitionUntilBuilder ForSubscriptionToTopic(this IPartitionForChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            return builder.ForSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring);
        }

        public static void ForQueue<TValueFinder>(this IPartitionForChannelBuilder builder, string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            builder.ForPointToPointChannel<TValueFinder>(path, connectionstringprovider);
        }
        public static void ForSubscriptionToTopic<TValueFinder>(this IPartitionForChannelBuilder builder, string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            builder.ForSubscriptionToPublishSubscribeChannel<TValueFinder>(path, subscription, connectionstringprovider);
        }
    }
}