using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class RoutesInitializer : AbstractStartupTask, IStartupTask
    {
        public RoutesInitializer(IComponentFactoryGateway factory, ILogger logger)
            :base(factory, logger)
        {
        }

        public Task Run()
        {
            Logger.Log("Initializing routes");

            var errors = new StringBuilder();

            foreach (var partition in Factory.Configuration.Runtime.Partitions)
            {
                var finder = Factory.CreateValueFinder(partition.Channel.ConnectionStringValueFinderType);

                partition.Channel.ToConnectionString = partition.Channel.ToConnectionStringProvider?.Invoke(finder);

                if (string.IsNullOrWhiteSpace(partition.Channel.ToConnectionString))
                {
                    var error = $"Empty connection string, partition {partition.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }


                if (string.IsNullOrWhiteSpace(partition.Channel.ToPath))
                {
                    var error = $"Empty path, partition {partition.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            foreach (var route in Factory.Configuration.Runtime.Routes)
            {
                if (route.Channels.Any())
                {
                    foreach (var channel in route.Channels)
                    {
                        var finder = Factory.CreateValueFinder(channel.ConnectionStringValueFinderType);

                        channel.ToConnectionString = channel.ToConnectionStringProvider?.Invoke(finder);

                        if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                        {
                            var error = $"Empty connection string, Handler {route.Name}";

                            errors.AppendLine(error);

                            Logger.Log(error);
                        }


                        if (string.IsNullOrWhiteSpace(channel.ToPath))
                        {
                            var error = $"Empty path, Handler {route.Name}";

                            errors.AppendLine(error);

                            Logger.Log(error);
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Handler {route.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Routes initialized");

            return Task.CompletedTask;
        }
    }
}