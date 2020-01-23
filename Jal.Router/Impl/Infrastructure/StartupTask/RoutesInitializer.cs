using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
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
                if (string.IsNullOrWhiteSpace(partition.Channel.ConnectionString))
                {
                    var error = $"Empty connection string, partition {partition.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }


                if (string.IsNullOrWhiteSpace(partition.Channel.Path))
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
                        if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                        {
                            var error = $"Empty connection string, Handler {route.Name}";

                            errors.AppendLine(error);

                            Logger.Log(error);
                        }


                        if (string.IsNullOrWhiteSpace(channel.Path))
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