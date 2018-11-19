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
}