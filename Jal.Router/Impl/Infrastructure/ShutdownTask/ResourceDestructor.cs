using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ResourceDestructor : IShutdownTask
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public ResourceDestructor(IComponentFactoryFacade factory, ILogger logger) 
        {
            _logger = logger;

            _factory = factory;
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            _logger.Log("Deleting resources");

            foreach (var resource in _factory.Configuration.Runtime.Resources)
            {
                try
                {
                    var manager = _factory.CreateResourceManager(resource.ChannelType);

                    var deleted = await manager.DeleteIfExist(resource).ConfigureAwait(false);

                    if (deleted)
                    {
                        _logger.Log($"Deleted {resource.FullPath} {resource.ToString()} resource");
                    }
                    else
                    {
                        _logger.Log($"The {resource.ToString()} resource {resource.FullPath} does not exist");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {resource.FullPath} {resource.ToString()} resource: {ex}";

                    errors.AppendLine(error);

                    _logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            _logger.Log("Resources deleted");
        }
    }
}