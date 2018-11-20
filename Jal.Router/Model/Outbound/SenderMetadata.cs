using Jal.Router.Interface.Inbound;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Outbound
{
    public class SenderMetadata
    {
        public Channel Channel { get; }

        public List<EndPoint> Endpoints { get; }

        public Func<object[]> CreateSenderMethod { get; set; }

        public Action<object[]> DestroySenderMethod { get; set; }

        public Func<object[], object, string> SendMethod { get; set; }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnMethod { get; set; }

        public object[] Sender { get; set; }

        public SenderMetadata(Channel channel)
        {
            Channel = channel;
            Endpoints = new List<EndPoint>();
        }
    }
}