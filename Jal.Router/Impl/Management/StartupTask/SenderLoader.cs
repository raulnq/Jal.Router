﻿using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.StartupTask
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouter _router;

        private readonly ISagaExecutionCoordinator _sec;

        public SenderLoader(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaExecutionCoordinator sec)
            : base(factory, configuration, logger)
        {
            _router = router;
            _sec = sec;
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
                    var sender = Configuration.Runtime.SendersMetadata.FirstOrDefault(x => x.GetId() == channel.GetId());

                    if (sender != null)
                    {
                        sender.Endpoints.Add(item);
                    }
                    else
                    {
                        var newsender = new SenderMetadata(channel.ToPath, channel.ToConnectionString, channel.Type);

                        newsender.Endpoints.Add(item);

                        Configuration.Runtime.SendersMetadata.Add(newsender);
                    }
                }
            }

            foreach (var sendermetadata in Configuration.Runtime.SendersMetadata)
            {
                if (sendermetadata.Type==ChannelType.PointToPoint)
                {
                    sendermetadata.CreateSenderMethod = pointtopointchannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = pointtopointchannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = pointtopointchannel.SendMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.GetPath()} {sendermetadata.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x=>x.Name))}");
                }

                if (sendermetadata.Type == ChannelType.PublishSubscriber)
                {
                    sendermetadata.CreateSenderMethod = publishsubscriberchannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = publishsubscriberchannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = publishsubscriberchannel.SendMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.GetPath()} {sendermetadata.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }

                if (sendermetadata.Type == ChannelType.RequestReplyPointToPoint)
                {
                    sendermetadata.CreateSenderMethod = requestreplychannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = requestreplychannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = requestreplychannel.SendMethodFactory(sendermetadata);

                    sendermetadata.ReceiveOnMethod = requestreplychannel.ReceiveOnPointToPointChannelMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.GetPath()} {sendermetadata.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }

                if (sendermetadata.Type == ChannelType.RequestReplyPublishSubscriber)
                {
                    sendermetadata.CreateSenderMethod = requestreplychannel.CreateSenderMethodFactory(sendermetadata);

                    sendermetadata.DestroySenderMethod = requestreplychannel.DestroySenderMethodFactory(sendermetadata);

                    sendermetadata.SendMethod = requestreplychannel.SendMethodFactory(sendermetadata);

                    sendermetadata.ReceiveOnMethod = requestreplychannel.ReceiveOnPublishSubscriberChannelMethodFactory(sendermetadata);

                    sendermetadata.Sender = sendermetadata.CreateSenderMethod();

                    Logger.Log($"Opening {sendermetadata.GetPath()} {sendermetadata.ToString()} channel ({sendermetadata.Endpoints.Count}): {string.Join(",", sendermetadata.Endpoints.Select(x => x.Name))}");
                }
            }

            Logger.Log("Senders loaded");
        }
    }
}