using Jal.Router.Interface;
using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype, Type connectionstringvaluefindertype, Func<IValueFinder, string> connectionstringprovider, string path)
        {
            Type = channeltype;

            ConnectionStringValueFinderType = connectionstringvaluefindertype;

            ConnectionStringProvider = connectionstringprovider;

            Path = path;
        }

        public Channel(ChannelType channeltype, Type connectionstringvaluefindertype,
            Func<IValueFinder, string> connectionstringprovider, string path, string subscription)
            :this(channeltype, connectionstringvaluefindertype, connectionstringprovider, path)
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

        public Type ConnectionStringValueFinderType { get; private set; }

        public Func<IValueFinder, string> ConnectionStringProvider { get; private set; }

        public string ConnectionString { get; private set; }

        public string Path { get; private set; }

        public string Subscription { get; private set; }

        public string ReplyPath { get; private set; }

        public int ReplyTimeOut { get; private set; }

        public string ReplySubscription { get; private set; }

        public Type ReplyConnectionStringValueFinderType { get; private set; }

        public Func<IValueFinder, string> ReplyConnectionStringProvider { get; private set; }

        public string ReplyConnectionString { get; private set; }

        public ChannelEntity ToEntity()
        {
            return new ChannelEntity(Path, Subscription, Type);
        }

        public void UpdateConnectionString(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public void UpdateReplyConnectionString(string replyconnectionstring)
        {
            ReplyConnectionString = replyconnectionstring;
        }

        public void UpdateReplyFromPointToPointChannel(string replypath, int replytimeout, 
            Type replyconnectionstringvaluefindertype, Func<IValueFinder, string> replyconnectionstringprovider)
        {
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            Type = ChannelType.RequestReplyToPointToPoint;
            ReplyConnectionStringValueFinderType = replyconnectionstringvaluefindertype;
            ReplyConnectionStringProvider = replyconnectionstringprovider;
        }

        public void UpdateReplyFromSubscriptionToPublishSubscribeChannel(string replypath, int replytimeout, string replysubscription,
            Type replyconnectionstringvaluefindertype, Func<IValueFinder, string> replyconnectionstringprovider)
        {
            ReplySubscription = replysubscription;
            ReplyPath = replypath;
            ReplyTimeOut = replytimeout;
            Type = ChannelType.RequestReplyToSubscriptionToPublishSubscribe;
            ReplyConnectionStringValueFinderType = replyconnectionstringvaluefindertype;
            ReplyConnectionStringProvider = replyconnectionstringprovider;
        }
    }
}