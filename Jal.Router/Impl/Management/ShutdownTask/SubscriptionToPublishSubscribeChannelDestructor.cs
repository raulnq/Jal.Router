using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.ShutdownTask
{
    public class SubscriptionToPublishSubscribeChannelDestructor : IShutdownTask
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public SubscriptionToPublishSubscribeChannelDestructor(IComponentFactoryGateway factory, ILogger logger)
        {
            _logger = logger;

            _factory = factory;
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            _logger.Log("Deleting subscription to publish subscribe channels");

            var manager = _factory.CreateChannelManager();

            foreach (var channel in _factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
            {
                try
                {
                    var deleted = await manager.DeleteIfExist(channel).ConfigureAwait(false);

                    if (deleted)
                    {
                        _logger.Log($"Deleted {channel.Subscription} subscription to publish subscribe channel");
                    }
                    else
                    {
                        _logger.Log($"Subscription to publish subscribe channel {channel.Subscription} does not exist");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Subscription} subscription to publish subscribe channel: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            _logger.Log("Publish subscribe channels deleted");
        }
    }
}