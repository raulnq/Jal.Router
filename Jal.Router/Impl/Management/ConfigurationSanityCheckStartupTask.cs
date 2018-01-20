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

            foreach (var source in _sources)
            {
                routes.AddRange(source.GetRoutes());

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


            foreach (var route in routes)
            {
                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(route.ConnectionStringExtractorType);

                var toconnectionextractor = route.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                route.ToConnectionString = toconnectionextractor?.Invoke(extractorconnectionstring);

                if (string.IsNullOrWhiteSpace(route.ToConnectionString))
                {
                    var error = $"Empty connection string {route.Name}";

                    errors.AppendLine(error);

                    Console.WriteLine(error);
                }
            }

            var groups = routes.GroupBy(x => x.ToPath + x.ToSubscription + x.ToConnectionString);

            foreach (var @group in groups)
            {
                var names = @group.GroupBy(y => y.Name);

                if (names.Count() > 1)
                {
                    var first = @group.First();

                    var error = $"Duplicate route with different name {first.ToConnectionString}/{first.ToPath}/{first.ToSubscription}: {string.Join(",", @group.ToArray().Select(x => x.Name))}";

                    errors.AppendLine(error);

                    Console.WriteLine(error);               
                }
            }


            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }
        }
    }
}