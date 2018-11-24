using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IConfiguration
    {
        Runtime Runtime { get; }
        IdentityConfiguration Identity { get; }
        StorageConfiguration Storage { get; set; }
        string ApplicationName { get; set; }
        string ChannelProviderName { get; set; }
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
        Type BusInterceptorType { get; set; }
        Type MessageSerializerType { get; }
        Type MessageStorageType { get; }
        IList<Type> InboundMiddlewareTypes { get; }
        IList<Type> OutboundMiddlewareTypes { get; }
        void UseChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler;
        void UseRequestReplyChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannel;
        void UsePublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        void UsePointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        void UseChannelManager<TChannelManager>() where TChannelManager : IChannelManager;
        void UseMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter;
        void UseMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage;
        void UseSagaStorage<TStorage>() where TStorage : ISagaStorage;
        void UseShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher;
        void AddInboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>;
        void AddOutboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>;
        void UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        void UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        void UseMessageSerializer<TSerializer>() where TSerializer : IMessageSerializer;
        void AddMonitoringTask<TMonitoringTask>(int intervalinseconds) where TMonitoringTask : IMonitoringTask;
        void AddStartupTask<TStartupTask>() where TStartupTask : IStartupTask;
        void AddShutdownTask<TShoutdown>() where TShoutdown : IShutdownTask;
        void AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>;
    }
}