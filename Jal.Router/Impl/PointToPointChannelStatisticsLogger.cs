using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
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
}