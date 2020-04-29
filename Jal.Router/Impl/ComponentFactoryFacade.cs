using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Impl
{
    public class ComponentFactoryFacade : IComponentFactoryFacade
    {
        private readonly IComponentFactory _factory;

        public IConfiguration Configuration { get; }

        public ComponentFactoryFacade(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            Configuration = configuration;
        }

        public IResource CreateResource(ChannelType channel)
        {
            if (channel == ChannelType.PointToPoint)
            {
                return CreatePointToPointChannelResource();
            }

            if (channel == ChannelType.SubscriptionToPublishSubscribe)
            {
                return CreateSubscriptionToPublishSubscribeChannelResource();
            }

            if (channel == ChannelType.PublishSubscribe)
            {
                return CreatePublishSubscribeChannelResource();
            }

            return null;
        }

        private IResource CreatePointToPointChannelResource()
        {
            return _factory.Create<IResource>(Configuration.PointToPointResourceType);
        }

        private IResource CreatePublishSubscribeChannelResource()
        {
            return _factory.Create<IResource>(Configuration.PublishSubscribeResourceType);
        }

        private IResource CreateSubscriptionToPublishSubscribeChannelResource()
        {
            return _factory.Create<IResource>(Configuration.SubscriptionToPublishSubscribeResourceType);
        }

        public IBusInterceptor CreateBusInterceptor()
        {
            return _factory.Create<IBusInterceptor>(Configuration.BusInterceptorType);
        }

        public IRouterInterceptor CreateRouterInterceptor()
        {
            return _factory.Create<IRouterInterceptor>(Configuration.RouterInterceptorType);
        }

        public IMessageSerializer CreateMessageSerializer()
        {
            return _factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);
        }

        public IChannelShuffler CreateChannelShuffler()
        {
            return _factory.Create<IChannelShuffler>(Configuration.ChannelShufflerType);
        }

        public IMessageAdapter CreateMessageAdapter(Type type)
        {
            if (type != null)
            {
                return _factory.Create<IMessageAdapter>(type);
            }
            else
            {
                return _factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);
            }
        }

        private IPointToPointChannel CreatePointToPointChannel(Type type)
        {
            if(type==null)
            {
                return _factory.Create<IPointToPointChannel>(Configuration.PointToPointChannelType);
            }
            else
            {
                return _factory.Create<IPointToPointChannel>(type);
            }
        }

        private IPublishSubscribeChannel CreatePublishSubscribeChannel(Type type)
        {
            if (type == null)
            {
                return _factory.Create<IPublishSubscribeChannel>(Configuration.PublishSubscribeChannelType);
            }
            else
            {
                return _factory.Create<IPublishSubscribeChannel>(type);
            }
        }

        private IRequestReplyChannelFromPointToPointChannel CreateRequestReplyChannelFromPointToPointChannel(Type type)
        {
            if (type == null)
            {
                return _factory.Create<IRequestReplyChannelFromPointToPointChannel>(Configuration.RequestReplyChannelFromPointToPointChannelType);
            }
            else
            {
                return _factory.Create<IRequestReplyChannelFromPointToPointChannel>(type);
            }
        }

        private IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel CreateRequestReplyFromSubscriptionToPublishSubscribeChannel(Type type)
        {
            if (type == null)
            {
                return _factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(Configuration.RequestReplyFromSubscriptionToPublishSubscribeChannelType);
            }
            else
            {
                return _factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(type);
            }
        }

        public IMessageStorage CreateMessageStorage()
        {
            return _factory.Create<IMessageStorage>(Configuration.MessageStorageType);
        }

        public IEntityStorage CreateEntityStorage()
        {
            return _factory.Create<IEntityStorage>(Configuration.StorageType);
        }

        public IStartupTask CreateStartupTask(Type type)
        {
            return _factory.Create<IStartupTask>(type);
        }

        public IShutdownTask CreateShutdownTask(Type type)
        {
            return _factory.Create<IShutdownTask>(type);
        }

        public IMonitoringTask CreateMonitoringTask(Type type)
        {
            return _factory.Create<IMonitoringTask>(type);
        }

        public IRouteErrorMessageHandler CreateRouteErrorMessageHandler(Type type)
        {
            return _factory.Create<IRouteErrorMessageHandler>(type);
        }

        public IRouteEntryMessageHandler CreateRouteEntryMessageHandler(Type type)
        {
            return _factory.Create<IRouteEntryMessageHandler>(type);
        }

        public IRouteExitMessageHandler CreateRouteExitMessageHandler(Type type)
        {
            return _factory.Create<IRouteExitMessageHandler>(type);
        }

        public IBusErrorMessageHandler CreateBusErrorMessageHandler(Type type)
        {
            return _factory.Create<IBusErrorMessageHandler>(type);
        }

        public IBusEntryMessageHandler CreateBusEntryMessageHandler(Type type)
        {
            return _factory.Create<IBusEntryMessageHandler>(type);
        }

        public IBusExitMessageHandler CreateBusExitMessageHandler(Type type)
        {
            return _factory.Create<IBusExitMessageHandler>(type);
        }

        public ILogger<T> CreateLogger<T>(Type type)
        {
            return _factory.Create<ILogger<T>>(type);
        }

        public TComponent CreateComponent<TComponent>(Type type) where TComponent : class
        {
            return _factory.Create<TComponent>(type);
        }

        public (ISenderChannel,IReaderChannel) CreateSenderChannel(ChannelType channel, Type type)
        {
            var senderchannel = default(ISenderChannel);

            var readerchannel = default(IReaderChannel);

            if (channel == ChannelType.PointToPoint)
            {
                senderchannel = CreatePointToPointChannel(type);
            }

            if (channel == ChannelType.PublishSubscribe)
            {
                senderchannel = CreatePublishSubscribeChannel(type);
            }

            if (channel == ChannelType.RequestReplyToPointToPoint)
            {
                var requestresplychannel = CreateRequestReplyChannelFromPointToPointChannel(type);

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            if (channel == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                var requestresplychannel = CreateRequestReplyFromSubscriptionToPublishSubscribeChannel(type);

                readerchannel = requestresplychannel;

                senderchannel = requestresplychannel;
            }

            return (senderchannel,readerchannel);
        }

        public IListenerChannel CreateListenerChannel(ChannelType channel, Type type)
        {
            if (channel == ChannelType.PointToPoint)
            {
                return CreatePointToPointChannel(type);
            }

            if (channel == ChannelType.SubscriptionToPublishSubscribe)
            {
                return CreatePublishSubscribeChannel(type);
            }

            return null;
        }
    }
}