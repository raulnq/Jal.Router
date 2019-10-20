using Jal.Router.Interface;
using System;
using System.Threading;

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
            var id = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"[{DateTime.UtcNow},{id},{_configuration.ApplicationName}] {message}");
        }
    }
}