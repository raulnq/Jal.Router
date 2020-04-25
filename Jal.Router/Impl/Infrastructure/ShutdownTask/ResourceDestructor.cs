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

            foreach (var context in _factory.Configuration.Runtime.ResourceContexts)
            {
                try
                {
                    var deleted = await context.DeleteIfExist().ConfigureAwait(false);

                    if (deleted)
                    {
                        _logger.Log($"Deleted {context.Resource.FullPath} {context.Resource.ToString()} resource");
                    }
                    else
                    {
                        _logger.Log($"The {context.Resource.ToString()} resource {context.Resource.FullPath} does not exist");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {context.Resource.FullPath} {context.Resource.ToString()} resource: {ex}";

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