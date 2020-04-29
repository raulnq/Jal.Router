using Jal.Router.Interface;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class ListenerContext
    {
        private readonly ILogger _logger;

        public Channel Channel { get; private set; }

        public IListenerChannel ListenerChannel { get; private set; }

        public IMessageAdapter MessageAdapter { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public IMessageStorage MessageStorage { get; private set; }

        public IEntityStorage EntityStorage { get; private set; }

        public IRouter Router { get; private set; }

        public Route Route { get; private set; }

        public static ListenerContext Create(IComponentFactoryFacade factory, IRouter router, ILogger logger, Channel channel, Route route)
        {
            var listenerchannel = factory.CreateListenerChannel(channel.ChannelType, channel.Type);

            var adapter = factory.CreateMessageAdapter(channel.AdapterType);

            var serializer = factory.CreateMessageSerializer();

            var messagestorage = factory.CreateMessageStorage();

            var entitystorage = factory.CreateEntityStorage();

            var context = new ListenerContext(route, channel, listenerchannel, adapter, router, serializer, messagestorage, entitystorage, logger);

            return context;
        }

        private ListenerContext(Route route, Channel channel, IListenerChannel listenerchannel, IMessageAdapter adapter,
            IRouter router, IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage,
            ILogger logger)
        {
            if (listenerchannel == null)
            {
                throw new ArgumentNullException(nameof(listenerchannel));
            }
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            Channel = channel;
            Route = route;
            ListenerChannel = listenerchannel;
            MessageAdapter = adapter;
            Router = router;
            MessageSerializer = serializer;
            MessageStorage = messagestorage;
            EntityStorage = entitystorage;
            _logger = logger;
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

            _logger.Log($"Listening {ToString()}");
        }

        public override string ToString()
        {
            return $"route name: {Route.ToString()} path: {Channel.FullPath} {Channel.ToString()} partition: {Channel.Partition} claimchek: {Channel.UseClaimCheck}";
        }
    }
}