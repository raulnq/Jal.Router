﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Interface
{
    public interface IComponentFactoryGateway
    {
        IRouteErrorMessageHandler CreateRouteErrorMessageHandler(Type type);

        IRouteEntryMessageHandler CreateRouteEntryMessageHandler(Type type);

        IRouteExitMessageHandler CreateRouteExitMessageHandler(Type type);

        IBusErrorMessageHandler CreateBusErrorMessageHandler(Type type);

        IBusEntryMessageHandler CreateBusEntryMessageHandler(Type type);

        IBusExitMessageHandler CreateBusExitMessageHandler(Type type);

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

        IChannelResource CreateChannelResource();

        ILogger<T> CreateLogger<T>(Type type);

        IStartupTask CreateStartupTask(Type type);

        IRequestReplyChannelFromPointToPointChannel CreateRequestReplyChannelFromPointToPointChannel();

        IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel CreateRequestReplyFromSubscriptionToPublishSubscribeChannel();

        IListenerChannel CreateListenerChannel(ChannelType channel);

        IShutdownTask CreateShutdownTask(Type type);

        IMonitoringTask CreateMonitoringTask(Type type);
    }
}