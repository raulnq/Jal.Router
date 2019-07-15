using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class PartitionForChannelBuilder : IPartitionForChannelBuilder, IPartitionUntilBuilder
    {
        private Partition _partition;

        public PartitionForChannelBuilder(Partition partition)
        {
            _partition = partition;
        }

        public IPartitionUntilBuilder ForPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            _partition.Channel = new Channel(ChannelType.PointToPoint, typeof(TValueFinder), connectionstringprovider, path);

            _partition.Until = x => false;

            return this;
        }

        public IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _partition.Channel = new Channel(ChannelType.SubscriptionToPublishSubscribe, typeof(TValueFinder), 
                connectionstringprovider, path, subscription);

            _partition.Until = x => false;

            return this;
        }

        public void Until(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _partition.Until = condition;
        }
    }
}