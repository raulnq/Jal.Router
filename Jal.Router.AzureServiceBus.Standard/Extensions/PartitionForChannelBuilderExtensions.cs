using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class PartitionForChannelBuilderExtensions
    {
        public static IPartitionUntilBuilder ForQueue(this IPartitionForChannelBuilder builder, string path, string connectionstring)
        {
            return builder.ForPointToPointChannel(path, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusQueue));
        }

        public static IPartitionUntilBuilder ForSubscriptionToTopic(this IPartitionForChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            return builder.ForSubscriptionToPublishSubscribeChannel(path, subscription, connectionstring, typeof(AzureServiceBusMessageAdapter), typeof(AzureServiceBusTopic));
        }
    }
}