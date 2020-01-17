using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{

    public class EndpointsInitializer : AbstractStartupTask, IStartupTask
    {
        public EndpointsInitializer(IComponentFactoryGateway factory, ILogger logger)
            :base(factory, logger)
        {
        }

        public Task Run()
        {
            Logger.Log("Initializing endpoints");

            var errors = new StringBuilder();

            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                if (endpoint.Channels.Any())
                {
                    foreach (var channel in endpoint.Channels)
                    {
                        if (channel.ConnectionStringValueFinderType != null)
                        {
                            var finder = Factory.CreateValueFinder(channel.ConnectionStringValueFinderType);

                            if (channel.ConnectionStringProvider!=null)
                            {
                                channel.UpdateConnectionString(channel.ConnectionStringProvider(finder));
                            }

                            if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                            {
                                var error = $"Empty connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.Path))
                            {
                                var error = $"Empty path, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }
                        }

                        if (channel.ReplyConnectionStringProvider != null)
                        {
                            var finder = Factory.CreateValueFinder(channel.ReplyConnectionStringValueFinderType);

                            if (channel.ReplyConnectionStringProvider!=null)
                            {
                                channel.UpdateReplyConnectionString(channel.ReplyConnectionStringProvider(finder));
                            }

                            if (string.IsNullOrWhiteSpace(channel.ReplyConnectionString))
                            {
                                var error = $"Empty reply connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                Logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ReplyPath))
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
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Endpoints initialized");

            return Task.CompletedTask;
        }
    }
}