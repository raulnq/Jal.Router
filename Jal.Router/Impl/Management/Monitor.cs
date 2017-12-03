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

        public Monitor(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
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
                            Console.WriteLine(ex.Message);
                        }
                        Thread.Sleep(type.Interval);
                    }
                });
            }
        }
    }
}