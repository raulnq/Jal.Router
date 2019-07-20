using System;
using Common.Logging;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Logger.Impl
{
    public class BeatLogger : ILogger<Beat>
    {
        private readonly ILog _log;

        public BeatLogger(ILog log)
        {
            _log = log;
        }

        public void Log(Beat message, DateTime datetime)
        {
            _log.Info($"{message.Name} - {message.Action}");
        }
    }
}