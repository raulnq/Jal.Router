using System;
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

                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(type.Interval).ConfigureAwait(false);

                    while (true)
                    {
                        try
                        {
                            await task.Run(DateTime.UtcNow).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _logger.Log($"Monitor exception {ex}");
                        }

                        await Task.Delay(type.Interval).ConfigureAwait(false);
                    }
                });
            }
        }
    }
}