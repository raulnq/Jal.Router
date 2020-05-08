using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Configuration : IConfiguration
    {
        public RuntimeContext Runtime { get; }
        public string DefaultTransportName { get; private set; }
        public Storage Storage { get; }
        public string ApplicationName { get; private set; }
        public IDictionary<Type, IList<Type>> LoggerTypes { get; }
        public IList<Type> StartupTaskTypes { get; }
        public IList<Type> ShutdownTaskTypes { get; }
        public IList<MonitorTask> MonitoringTaskTypes { get; }
        public IList<Type> ShutdownWatcherTypes { get; private set; }
        public Type PointToPointChannelType { get; private set; }
        public Type PublishSubscribeChannelType { get; private set; }
        public Type SubscriptionToPublishSubscribeChannelType { get; private set; }

        public Type StorageType { get; private set; }
        public Type MessageAdapterType { get; private set; }
        public Type ChannelShufflerType { get; private set; }
        public Type MessageStorageType { get; private set; }
        public IList<Type> RouterLoggerTypes { get; }
        public Type RouterInterceptorType { get; private set; }
        public Type BusInterceptorType { get; private set; }
        public IList<Type> RouteMiddlewareTypes { get; }
        public IList<Type> EndpointMiddlewareTypes { get; }
        public Type MessageSerializerType { get; private set; }
        public IDictionary<string, object> Parameters { get; private set; }
        public IConfiguration EnableStorage(bool ignoreexceptions = true)
        {
            Storage.Enabled = true;
            Storage.IgnoreExceptions = ignoreexceptions;
            return this;
        }

        public IConfiguration DisableStorage()
        {
            Storage.Enabled = false;
            Storage.IgnoreExceptions = true;
            return this;
        }
        public IConfiguration UseChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler
        {
            ChannelShufflerType = typeof(TChannelShuffler);
            return this;
        }
        public IConfiguration UsePublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel
        {
            PublishSubscribeChannelType = typeof(TPublishSubscribeChannel);
            return this;
        }
        public IConfiguration UseSubscriptionToPublishSubscribeChannel<TSubscriptionToPublishSubscribeChannel>() where TSubscriptionToPublishSubscribeChannel : ISubscriptionToPublishSubscribeChannel
        {
            SubscriptionToPublishSubscribeChannelType = typeof(TSubscriptionToPublishSubscribeChannel);
            return this;
        }
        
        public IConfiguration UseMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter
        {
            MessageAdapterType = typeof(TMessageAdapter);
            return this;
        }

        public IConfiguration UseMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage
        {
            MessageStorageType = typeof(TMessageStorage);
            return this;
        }

        public IConfiguration UsePointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel
        {
            PointToPointChannelType = typeof(TPointToPointChannel);
            return this;
        }

        public IConfiguration AddShutdownWatcher<TShutdownWatcher, TParameter>(TParameter parameter) where TShutdownWatcher : IShutdownWatcher
        {
            ShutdownWatcherTypes.Add(typeof(TShutdownWatcher));

            AddParameter(parameter);

            return this;
        }

        public IConfiguration AddShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher
        {
            ShutdownWatcherTypes.Add(typeof(TShutdownWatcher));

            return this;
        }

        public IConfiguration UseMessageSerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageSerializer
        {
            MessageSerializerType = typeof(TMessageBodySerializer);
            return this;
        }

        public IConfiguration UseEntityStorage<TSagaStorage>() where TSagaStorage : IEntityStorage
        {
            StorageType = typeof(TSagaStorage);
            return this;
        }

        public IConfiguration AddRouteMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>
        {
            RouteMiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IConfiguration AddEndpointMiddleware<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>
        {
            EndpointMiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IConfiguration UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor
        {
            RouterInterceptorType = typeof(TRouterInterceptor);
            return this;
        }
        public IConfiguration UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor
        {
            BusInterceptorType = typeof(TBusInterceptor);
            return this;
        }

        public IConfiguration AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>
        {
            if (LoggerTypes.ContainsKey(typeof (TInfo)))
            {
                var list = LoggerTypes[typeof (TInfo)];
                list.Add(typeof(TLogger));
            }
            else
            {
                LoggerTypes.Add(typeof(TInfo), new List<Type>() {typeof(TLogger)});
            }
            return this;
        }

        public IConfiguration AddParameter<TParameter>(TParameter parameter)
        {
            var key = typeof(TParameter).FullName;

            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = parameter;
            }
            else
            {
                Parameters.Add(key, parameter);
            }
            return this;
        }

        public IConfiguration AddMonitoringTask<TMonitoringTask>(int intervalinseconds, bool runimmediately=false) where TMonitoringTask : IMonitoringTask
        {
            MonitoringTaskTypes.Add(new MonitorTask(typeof(TMonitoringTask), intervalinseconds * 1000, runimmediately));
            return this;
        }
        public IConfiguration AddStartupTask<TStartupTask>() where TStartupTask : IStartupTask
        {
            StartupTaskTypes.Add(typeof(TStartupTask));
            return this;
        }

        public IConfiguration AddShutdownTask<TShutdownTask>() where TShutdownTask : IShutdownTask
        {
            ShutdownTaskTypes.Add(typeof(TShutdownTask));
            return this;
        }

        public IConfiguration SetDefaultTransportName(string name)
        {
            DefaultTransportName = name;
            return this;
        }

        public IConfiguration SetApplicationName(string name)
        {
            ApplicationName = name;
            return this;
        }

        public Configuration()
        {
            UseChannelShuffler<DefaultChannelShuffler>();
            UseRouterInterceptor<NullRouterInterceptor>();
            UseBusInterceptor<NullBusInterceptor>();
            UseEntityStorage<NullEntityStorage>();
            UseMessageStorage<NullMessageStorage>();
            UsePointToPointChannel<NullPointToPointChannel>();
            UsePublishSubscribeChannel<NullPublishSubscribeChannel>();
            UseSubscriptionToPublishSubscribeChannel<NullSubscriptionToPublishSubscribeChannel>();
            UseMessageSerializer<NullMessageSerializer>();
            UseMessageAdapter<NullMessageAdapter>();
            RouteMiddlewareTypes = new List<Type>();
            RouterLoggerTypes = new List<Type>();
            MonitoringTaskTypes = new List<MonitorTask>();
            StartupTaskTypes = new List<Type>();
            ShutdownTaskTypes = new List<Type>();
            LoggerTypes = new Dictionary<Type, IList<Type>>();
            Parameters = new Dictionary<string, object>();
            EndpointMiddlewareTypes = new List<Type>();
            ShutdownWatcherTypes = new List<Type>();
            AddRouteMiddleware<RouterMiddleware>();
            AddEndpointMiddleware<BusMiddleware>();
            AddLogger<BeatLogger, Beat>();
            AddLogger<StatisticLogger, Statistic>();
            AddStartupTask<StartupLogger>();
            AddStartupTask<RuntimeLoader>();
            AddStartupTask<EndpointValidator>();
            AddStartupTask<RouteValidator>();
            AddStartupTask<SenderLoader>();
            AddStartupTask<ListenerLoader>();
            AddShutdownTask<ListenerShutdownTask>();
            AddShutdownTask<SenderShutdownTask>();
            AddShutdownTask<Jal.Router.Impl.ShutdownTask>();
            AddShutdownWatcher<CtrlCShutdownWatcher>();
            Storage = new Storage();
            Runtime = new RuntimeContext();
            ApplicationName = "Empty application name";
            DefaultTransportName = "Empty transport name";
        }
    }
}
