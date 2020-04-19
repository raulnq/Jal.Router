using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SenderContextLoader : ISenderContextLoader
    {
        private readonly ISenderContextLifecycle _lifecycle;

        private readonly ILogger _logger;

        public SenderContextLoader(ISenderContextLifecycle lifecycle, ILogger logger)
        {
            _lifecycle = lifecycle;

            _logger = logger;
        }

        public void Add(EndPoint endpoint)
        {
            foreach (var channel in endpoint.Channels)
            {
                var sendercontext = _lifecycle.Get(channel);

                if (sendercontext == null)
                {
                    sendercontext = _lifecycle.Add(channel);

                    if (sendercontext.Open())
                    {
                        _logger.Log($"Opening {sendercontext.Id}");
                    }
                }

                sendercontext.Endpoints.Add(endpoint);
            }
        }

        public async Task Remove(EndPoint endpoint)
        {
            foreach (var channel in endpoint.Channels)
            {
                var sendercontext = _lifecycle.Remove(channel);

                if (sendercontext == null)
                {
                    if (await sendercontext.Close().ConfigureAwait(false))
                    {
                        _logger.Log($"Shutdown {sendercontext.Id}");
                    }
                }
            }
        }
    }
}