using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SenderContextLoader : ISenderContextLoader
    {
        private readonly ISenderContextLifecycle _lifecycle;

        public SenderContextLoader(ISenderContextLifecycle lifecycle)
        {
            _lifecycle = lifecycle;
        }

        public void Add(EndPoint endpoint)
        {
            foreach (var channel in endpoint.Channels)
            {
                var context = _lifecycle.Get(channel);

                if (context == null)
                {
                    context = _lifecycle.Add(endpoint, channel);

                    context.Open();
                }
            }
        }

        public async Task Remove(EndPoint endpoint)
        {
            foreach (var channel in endpoint.Channels)
            {
                var context = _lifecycle.Remove(channel);

                if (context == null)
                {
                    await context.Close().ConfigureAwait(false);
                }
            }
        }
    }
}