using System;
using System.Linq;
using System.Text;
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

        private readonly ILogger _logger;

        public ChannelStartupTask(IRouterConfigurationSource[] sources, IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            _sources = sources;
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
        }

        public void Run()
        {
            var errors = new StringBuilder();

            var channelmanager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

            foreach (var source in _sources)
            {
                var queues = source.GetPointToPointChannels();

                foreach (var queue in queues)
                {
                    CreatePointToPointChannel(queue, channelmanager, errors);
                }
            }

            foreach (var source in _sources)
            {
                var topics = source.GetPublishSubscribeChannels();

                foreach (var topic in topics)
                {
                    CreatePublishSubscriberChannel(topic, channelmanager, errors);
                }
            }

            foreach (var source in _sources)
            {
                var subscriptions = source.GetSubscriptionsToPublishSubscribeChannel();

                foreach (var subscription in subscriptions)
                {
                    CreateSubscriptionToPublishSubscribeChannel(subscription, channelmanager, errors);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }
        }

        public void CreateSubscriptionToPublishSubscribeChannel(SubscriptionToPublishSubscribeChannel subscription, IChannelManager manager, StringBuilder errors)
        {
            if (subscription.ConnectionStringExtractorType != null)
            {
                var finder = _factory.Create<IValueSettingFinder>(subscription.ConnectionStringExtractorType);

                var extractor = subscription.ConnectionStringExtractor as Func<IValueSettingFinder, string>;

                subscription.ConnectionString = extractor?.Invoke(finder);

                if (string.IsNullOrWhiteSpace(subscription.ConnectionString))
                {
                    var error = $"Empty connection string, subscription to publish subscriber channel {subscription.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                if (string.IsNullOrWhiteSpace(subscription.Path))
                {
                    var error = $"Empty path, subscription to publish subscriber channel {subscription.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                if (string.IsNullOrWhiteSpace(subscription.Subscription))
                {
                    var error = $"Empty subscription, subscription to publish subscriber channel {subscription.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                if (subscription.Rules.Count==0)
                {
                    var error = $"Missing rules, subscription to publish subscriber channel {subscription.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                try
                {
                    var created = manager.CreateIfNotExistSubscriptionToPublishSubscribeChannel(subscription.ConnectionString, subscription.Path, subscription.Subscription, subscription.Rules.FirstOrDefault());

                    if (created)
                    {
                        _logger.Log($"Created subscription to {subscription.Path}/{subscription.Subscription} publish subscriber channel");
                    }
                    else
                    {
                        _logger.Log($"Subscription to publish subscriber channel {subscription.Path}/{subscription.Subscription} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {subscription.Path}/{subscription.Subscription} subscription to publish subscriber channel: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }
        }

        public void CreatePointToPointChannel(PointToPointChannel channel, IChannelManager manager, StringBuilder errors)
        {
            if (channel.ConnectionStringExtractorType != null)
            {
                var finder = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                var extractor = channel.ConnectionStringExtractor as Func<IValueSettingFinder, string>;

                channel.ConnectionString = extractor?.Invoke(finder);

                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                try
                {
                    var created = manager.CreateIfNotExistPointToPointChannel(channel.ConnectionString, channel.Path);

                    if (created)
                    {
                        _logger.Log($"Created {channel.Path} point to point channel");
                    }
                    else
                    {
                        _logger.Log($"Point to point channel {channel.Path} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Path} point to point channel: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }
        }

        public void CreatePublishSubscriberChannel(PublishSubscribeChannel channel, IChannelManager manager, StringBuilder errors)
        {
            if (channel.ConnectionStringExtractorType != null)
            {
                var finder = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                var extractor = channel.ConnectionStringExtractor as Func<IValueSettingFinder, string>;

                channel.ConnectionString = extractor?.Invoke(finder);

                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, publish subscriber channel {channel.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, publish subscriber channel {channel.Path}";

                    errors.AppendLine(error);

                    _logger.Log(error);

                    return;
                }

                try
                {
                    var created = manager.CreateIfNotExistPublishSubscribeChannel(channel.ConnectionString, channel.Path);

                    if (created)
                    {
                        _logger.Log($"Created {channel.Path} publish subscriber channel");
                    }
                    else
                    {
                        _logger.Log($"Publish subscriber channel {channel.Path} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Path} publish subscriber channel: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }
        }
    }
}