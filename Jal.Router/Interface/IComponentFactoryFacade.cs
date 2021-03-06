﻿using Jal.Router.Model;
using System;

namespace Jal.Router.Interface
{
    public interface IComponentFactoryFacade
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

        IMessageAdapter CreateMessageAdapter(Type type);

        (IChannelSender, IChannelReader, IChannelCreator, IChannelDeleter, IChannelStatisticProvider) CreateSenderChannel(ChannelType channel, Type type);

        IMessageStorage CreateMessageStorage();

        IChannelShuffler CreateChannelShuffler();

        IEntityStorage CreateEntityStorage();

        TComponent CreateComponent<TComponent>(Type type) where TComponent : class;

        ILogger<T> CreateLogger<T>(Type type);

        IStartupTask CreateStartupTask(Type type);

        (IChannelListener, IChannelCreator, IChannelDeleter, IChannelStatisticProvider) CreateListenerChannel(ChannelType channel, Type type);

        IShutdownTask CreateShutdownTask(Type type);

        IMonitoringTask CreateMonitoringTask(Type type);
    }
}