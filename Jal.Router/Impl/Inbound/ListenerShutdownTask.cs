using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.Inbound
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

        public void Run()
        {
            foreach (var listenermetadata in _configuration.RuntimeInfo.ListenersMetadata)
            {
                if(listenermetadata.CanShutdown())
                {
                    listenermetadata.Shutdown();

                    _logger.Log($"Shutdown {listenermetadata.GetPath()} {listenermetadata.ToString()} channel");
                }
            }
        }
    }
}