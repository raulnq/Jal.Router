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
            var senderchannel = default(ISenderChannel);

            var readerchannel = default(IReaderChannel);

            if (channel.Type == ChannelType.PointToPoint)
            {
                senderchannel = _factory.CreatePointToPointChannel();
            }

            if (channel.Type == ChannelType.PublishSubscribe)
            {
                senderchannel = _factory.CreatePublishSubscribeChannel();
            }

            if (channel.Type == ChannelType.RequestReplyToPointToPoint)
            {
                var requestresplychannel = _factory.CreateRequestReplyChannelFromPointToPointChannel();

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            if (channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                var requestresplychannel = _factory.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            var sendercontext = new SenderContext(channel, senderchannel, readerchannel);

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