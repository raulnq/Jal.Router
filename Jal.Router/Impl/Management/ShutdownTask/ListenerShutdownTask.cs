using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using System.Threading.Tasks;

namespace Jal.Router.Impl.ShutdownTask
{
    public class ListenerShutdownTask : IShutdownTask
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public ListenerShutdownTask(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Run()
        {
            foreach (var metadata in _configuration.Runtime.ListenersMetadata)
            {
                if (metadata.Listener != null)
                {
                    await metadata.Listener.Close().ConfigureAwait(false);

                    _logger.Log($"Shutdown {metadata.Signature()}");
                }
            }
        }
    }
}