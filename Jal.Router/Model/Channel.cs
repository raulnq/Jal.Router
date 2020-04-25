using Jal.Router.Interface;
using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype, string connectionstring, string path, Type adaptertype, Type type, bool fromrouterconfigurationsource = true)
        {
            ChannelType = channeltype;

            ConnectionString = connectionstring;

            Path = path;

            RouterConfigurationSource = fromrouterconfigurationsource;

            AdapterType = adaptertype;

            Type = type;
        }

        public Channel(ChannelType channeltype, string connectionstring, string path, string subscription, Type adaptertype, Type type, bool fromrouterconfigurationsource = true)
            :this(channeltype, connectionstring, path, adaptertype, type, fromrouterconfigurationsource)
        {
            Subscription = subscription;
        }

        public bool RouterConfigurationSource { get; private set; }

        public Type AdapterType { get; set; }

        public Type Type { get; set; }

        public ChannelType ChannelType { get; private set; }

        public string Id
        {
            get
            {
                return Path + Subscription + ConnectionString;
            }
        }

        public override string ToString()
        {
            if (ChannelType == ChannelType.PointToPoint)
            {
                return "point to point";
            }
            if (ChannelType == ChannelType.RequestReplyToPointToPoint)
            {
                return "request reply to point to point";
            }
            if (ChannelType == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                return "request reply to subscription to publish subscribe";
            }
            if (ChannelType == ChannelType.PublishSubscribe)
            {
                return "publish subscribe";
            }
            if (ChannelType == ChannelType.SubscriptionToPublishSubscribe)
            {
                return "subscription to publish subscribe";
            }

            return string.Empty;
        }

        public string FullPath
        {
            get
                {
                var description = string.Empty;

                if (!string.IsNullOrWhiteSpace(Path))
                {
                    description = $"{description}/{Path}";
                }

                if (!string.IsNullOrWhiteSpace(Subscription))
                {
                    description = $"{description}/{Subscription}";
                }

                if (!string.IsNullOrWhiteSpace(ReplyPath))
                {
                    description = $"{description}/{ReplyPath}";
                }

                if (!string.IsNullOrWhiteSpace(ReplySubscription))
                {
                    description = $"{description}/{ReplySubscription}";
                }

                return description;
            }

        }

        public string ConnectionString { get; private set; }

        public string Path { get; private set; }

        public string Subscription { get; private set; }

        public string ReplyPath { get; private set; }

        public int ReplyTimeOut { get; private set; }

        public string ReplySubscription { get; private set; }

        public string ReplyConnectionString { get; private set; }

        public ChannelEntity ToEntity()
        {
            return new ChannelEntity(Path, Subscription, ChannelType);
        }

        public void UpdateReplyFromPointToPointChannel(string replypath, int replytimeout, 
            string replyconnectionstring, Type adapter, Type type)
        {
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            ChannelType = ChannelType.RequestReplyToPointToPoint;
            ReplyConnectionString = replyconnectionstring;
            AdapterType = adapter;
            Type = type;
        }

        public void UpdateReplyFromSubscriptionToPublishSubscribeChannel(string replypath, int replytimeout, string replysubscription,
            string replyconnectionstring, Type adapter, Type type)
        {
            ReplySubscription = replysubscription;
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            ChannelType = ChannelType.RequestReplyToSubscriptionToPublishSubscribe;
            ReplyConnectionString = replyconnectionstring;
            AdapterType = adapter;
            Type = type;
        }
    }
}