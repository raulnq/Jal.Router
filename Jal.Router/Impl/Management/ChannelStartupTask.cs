using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class ChannelStartupTask : IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public ChannelStartupTask(IRouterConfigurationSource[] sources, IComponentFactory factory, IConfiguration configuration)
        {
            _sources = sources;
            _factory = factory;
            _configuration = configuration;
        }

        public void Run()
        {
            var channelmanager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

            foreach (var source in _sources)
            {
                var queues = source.GetPointToPointChannels();

                foreach (var queue in queues)
                {
                    CreatePointToPointChannel(queue, channelmanager);
                }
            }

            foreach (var source in _sources)
            {
                var topics = source.GetPublishSubscribeChannels();

                foreach (var topic in topics)
                {
                    CreatePublishSubscriberChannel(topic, channelmanager);
                }
            }

            foreach (var source in _sources)
            {
                var susbcribers = source.GetSubscriptionsToPublishSubscribeChannel();

                foreach (var susbcriber in susbcribers)
                {
                    CreateSubscriptionToPublishSubscribeChannel(susbcriber, channelmanager);
                }
            }
        }

        public void CreateSubscriptionToPublishSubscribeChannel(SubscriptionToPublishSubscribeChannel subscriptionToPublishSubscribeChannel, IChannelManager channelmanager)
        {
            if (subscriptionToPublishSubscribeChannel.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(subscriptionToPublishSubscribeChannel.ConnectionStringExtractorType);

                var toconnectionextractor = subscriptionToPublishSubscribeChannel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null && !string.IsNullOrWhiteSpace(subscriptionToPublishSubscribeChannel.Path) && !string.IsNullOrWhiteSpace(subscriptionToPublishSubscribeChannel.Origin.Key))
                {
                    channelmanager.CreateSubscriptionToPublishSubscribeChannel(toconnectionextractor(extractorconnectionstring), subscriptionToPublishSubscribeChannel.Path, subscriptionToPublishSubscribeChannel.Subscription, subscriptionToPublishSubscribeChannel.Origin.Key);

                    Console.WriteLine($"Created subscription to {toconnectionextractor}/{subscriptionToPublishSubscribeChannel.Path}/{subscriptionToPublishSubscribeChannel.Subscription} publish subscriber channel");
                }
            }

        }

        public void CreatePointToPointChannel(PointToPointChannel pointToPointChannel, IChannelManager channelmanager)
        {
            if (pointToPointChannel.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(pointToPointChannel.ConnectionStringExtractorType);

                var toconnectionextractor = pointToPointChannel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null)
                {
                    channelmanager.CreatePointToPointChannel(toconnectionextractor(extractorconnectionstring), pointToPointChannel.Path);

                    Console.WriteLine($"Created {toconnectionextractor}/{pointToPointChannel.Path} point to point channel");
                }
            }
        }

        public void CreatePublishSubscriberChannel(PublishSubscribeChannel publishSubscribeChannel, IChannelManager channelmanager)
        {
            if (publishSubscribeChannel.ConnectionStringExtractorType != null)
            {
                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(publishSubscribeChannel.ConnectionStringExtractorType);

                var toconnectionextractor = publishSubscribeChannel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null)
                {
                    channelmanager.CreatePublishSubscribeChannel(toconnectionextractor(extractorconnectionstring), publishSubscribeChannel.Path);

                    Console.WriteLine($"Created {toconnectionextractor}/{publishSubscribeChannel.Path} publish subscriber channel");
                }
            }
        }
    }
}