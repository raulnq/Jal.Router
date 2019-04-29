using Jal.Router.Interface;
using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public Channel(ChannelType channeltype)
        {
            Type = channeltype;
        }

        public ChannelType Type { get; set; }

        public string GetId()
        {
            return ToPath + ToSubscription + ToConnectionString;
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

        public string GetPath()
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

        public Type ConnectionStringValueFinderType { get; set; }

        public Func<IValueFinder, string> ToConnectionStringProvider { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public string ToReplyPath { get; set; }

        public int ToReplyTimeOut { get; set; }

        public string ToReplySubscription { get; set; }

        public Type ReplyConnectionStringValueFinderType { get; set; }

        public Func<IValueFinder, string> ToReplyConnectionStringProvider { get; set; }

        public string ToReplyConnectionString { get; set; }
    }
}