using Jal.Router.Interface;
using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype, Type connectionstringvaluefindertype, Func<IValueFinder, string> toconnectionstringprovider, string topath)
        {
            Type = channeltype;

            ConnectionStringValueFinderType = connectionstringvaluefindertype;

            ToConnectionStringProvider = toconnectionstringprovider;

            ToPath = topath;
        }

        public Channel(ChannelType channeltype, Type connectionstringvaluefindertype,
            Func<IValueFinder, string> toconnectionstringprovider, string topath, string tosubscription)
            :this(channeltype, connectionstringvaluefindertype, toconnectionstringprovider, topath)
        {
            ToSubscription = tosubscription;
        }

        public ChannelType Type { get; private set; }

        public string Id
        {
            get
            {
                return ToPath + ToSubscription + ToConnectionString;
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

                if (!string.IsNullOrWhiteSpace(ToPath))
                {
                    description = $"{description}/{ToPath}";
                }

                if (!string.IsNullOrWhiteSpace(ToSubscription))
                {
                    description = $"{description}/{ToSubscription}";
                }

                if (!string.IsNullOrWhiteSpace(ToReplyPath))
                {
                    description = $"{description}/{ToReplyPath}";
                }

                if (!string.IsNullOrWhiteSpace(ToReplySubscription))
                {
                    description = $"{description}/{ToReplySubscription}";
                }

                return description;
            }

        }

        public Type ConnectionStringValueFinderType { get; private set; }

        public Func<IValueFinder, string> ToConnectionStringProvider { get; private set; }

        public string ToConnectionString { get; private set; }

        public string ToPath { get; private set; }

        public string ToSubscription { get; private set; }

        public string ToReplyPath { get; private set; }

        public int ToReplyTimeOut { get; private set; }

        public string ToReplySubscription { get; private set; }

        public Type ReplyConnectionStringValueFinderType { get; private set; }

        public Func<IValueFinder, string> ToReplyConnectionStringProvider { get; private set; }

        public string ToReplyConnectionString { get; private set; }

        public ChannelEntity ToEntity()
        {
            return new ChannelEntity(ToPath, ToSubscription, Type);
        }

        public void UpdateToConnectionString(string toconnectionstring)
        {
            ToConnectionString = toconnectionstring;
        }

        public void UpdateToReplyConnectionString(string toreplyconnectionstring)
        {
            ToReplyConnectionString = toreplyconnectionstring;
        }

        public void UpdateReplyFromPointToPointChannel(string toreplypath, int toreplytimeout, 
            Type replyconnectionstringvaluefindertype, Func<IValueFinder, string> toreplyconnectionstringprovider)
        {
            ToReplyPath = toreplypath;
            ToReplyTimeOut = toreplytimeout;
            Type = ChannelType.RequestReplyToPointToPoint;
            ReplyConnectionStringValueFinderType = replyconnectionstringvaluefindertype;
            ToReplyConnectionStringProvider = toreplyconnectionstringprovider;
        }

        public void UpdateReplyFromSubscriptionToPublishSubscribeChannel(string toreplypath, int toreplytimeout, string toreplysubscription,
            Type replyconnectionstringvaluefindertype, Func<IValueFinder, string> toreplyconnectionstringprovider)
        {
            ToReplySubscription = toreplysubscription;
            ToReplyPath = toreplypath;
            ToReplyTimeOut = toreplytimeout;
            Type = ChannelType.RequestReplyToSubscriptionToPublishSubscribe;
            ReplyConnectionStringValueFinderType = replyconnectionstringvaluefindertype;
            ToReplyConnectionStringProvider = toreplyconnectionstringprovider;
        }
    }
}