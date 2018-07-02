using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Management
{
    public class HandlerAndEndpointStartupTask : IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IComponentFactory _factory;

        private readonly ILogger _logger;

        public HandlerAndEndpointStartupTask(IRouterConfigurationSource[] sources, IComponentFactory factory, ILogger logger)
        {
            _sources = sources;

            _factory = factory;

            _logger = logger;
        }

        public void Run()
        {
            var errors = new StringBuilder();

            var routes = new List<Route>();

            var endpoints = new List<EndPoint>();

            foreach (var source in _sources)
            {
                routes.AddRange(source.GetRoutes());

                endpoints.AddRange(source.GetEndPoints());

                foreach (var saga in source.GetSagas())
                {
                    if (saga.StartingRoute != null)
                    {
                        routes.Add(saga.StartingRoute);
                    }
                    if (saga.EndingRoute != null)
                    {
                        routes.Add(saga.EndingRoute);
                    }
                    routes.AddRange(saga.NextRoutes);
                }
            }

            foreach (var endpoint in endpoints)
            {
                if (endpoint.Channels.Any())
                {
                    foreach (var channel in endpoint.Channels)
                    {
                        if (channel.ConnectionStringExtractorType != null)
                        {
                            var finder = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                            if (channel.ToConnectionStringExtractor is Func<IValueSettingFinder, string> extractor)
                            {
                                channel.ToConnectionString = extractor(finder);
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

                        if (channel.ToReplyConnectionStringExtractor != null)
                        {
                            var finder = _factory.Create<IValueSettingFinder>(channel.ReplyConnectionStringExtractorType);

                            if (channel.ToReplyConnectionStringExtractor is Func<IValueSettingFinder, string> extractor)
                            {
                                channel.ToReplyConnectionString = extractor(finder);
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

            foreach (var route in routes)
            {
                if (route.Channels.Any())
                {
                    foreach (var channel in route.Channels)
                    {
                        var finder = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                        var extractor = channel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                        channel.ToConnectionString = extractor?.Invoke(finder);

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

                    _logger.Log(error);

                    _logger.Log(error);
                }

            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }
        }
    }
}