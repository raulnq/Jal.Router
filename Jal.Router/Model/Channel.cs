using Jal.Router.Interface;
using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype, string connectionstring, string path)
        {
            Type = channeltype;

            ConnectionString = connectionstring;

            Path = path;
        }

        public Channel(ChannelType channeltype, string connectionstring, string path, string subscription)
            :this(channeltype, connectionstring, path)
        {
            Subscription = subscription;
        }

        public ChannelType Type { get; private set; }

        public string Id
        {
            get
            {
                return Path + Subscription + ConnectionString;
            }
        }

        public override string ToString()
        {
            if (Type == ChannelType.PointToPoint)
            {
                return "point to point";
            }
            if (Type == ChannelType.RequestReplyToPointToPoint)
            {
                return "request reply to point to point";
            }
            if (Type == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                return "request reply to subscription to publish subscribe";
            }
            if (Type == ChannelType.PublishSubscribe)
            {
                return "publish subscribe";
            }
            if (Type == ChannelType.SubscriptionToPublishSubscribe)
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
            return new ChannelEntity(Path, Subscription, Type);
        }

        public void UpdateReplyFromPointToPointChannel(string replypath, int replytimeout, 
            string replyconnectionstring)
        {
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            Type = ChannelType.RequestReplyToPointToPoint;
            ReplyConnectionString = replyconnectionstring;
        }

        public void UpdateReplyFromSubscriptionToPublishSubscribeChannel(string replypath, int replytimeout, string replysubscription,
            string replyconnectionstring)
        {
            ReplySubscription = replysubscription;
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            Type = ChannelType.RequestReplyToSubscriptionToPublishSubscribe;
            ReplyConnectionString = replyconnectionstring;
        }
    }
}