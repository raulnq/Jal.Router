using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class StatisticMonitor : AbstractMonitoringTask, IMonitoringTask
    {

        public StatisticMonitor(IComponentFactoryFacade factory, ILogger logger)
            :base(factory, logger)
        {
        }

        public async Task Run(DateTime datetime)
        {
            if (Factory.Configuration.LoggerTypes.ContainsKey(typeof(Statistic)))
            {
                var loggertypes = Factory.Configuration.LoggerTypes[typeof(Statistic)];

                var loggers = loggertypes.Select(x => Factory.CreateLogger<Statistic>(x)).ToArray();

                foreach (var context in Factory.Configuration.Runtime.Contexts)
                {
                    var message = await context.GetStatistic().ConfigureAwait(false);

                    if (message != null)
                    {
                        Array.ForEach(loggers, x => x.Log(message, datetime));
                    }
                }
            }
        }
    }
}