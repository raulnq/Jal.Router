using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class SenderContext
    {
        public Channel Channel { get; private set; }

        public List<EndPoint> Endpoints { get; private set; }

        public IReaderChannel ReaderChannel { get; private set; }

        public ISenderChannel SenderChannel { get; private set; }

        public SenderContext(Channel channel)
        {
            Channel = channel;
            Endpoints = new List<EndPoint>();
        }

        public string Id
        {
            get{
                return $"{Channel.FullPath} {Channel.ToString()} channel ({Endpoints.Count}): {string.Join(",", Endpoints.Select(x => x.Name))}";
            }
        }

        public void UpdateReaderChannel(IReaderChannel reader)
        {
            ReaderChannel = reader;
        }

        public void UpdateSenderChannel(ISenderChannel sender)
        {
            SenderChannel = sender;
        }
    }
}