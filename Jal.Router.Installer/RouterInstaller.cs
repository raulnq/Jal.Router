using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using System;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {
        private readonly Action<IRouterBuilder> _action;

        public RouterInstaller(Action<IRouterBuilder> action)
        {
            _action = action;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddChainOfResponsability();

            var builder = new RouterBuilder(container);

            builder.Init();

            container.Register(Component.For<IComponentFactoryFacade>().ImplementedBy<ComponentFactoryFacade>().LifestyleSingleton());

            container.Register(Component.For<IHost>().ImplementedBy<Host>().Named(typeof(Host).FullName).LifestyleSingleton());

            container.Register(Component.For<IEntityStorageFacade>().ImplementedBy<EntityStorageFacade>().LifestyleSingleton());

            container.Register(Component.For<IInMemoryTransport>().ImplementedBy<InMemoryTransport>().LifestyleSingleton());

            container.Register(Component.For<IFileSystemTransport>().ImplementedBy<FileSystemTransport>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextLifecycle>().ImplementedBy<ListenerContextLifecycle>().LifestyleSingleton());

            container.Register(Component.For<IListenerContextLoader>().ImplementedBy<ListenerContextLoader>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextLifecycle>().ImplementedBy<SenderContextLifecycle>().LifestyleSingleton());

            container.Register(Component.For<ISenderContextLoader>().ImplementedBy<SenderContextLoader>().LifestyleSingleton());

            container.Register(Component.For<ILogger>().ImplementedBy<Impl.ConsoleLogger>().LifestyleSingleton());

            container.Register(Component.For<IRouter>().ImplementedBy<Impl.Router>().LifestyleSingleton());

            container.Register(Component.For<IParameterProvider>().ImplementedBy<ParameterProvider>().LifestyleSingleton());

            container.Register(Component.For<IConsumer>().ImplementedBy<Consumer>().LifestyleSingleton());

            container.Register(Component.For<ITypedConsumer>().ImplementedBy<TypedConsumer>().LifestyleSingleton());

            container.Register(Component.For<IEndPointProvider>().ImplementedBy<EndPointProvider>().LifestyleSingleton());

            container.Register(Component.For<IBus>().ImplementedBy<Bus>().LifestyleSingleton());

            container.Register(Component.For<IProducer>().ImplementedBy<Producer>().LifestyleSingleton());

            container.Register(Component.For<IComponentFactory>().ImplementedBy<ComponentFactory>().LifestyleSingleton());

            container.Register(Component.For<IStartup>().ImplementedBy<Startup>().LifestyleSingleton());

            container.Register(Component.For<IShutdown>().ImplementedBy<Shutdown>().LifestyleSingleton());

            container.Register(Component.For<IConfiguration>().ImplementedBy<Configuration>().LifestyleSingleton());

            container.Register(Component.For<IChannelValidator>().ImplementedBy<ChannelValidator>().LifestyleSingleton());

            container.Register(Component.For<IMonitor>().ImplementedBy<Monitor>().LifestyleSingleton());

            if (_action!=null)
            {
                _action(builder);
            }
        }
    }
}
