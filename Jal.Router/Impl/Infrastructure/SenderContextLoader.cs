using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SenderContextLoader : ISenderContextLoader
    {
        private IComponentFactoryGateway _factory;

        private ILogger _logger;

        public SenderContextLoader(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public SenderContext Load(Channel channel)
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

            if (senderchannel != null)
            {
                senderchannel.Open(sendercontext);

                _logger.Log($"Opening {sendercontext.Id}");
            }

            return sendercontext;
        }
    }
}