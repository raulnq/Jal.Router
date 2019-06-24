using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Interface.Management
{
    public interface IConfiguration
    {
        Runtime Runtime { get; }
        Storage Storage { get; }
        string ApplicationName { get; }
        string ChannelProviderName { get; }
        IDictionary<Type, IList<Type>> LoggerTypes { get; }
        IDictionary<string, object> Parameters { get; }
        IList<Type> StartupTaskTypes { get; }
        IList<Type> ShutdownTaskTypes { get; }
        IList<TaskMetadata> MonitoringTaskTypes { get; }
        Type ChannelManagerType { get; }
        IList<Type> ShutdownWatcherTypes { get; }
        Type ChannelShufflerType { get; }
        Type PointToPointChannelType { get; }
        Type RequestReplyChannelFromPointToPointChannelType { get; }
        Type RequestReplyFromSubscriptionToPublishSubscribeChannelType { get; }

        Type PublishSubscribeChannelType { get; }
        Type StorageType { get;  }
        Type MessageAdapterType { get; }
        IList<Type> RouterLoggerTypes { get; }
        Type RouterInterceptorType { get; set; }
        Type BusInterceptorType { get; set; }
        Type MessageSerializerType { get; }
        Type MessageStorageType { get; }
        IList<Type> InboundMiddlewareTypes { get; }
        IList<Type> OutboundMiddlewareTypes { get; }
        IConfiguration EnableEntityStorage(bool ignoreexceptions = true);
        IConfiguration DisableEntityStorage();
        IConfiguration SetChannelProviderName(string name);
        IConfiguration SetApplicationName(string name);
        IConfiguration UseChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler;
        IConfiguration UseRequestReplyChannelFromPointToPointChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromPointToPointChannel;
        IConfiguration UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel;

        IConfiguration UsePublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        IConfiguration UsePointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        IConfiguration UseChannelManager<TChannelManager>() where TChannelManager : IChannelManager;
        IConfiguration UseMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter;
        IConfiguration UseMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage;
        IConfiguration UseStorage<TStorage>() where TStorage : IEntityStorage;
        IConfiguration AddShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher;
        IConfiguration AddInboundMiddleware<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>;
        IConfiguration AddOutboundMiddleware<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>;
        IConfiguration UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor;
        IConfiguration UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor;
        IConfiguration UseMessageSerializer<TSerializer>() where TSerializer : IMessageSerializer;
        IConfiguration AddMonitoringTask<TMonitoringTask>(int intervalinseconds) where TMonitoringTask : IMonitoringTask;
        IConfiguration AddStartupTask<TStartupTask>() where TStartupTask : IStartupTask;
        IConfiguration AddShutdownTask<TShoutdown>() where TShoutdown : IShutdownTask;
        IConfiguration AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>;
        IConfiguration AddParameter<TParameter>(TParameter parameter);
        IConfiguration AddShutdownWatcher<TShutdownWatcher, TParameter>(TParameter parameter) where TShutdownWatcher : IShutdownWatcher;
    }
}