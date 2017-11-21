using System;
using Common.Logging;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Impl
{
    public class HeartBeatLogger : ILogger<HeartBeat>
    {
        private readonly ILog _log;

        public HeartBeatLogger(ILog log)
        {
            _log = log;
        }

        public void Log(HeartBeat info, DateTime datetime)
        {
            _log.Info($"{info.Name} Running");
        }
    }
}