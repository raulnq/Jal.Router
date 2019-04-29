using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class BeatLogger : ILogger<Beat>
    {
        private readonly ILogger _logger;
        public BeatLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(Beat message, DateTime datetime)
        {
            _logger.Log($"{message.Name} - {message.Action}");
        }
    }

    public class PointToPointChannelStatisticsLogger : ILogger<PointToPointChannelStatistics>
    {
        private readonly ILogger _logger;
        public PointToPointChannelStatisticsLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(PointToPointChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _logger.Log($"{statistics.Path} {item.Key}:{item.Value}");
            }
        }
    }

    public class PublishSubscribeChannelStatisticsLogger : ILogger<PublishSubscribeChannelStatistics>
    {
        private readonly ILogger _logger;
        public PublishSubscribeChannelStatisticsLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(PublishSubscribeChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _logger.Log($"{statistics.Path} {item.Key}:{item.Value}");
            }
        }
    }

    public class SubscriptionToPublishSubscribeChannelStatisticsLogger : ILogger<SubscriptionToPublishSubscribeChannelStatistics>
    {
        private readonly ILogger _logger;
        public SubscriptionToPublishSubscribeChannelStatisticsLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(SubscriptionToPublishSubscribeChannelStatistics statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _logger.Log($"{statistics.Path} {item.Key}:{item.Value}");
            }
        }
    }
}