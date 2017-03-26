using System.Linq;
using System.Reflection;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterBrokeredMessageRouter(this IServiceContainer container, Assembly[] sourceassemblies)
        {
            container.Register<IBrokeredMessageAdapter, BrokeredMessageAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageRouter, BrokeredMessageRouter>(new PerContainerLifetime());

            container.Register<IBus, BrokeredMessageBus>(new PerContainerLifetime());

            container.Register<IContextBuilder, ContextBuilder>(new PerContainerLifetime());
        }
    }
}
