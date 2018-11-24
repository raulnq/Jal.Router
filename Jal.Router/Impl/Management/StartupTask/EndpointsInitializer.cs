using System;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{

    public class EndpointsInitializer : AbstractStartupTask, IStartupTask
    {
        public EndpointsInitializer(IComponentFactory factory, ILogger logger, IConfiguration configuration)
            :base(factory, configuration, logger)
        {
        }

        public void Run()
        {
            Logger.Log("Initializing endpoints");

            var errors = new StringBuilder();

            foreach (var endpoint in Configuration.Runtime.EndPoints)
            {
                if (endpoint.Channels.Any())
                {
                    foreach (var channel in endpoint.Channels)
                    {
                        if (channel.ConnectionStringValueFinderType != null)
                        {
                            var finder = Factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                            if (channel.ToConnectionStringProvider is Func<IValueFinder, string> provider)
                            {
                                channel.ToConnectionString = provider(finder);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                            {
                                var error = $"Empty connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToPath))
                            {
                                var error = $"Empty path, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }
                        }

                        if (channel.ToReplyConnectionStringProvider != null)
                        {
                            var finder = Factory.Create<IValueFinder>(channel.ReplyConnectionStringValueFinderType);

                            if (channel.ToReplyConnectionStringProvider is Func<IValueFinder, string> provider)
                            {
                                channel.ToReplyConnectionString = provider(finder);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToReplyConnectionString))
                            {
                                var error = $"Empty reply connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToReplyPath))
                            {
                                var error = $"Empty reply path, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Endpoint {endpoint.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new Exception(errors.ToString());
            }

            Logger.Log("Endpoints initialized");
        }
    }
}