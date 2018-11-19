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
            foreach (var listenermetadata in _configuration.Runtime.ListenersMetadata)
            {
                if(listenermetadata.DestroyListenerMethod!=null)
                {
                    listenermetadata.DestroyListenerMethod(listenermetadata.Listener);

                    _logger.Log($"Shutdown {listenermetadata.GetPath()} {listenermetadata.ToString()} channel");
                }
            }
        }
    }
}