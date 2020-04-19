using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PointToPointChannelResourceDestructor : IShutdownTask
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public PointToPointChannelResourceDestructor(IComponentFactoryFacade factory, ILogger logger) 
        {
            _logger = logger;

            _factory = factory;
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            _logger.Log("Deleting point to point channels");

            var manager = _factory.CreatePointToPointChannelResourceManager();

            foreach (var channel in _factory.Configuration.Runtime.PointToPointChannelResources)
            {
                try
                {
                    var deleted = await manager.DeleteIfExist(channel).ConfigureAwait(false);

                    if (deleted)
                    {
                        _logger.Log($"Deleted {channel.Path} point to point channel");
                    }
                    else
                    {
                        _logger.Log($"Point to point channel {channel.Path} does not exist");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Path} point to point channel: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            _logger.Log("Point to point channels deleted");
        }
    }
}