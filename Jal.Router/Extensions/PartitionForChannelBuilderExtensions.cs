using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl.ValueFinder;
using Jal.Router.Interface;

namespace Jal.Router.Extensions
{
    public static class PartitionForChannelBuilderExtensions
    {
        public static IPartitionUntilBuilder ForPointToPointChannel(this IPartitionForChannelBuilder builder, string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            return builder.ForPointToPointChannel<NullValueFinder>(path, provider);
        }

        public static IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel(this IPartitionForChannelBuilder builder, string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            return builder.ForSubscriptionToPublishSubscribeChannel<NullValueFinder>(path, subscription, provider);
        }


    }
}