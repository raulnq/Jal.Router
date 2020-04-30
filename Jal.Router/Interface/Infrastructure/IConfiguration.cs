using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IConfiguration
    {
        RuntimeContext Runtime { get; }
        Storage Storage { get; }
        string ApplicationName { get; }
        string DefaultTransportName { get; }
        IDictionary<Type, IList<Type>> LoggerTypes { get; }
        IDictionary<string, object> Parameters { get; }
        IList<Type> StartupTaskTypes { get; }
        IList<Type> ShutdownTaskTypes { get; }
        IList<MonitorTask> MonitoringTaskTypes { get; }
        IList<Type> ShutdownWatcherTypes { get; }
        Type ChannelShufflerType { get; }
        Type PointToPointChannelType { get; }
        Type PublishSubscribeChannelType { get; }
        Type SubscriptionToPublishSubscribeChannelType { get; }
        Type StorageType { get;  }
        Type MessageAdapterType { get; }
        IList<Type> RouterLoggerTypes { get; }
        Type RouterInterceptorType { get; }
        Type BusInterceptorType { get; }
        Type MessageSerializerType { get; }
        Type MessageStorageType { get; }
        IList<Type> RouteMiddlewareTypes { get; }
        IList<Type> EndpointMiddlewareTypes { get; }
        IConfiguration EnableStorage(bool ignoreexceptions = true);
        IConfiguration DisableStorage();
        IConfiguration SetDefaultTransportName(string name);
        IConfiguration SetApplicationName(string name);
        IConfiguration UseChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler;
        IConfiguration UsePublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel;
        IConfiguration UseSubscriptionToPublishSubscribeChannel<TSubscriptionToPublishSubscribeChannel>() where TSubscriptionToPublishSubscribeChannel : ISubscriptionToPublishSubscribeChannel;      
        IConfiguration UsePointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel;
        IConfiguration UseMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter;
        IConfiguration UseMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage;
        IConfiguration UseEntityStorage<TStorage>() where TStorage : IEntityStorage;
        IConfiguration AddShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher;
        IConfiguration AddRouteMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>;
        IConfiguration AddEndpointMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>;
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