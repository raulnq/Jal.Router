using System;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Outbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Impl.Management
{
    public class Configuration : IConfiguration
    {
        public Type ChannelManagerType { get; private set; }
        public Type PointToPointChannelType { get; private set; }
        public Type PublishSubscribeChannelType { get; private set; }
        public Type StorageType { get; private set; }
        public Type MessageBodyAdapterType { get; private set; }
        public Type RouterLoggerType { get; private set; }
        public Type RouterInterceptorType { get; private set; }
        public Type BusLoggerType { get; private set; }
        public Type BusInterceptorType { get; private set; }
        public IList<Type> MiddlewareTypes { get; private set; }
        public Type MessageMetadataAdapterType { get; private set; }
        public Type MessageBodySerializerType { get; private set; }
        public void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel
        {
            PublishSubscribeChannelType = typeof(TPublishSubscribeChannel);
        }
        public void UsingMessageMetadataAdapter<TMessageContextAdapter>() where TMessageContextAdapter : IMessageMetadataAdapter
        {
            MessageMetadataAdapterType = typeof(TMessageContextAdapter);
        }

        public void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel
        {
            PointToPointChannelType = typeof(TPointToPointChannel);
        }

        public void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager
        {
            ChannelManagerType = typeof(TChannelManager);
        }

        public void UsingMessageBodySerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageBodySerializer
        {
            MessageBodySerializerType = typeof(TMessageBodySerializer);
        }

        public void UsingMessageBodyAdapter<TMessageAdapter>() where TMessageAdapter : IMessageBodyAdapter
        {
            MessageBodyAdapterType = typeof (TMessageAdapter);
        }

        public void UsingRouterLogger<TRouterLogger>() where TRouterLogger : IRouterLogger
        {
            RouterLoggerType = typeof(TRouterLogger);
        }
        public void UsingStorage<TStorage>() where TStorage : IStorage
        {
            StorageType = typeof(TStorage);
        }

        public void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware
        {
            MiddlewareTypes.Add(typeof(TMiddleware));
        }

        public void UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor
        {
            RouterInterceptorType = typeof(TRouterInterceptor);
        }
        public void UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor
        {
            BusInterceptorType = typeof(TBusInterceptor);
        }
        public void UsingBusLogger<TBusLogger>() where TBusLogger : IBusLogger
        {
            BusLoggerType = typeof(TBusLogger);
        }
        public Configuration()
        {
            UsingRouterLogger<NullRouterLogger>();
            UsingRouterInterceptor<NullRouterInterceptor>();
            UsingBusInterceptor<NullBusInterceptor>();
            UsingBusLogger<NullBusLogger>();
            UsingStorage<NullStorage>();
            UsingChannelManager<NullChannelManager>();
            UsingPointToPointChannel<NullPointToPointChannel>();
            UsingPublishSubscribeChannel<NullPublishSubscribeChannel>();
            UsingMessageBodySerializer<NullMessageBodySerializer>();
            UsingMessageBodyAdapter<NullMessageBodyAdapter>();
            UsingMessageMetadataAdapter<NullMessageMetadataAdapter>();
            MiddlewareTypes = new List<Type>();
        }
    }
}
