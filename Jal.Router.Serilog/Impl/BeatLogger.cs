using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using l = Serilog.Log;

namespace Jal.Router.Serilog
{
    public class BeatLogger : ILogger<Beat>
    {

        public void Log(Beat message, DateTime datetime)
        {
            l.ForContext("Type", $"job-{message.Action.ToLower()}").Information("{Name} - {Action}", message.Name, message.Action);
        }
    }
}