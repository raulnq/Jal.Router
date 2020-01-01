using Jal.Router.Interface;
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

        public SenderContext(Channel channel, ISenderChannel senderchannel, IReaderChannel readerchannel)
        {
            Channel = channel;
            Endpoints = new List<EndPoint>();
            SenderChannel = senderchannel;
            ReaderChannel = readerchannel;
        }

        public string Id
        {
            get
            {
                return $"{Channel.FullPath} {Channel.ToString()} channel ({Endpoints.Count}): {string.Join(",", Endpoints.Select(x => x.Name))}";
            }
        }
    }
}