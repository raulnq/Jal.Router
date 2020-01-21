using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Configuration : IConfiguration
    {
        public RuntimeContext Runtime { get; }
        public string ChannelProviderName { get; private set; }
        public Storage Storage { get; set; }
        public string ApplicationName { get; private set; }
        public IDictionary<Type, IList<Type>> LoggerTypes { get; }
        public IList<Type> StartupTaskTypes { get; }
        public IList<Type> ShutdownTaskTypes { get; }
        public IList<MonitorTask> MonitoringTaskTypes { get; }
        public Type PointToPointChannelResourceType { get; private set; }
        public Type PublishSubscribeChannelResourceType { get; private set; }
        public Type SubscriptionToPublishSubscribeChannelResourceType { get; private set; }
        public IList<Type> ShutdownWatcherTypes { get; private set; }
        public Type RequestReplyChannelFromPointToPointChannelType { get; private set; }
        public Type RequestReplyFromSubscriptionToPublishSubscribeChannelType { get; private set; }
        public Type PointToPointChannelType { get; private set; }
        public Type PublishSubscribeChannelType { get; private set; }
        public Type StorageType { get; private set; }
        public Type MessageAdapterType { get; private set; }
        public Type ChannelShufflerType { get; private set; }
        public Type MessageStorageType { get; private set; }
        public IList<Type> RouterLoggerTypes { get; }
        public Type RouterInterceptorType { get; set; }
        public Type BusInterceptorType { get; set; }
        public IList<Type> InboundMiddlewareTypes { get; }
        public IList<Type> OutboundMiddlewareTypes { get; }
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

        public IConfiguration UseRequestReplyChannelFromPointToPointChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromPointToPointChannel
        {
            RequestReplyChannelFromPointToPointChannelType = typeof(TRequestReplyChannel);
            return this;
        }

        public IConfiguration UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
        {
            RequestReplyFromSubscriptionToPublishSubscribeChannelType = typeof(TRequestReplyChannel);
            return this;
        }

        public IConfiguration UsePublishSubscribeChannelResource<TChannel>() where TChannel : IChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>
        {
            PublishSubscribeChannelResourceType = typeof(TChannel);
            return this;
        }

        public IConfiguration UsePointToPointChannelResource<TChannel>() where TChannel : IChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>
        {
            PointToPointChannelResourceType = typeof(TChannel);
            return this;
        }

        public IConfiguration UseSubscriptionToPublishSubscribeChannelResource<TChannel>() where TChannel : IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>
        {
            SubscriptionToPublishSubscribeChannelResourceType = typeof(TChannel);
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

        public IConfiguration UseStorage<TSagaStorage>() where TSagaStorage : IEntityStorage
        {
            StorageType = typeof(TSagaStorage);
            return this;
        }

        public IConfiguration AddInboundMiddleware<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>
        {
            InboundMiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IConfiguration AddOutboundMiddleware<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>
        {
            OutboundMiddlewareTypes.Add(typeof(TMiddleware));
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

        public IConfiguration AddMonitoringTask<TMonitoringTask>(int intervalinseconds) where TMonitoringTask : IMonitoringTask
        {
            MonitoringTaskTypes.Add(new MonitorTask(typeof(TMonitoringTask), intervalinseconds * 1000));
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

        public IConfiguration SetChannelProviderName(string name)
        {
            ChannelProviderName = name;
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
            UseStorage<NullStorage>();
            UseMessageStorage<NullMessageStorage>();
            UsePointToPointChannelResource<NullChannelResource<PointToPointChannelResource, PointToPointChannelStatistics>>();
            UsePublishSubscribeChannelResource<NullChannelResource<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>();
            UseSubscriptionToPublishSubscribeChannelResource<NullChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();
            UsePointToPointChannel<NullPointToPointChannel>();
            UsePublishSubscribeChannel<NullPublishSubscribeChannel>();
            UseRequestReplyChannelFromPointToPointChannel<NullRequestReplyChannelFromPointToPointChannel>();
            UseRequestReplyChannelFromSubscriptionToPublishSubscribeChannel<NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();
            UseMessageSerializer<NullMessageSerializer>();
            UseMessageAdapter<NullMessageAdapter>();
            InboundMiddlewareTypes = new List<Type>();
            RouterLoggerTypes = new List<Type>();
            MonitoringTaskTypes = new List<MonitorTask>();
            StartupTaskTypes = new List<Type>();
            ShutdownTaskTypes = new List<Type>();
            LoggerTypes = new Dictionary<Type, IList<Type>>();
            Parameters = new Dictionary<string, object>();
            OutboundMiddlewareTypes = new List<Type>();
            ShutdownWatcherTypes = new List<Type>();
            AddLogger<BeatLogger, Beat>();
            AddLogger<PointToPointChannelStatisticsLogger, PointToPointChannelStatistics>();
            AddLogger<PublishSubscribeChannelStatisticsLogger, PublishSubscribeChannelStatistics>();
            AddLogger<SubscriptionToPublishSubscribeChannelStatisticsLogger, SubscriptionToPublishSubscribeChannelStatistics>();
            AddStartupTask<StartupBeatLogger>();
            AddStartupTask<RuntimeConfigurationLoader>();
            AddStartupTask<EndpointsInitializer>();
            AddStartupTask<RoutesInitializer>();
            AddStartupTask<PointToPointChannelResourceCreator>();
            AddStartupTask<PublishSubscribeChannelResourceCreator>();
            AddStartupTask<SubscriptionToPublishSubscribeChannelResourceCreator>();
            AddStartupTask<SenderLoader>();
            AddStartupTask<ListenerLoader>();
            AddShutdownTask<ListenerShutdownTask>();
            AddShutdownTask<SenderShutdownTask>();
            AddShutdownTask<Jal.Router.Impl.ShutdownTask>();
            AddShutdownWatcher<CtrlCShutdownWatcher>();
            Storage = new Storage();
            Runtime = new RuntimeContext();
            ApplicationName = "Empty app name";
            ChannelProviderName = "Empty channel provider name";
        }
    }
}
