using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class StatisticLogger : ILogger<Statistic>
    {
        private readonly ILogger _logger;
        public StatisticLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(Statistic statistics, DateTime datetime)
        {
            foreach (var item in statistics.Properties)
            {
                _logger.Log($"{statistics.Path} {item.Key}:{item.Value}");
            }
        }
    }
}