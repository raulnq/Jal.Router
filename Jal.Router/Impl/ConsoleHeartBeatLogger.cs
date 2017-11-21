using System;
using Jal.Router.Interface;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public class ConsoleHeartBeatLogger : ILogger<HeartBeat>
    {
        public void Log(HeartBeat info, DateTime datetime)
        {
            Console.WriteLine($"{info.Name} Running");
        }
    }
}