using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class ListenerShutdownTask : IShutdownTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        public ListenerShutdownTask(IRouterConfigurationSource[] sources)
        {
            _sources = sources;
        }

        public void Run()
        {
            foreach (var source in _sources)
            {
                foreach (var route in source.GetRoutes())
                {
                    Shutdown(route);
                }

                foreach (var saga in source.GetSagas())
                {
                    Shutdown(saga, saga.StartingRoute);

                    foreach (var nextroute in saga.NextRoutes)
                    {
                        Shutdown(saga, nextroute);
                    }
                }
            }
        }

        private void Shutdown(Route route)
        {
            if (route.ShutdownAction != null)
            {
                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    route.ShutdownAction();

                    Console.WriteLine($"Shutdown {route.Name}/{route.ToPath} point to point channel");
                }

                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    route.ShutdownAction();

                    Console.WriteLine($"Shutdown {route.Name}/{route.ToPath}/{route.ToSubscription} publish subscriber channel");
                }
            }
        }

        private void Shutdown(Saga saga, Route route)
        {
            if (route.ShutdownAction != null)
            {
                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    route.ShutdownAction();

                    Console.WriteLine($"Shutdown {saga.Name}/{route.Name}/{route.ToPath} point to point channel");
                }

                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    route.ShutdownAction();

                    Console.WriteLine($"Shutdown {saga.Name}/{route.Name}/{route.ToPath}/{route.ToSubscription} publish subscriber channel");
                }
            }
        }
    }
}