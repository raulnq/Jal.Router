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
                    try
                    {
                        var connectionstring = toconnectionextractor(extractorconnectionstring);

                        var created = channelmanager.CreateIfNotExistSubscriptionToPublishSubscribeChannel(connectionstring, subscriptionToPublishSubscribeChannel.Path, subscriptionToPublishSubscribeChannel.Subscription, subscriptionToPublishSubscribeChannel.Origin.Key);

                        if (created)
                        {
                            Console.WriteLine($"Created subscription to {subscriptionToPublishSubscribeChannel.Path}/{subscriptionToPublishSubscribeChannel.Subscription} publish subscriber channel");
                        }
                        else
                        {
                            Console.WriteLine($"Subscription to publish subscriber channel {subscriptionToPublishSubscribeChannel.Path}/{subscriptionToPublishSubscribeChannel.Subscription} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {subscriptionToPublishSubscribeChannel.Path}/{subscriptionToPublishSubscribeChannel.Subscription} subscription to publish subscriber channel: {ex}");
                    }
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
                    try
                    {
                        var connectionstring = toconnectionextractor(extractorconnectionstring);

                        var created = channelmanager.CreateIfNotExistPointToPointChannel(connectionstring, pointToPointChannel.Path);

                        if (created)
                        {
                            Console.WriteLine($"Created {pointToPointChannel.Path} point to point channel");
                        }
                        else
                        {
                            Console.WriteLine($"Point to point channel {pointToPointChannel.Path} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {pointToPointChannel.Path} point to point channel: {ex}");
                    }

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
                    try
                    {
                        var connectionstring = toconnectionextractor(extractorconnectionstring);

                        var created = channelmanager.CreateIfNotExistPublishSubscribeChannel(connectionstring, publishSubscribeChannel.Path);

                        if (created)
                        {
                            Console.WriteLine($"Created {publishSubscribeChannel.Path} publish subscriber channel");
                        }
                        else
                        {
                            Console.WriteLine($"Publish subscriber channel {publishSubscribeChannel.Path} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {publishSubscribeChannel.Path} publish subscriber channel: {ex}");
                    }
                }
            }
        }
    }
}