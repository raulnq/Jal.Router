using Jal.Router.Fluent.Interface;

namespace Jal.Router.AzureServiceBus.Standard
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
    }
}