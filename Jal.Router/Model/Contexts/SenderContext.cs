using Jal.Router.Interface;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class SenderContext
    {
        private readonly ILogger _logger;

        public Channel Channel { get; private set; }

        public EndPoint EndPoint { get; private set; }

        public IReaderChannel ReaderChannel { get; private set; }

        public ISenderChannel SenderChannel { get; private set; }

        public IMessageAdapter MessageAdapter { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public IMessageStorage MessageStorage { get; private set; }

        public IEntityStorage EntityStorage { get; private set; }

        public static SenderContext Create(IComponentFactoryFacade factory, ILogger logger, Channel channel, EndPoint endpoint)
        {
            var (senderchannel, readerchannel) = factory.CreateSenderChannel(channel.ChannelType, channel.Type);

            var adapter = factory.CreateMessageAdapter(channel.AdapterType);

            var serializer = factory.CreateMessageSerializer();

            var messagestorage = factory.CreateMessageStorage();

            var entitystorage = factory.CreateEntityStorage();

            return new SenderContext(endpoint, channel, senderchannel, readerchannel, adapter, serializer, messagestorage, entitystorage, logger);
        }

        private SenderContext(EndPoint endpoint, Channel channel, ISenderChannel senderchannel, IReaderChannel readerchannel, IMessageAdapter adapter, IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage, ILogger logger)
        {
            if(senderchannel == null)
            {
                throw new ArgumentNullException(nameof(senderchannel));
            }
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            Channel = channel;
            EndPoint = endpoint;
            SenderChannel = senderchannel;
            ReaderChannel = readerchannel;
            MessageAdapter = adapter;
            MessageSerializer = serializer;
            MessageStorage = messagestorage;
            EntityStorage = entitystorage;
            _logger = logger;
        }

        public Task Close()
        {
            _logger.Log($"Shutdown {ToString()}");

            return SenderChannel.Close(this);
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
            return SenderChannel.Send(this, message);
        }

        public bool IsActive()
        {
            return SenderChannel.IsActive(this);
        }

        public void Open()
        {
            SenderChannel.Open(this);

            _logger.Log($"Opening {ToString()}");
        }

        public override string ToString()
        {
            return $"endpoint name: {EndPoint.ToString()} path: {Channel.FullPath} {Channel.ToString()} claimchek: {Channel.UseClaimCheck}";
        }

    }
}