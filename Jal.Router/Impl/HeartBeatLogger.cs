using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class HeartBeatLogger : ILogger<HeartBeat>
    {
        private readonly ILogger _logger;
        public HeartBeatLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(HeartBeat info, DateTime datetime)
        {
            _logger.Log($"{info.Name} Running");
        }
    }
}