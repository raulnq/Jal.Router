using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.Attributes;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Installer
{
    public class RouterInstaller : IWindsorInstaller
    {

        private readonly Assembly[] _senderSourceAssemblies;

        private readonly Assembly[] _requestRouterConfigurationSourceAssemblies;

        public RouterInstaller(Assembly[] senderSourceAssemblies, Assembly[] requestRouterConfigurationSourceAssemblies)
        {
            _senderSourceAssemblies = senderSourceAssemblies;

            _requestRouterConfigurationSourceAssemblies = requestRouterConfigurationSourceAssemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = _senderSourceAssemblies;
           
            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    var types = (assembly.GetTypes().Where(type =>
                                                           {
                                                               var isTransient = type.GetCustomAttributes(false).OfType<IsTransientAttribute>().Any();
                                                               return isTransient && typeof(IRequestSender).IsAssignableFrom(type);
                    }));
                    foreach (var t in types)
                    {
                        var service = t.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestSender<,>));
                        if (service != null)
                        {
                            container.Register((Component.For(service).ImplementedBy(t).LifestyleTransient().Named(t.FullName)));
                        }
                    }

                    types = (assembly.GetTypes().Where(type => !type.GetCustomAttributes(false).OfType<IsTransientAttribute>().Any() && typeof(IRequestSender).IsAssignableFrom(type)));
                    foreach (var t in types)
                    {
                        var service = t.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestSender<,>));
                        if (service != null)
                        {
                            container.Register((Component.For(service).ImplementedBy(t).LifestyleSingleton().Named(t.FullName)));
                        }
                    }
                }
                container.Register(Component.For(typeof(IRequestRouter)).ImplementedBy(typeof(Impl.RequestRouter)).LifestyleSingleton());
                container.Register(Component.For(typeof(IRouteProvider)).ImplementedBy(typeof(RouteProvider)).LifestyleSingleton());
                container.Register(Component.For(typeof(IEndPointSource)).ImplementedBy(typeof(EndPointSource)).LifestyleSingleton());
                container.Register(Component.For(typeof(IRequestSenderSource)).ImplementedBy(typeof(RequestSenderSource)).LifestyleSingleton());
            }

            var assembliessource = _requestRouterConfigurationSourceAssemblies;

            if (assembliessource != null)
            {
                foreach (var assembly in assembliessource)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractRequestRouterConfigurationSource>().WithServiceAllInterfaces());
                }
            }
        }
    }
}
