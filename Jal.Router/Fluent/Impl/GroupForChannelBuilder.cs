using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class GroupForChannelBuilder : IGroupForChannelBuilder, IGroupUntilBuilder
    {
        private Group _group;

        public GroupForChannelBuilder(Group group)
        {
            _group = group;
        }

        public IGroupUntilBuilder ForPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            _group.Channel = new Channel(ChannelType.PointToPoint)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringprovider,

                ConnectionStringValueFinderType = typeof(TValueFinder)
            };

            _group.Until = x => false;

            return this;
        }

        public IGroupUntilBuilder ForSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
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

            _group.Channel = new Channel(ChannelType.PublishSubscribe)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringprovider,

                ConnectionStringValueFinderType = typeof(TValueFinder),

                ToSubscription = subscription
            };

            _group.Until = x => false;

            return this;
        }

        public void Until(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _group.Until = condition;
        }
    }
}