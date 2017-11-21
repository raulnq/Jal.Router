using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class HeartBeatMonitor : IMonitoringTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public HeartBeatMonitor( IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Run(DateTime datetime)
        {
            if (_configuration.LoggerTypes.ContainsKey(typeof(HeartBeat)))
            {
                var loggertypes = _configuration.LoggerTypes[typeof(HeartBeat)];

                var loggers = loggertypes.Select(x => _factory.Create<ILogger<HeartBeat>>(x)).ToArray();

                Array.ForEach(loggers, x => x.Log(new HeartBeat() {Name = _configuration.ApplicationName }, datetime));
            }
        }
    }
}