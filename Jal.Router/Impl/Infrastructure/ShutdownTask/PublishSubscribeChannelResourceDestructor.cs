using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PublishSubscribeChannelResourceDestructor : IShutdownTask
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public PublishSubscribeChannelResourceDestructor(IComponentFactoryGateway factory, ILogger logger)
        {
            _logger = logger;

            _factory = factory;
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            _logger.Log("Deleting publish subscribe channels");

            var manager = _factory.CreateChannelManager();

            foreach (var channel in _factory.Configuration.Runtime.PublishSubscribeChannels)
            {
                try
                {
                    var deleted = await manager.DeleteIfExist(channel).ConfigureAwait(false);

                    if (deleted)
                    {
                        _logger.Log($"Deleted {channel.Path} publish subscribe channel");
                    }
                    else
                    {
                        _logger.Log($"Publish subscribe channel {channel.Path} does not exist");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Path} publish subscribe channel: {ex}";

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