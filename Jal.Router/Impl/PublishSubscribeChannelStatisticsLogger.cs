using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
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
}