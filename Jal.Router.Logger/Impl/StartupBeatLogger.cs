using System;
using Common.Logging;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Impl
{
    public class StartupBeatLogger : ILogger<StartupBeat>
    {
        private readonly ILog _log;

        public StartupBeatLogger(ILog log)
        {
            _log = log;
        }

        public void Log(StartupBeat info, DateTime datetime)
        {
            _log.Info($"{info.Name} Started");
        }
    }
}