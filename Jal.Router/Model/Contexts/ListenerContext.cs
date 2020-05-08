using Jal.Router.Interface;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{

    public class ListenerContext : AbstractContext
    {
        public IChannelListener ListenerChannel { get; private set; }

        public IRouter Router { get; private set; }

        public Route Route { get; private set; }

        public static ListenerContext Create(IComponentFactoryFacade factory, IRouter router, ILogger logger, IHasher hasher,  Channel channel, Route route)
        {
            var (listenerchannel, channelcreator, channeldeleter, channelprovider) = factory.CreateListenerChannel(channel.ChannelType, channel.Type);

            var adapter = factory.CreateMessageAdapter(channel.AdapterType);

            var serializer = factory.CreateMessageSerializer();

            var messagestorage = factory.CreateMessageStorage();

            var entitystorage = factory.CreateEntityStorage();

            var context = new ListenerContext(route, channel, channelcreator, channeldeleter, channelprovider, listenerchannel, adapter, router, serializer, messagestorage, entitystorage, logger, hasher);

            return context;
        }

        private ListenerContext(Route route, Channel channel, IChannelCreator channelcreator, IChannelDeleter channeldeleter, IChannelStatisticProvider channelprovider, IChannelListener listenerchannel, IMessageAdapter adapter,
            IRouter router, IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage,
            ILogger logger, IHasher hasher) : base(channel, channelcreator, channeldeleter, channelprovider, adapter, serializer, messagestorage, entitystorage, logger, hasher)
        {
            if (listenerchannel == null)
            {
                throw new ArgumentNullException(nameof(listenerchannel));
            }
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            Route = route;
            ListenerChannel = listenerchannel;
            Router = router;
        }

        public Task Dispatch(MessageContext context)
        {
            var when = true;

            if(Channel.Condition!=null)
            {
                when = Channel.Condition(context);
            }

            if(when)
            {
                return Router.Route(context);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task<MessageContext> Read(object message)
        {
            return MessageAdapter.ReadFromPhysicalMessage(message, this);
        }

        public Task Close()
        {
            _logger.Log($"Shutdown {this.ToString()}");

            return ListenerChannel.Close(this);
        }

        public bool IsActive()
        {
            return ListenerChannel.IsActive(this);
        }

        public void Open()
        {
            ListenerChannel.Open(this);

            ListenerChannel.Listen(this);

            Hash();

            _logger.Log($"Listening {ToString()}");
        }

        public override string ToString()
        {
            return $"route name: {Route.ToString()} path: {Channel.FullPath} {Channel.ToString()} partition: {Channel.UsePartition} claimchek: {Channel.UseClaimCheck}";
        }
    }
}