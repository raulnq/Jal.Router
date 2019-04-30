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
            foreach (var sendermetadata in Factory.Configuration.Runtime.SendersMetadata)
            {
                var senderchannel = default(ISenderChannel);

                var readerchannel = default(IReaderChannel);

                if (sendermetadata.Channel.Type == ChannelType.PointToPoint)
                {
                    senderchannel = Factory.CreatePointToPointChannel();
                }

                if (sendermetadata.Channel.Type == ChannelType.PublishSubscribe)
                {
                    senderchannel = Factory.CreatePublishSubscribeChannel();
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToPointToPoint)
                {
                    var requestresplychannel = Factory.CreateRequestReplyChannelFromPointToPointChannel();

                    readerchannel = requestresplychannel;

                    senderchannel = requestresplychannel;
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
                {
                    var requestresplychannel = Factory.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

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
            foreach (var item in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in item.Channels)
                {
                    var sender = Factory.Configuration.Runtime.SendersMetadata.FirstOrDefault(x => x.Channel.GetId() == channel.GetId());

                    if (sender != null)
                    {
                        sender.Endpoints.Add(item);
                    }
                    else
                    {
                        var newsender = new SenderMetadata(channel);

                        newsender.Endpoints.Add(item);

                        Factory.Configuration.Runtime.SendersMetadata.Add(newsender);
                    }
                }
            }
        }
    }
}