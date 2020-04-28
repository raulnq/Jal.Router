using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLoader : IListenerContextLoader
    {
        private readonly IListenerContextLifecycle _lifecycle;

        public ListenerContextLoader(IListenerContextLifecycle lifecycle)
        {
            _lifecycle = lifecycle;
        }

        public void Add(Route route)
        {
            foreach (var channel in route.Channels)
            {
                var listenercontext = _lifecycle.Get(channel);

                if (listenercontext == null)
                {
                    listenercontext = _lifecycle.Add(route, channel);

                    listenercontext.Open();
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
                    await listenercontext.Close().ConfigureAwait(false);
                }
            }
        }
    }
}