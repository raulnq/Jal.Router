using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using System;

namespace Jal.Router.Impl
{
    public class ConsoleLogger : ILogger
    {
        private readonly IConfiguration _configuration;

        public ConsoleLogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow},{_configuration.ApplicationName}] {message}");
        }
    }
}