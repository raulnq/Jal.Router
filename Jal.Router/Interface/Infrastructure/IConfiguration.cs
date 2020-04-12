using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IConfiguration
    {
        RuntimeContext Runtime { get; }
        Storage Storage { get; }
        string ApplicationName { get; }
        string TransportName { get; }
        IDictionary<Type, IList<Type>> LoggerTypes { get; }
        IDictionary<string, object> Parameters { get; }
        IList<Type> StartupTaskTypes { get; }
        IList<Type> ShutdownTaskTypes { get; }
        IList<MonitorTask> MonitoringTaskTypes { get; }
        Type PublishSubscribeChannelResourceType { get; }
        Type PointToPointChannelResourceType { get; }
        Type SubscriptionToPublishSubscribeChannelResourceType { get; }
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
        IConfiguration EnableStorage(bool ignoreexceptions = true);
        IConfiguration DisableStorage();
        IConfiguration SetTransportName(string name);
        IConfiguration SetApplicationName(string name);
        IConfiguration UseChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler;
        IConfiguration UseRequestReplyChannelFromPointToPointChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromPointToPointChannel;
        IConfiguration UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel;

        IConfiguration UsePublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        IConfiguration UsePointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        IConfiguration UsePointToPointChannelResourceManager<TChannel>() where TChannel : IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>;

        IConfiguration UsePublishSubscribeChannelResourceManager<TChannel>() where TChannel : IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>;

        IConfiguration UseSubscriptionToPublishSubscribeChannelResourceManager<TChannel>() where TChannel : IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>;

        IConfiguration UseMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter;
        IConfiguration UseMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage;
        IConfiguration UseEntityStorage<TStorage>() where TStorage : IEntityStorage;
        IConfiguration AddShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher;
        IConfiguration AddInboundMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>;
        IConfiguration AddOutboundMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>;
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