using Jal.ChainOfResponsability.Microsoft.Extensions.DependencyInjection;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jal.Router.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static void AddRouter(this IServiceCollection container, Action<IRouterBuilder> action = null)
        {
            container.AddChainOfResponsability();

            var builder = new RouterBuilder(container);

            builder.Init();

            container.AddSingleton<IMonitor, Monitor>();

            container.AddSingleton<IEntityStorageFacade, EntityStorageFacade>();

            container.AddSingleton<ILogger, ConsoleLogger>();

            container.AddSingleton<IHasher, Hasher>();

            container.AddSingleton<IInMemoryTransport, InMemoryTransport>();

            container.AddSingleton<IFileSystemTransport, FileSystemTransport>();

            container.AddSingleton<IListenerContextLifecycle, ListenerContextLifecycle>();

            container.AddSingleton<IListenerContextLoader, ListenerContextLoader>();

            container.AddSingleton<ISenderContextLifecycle, SenderContextLifecycle>();

            container.AddSingleton<ISenderContextLoader, SenderContextLoader>();

            container.AddSingleton<IParameterProvider, ParameterProvider>();

            container.AddSingleton<IComponentFactoryFacade, ComponentFactoryFacade>();

            container.AddSingleton<IHost, Host>();

            container.AddSingleton<IProducer, Producer>();

            container.AddSingleton<IRouter, Impl.Router>();

            container.AddSingleton<IConsumer, Consumer>();

            container.AddSingleton<ITypedConsumer, TypedConsumer>();

            container.AddSingleton<IEndPointProvider, EndPointProvider>();

            container.AddSingleton<IComponentFactory, ComponentFactory>();

            container.AddSingleton<IBus, Bus>();

            container.AddSingleton<IConfiguration, Configuration>();

            container.AddSingleton<IStartup, Startup>();

            container.AddSingleton<IShutdown, Shutdown>();

            container.AddSingleton<IChannelValidator, ChannelValidator>();

            if (action != null)
            {
                action(builder);
            }
        }
    }
}
