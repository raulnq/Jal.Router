using System.Linq;
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

        public void Run()
        {
            Logger.Log("Loading senders");

            var pointtopointchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);

            var publishsubscriberchannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);

            var requestreplychannel = Factory.Create<IRequestReplyChannel>(Configuration.RequestReplyChannelType);

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

            foreach (var sendermetadata in Configuration.Runtime.SendersMetadata)
            {
                if (sendermetadata.Channel.Type==ChannelType.PointToPoint)
                {
                    sendermetadata.CreateSenderMethod = pointtopointchannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = pointtopointchannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = pointtopointchannel.SendMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.Channel.GetPath()} {sendermetadata.Channel.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x=>x.Name))}");
                }

                if (sendermetadata.Channel.Type == ChannelType.PublishSubscriber)
                {
                    sendermetadata.CreateSenderMethod = publishsubscriberchannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = publishsubscriberchannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = publishsubscriberchannel.SendMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.Channel.GetPath()} {sendermetadata.Channel.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToPointToPoint)
                {
                    sendermetadata.CreateSenderMethod = requestreplychannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = requestreplychannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = requestreplychannel.SendMethodFactory(sendermetadata);

                    sendermetadata.ReceiveOnMethod = requestreplychannel.ReceiveOnPointToPointChannelMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.Channel.GetPath()} {sendermetadata.Channel.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }

                if (sendermetadata.Channel.Type == ChannelType.RequestReplyToSubscriptionToPublishSubscriber)
                {
                    sendermetadata.CreateSenderMethod = requestreplychannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = requestreplychannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = requestreplychannel.SendMethodFactory(sendermetadata);

                    sendermetadata.ReceiveOnMethod = requestreplychannel.ReceiveOnPublishSubscriberChannelMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.Channel.GetPath()} {sendermetadata.Channel.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }
            }

            Logger.Log("Senders loaded");
        }
    }
}