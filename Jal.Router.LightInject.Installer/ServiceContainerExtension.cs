using System.Reflection;
using Jal.Router.Impl;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouter(this IServiceContainer container, Assembly[] sourceassemblies)
        {
            container.Register<IRouter, Impl.Router>(new PerContainerLifetime());

            container.Register<IConsumerFactory, ConsumerFactory>(new PerContainerLifetime());

            container.Register<IRouteProvider, RouteProvider>(new PerContainerLifetime());

            if (sourceassemblies != null)
            {
                foreach (var assemblysource in sourceassemblies)
                {
                    foreach (var exportedType in assemblysource.ExportedTypes)
                    {
                        if (exportedType.IsSubclassOf(typeof(AbstractRouterConfigurationSource)))
                        {
                            container.Register(typeof(AbstractRouterConfigurationSource), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }
    }
}
