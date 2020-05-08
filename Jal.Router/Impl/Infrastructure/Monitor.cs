using System;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class Monitor : IMonitor
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public Monitor(IComponentFactoryFacade factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }

        public void Run()
        {
            foreach (var type in _factory.Configuration.MonitoringTaskTypes)
            {
                var task = _factory.CreateMonitoringTask(type.Type);

                Task.Factory.StartNew(async () =>
                {
                    if(!type.RunImmediately)
                    {
                        await Task.Delay(type.IntervalInMilliSeconds).ConfigureAwait(false);
                    }

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

                        await Task.Delay(type.IntervalInMilliSeconds).ConfigureAwait(false);
                    }
                });
            }
        }
    }
}