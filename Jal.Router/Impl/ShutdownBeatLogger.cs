using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class ShutdownBeatLogger : ILogger<ShutdownBeat>
    {
        private readonly ILogger _logger;
        public ShutdownBeatLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(ShutdownBeat info, DateTime datetime)
        {
            _logger.Log($"{info.Name} Stopped");
        }
    }
}