using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class ConsoleStartupBeatLogger : ILogger<StartupBeat>
    {
        public void Log(StartupBeat info, DateTime datetime)
        {
            Console.WriteLine($"{info.Name} Started");
        }
    }
}