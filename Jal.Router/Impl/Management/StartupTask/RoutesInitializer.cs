using System;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class RoutesInitializer : AbstractStartupTask, IStartupTask
    {
        public RoutesInitializer(IComponentFactory factory, ILogger logger, IConfiguration configuration)
            :base(factory, configuration, logger)
        {
        }

        public void Run()
        {
            Logger.Log("Initializing routes");

            var errors = new StringBuilder();

            foreach (var route in Configuration.Runtime.All)
            {
                if (route.Channels.Any())
                {
                    foreach (var channel in route.Channels)
                    {
                        var finder = Factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                        var provider = channel.ToConnectionStringProvider as Func<IValueFinder, string>;

                        channel.ToConnectionString = provider?.Invoke(finder);

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
                throw new Exception(errors.ToString());
            }

            Logger.Log("Routes initialized");
        }
    }
}