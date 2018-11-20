using System;
using System.Collections.Generic;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
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
        public string ChannelProviderName { get; set; }
        public StorageConfiguration Storage { get; set; }
        public string ApplicationName { get; set; }
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
        public Type BusInterceptorType { get; private set; }
        public IList<Type> InboundMiddlewareTypes { get; }
        public IList<Type> OutboundMiddlewareTypes { get; }
        public Type MessageSerializerType { get; private set; }
        public void UsingChannelShuffler<TChannelShuffler>() where TChannelShuffler : IChannelShuffler
        {
            ChannelShufflerType = typeof(TChannelShuffler);
        }
        public void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel
        {
            PublishSubscribeChannelType = typeof(TPublishSubscribeChannel);
        }
        public void UsingMessageAdapter<TMessageAdapter>() where TMessageAdapter : IMessageAdapter
        {
            MessageAdapterType = typeof(TMessageAdapter);
        }

        public void UsingMessageStorage<TMessageStorage>() where TMessageStorage : IMessageStorage
        {
            MessageStorageType = typeof(TMessageStorage);
        }

        public void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel
        {
            PointToPointChannelType = typeof(TPointToPointChannel);
        }

        public void UsingRequestReplyChannel<TRequestReplyChannel>() where TRequestReplyChannel : IRequestReplyChannel
        {
            RequestReplyChannelType = typeof(TRequestReplyChannel);
        }

        public void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager
        {
            ChannelManagerType = typeof(TChannelManager);
        }

        public void UsingShutdownWatcher<TShutdownWatcher>() where TShutdownWatcher : IShutdownWatcher
        {
            ShutdownWatcherType = typeof(TShutdownWatcher);
        }

        public void UsingMessageSerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageSerializer
        {
            MessageSerializerType = typeof(TMessageBodySerializer);
        }

        public void UsingSagaStorage<TSagaStorage>() where TSagaStorage : ISagaStorage
        {
            SagaStorageType = typeof(TSagaStorage);
        }

        public void AddInboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>
        {
            InboundMiddlewareTypes.Add(typeof(TMiddleware));
        }

        public void AddOutboundMiddleware<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>
        {
            OutboundMiddlewareTypes.Add(typeof(TMiddleware));
        }

        public void UsingRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor
        {
            RouterInterceptorType = typeof(TRouterInterceptor);
        }
        public void UsingBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor
        {
            BusInterceptorType = typeof(TBusInterceptor);
        }

        public void AddLogger<TLogger, TInfo>() where TLogger : ILogger<TInfo>
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
        }

        public void AddMonitoringTask<TMonitoringTask>(int intervalinseconds) where TMonitoringTask : IMonitoringTask
        {
            MonitoringTaskTypes.Add(new TaskMetadata() {Type = typeof(TMonitoringTask), Interval = intervalinseconds*1000 });
        }
        public void AddStartupTask<TStartupTask>() where TStartupTask : IStartupTask
        {
            StartupTaskTypes.Add(typeof(TStartupTask));
        }

        public void AddShutdownTask<TShutdownTask>() where TShutdownTask : IShutdownTask
        {
            ShutdownTaskTypes.Add(typeof(TShutdownTask));
        }

        public Configuration()
        {
            UsingChannelShuffler<DefaultChannelShuffler>();
            UsingRouterInterceptor<NullRouterInterceptor>();
            UsingBusInterceptor<NullBusInterceptor>();
            UsingSagaStorage<NullSagaStorage>();
            UsingMessageStorage<NullMessageStorage>();
            UsingChannelManager<NullChannelManager>();
            UsingPointToPointChannel<NullPointToPointChannel>();
            UsingPublishSubscribeChannel<NullPublishSubscribeChannel>();
            UsingRequestReplyChannel<NullRequestReplyChannel>();
            UsingMessageSerializer<NullMessageSerializer>();
            UsingMessageAdapter<NullMessageAdapter>();
            InboundMiddlewareTypes = new List<Type>();
            RouterLoggerTypes = new List<Type>();
            MonitoringTaskTypes = new List<TaskMetadata>();
            StartupTaskTypes = new List<Type>();
            ShutdownTaskTypes = new List<Type>();
            LoggerTypes = new Dictionary<Type, IList<Type>>();
            OutboundMiddlewareTypes = new List<Type>();
            AddLogger<BeatLogger, Beat>(); 
            AddStartupTask<StartupBeatLogger>();
            AddStartupTask<RuntimeConfigurationLoader>();
            AddStartupTask<EndpointsInitializer>();
            AddStartupTask<RoutesInitializer>();
            AddStartupTask<PointToPointChannelCreator>();
            AddStartupTask<PublishSubscriberChannelCreator>();
            AddStartupTask<SubscriptionToPublishSubscribeChannelCreator>();
            AddStartupTask<ListenerLoader>();
            AddStartupTask<SenderLoader>();
            AddShutdownTask<ListenerShutdownTask>();
            AddShutdownTask<ShutdownTask>();
            UsingShutdownWatcher<ShutdownNullWatcher>();
            Storage = new StorageConfiguration();
            Identity = new IdentityConfiguration();
            Runtime = new Runtime();
            ApplicationName = "[Empty]";
            ChannelProviderName = "[Empty]";
        }
    }
}
