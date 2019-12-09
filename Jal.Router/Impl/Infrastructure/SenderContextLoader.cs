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

        public void Load(EndPoint endpoint, Channel channel)
        {
            var newsendercontext = new SenderContext(channel);

            newsendercontext.Endpoints.Add(endpoint);

            _factory.Configuration.Runtime.SenderContexts.Add(newsendercontext);

            var senderchannel = default(ISenderChannel);

            var readerchannel = default(IReaderChannel);

            if (newsendercontext.Channel.Type == ChannelType.PointToPoint)
            {
                senderchannel = _factory.CreatePointToPointChannel();
            }

            if (newsendercontext.Channel.Type == ChannelType.PublishSubscribe)
            {
                senderchannel = _factory.CreatePublishSubscribeChannel();
            }

            if (newsendercontext.Channel.Type == ChannelType.RequestReplyToPointToPoint)
            {
                var requestresplychannel = _factory.CreateRequestReplyChannelFromPointToPointChannel();

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            if (newsendercontext.Channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                var requestresplychannel = _factory.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            if (senderchannel != null)
            {
                senderchannel.Open(newsendercontext);

                newsendercontext.UpdateSenderChannel(senderchannel);

                if (readerchannel != null)
                {
                    newsendercontext.UpdateReaderChannel(readerchannel);
                }

                _logger.Log($"Opening {newsendercontext.Id}");
            }
        }
    }
}