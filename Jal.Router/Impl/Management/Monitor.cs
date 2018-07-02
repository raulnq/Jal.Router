using System;
using System.Threading;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class Monitor : IMonitor
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public Monitor(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
        }

        public void Start()
        {
            foreach (var type in _configuration.MonitoringTaskTypes)
            {
                var task = _factory.Create<IMonitoringTask>(type.Type);

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            task.Run(DateTime.UtcNow);
                        }
                        catch (Exception ex)
                        {
                            _logger.Log($"Monitor exception {ex}");
                        }
                        Thread.Sleep(type.Interval);
                    }
                });
            }
        }
    }
}