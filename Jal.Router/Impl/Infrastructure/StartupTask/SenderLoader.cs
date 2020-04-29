using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {
        private readonly ISenderContextLifecycle _lifecycle;

        public SenderLoader(IComponentFactoryFacade factory, ISenderContextLifecycle lifecycle, ILogger logger)
            : base(factory, logger)
        {
            _lifecycle = lifecycle;
        }

        public async Task Run()
        {
            Logger.Log("Loading senders");

            Add();

            await Create();

            Open();

            Logger.Log("Senders loaded");
        }

        private void Open()
        {
            foreach (var context in Factory.Configuration.Runtime.SenderContexts)
            {
                context.Open();

                if (Factory.Configuration.Runtime.Contexts.All(x => x.Id != context.Id))
                {
                    Factory.Configuration.Runtime.Contexts.Add(context);
                }
            }
        }

        private async Task Create()
        {
            foreach (var context in Factory.Configuration.Runtime.SenderContexts)
            {
                if (context.Channel.UseCreateIfNotExists)
                {
                    await context.CreateIfNotExist();
                }
            }
        }

        private void Add()
        {
            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in endpoint.Channels)
                {
                    _lifecycle.Add(endpoint, channel);
                }
            }
        }
    }
}