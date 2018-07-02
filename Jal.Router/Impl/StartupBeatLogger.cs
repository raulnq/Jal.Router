using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class StartupBeatLogger : ILogger<StartupBeat>
    {
        private readonly ILogger _logger;
        public StartupBeatLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(StartupBeat info, DateTime datetime)
        {
            _logger.Log($"{info.Name} Started");
        }
    }
}