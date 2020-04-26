using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLoader : IListenerContextLoader
    {
        private readonly IListenerContextLifecycle _lifecycle;

        private readonly ILogger _logger;

        public ListenerContextLoader(IListenerContextLifecycle lifecycle, ILogger logger)
        {
            _lifecycle = lifecycle;

            _logger = logger;
        }

        public void Add(Route route)
        {
            foreach (var channel in route.Channels)
            {
                var listenercontext = _lifecycle.Get(channel);

                if (listenercontext == null)
                {
                    listenercontext = _lifecycle.Add(route, channel);

                    if (listenercontext.Open())
                    {
                        _logger.Log($"Listening {listenercontext.ToString()}");
                    }
                }
            }
        }

        public async Task Remove(Route route)
        {
            foreach (var channel in route.Channels)
            {
                var listenercontext = _lifecycle.Remove(channel);

                if (listenercontext != null)
                {
                    if (await listenercontext.Close().ConfigureAwait(false))
                    {
                        _logger.Log($"Shutdown {listenercontext.ToString()}");
                    }
                }
            }
        }
    }
}