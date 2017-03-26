using System.Linq;
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

            container.Register<IHandlerFactory, HandlerFactory>(new PerContainerLifetime());

            container.Register<IRouteProvider, RouteProvider>(new PerContainerLifetime());

            container.Register<IEndPointProvider, EndPointProvider>(new PerContainerLifetime());

            container.Register<IEndPointSettingFinderFactory, EndPointSettingFinderFactory>(new PerContainerLifetime());

            container.Register<IEndPointValueSettingFinder, AppSettingEndPointValueSettingFinder>(typeof(AppSettingEndPointValueSettingFinder).FullName ,new PerContainerLifetime());

            container.Register<IRouterConfigurationSource, EmptyRouterConfigurationSource>(typeof(EmptyRouterConfigurationSource).FullName, new PerContainerLifetime());

            container.Register<IBus, Bus>(new PerContainerLifetime());

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

                        if (typeof(IEndPointValueSettingFinder).IsAssignableFrom(exportedType))
                        {
                            container.Register(typeof(IEndPointValueSettingFinder), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }

                        if (exportedType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEndPointSettingFinder<>)))
                        {
                            var argument = exportedType.GetGenericArguments();

                            var type = typeof(IEndPointSettingFinder<>);

                            var genericType = type.MakeGenericType(argument);

                            container.Register(genericType, exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }
    }
}
