using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using System.Threading.Tasks;

namespace Jal.Router.Impl.ShutdownTask
{
    public class SenderShutdownTask : IShutdownTask
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public SenderShutdownTask(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Run()
        {
            foreach (var sendercontext in _configuration.Runtime.SenderContexts)
            {
                if (sendercontext.SenderChannel != null)
                {
                    await sendercontext.SenderChannel.Close(sendercontext).ConfigureAwait(false);

                    _logger.Log($"Shutdown {sendercontext.Id}");
                }
            }
        }
    }
}