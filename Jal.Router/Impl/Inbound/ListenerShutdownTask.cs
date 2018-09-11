using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class ListenerShutdownTask : IShutdownTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly ILogger _logger;

        public ListenerShutdownTask(IRouterConfigurationSource[] sources, ILogger logger)
        {
            _sources = sources;
            _logger = logger;
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
                    Shutdown(saga.FirstRoute);

                    foreach (var routes in saga.Routes)
                    {
                        Shutdown(routes);
                    }

                    Shutdown(saga.LastRoute);
                }
            }
        }

        private void Shutdown(Route route)
        {
            foreach (var channel in route.Channels)
            {
                if (channel.IsActive())
                {
                    channel.Shutdown();

                    _logger.Log($"Shutdown {channel.GetPath()} {channel.ToString()} channel");
                }
            }
        }
    }
}