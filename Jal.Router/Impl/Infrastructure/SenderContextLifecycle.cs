using Jal.Router.Interface;
using Jal.Router.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class SenderContextLifecycle : ISenderContextLifecycle
    {
        private IComponentFactoryFacade _factory;

        private ILogger _logger;

        public SenderContextLifecycle(IComponentFactoryFacade factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public SenderContext AddOrGet(Channel channel)
        {
            var sendercontext = Get(channel);

            if (sendercontext == null)
            {
                sendercontext = Add(channel);
            }

            return sendercontext;
        }

        public SenderContext Add(Channel channel)
        {
            var (senderchannel, readerchannel) = _factory.CreateSenderChannel(channel.ChannelType, channel.Type);

            var adapter = _factory.CreateMessageAdapter(channel.AdapterType);

            var sendercontext = new SenderContext(channel, senderchannel, readerchannel, adapter);

            _factory.Configuration.Runtime.SenderContexts.Add(sendercontext);

            return sendercontext;
        }

        public SenderContext Get(Channel channel)
        {
            return _factory.Configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);
        }

        public bool Exist(Channel channel)
        {
            return _factory.Configuration.Runtime.SenderContexts.Any(x => x.Channel.Id == channel.Id);
        }

        public SenderContext Remove(Channel channel)
        {
            var sendercontext = Get(channel);

            if (sendercontext == null)
            { 
                _factory.Configuration.Runtime.SenderContexts.Remove(sendercontext);
            }

            return sendercontext;
        }
    }
}