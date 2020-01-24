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

        public IPartitionUntilBuilder ForPointToPointChannel(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            _partition.UpdateChannel(new Channel(ChannelType.PointToPoint, connectionstring, path));

            return this;
        }

        public IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _partition.UpdateChannel(new Channel(ChannelType.SubscriptionToPublishSubscribe, connectionstring, path, subscription));

            return this;
        }

        public void Until(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _partition.UpdateUntil(condition);
        }
    }
}