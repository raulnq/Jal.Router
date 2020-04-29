using Jal.ChainOfResponsability.LightInject.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using LightInject;
using System;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddRouter(this IServiceContainer container, Action<IRouterBuilder> action = null)
        {
            container.AddChainOfResponsability();

            var builder = new RouterBuilder(container);

            builder.Init();

            container.Register<IMonitor, Monitor>(new PerContainerLifetime());

            container.Register<IEntityStorageFacade, EntityStorageFacade>(new PerContainerLifetime());

            container.Register<ILogger, ConsoleLogger>(new PerContainerLifetime());

            container.Register<IHasher, Hasher>(new PerContainerLifetime());

            container.Register<IInMemoryTransport, InMemoryTransport>(new PerContainerLifetime());

            container.Register<IFileSystemTransport, FileSystemTransport>(new PerContainerLifetime());

            container.Register<IListenerContextLifecycle, ListenerContextLifecycle>(new PerContainerLifetime());

            container.Register<IListenerContextLoader, ListenerContextLoader>(new PerContainerLifetime());

            container.Register<ISenderContextLifecycle, SenderContextLifecycle>(new PerContainerLifetime());

            container.Register<IResourceContextLifecycle, ResourceContextLifecycle>(new PerContainerLifetime());

            container.Register<ISenderContextLoader, SenderContextLoader>(new PerContainerLifetime());

            container.Register<IParameterProvider, ParameterProvider>(new PerContainerLifetime());

            container.Register<IComponentFactoryFacade, ComponentFactoryFacade>(new PerContainerLifetime());

            container.Register<IHost, Host>(new PerContainerLifetime());

            container.Register<IProducer, Producer>(new PerContainerLifetime());

            container.Register<IRouter, Impl.Router>(new PerContainerLifetime());

            container.Register<IConsumer, Consumer>(new PerContainerLifetime());

            container.Register<ITypedConsumer, TypedConsumer>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IComponentFactory, ComponentFactory>(new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

            container.Register<IConfiguration, Configuration>(new PerContainerLifetime());

            container.Register<IStartup, Startup>(new PerContainerLifetime());

            container.Register<IShutdown, Shutdown>(new PerContainerLifetime());

            container.Register<IChannelValidator, ChannelValidator>(new PerContainerLifetime());

            if (action!=null)
            {
                action(builder);
            }
        }
    }
}
