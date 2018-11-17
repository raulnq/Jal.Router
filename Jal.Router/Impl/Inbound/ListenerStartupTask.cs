using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{

    public class ListenerStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        private readonly ISagaExecutionCoordinator _sec;

        private readonly ILogger _logger;

        public ListenerStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaExecutionCoordinator sec)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
            _router = router;
            _logger = logger;
            _sec = sec;
        }

        public void Run()
        {
            var pointtopointchannel = _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);

            var publishsubscriberchannel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

            foreach (var listenermetadata in _configuration.RuntimeInfo.ListenersMetadata)
            {
                if (listenermetadata.IsPointToPoint())
                {
                    pointtopointchannel.Listen(listenermetadata);

                    _logger.Log($"Listening {listenermetadata.GetPath()} {listenermetadata.ToString()} channel ({listenermetadata.Handlers.Count}): {string.Join(",", listenermetadata.Names)}");
                }

                if (listenermetadata.IsPublishSubscriber())
                {
                    publishsubscriberchannel.Listen(listenermetadata);

                    _logger.Log($"Listening {listenermetadata.GetPath()} {listenermetadata.ToString()} channel ({listenermetadata.Handlers.Count}): {string.Join(",", listenermetadata.Names)}");
                }
            }
        }
    }
}