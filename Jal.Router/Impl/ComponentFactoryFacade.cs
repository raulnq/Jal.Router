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

        private ISubscriptionToPublishSubscribeChannel CreateSubscriptionToPublishSubscribeChannel(Type type)
        {
            if (type == null)
            {
                return _factory.Create<ISubscriptionToPublishSubscribeChannel>(Configuration.SubscriptionToPublishSubscribeChannelType);
            }
            else
            {
                return _factory.Create<ISubscriptionToPublishSubscribeChannel>(type);
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

        public (IChannelSender,IChannelReader,IChannelCreator, IChannelDeleter, IChannelStatisticProvider) CreateSenderChannel(ChannelType channel, Type type)
        {
            var senderchannel = default(IChannelSender);

            var readerchannel = default(IChannelReader);

            var channelcreator = default(IChannelCreator);

            var channeldeleter = default(IChannelDeleter);

            var channelprovider = default(IChannelStatisticProvider);

            if (channel == ChannelType.PointToPoint)
            {
                var pointtopointchannel = CreatePointToPointChannel(type);

                senderchannel = pointtopointchannel;

                channelcreator = pointtopointchannel;

                readerchannel = pointtopointchannel;

                channeldeleter = pointtopointchannel;

                channelprovider = pointtopointchannel;
            }

            if (channel == ChannelType.PublishSubscribe)
            {
                var publishsubscribechannel = CreatePublishSubscribeChannel(type);

                senderchannel = publishsubscribechannel;

                channelcreator = publishsubscribechannel;

                readerchannel = publishsubscribechannel;

                channeldeleter = publishsubscribechannel;

                channelprovider = publishsubscribechannel;
            }

            return (senderchannel,readerchannel, channelcreator, channeldeleter, channelprovider);
        }

        public (IChannelListener, IChannelCreator, IChannelDeleter, IChannelStatisticProvider) CreateListenerChannel(ChannelType channel, Type type)
        {
            var listenerchannel = default(IChannelListener);

            var channelcreator = default(IChannelCreator);

            var channeldeleter = default(IChannelDeleter);

            var channelprovider = default(IChannelStatisticProvider);

            if (channel == ChannelType.PointToPoint)
            {
                var pointtopointchannel = CreatePointToPointChannel(type);

                listenerchannel = pointtopointchannel;

                channelcreator = pointtopointchannel;

                channeldeleter = pointtopointchannel;

                channelprovider = pointtopointchannel;
            }

            if (channel == ChannelType.SubscriptionToPublishSubscribe)
            {
                var subscriptiontopublishsubscribechannel = CreateSubscriptionToPublishSubscribeChannel(type);

                listenerchannel = subscriptiontopublishsubscribechannel;

                channelcreator = subscriptiontopublishsubscribechannel;

                channeldeleter = subscriptiontopublishsubscribechannel;

                channelprovider = subscriptiontopublishsubscribechannel;
            }

            return (listenerchannel, channelcreator, channeldeleter, channelprovider);
        }
    }
}