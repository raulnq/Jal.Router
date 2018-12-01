using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Management.ShutdownWatcher;
using Jal.Router.Impl.Outbound;
using Jal.Router.Impl.Outbound.ChannelShuffler;
using Jal.Router.Impl.StartupTask;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class Configuration : IConfiguration
    {
        public Runtime Runtime { get; }
        public IdentityConfiguration Identity { get; }
        public string ChannelProviderName { get; private set; }
        public StorageConfiguration Storage { get; set; }
        public string ApplicationName { get; private set; }
        public IDictionary<Type, IList<Type>> LoggerTypes { get; }
        public IList<Type> StartupTaskTypes { get; }
        public IList<Type> ShutdownTaskTypes { get; }
        public IList<TaskMetadata> MonitoringTaskTypes { get; }
        public Type ChannelManagerType { get; private set; }
        public Type ShutdownWatcherType { get; private set; }
        public Type RequestReplyChannelType { get; private set; }
        public Type PointToPointChannelType { get; private set; }
        public Type PublishSubscribeChannelType { get; private set; }
        public Type SagaStorageType { get; private set; }
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

        public IConfiguration UseRequestReplyChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannel
        {
            RequestReplyChannelType = typeof(TRequestReplyChannel);
            return this;
        }

        public IConfiguration UseChannelManager<TChannelManager>() where TChannelManager : IChannelManager
        {
            ChannelManagerType = typeof(TChannelManager);
            return this;
        }

        public IConfiguration UseShutdownWatcher<TShutdownWatcher, TParameter>(TParameter parameter) where TShutdownWatcher : IShutdownWatcher
        {
            ShutdownWatcherType = typeof(TShutdownWatcher);

            AddParameter(parameter);

            return this;
        }

        public IConfiguration UseShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher
        {
            ShutdownWatcherType = typeof(TShutdownWatcher);
            return this;
        }

        public IConfiguration UseMessageSerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageSerializer
        {
            MessageSerializerType = typeof(TMessageBodySerializer);
            return this;
        }

        public IConfiguration UseSagaStorage<TSagaStorage>() where TSagaStorage : ISagaStorage
        {
            SagaStorageType = typeof(TSagaStorage);
            return this;
        }

        public IConfiguration AddInboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>
        {
            InboundMiddlewareTypes.Add(typeof(TMiddleware));
            return this;
        }

        public IConfiguration AddOutboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>
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
            MonitoringTaskTypes.Add(new TaskMetadata() {Type = typeof(TMonitoringTask), Interval = intervalinseconds*1000 });
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
            UseSagaStorage<NullSagaStorage>();
            UseMessageStorage<NullMessageStorage>();
            UseChannelManager<NullChannelManager>();
            UsePointToPointChannel<NullPointToPointChannel>();
            UsePublishSubscribeChannel<NullPublishSubscribeChannel>();
            UseRequestReplyChannel<NullRequestReplyChannel>();
            UseMessageSerializer<NullMessageSerializer>();
            UseMessageAdapter<NullMessageAdapter>();
            InboundMiddlewareTypes = new List<Type>();
            RouterLoggerTypes = new List<Type>();
            MonitoringTaskTypes = new List<TaskMetadata>();
            StartupTaskTypes = new List<Type>();
            ShutdownTaskTypes = new List<Type>();
            LoggerTypes = new Dictionary<Type, IList<Type>>();
            Parameters = new Dictionary<string, object>();
            OutboundMiddlewareTypes = new List<Type>();
            AddLogger<BeatLogger, Beat>(); 
            AddStartupTask<StartupBeatLogger>();
            AddStartupTask<RuntimeConfigurationLoader>();
            AddStartupTask<EndpointsInitializer>();
            AddStartupTask<RoutesInitializer>();
            AddStartupTask<PointToPointChannelCreator>();
            AddStartupTask<PublishSubscriberChannelCreator>();
            AddStartupTask<SubscriptionToPublishSubscribeChannelCreator>();
            AddStartupTask<SenderLoader>();
            AddStartupTask<ListenerLoader>();
            AddShutdownTask<ListenerShutdownTask>();
            AddShutdownTask<SenderShutdownTask>();
            AddShutdownTask<ShutdownTask>();
            UseShutdownWatcher<CtrlCShutdownWatcher>();
            Storage = new StorageConfiguration();
            Identity = new IdentityConfiguration();
            Runtime = new Runtime();
            ApplicationName = "Empty app name";
            ChannelProviderName = "Empty channel provider name";
        }
    }
}
