using System;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Management
{
    public class HandlerAndEndpointStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public HandlerAndEndpointStartupTask(IComponentFactory factory, ILogger logger, IConfiguration configuration)
        {
            _factory = factory;

            _logger = logger;

            _configuration = configuration;
        }

        public void Run()
        {
            var errors = new StringBuilder();

            EvaluateEndpoints(errors);

            EvaluateRoutes(errors);

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }
        }

        private void EvaluateEndpoints(StringBuilder errors)
        {
            foreach (var endpoint in _configuration.RuntimeInfo.EndPoints)
            {
                if (endpoint.Channels.Any())
                {
                    foreach (var channel in endpoint.Channels)
                    {
                        if (channel.ConnectionStringValueFinderType != null)
                        {
                            var finder = _factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                            if (channel.ToConnectionStringProvider is Func<IValueFinder, string> provider)
                            {
                                channel.ToConnectionString = provider(finder);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                            {
                                var error = $"Empty connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                _logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToPath))
                            {
                                var error = $"Empty path, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                _logger.Log(error);
                            }
                        }

                        if (channel.ToReplyConnectionStringProvider != null)
                        {
                            var finder = _factory.Create<IValueFinder>(channel.ReplyConnectionStringValueFinderType);

                            if (channel.ToReplyConnectionStringProvider is Func<IValueFinder, string> provider)
                            {
                                channel.ToReplyConnectionString = provider(finder);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToReplyConnectionString))
                            {
                                var error = $"Empty reply connection string, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                _logger.Log(error);
                            }

                            if (string.IsNullOrWhiteSpace(channel.ToReplyPath))
                            {
                                var error = $"Empty reply path, Endpoint {endpoint.Name}";

                                errors.AppendLine(error);

                                _logger.Log(error);
                            }
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Endpoint {endpoint.Name}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }
        }

        private void EvaluateRoutes(StringBuilder errors)
        {
            foreach (var route in _configuration.RuntimeInfo.RoutesMetadata)
            {
                if (route.Route.Channels.Any())
                {
                    foreach (var channel in route.Route.Channels)
                    {
                        var finder = _factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                        var provider = channel.ToConnectionStringProvider as Func<IValueFinder, string>;

                        channel.ToConnectionString = provider?.Invoke(finder);

                        if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                        {
                            var error = $"Empty connection string, Handler {route.Name}";

                            errors.AppendLine(error);

                            _logger.Log(error);
                        }


                        if (string.IsNullOrWhiteSpace(channel.ToPath))
                        {
                            var error = $"Empty path, Handler {route.Name}";

                            errors.AppendLine(error);

                            _logger.Log(error);
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Handler {route.Name}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }

            }
        }
    }
}