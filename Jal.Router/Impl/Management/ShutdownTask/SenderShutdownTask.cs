using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Inbound
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

        public void Run()
        {
            foreach (var sendermetadata in _configuration.Runtime.SendersMetadata)
            {
                if (sendermetadata.DestroySenderMethod != null)
                {
                    sendermetadata.DestroySenderMethod(sendermetadata.Sender);

                    _logger.Log($"Shutdown {sendermetadata.Channel.GetPath()} {sendermetadata.Channel.ToString()} channel");
                }
            }
        }
    }
}