using System;
using System.Collections.Generic;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IConfiguration
    {
        string ApplicationName { get; set; }
        IDictionary<Type, IList<Type>> LoggerTypes { get; }
        IList<Type> StartupTaskTypes { get; }
        IList<MonitoringTaskMetadata> MonitoringTaskTypes { get; }
        Type ChannelManagerType { get; }
        Type PointToPointChannelType { get; }
        Type PublishSubscribeChannelType { get; }
        Type StorageType { get;  }
        Type MessageBodyAdapterType { get; }
        Type MessageMetadataAdapterType { get; }
        IList<Type> RouterLoggerTypes { get; }
        Type RouterInterceptorType { get; }
        Type BusInterceptorType { get; }
        Type MessageBodySerializerType { get; }
        IList<Type> InboundMiddlewareTypes { get; }
        IList<Type> OutboundMiddlewareTypes { get; }
        void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager;
        void UsingMessageBodyAdapter<TMessageAdapter>() where TMessageAdapter : IMessageBodyAdapter;
        void UsingStorage<TStorage>() where TStorage : IStorage;
        void AddInboundMiddleware<TMiddleware>() where TMiddleware : Inbound.IMiddleware;
        void AddOutboundMiddleware<TMiddleware>() where TMiddleware : Outbound.IMiddleware;
        void UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        void UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        void UsingMessageBodySerializer<TSerializer>() where TSerializer : IMessageBodySerializer;
        void UsingMessageMetadataAdapter<TMessageContextAdapter>() where TMessageContextAdapter : IMessageMetadataAdapter;
        void AddMonitoringTask<TMonitoringTask>(int interval) where TMonitoringTask : IMonitoringTask;
        void AdStartupTask<TStartupTask>() where TStartupTask : IStartupTask;
        void AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>;
    }
}