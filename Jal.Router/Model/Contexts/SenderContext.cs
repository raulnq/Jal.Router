using Jal.Router.Interface;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{

    public class SenderContext : AbstractContext
    {
        public EndPoint EndPoint { get; private set; }

        public IChannelReader ReaderChannel { get; private set; }

        public IChannelSender SenderChannel { get; private set; }

        public static SenderContext Create(IComponentFactoryFacade factory, ILogger logger, IHasher hasher, Channel channel, EndPoint endpoint)
        {
            var (senderchannel, readerchannel, channelcreator, channeldeleter, channelprovider) = factory.CreateSenderChannel(channel.ChannelType, channel.Type);

            var adapter = factory.CreateMessageAdapter(channel.AdapterType);

            var serializer = factory.CreateMessageSerializer();

            var messagestorage = factory.CreateMessageStorage();

            var entitystorage = factory.CreateEntityStorage();

            return new SenderContext(endpoint, channel, channelcreator, channeldeleter, channelprovider, senderchannel, readerchannel, adapter, serializer, messagestorage, entitystorage, logger, hasher);
        }

        private SenderContext(EndPoint endpoint, Channel channel, IChannelCreator channelcreator, IChannelDeleter channeldeleter, IChannelStatisticProvider channelprovider,
            IChannelSender senderchannel, IChannelReader readerchannel, IMessageAdapter adapter, 
            IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage, ILogger logger, IHasher hasher)
            :base(channel, channelcreator, channeldeleter, channelprovider, adapter, serializer, messagestorage, entitystorage, logger, hasher)
        {

            if (senderchannel == null)
            {
                throw new ArgumentNullException(nameof(senderchannel));
            }
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            EndPoint = endpoint;
            SenderChannel = senderchannel;
            ReaderChannel = readerchannel;
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

            Hash();

            _logger.Log($"Opening {ToString()}");
        }



        public override string ToString()
        {
            return $"endpoint name: {EndPoint.ToString()} path: {Channel.FullPath} {Channel.ToString()} claimchek: {Channel.UseClaimCheck}";
        }

    }
}