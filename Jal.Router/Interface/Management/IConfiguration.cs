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
        StorageConfiguration Storage { get; set; }
        string ApplicationName { get; set; }
        IDictionary<Type, IList<Type>> LoggerTypes { get; }
        IList<Type> StartupTaskTypes { get; }
        IList<Type> ShutdownTaskTypes { get; }
        IList<TaskMetadata> MonitoringTaskTypes { get; }
        Type ChannelManagerType { get; }
        Type ShutdownWatcherType { get; }
        Type ChannelShufflerType { get; }
        Type PointToPointChannelType { get; }
        Type RequestReplyChannelType { get; }
        Type PublishSubscribeChannelType { get; }
        Type SagaStorageType { get;  }
        Type MessageAdapterType { get; }
        IList<Type> RouterLoggerTypes { get; }
        Type RouterInterceptorType { get; set; }
        Type BusInterceptorType { get; }
        Type MessageSerializerType { get; }
        Type MessageStorageType { get; }
        IList<Type> InboundMiddlewareTypes { get; }
        IList<Type> OutboundMiddlewareTypes { get; }
        void UsingChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler;
        void UsingRequestReplyChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannel;
        void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager;
        void UsingMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter;
        void UsingMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage;
        void UsingSagaStorage<TStorage>() where TStorage : ISagaStorage;
        void UsingShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher;
        void AddInboundMiddleware<TMiddleware>() where TMiddleware : Inbound.IMiddleware;
        void AddOutboundMiddleware<TMiddleware>() where TMiddleware : Outbound.IMiddleware;
        void UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        void UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        void UsingMessageSerializer<TSerializer>() where TSerializer : IMessageSerializer;
        void AddMonitoringTask<TMonitoringTask>(int interval) where TMonitoringTask : IMonitoringTask;
        void AddStartupTask<TStartupTask>() where TStartupTask : IStartupTask;
        void AddShutdownTask<TShoutdown>() where TShoutdown : IShutdownTask;
        void AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>;
    }
}