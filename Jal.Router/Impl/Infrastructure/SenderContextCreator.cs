using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SenderContextCreator : ISenderContextCreator
    {
        private IComponentFactoryGateway _factory;

        private ILogger _logger;

        public SenderContextCreator(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public SenderContext Create(Channel channel)
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

            return sendercontext;
        }

        public void Open(SenderContext sendercontext)
        {
            if (sendercontext.SenderChannel != null)
            {
                sendercontext.SenderChannel.Open(sendercontext);

                _logger.Log($"Opening {sendercontext.Id}");
            }
        }
    }
}