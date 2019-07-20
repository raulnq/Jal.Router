using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class Monitor : IMonitor
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public Monitor(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }

        public void Start()
        {
            foreach (var type in _factory.Configuration.MonitoringTaskTypes)
            {
                var task = _factory.CreateMonitoringTask(type.Type);

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