using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using l = Serilog.Log;

namespace Jal.Router.Serilog.Impl
{
    public class BeatLogger : ILogger<Beat>
    {

        public void Log(Beat message, DateTime datetime)
        {
            l.Information("{name} - {action}", message.Name, message.Action);
        }
    }
}