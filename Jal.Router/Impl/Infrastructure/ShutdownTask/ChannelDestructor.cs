using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ChannelDestructor : IShutdownTask
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public ChannelDestructor(IComponentFactoryFacade factory, ILogger logger) 
        {
            _logger = logger;

            _factory = factory;
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            _logger.Log("Deleting channels");

            foreach (var context in _factory.Configuration.Runtime.Contexts)
            {
                if (context.Channel.UseCreateIfNotExists)
                {
                    await context.DeleteIfExist().ConfigureAwait(false);
                }
            }

            _logger.Log("Channels deleted");
        }
    }
}