﻿using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using System;

namespace Jal.Router.Interface
{
    public interface IComponentFactoryGateway
    {
        IConfiguration Configuration { get; }
        IBusInterceptor CreateBusInterceptor();

        IRouterInterceptor CreateRouterInterceptor();

        IMessageSerializer CreateMessageSerializer();

        IMessageAdapter CreateMessageAdapter();

        IPublishSubscribeChannel CreatePublishSubscribeChannel();

        IPointToPointChannel CreatePointToPointChannel();

        ISenderChannel CreateSenderChannel(ChannelType channel);

        IMessageStorage CreateMessageStorage();

        IChannelShuffler CreateChannelShuffler();

        IValueFinder CreateValueFinder(Type type);

        IEntityStorage CreateEntityStorage();

        TComponent CreateComponent<TComponent>(Type type) where TComponent : class;

        IChannelManager CreateChannelManager();

        ILogger<T> CreateLogger<T>(Type type);

        IStartupTask CreateStartupTask(Type type);

        IRequestReplyChannelFromPointToPointChannel CreateRequestReplyChannelFromPointToPointChannel();

        IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

        IListenerChannel CreateListenerChannel(ChannelType channel);

        IShutdownTask CreateShutdownTask(Type type);

        IMonitoringTask CreateMonitoringTask(Type type);
    }
}