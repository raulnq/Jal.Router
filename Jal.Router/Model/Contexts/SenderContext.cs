using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class SenderContext
    {
        public Channel Channel { get; private set; }

        public EndPoint EndPoint { get; private set; }

        public IReaderChannel ReaderChannel { get; private set; }

        public ISenderChannel SenderChannel { get; private set; }

        public IMessageAdapter MessageAdapter { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public IMessageStorage MessageStorage { get; private set; }

        public SenderContext(EndPoint endpoint, Channel channel, ISenderChannel senderchannel, IReaderChannel readerchannel, IMessageAdapter adapter, IMessageSerializer serializer, IMessageStorage storage)
        {
            Channel = channel;
            EndPoint = endpoint;
            SenderChannel = senderchannel;
            ReaderChannel = readerchannel;
            MessageAdapter = adapter;
            MessageSerializer = serializer;
            MessageStorage = storage;
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

        public Task<MessageContext> Read(MessageContext context)
        {
            return ReaderChannel.Read(this, context, MessageAdapter);
        }

        public Task<object> Write(MessageContext context)
        {
            return MessageAdapter.WritePhysicalMessage(context, this);
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

        public override string ToString()
        {
            return $"{Channel.FullPath} {Channel.ToString()} channel: {EndPoint.Name}";
        }

    }
}