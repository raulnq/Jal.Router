using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Management
{
    public class ConfigurationSanityCheckStartupTask : IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IComponentFactory _factory;

        public ConfigurationSanityCheckStartupTask(IRouterConfigurationSource[] sources, IComponentFactory factory)
        {
            _sources = sources;

            _factory = factory;
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
                foreach (var channel in endpoint.Channels)
                {
                    if (channel.ConnectionStringExtractorType != null)
                    {
                        var finder = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                        var extractor = channel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                        if (extractor != null)
                        {
                            channel.ToConnectionString = extractor(finder);
                        }

                        if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                        {
                            var error = $"Empty connection string {endpoint.Name}";

                            errors.AppendLine(error);

                            Console.WriteLine(error);
                        }
                    }

                    if (channel.ToReplyConnectionStringExtractor != null)
                    {
                        var finder = _factory.Create<IValueSettingFinder>(channel.ReplyConnectionStringExtractorType);

                        var extractor = channel.ToReplyConnectionStringExtractor as Func<IValueSettingFinder, string>;

                        if (extractor != null)
                        {
                            channel.ToReplyConnectionString = extractor(finder);
                        }

                        if (string.IsNullOrWhiteSpace(channel.ToReplyConnectionString))
                        {
                            var error = $"Empty reply connection string {endpoint.Name}";

                            errors.AppendLine(error);

                            Console.WriteLine(error);
                        }
                    }
                }
            }

            foreach (var route in routes)
            {
                foreach (var channel in route.Channels)
                {
                    var extractorconnectionstring = _factory.Create<IValueSettingFinder>(channel.ConnectionStringExtractorType);

                    var toconnectionextractor = channel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                    channel.ToConnectionString = toconnectionextractor?.Invoke(extractorconnectionstring);

                    if (string.IsNullOrWhiteSpace(channel.ToConnectionString))
                    {
                        var error = $"Empty connection string {route.Name}";

                        errors.AppendLine(error);

                        Console.WriteLine(error);
                    }
                }
            }

            var counters = new Dictionary<string, List<string>>();

            foreach (var route in routes)
            {
                foreach (var channel in route.Channels)
                {
                    if (!counters.ContainsKey($"{channel.ToConnectionString}/{channel.ToPath}/{channel.ToSubscription}"))
                        counters.Add($"{channel.ToConnectionString}/{channel.ToPath}/{channel.ToSubscription}", new List<string> { route.Name });
                    else
                        counters[$"{channel.ToConnectionString}/{channel.ToPath}/{channel.ToSubscription}"].Add(route.Name);
                }
            }

            var groups = counters.Where(x => x.Value.Count > 1);

            foreach (var @group in groups)
            {
                var error = $"Duplicate route with different name {@group.Key}: {string.Join(",", @group.Value.ToArray())}";

                errors.AppendLine(error);

                Console.WriteLine(error);
            }
            
            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }
        }
    }
}