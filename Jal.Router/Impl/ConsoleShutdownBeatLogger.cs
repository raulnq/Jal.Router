using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class ConsoleShutdownBeatLogger : ILogger<ShutdownBeat>
    {
        public void Log(ShutdownBeat info, DateTime datetime)
        {
            Console.WriteLine($"{info.Name} Stopped");
        }
    }
}