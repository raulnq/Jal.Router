using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _sourceassemblies;

        public RouterInstaller(Assembly[] sourceassemblies)
        {
            _sourceassemblies = sourceassemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(ITypedSagaRouter)).ImplementedBy(typeof(TypedSagaRouter)).LifestyleSingleton());
            container.Register(Component.For(typeof(ISagaRouterInvoker)).ImplementedBy(typeof(SagaRouterInvoker)).LifestyleSingleton());
            container.Register(Component.For(typeof(ISagaRouterProvider)).ImplementedBy(typeof(SagaRouterProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouterInvoker)).ImplementedBy(typeof(RouterInvoker)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRetryExecutor)).ImplementedBy(typeof(RetryExecutor)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRoutePicker)).ImplementedBy(typeof(RoutePicker)).LifestyleSingleton());
            container.Register(Component.For(typeof(IHandlerExecutor)).ImplementedBy(typeof(HandlerExecutor)).LifestyleSingleton());
            container.Register(Component.For(typeof(ITypedRouter)).ImplementedBy(typeof(TypedRouter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IHandlerFactory)).ImplementedBy(typeof(HandlerFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouteProvider)).ImplementedBy(typeof(RouteProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IEndPointProvider)).ImplementedBy(typeof(EndPointProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IEndPointSettingFinderFactory)).ImplementedBy(typeof(EndPointSettingFinderFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IValueSettingFinderFactory)).ImplementedBy(typeof(ValueSettingFinderFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(ConnectionStringValueSettingFinder)).Named(typeof(ConnectionStringValueSettingFinder).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IValueSettingFinder)).ImplementedBy(typeof(AppSettingValueSettingFinder)).Named(typeof(AppSettingValueSettingFinder).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouterConfigurationSource)).ImplementedBy(typeof(EmptyRouterConfigurationSource)).Named(typeof(EmptyRouterConfigurationSource).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IBus)).ImplementedBy(typeof(Bus)).LifestyleSingleton());
            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn<IValueSettingFinder>().WithServiceAllInterfaces());
                    container.Register(assemblyDescriptor.BasedOn(typeof(IEndPointSettingFinder<>)).WithServiceAllInterfaces());
                }
            }
            container.Register(Component.For(typeof(IStarter)).ImplementedBy(typeof(Starter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IStep)).ImplementedBy(typeof(TransportSetupStep)).LifestyleSingleton().Named(typeof(TransportSetupStep).FullName));
        }
    }
}
