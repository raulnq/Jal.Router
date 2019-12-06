using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public interface IDynamicSenderLoader
    {
        void AddPublishSubscribeChannel<TMessage>(string name, string connectionstring, string path);

        void AddPointToPointChannel<TMessage>(string name, string connectionstring, string path);
    }

    public class DynamicSenderLoader
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public DynamicSenderLoader(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;

            _logger = logger;
        }

        public void AddPointToPointChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PointToPoint, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            CreateAndOpen(newendpoint, newchannel);
        }

        public void AddPublishSubscribeChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PublishSubscribe, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            CreateAndOpen(newendpoint, newchannel);
        }

        private void CreateAndOpen(EndPoint newendpoint, Channel newchannel)
        {
            var sender = _factory.Configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == newchannel.Id);

            if (sender != null)
            {
                sender.Endpoints.Add(newendpoint);
            }
            else
            {
                var newsender = new SenderContext(newchannel);

                newsender.Endpoints.Add(newendpoint);

                _factory.Configuration.Runtime.SenderContexts.Add(newsender);

                var senderchannel = _factory.CreateSenderChannel(newsender.Channel.Type);

                if (senderchannel != null)
                {
                    senderchannel.Open(newsender);

                    newsender.UpdateSenderChannel(senderchannel);

                    _logger.Log($"Opening {newsender.Id}");
                }
            }
        }
    }

    public class SenderLoader : AbstractStartupTask, IStartupTask
    {

        public SenderLoader(IComponentFactoryGateway factory, IRouterConfigurationSource[] sources, ILogger logger)
            : base(factory, logger)
        {

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
                var senderchannel = default(ISenderChannel);

                var readerchannel = default(IReaderChannel);

                if (sendercontext.Channel.Type == ChannelType.PointToPoint)
                {
                    senderchannel = Factory.CreatePointToPointChannel();
                }

                if (sendercontext.Channel.Type == ChannelType.PublishSubscribe)
                {
                    senderchannel = Factory.CreatePublishSubscribeChannel();
                }

                if (sendercontext.Channel.Type == ChannelType.RequestReplyToPointToPoint)
                {
                    var requestresplychannel = Factory.CreateRequestReplyChannelFromPointToPointChannel();

                    readerchannel = requestresplychannel;

                    senderchannel = requestresplychannel;
                }

                if (sendercontext.Channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
                {
                    var requestresplychannel = Factory.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

                    readerchannel = requestresplychannel;

                    senderchannel = requestresplychannel;
                }

                if(senderchannel!=null)
                {
                    senderchannel.Open(sendercontext);

                    sendercontext.UpdateSenderChannel(senderchannel);

                    if(readerchannel!=null)
                    {
                        sendercontext.UpdateReaderChannel(readerchannel);
                    }

                    Logger.Log($"Opening {sendercontext.Id}");
                }
            }
        }

        private void Create()
        {
            foreach (var item in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in item.Channels)
                {
                    var sender = Factory.Configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (sender != null)
                    {
                        sender.Endpoints.Add(item);
                    }
                    else
                    {
                        var newsender = new SenderContext(channel);

                        newsender.Endpoints.Add(item);

                        Factory.Configuration.Runtime.SenderContexts.Add(newsender);
                    }
                }
            }
        }
    }
}