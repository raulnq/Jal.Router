using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.StartupTask
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {

        public SenderLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, ILogger logger)
            : base(factory, configuration, logger)
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
            foreach (var sendermetadata in Configuration.Runtime.SendersMetadata)
            {
                var senderchannel = default(ISenderChannel);

                var readerchannel = default(IReaderChannel);

                if (sendermetadata.Channel.Type == ChannelType.PointToPoint)
                {
                    senderchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);
                }

                if (sendermetadata.Channel.Type == ChannelType.PublishSubscribe)
                {
                    senderchannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToPointToPoint)
                {
                    var requestresplychannel = Factory.Create<IRequestReplyChannelFromPointToPointChannel>(Configuration.RequestReplyChannelFromPointToPointChannelType);

                    readerchannel = requestresplychannel;

                    senderchannel = requestresplychannel;
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
                {
                    var requestresplychannel = Factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(Configuration.RequestReplyFromSubscriptionToPublishSubscribeChannelType);

                    readerchannel = requestresplychannel;

                    senderchannel = requestresplychannel;
                }

                if(senderchannel!=null)
                {
                    senderchannel.Open(sendermetadata);

                    sendermetadata.Sender = senderchannel;

                    if(readerchannel!=null)
                    {
                        sendermetadata.Reader = readerchannel;
                    }

                    Logger.Log($"Opening {sendermetadata.Signature()}");
                }
            }
        }

        private void Create()
        {
            foreach (var item in Configuration.Runtime.EndPoints)
            {
                foreach (var channel in item.Channels)
                {
                    var sender = Configuration.Runtime.SendersMetadata.FirstOrDefault(x => x.Channel.GetId() == channel.GetId());

                    if (sender != null)
                    {
                        sender.Endpoints.Add(item);
                    }
                    else
                    {
                        var newsender = new SenderMetadata(channel);

                        newsender.Endpoints.Add(item);

                        Configuration.Runtime.SendersMetadata.Add(newsender);
                    }
                }
            }
        }
    }
}