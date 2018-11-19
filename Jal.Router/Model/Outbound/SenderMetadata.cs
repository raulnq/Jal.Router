using Jal.Router.Interface.Inbound;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Outbound
{
    public class SenderMetadata
    {
        public ChannelType Type { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToReplyPath { get; set; }

        public int ToReplyTimeOut { get; set; }

        public string ToReplySubscription { get; set; }

        public string ToReplyConnectionString { get; set; }

        public List<EndPoint> Endpoints { get; set; }

        public string GetId()
        {
            return ToPath + ToConnectionString;
        }

        public Func<object[]> CreateSenderMethod { get; set; }

        public Action<object[]> DestroySenderMethod { get; set; }

        public Func<object[], object, string> SendMethod { get; set; }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnMethod { get; set; }

        public object[] Sender { get; set; }

        public override string ToString()
        {
            if (Type == ChannelType.PointToPoint)
            {
                return "point to point";
            }
            if (Type == ChannelType.RequestReplyPointToPoint)
            {
                return "request reply to point to point";
            }
            if (Type == ChannelType.RequestReplyPublishSubscriber)
            {
                return "request reply to publish subscriber";
            }
            if (Type == ChannelType.PublishSubscriber)
            {
                return "publish subscriber";
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

            if (!string.IsNullOrWhiteSpace(ToReplyPath))
            {
                description = $"- {description}/{ToReplyPath}";
            }

            if (!string.IsNullOrWhiteSpace(ToReplySubscription))
            {
                description = $"{description}/{ToReplySubscription}";
            }

            return description;
        }

        public SenderMetadata(string topath, string toconnectionstring, ChannelType channeltype)
        {
            ToPath = topath;
            ToConnectionString = toconnectionstring;
            Type = channeltype;
            Endpoints = new List<EndPoint>();
        }
    }
}