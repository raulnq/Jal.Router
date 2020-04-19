using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> Close()
        {
            if (SenderChannel != null)
            {
                await SenderChannel.Close(this).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public Task<MessageContext> Read(MessageContext context, IMessageAdapter adapter)
        {
            return ReaderChannel.Read(this, context, adapter);
        }

        public Task<string> Send(object message)
        {
            if (SenderChannel != null)
            {
                return SenderChannel.Send(this, message);
            }

            return Task.FromResult(string.Empty);
        }

        public bool IsActive()
        {
            if (SenderChannel != null)
            {
                return SenderChannel.IsActive(this);
            }

            return false;
        }

        public bool Open()
        {
            if (SenderChannel != null)
            {
                SenderChannel.Open(this);

                return true;
            }

            return false;
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