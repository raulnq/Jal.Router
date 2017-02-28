using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Attributes;
using Jal.Router.Impl;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouter(this IServiceContainer container, Assembly[] messageHandlerSourceAssemblies, Assembly[] messageRouterConfigurationSourceAssemblies)
        {
            container.Register<IMessageRouter, MessageRouter>(new PerContainerLifetime());

            container.Register<IMessageHandlerFactory, MessageHandlerFactory>(new PerContainerLifetime());

            var assemblysources = messageRouterConfigurationSourceAssemblies;

            if (assemblysources != null)
            {
                foreach (var assemblysource in assemblysources)
                {
                    foreach (var exportedType in assemblysource.ExportedTypes)
                    {
                        if (exportedType.IsSubclassOf(typeof(AbstractMessageRouterConfigurationSource)))
                        {
                            container.Register(typeof(AbstractMessageRouterConfigurationSource), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }

            if (messageHandlerSourceAssemblies != null)
            {
                foreach (var assembly in messageHandlerSourceAssemblies)
                {
                    var types = (assembly.GetTypes().Where(type =>
                    {
                        var isTransient = type.GetCustomAttributes(false).OfType<IsTransientAttribute>().Any();

                        return isTransient && typeof(IMessageHandler).IsAssignableFrom(type);
                    }));

                    foreach (var t in types)
                    {
                        var service = t.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
                        if (service != null)
                        {
                            container.Register(service, t, t.FullName);
                        }
                    }

                    types = (assembly.GetTypes().Where(type => !type.GetCustomAttributes(false).OfType<IsTransientAttribute>().Any() && typeof(IMessageHandler).IsAssignableFrom(type)));

                    foreach (var t in types)
                    {
                        var service = t.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
                        if (service != null)
                        {
                            container.Register(service, t, t.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }
    }
}
