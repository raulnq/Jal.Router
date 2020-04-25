using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ResourceLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IResourceContextLifecycle _lifecycle;

        public ResourceLoader(IComponentFactoryFacade factory, ILogger logger, IResourceContextLifecycle lifecycle) : base(factory, logger)
        {
            _lifecycle = lifecycle;
        }

        public Task Run()
        {
            Logger.Log("Loading resources");

            foreach (var resource in Factory.Configuration.Runtime.Resources)
            {
                _lifecycle.AddOrGet(resource);
            }

            Logger.Log("Resources loaded");

            return Task.CompletedTask;
        }
    }
}