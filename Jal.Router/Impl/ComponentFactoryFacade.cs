using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Impl
{
    public class ComponentFactoryFacade : IComponentFactoryFacade
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public ComponentFactoryFacade(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public IChannelResourceManager<R, S> CreateChannelResourceManager<R, S>()
        {
            return _factory.Create<IChannelResourceManager<R, S>>(_configuration.PointToPointChannelResourceType);
        }

        public IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics> CreatePointToPointChannelResourceManager()
        {
            return _factory.Create<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>>(_configuration.PointToPointChannelResourceType);
        }

        public IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics> CreatePublishSubscribeChannelResourceManager()
        {
            return _factory.Create<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>(_configuration.PublishSubscribeChannelResourceType);
        }

        public IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics> CreateSubscriptionToPublishSubscribeChannelResourceManager()
        {
            return _factory.Create<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>(_configuration.SubscriptionToPublishSubscribeChannelResourceType);
        }

        public IBusInterceptor CreateBusInterceptor()
        {
            return _factory.Create<IBusInterceptor>(_configuration.BusInterceptorType);
        }

        public IRouterInterceptor CreateRouterInterceptor()
        {
            return _factory.Create<IRouterInterceptor>(_configuration.RouterInterceptorType);
        }

        public IMessageSerializer CreateMessageSerializer()
        {
            return _factory.Create<IMessageSerializer>(_configuration.MessageSerializerType);
        }

        public IChannelShuffler CreateChannelShuffler()
        {
            return _factory.Create<IChannelShuffler>(_configuration.ChannelShufflerType);
        }

        public IMessageAdapter CreateMessageAdapter(Type type)
        {
            if (type != null)
            {
                return _factory.Create<IMessageAdapter>(type);
            }
            else
            {
                return _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);
            }
        }

        private IPointToPointChannel CreatePointToPointChannel(Type type)
        {
            if(type==null)
            {
                return _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);
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
                return _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);
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
                return _factory.Create<IRequestReplyChannelFromPointToPointChannel>(_configuration.RequestReplyChannelFromPointToPointChannelType);
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
                return _factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(_configuration.RequestReplyFromSubscriptionToPublishSubscribeChannelType);
            }
            else
            {
                return _factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(type);
            }
        }

        public IMessageStorage CreateMessageStorage()
        {
            return _factory.Create<IMessageStorage>(_configuration.MessageStorageType);
        }

        public IEntityStorage CreateEntityStorage()
        {
            return _factory.Create<IEntityStorage>(_configuration.StorageType);
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