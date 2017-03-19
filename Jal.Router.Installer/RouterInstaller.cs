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
            container.Register(Component.For(typeof(IConsumerFactory)).ImplementedBy(typeof(ConsumerFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouteProvider)).ImplementedBy(typeof(RouteProvider)).LifestyleSingleton());

            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRouterConfigurationSource>().WithServiceAllInterfaces());
                }
            }
        }
    }
}
