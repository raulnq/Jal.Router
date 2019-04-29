﻿using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class Sender : ISender
    {
        public Sender(IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }

        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        public object Send(Channel channel, MessageContext context)
        {
            var id = string.Empty;

            try
            {
                var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

                var message = adapter.WriteMetadataAndContent(context, context.EndPoint.UseClaimCheck);

                var metadata = Configuration.Runtime.SendersMetadata.FirstOrDefault(x => x.Channel.GetId() == channel.GetId());

                if (metadata == null)
                {
                    metadata = DynamicEndpointLoader(channel, context);
                }

                id = metadata.Sender.Send(message);

                if (metadata.Reader != null)
                {
                    MessageContext outputcontext = null;

                    try
                    {
                        outputcontext = metadata.Reader.Read(context, adapter);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {outputcontext?.IdentityContext.Id} failed to arrived to {channel.ToString()} channel {context.EndPoint.Name}/{channel.GetPath()} {ex}");

                        throw;
                    }
                    finally
                    {
                        Logger.Log($"Message {outputcontext?.IdentityContext.Id} arrived to {channel.ToString()} channel {context.EndPoint.Name}/{channel.GetPath()}");
                    }

                    if (outputcontext != null)
                    {
                        return adapter.Deserialize(outputcontext.Content, outputcontext.ContentType);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {id} failed to sent to {channel.ToString()} channel {context.EndPoint.Name}/{channel.GetPath()}  {ex}");

                throw;
            }
            finally
            {
                Logger.Log($"Message {id} sent to {channel.ToString()} channel {context.EndPoint.Name}/{channel.GetPath()}");
            }

            return null;
        }

        private SenderMetadata DynamicEndpointLoader(Channel channel, MessageContext context)
        {
            var newsender = new SenderMetadata(channel);

            newsender.Endpoints.Add(context.EndPoint);

            if (newsender.Channel.Type == ChannelType.PointToPoint)
            {
                var pointtopointchannel = Factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);

                pointtopointchannel.Open(newsender);

                newsender.Sender = pointtopointchannel;

                Logger.Log($"Opening {newsender.Channel.GetPath()} {newsender.Channel.ToString()} channel ({newsender.Endpoints.Count}): {string.Join(",", newsender.Endpoints.Select(x => x.Name))}");
            }

            if (newsender.Channel.Type == ChannelType.PublishSubscribe)
            {
                var publishsubscribechannel = Factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);

                publishsubscribechannel.Open(newsender);

                newsender.Sender = publishsubscribechannel;

                Logger.Log($"Opening {newsender.Channel.GetPath()} {newsender.Channel.ToString()} channel ({newsender.Endpoints.Count}): {string.Join(",", newsender.Endpoints.Select(x => x.Name))}");
            }

            Configuration.Runtime.SendersMetadata.Add(newsender);

            return newsender;
        }
    }
}