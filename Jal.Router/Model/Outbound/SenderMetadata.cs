using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model.Outbound
{
    public class SenderMetadata
    {
        public Channel Channel { get; }

        public List<EndPoint> Endpoints { get; }

        public IReaderChannel Reader { get; set; }

        public ISenderChannel Sender { get; set; }

        public SenderMetadata(Channel channel)
        {
            Channel = channel;
            Endpoints = new List<EndPoint>();
        }

        public string Signature()
        {
            return $"{Channel.GetPath()} {Channel.ToString()} channel ({Endpoints.Count}): {string.Join(",", Endpoints.Select(x => x.Name))}";
        }
    }
}