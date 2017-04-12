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

            container.Register(Component.For(typeof(IRouter)).ImplementedBy(typeof(Impl.Router)).LifestyleSingleton());
            container.Register(Component.For(typeof(IHandlerFactory)).ImplementedBy(typeof(HandlerFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouteProvider)).ImplementedBy(typeof(RouteProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IEndPointProvider)).ImplementedBy(typeof(EndPointProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRetryPolicyProvider)).ImplementedBy(typeof(RetryPolicyProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IEndPointSettingFinderFactory)).ImplementedBy(typeof(EndPointSettingFinderFactory)).LifestyleSingleton());
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
        }
    }
}
