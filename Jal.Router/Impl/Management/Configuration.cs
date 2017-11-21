using System;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound;
using Jal.Router.Impl.Inbound.Sagas;
using Jal.Router.Impl.Outbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class Configuration : IConfiguration
    {
        public string ApplicationName { get; set; }
        public IDictionary<Type, IList<Type>> LoggerTypes { get; private set; }
        public IList<Type> StartupTaskTypes { get; private set; }
        public IList<MonitoringTaskMetadata> MonitoringTaskTypes { get; private set; }
        public Type ChannelManagerType { get; private set; }
        public Type PointToPointChannelType { get; private set; }
        public Type PublishSubscribeChannelType { get; private set; }
        public Type StorageType { get; private set; }
        public Type MessageBodyAdapterType { get; private set; }
        public IList<Type> RouterLoggerTypes { get; private set; }
        public Type RouterInterceptorType { get; private set; }
        public Type BusInterceptorType { get; private set; }
        public IList<Type> InboundMiddlewareTypes { get; private set; }
        public IList<Type> OutboundMiddlewareTypes { get; private set; }
        public Type MessageMetadataAdapterType { get; private set; }
        public Type MessageBodySerializerType { get; private set; }
        public void UsingPublishSubscribeChannel<TPublishSubscribeChannel>() where TPublishSubscribeChannel : IPublishSubscribeChannel
        {
            PublishSubscribeChannelType = typeof(TPublishSubscribeChannel);
        }
        public void UsingMessageMetadataAdapter<TMessageContextAdapter>() where TMessageContextAdapter : IMessageMetadataAdapter
        {
            MessageMetadataAdapterType = typeof(TMessageContextAdapter);
        }

        public void UsingPointToPointChannel<TPointToPointChannel>() where TPointToPointChannel : IPointToPointChannel
        {
            PointToPointChannelType = typeof(TPointToPointChannel);
        }

        public void UsingChannelManager<TChannelManager>() where TChannelManager : IChannelManager
        {
            ChannelManagerType = typeof(TChannelManager);
        }

        public void UsingMessageBodySerializer<TMessageBodySerializer>() where TMessageBodySerializer : IMessageBodySerializer
        {
            MessageBodySerializerType = typeof(TMessageBodySerializer);
        }

        public void UsingMessageBodyAdapter<TMessageAdapter>() where TMessageAdapter : IMessageBodyAdapter
        {
            MessageBodyAdapterType = typeof (TMessageAdapter);
        }

        public void UsingStorage<TStorage>() where TStorage : IStorage
        {
            StorageType = typeof(TStorage);
        }

        public void AddInboundMiddleware<TMiddleware>() where TMiddleware : Jal.Router.Interface.Inbound.IMiddleware
        {
            InboundMiddlewareTypes.Add(typeof(TMiddleware));
        }

        public void AddOutboundMiddleware<TMiddleware>() where TMiddleware : Jal.Router.Interface.Outbound.IMiddleware
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

        public void AddMonitoringTask<TMonitoringTask>(int interval) where TMonitoringTask : IMonitoringTask
        {
            MonitoringTaskTypes.Add(new MonitoringTaskMetadata() {Type = typeof(TMonitoringTask), Interval = interval });
        }
        public void AdStartupTask<TStartupTask>() where TStartupTask : IStartupTask
        {
            StartupTaskTypes.Add(typeof(TStartupTask));
        }
        public Configuration()
        {
            UsingRouterInterceptor<NullRouterInterceptor>();
            UsingBusInterceptor<NullBusInterceptor>();
            UsingStorage<NullStorage>();
            UsingChannelManager<NullChannelManager>();
            UsingPointToPointChannel<NullPointToPointChannel>();
            UsingPublishSubscribeChannel<NullPublishSubscribeChannel>();
            UsingMessageBodySerializer<NullMessageBodySerializer>();
            UsingMessageBodyAdapter<NullMessageBodyAdapter>();
            UsingMessageMetadataAdapter<NullMessageMetadataAdapter>();
            InboundMiddlewareTypes = new List<Type>();
            RouterLoggerTypes = new List<Type>();
            MonitoringTaskTypes = new List<MonitoringTaskMetadata>();
            StartupTaskTypes = new List<Type>();
            LoggerTypes = new Dictionary<Type, IList<Type>>();
            OutboundMiddlewareTypes = new List<Type>();
        }
    }
}
