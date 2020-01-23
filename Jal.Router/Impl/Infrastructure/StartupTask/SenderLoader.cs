using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {
        private ISenderContextCreator _loader;

        public SenderLoader(IComponentFactoryGateway factory, ISenderContextCreator loader, ILogger logger)
            : base(factory, logger)
        {
            _loader = loader;
        }

        public Task Run()
        {
            Logger.Log("Loading senders");

            Create();

            Open();

            Logger.Log("Senders loaded");

            return Task.CompletedTask;
        }

        private void Open()
        {
            foreach (var sendercontext in Factory.Configuration.Runtime.SenderContexts)
            {
                _loader.Open(sendercontext);
            }
        }

        private void Create()
        {
            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in endpoint.Channels)
                {
                    var sendercontext = Factory.Configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (sendercontext == null)
                    {
                        sendercontext = _loader.Create(channel);

                        Factory.Configuration.Runtime.SenderContexts.Add(sendercontext);
                    }

                    sendercontext.Endpoints.Add(endpoint);
                }
            }
        }
    }
}