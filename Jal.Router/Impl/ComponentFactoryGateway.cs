using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Impl
{
    public class ComponentFactoryGateway : IComponentFactoryGateway
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

        public ComponentFactoryGateway(IComponentFactory factory, IConfiguration configuration)
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

        public IMessageAdapter CreateMessageAdapter()
        {
            return _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);
        }

        public IPointToPointChannel CreatePointToPointChannel()
        {
            return _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);
        }

        public IPublishSubscribeChannel CreatePublishSubscribeChannel()
        {
            return _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);
        }

        public IRequestReplyChannelFromPointToPointChannel CreateRequestReplyChannelFromPointToPointChannel()
        {
            return _factory.Create<IRequestReplyChannelFromPointToPointChannel>(_configuration.RequestReplyChannelFromPointToPointChannelType);
        }

        public IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel CreateRequestReplyFromSubscriptionToPublishSubscribeChannel()
        {
            return _factory.Create<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>(_configuration.RequestReplyFromSubscriptionToPublishSubscribeChannelType);
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

        public ISenderChannel CreateSenderChannel(ChannelType channel)
        {
            if (channel == ChannelType.PointToPoint)
            {
                return CreatePointToPointChannel();
            }

            if (channel == ChannelType.PublishSubscribe)
            {
                return CreatePublishSubscribeChannel();
            }

            if (channel == ChannelType.RequestReplyToPointToPoint)
            {
                return CreateRequestReplyChannelFromPointToPointChannel();
            }

            if (channel == ChannelType.RequestReplyToSubscriptionToPublishSubscribe)
            {
                return CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();
            }

            return null;
        }

        public IListenerChannel CreateListenerChannel(ChannelType channel)
        {
            if (channel == ChannelType.PointToPoint)
            {
                return CreatePointToPointChannel();
            }

            if (channel == ChannelType.SubscriptionToPublishSubscribe)
            {
                return CreatePublishSubscribeChannel();
            }

            return null;
        }
    }
}