using System;
using System.Collections.Generic;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Interface.Management
{
    public interface IConfiguration
    {
        Type ChannelManagerType { get; }
        Type PointToPointChannelType { get; }
        Type PublishSubscribeChannelType { get; }
        Type StorageType { get;  }
        Type MessageBodyAdapterType { get; }
        Type MessageMetadataAdapterType { get; }
        IList<Type> RouterLoggerTypes { get; }
        Type RouterInterceptorType { get; }
        IList<Type> BusLoggerTypes { get; }
        Type BusInterceptorType { get; }
        Type MessageBodySerializerType { get; }
        IList<Type> MiddlewareTypes { get; }
        void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager;
        void UsingMessageBodyAdapter<TMessageAdapter>() where TMessageAdapter : IMessageBodyAdapter;
        void AddRouterLogger<TRouterLogger>() where TRouterLogger : IRouterLogger;
        void UsingStorage<TStorage>() where TStorage : IStorage;
        void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware;
        void UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        void UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        void AddBusLogger<TBusLogger>() where TBusLogger : IBusLogger;
        void UsingMessageBodySerializer<TSerializer>() where TSerializer : IMessageBodySerializer;
        void UsingMessageMetadataAdapter<TMessageContextAdapter>() where TMessageContextAdapter : IMessageMetadataAdapter;
    }
}