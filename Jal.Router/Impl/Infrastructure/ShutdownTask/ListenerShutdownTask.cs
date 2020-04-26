using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
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
            foreach (var listenercontext in _configuration.Runtime.ListenerContexts)
            {
                if (await listenercontext.Close().ConfigureAwait(false))
                {
                    _logger.Log($"Shutdown {listenercontext.ToString()}");
                }
            }
        }
    }
}