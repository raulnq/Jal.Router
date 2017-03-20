using System.Reflection;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterBrokeredMessageRouter(this IServiceContainer container, Assembly[] sourceassemblies)
        {
            container.Register<IBrokeredMessageAdapter, BrokeredMessageAdapter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageRouter, BrokeredMessageRouter>(new PerContainerLifetime());

            container.Register<IBrokeredMessageContextBuilder, BrokeredMessageContextBuilder>(new PerContainerLifetime());

            container.Register<IBrokeredMessageEndPointProvider, BrokeredMessageEndPointProvider>(new PerContainerLifetime());

            container.Register<IBrokeredMessageSettingsExtractorFactory, BrokeredMessageSettingsExtractorFactory>(new PerContainerLifetime());

            container.Register<IBrokeredMessageSettingsExtractor, AppBrokeredMessageSettingsExtractor>(new PerContainerLifetime());

            container.Register<IBrokeredMessageRouterConfigurationSource, EmptyBrokeredMessageRouterConfigurationSource>(typeof(EmptyBrokeredMessageRouterConfigurationSource).FullName,new PerContainerLifetime());

            if (sourceassemblies != null)
            {
                foreach (var assemblysource in sourceassemblies)
                {
                    foreach (var exportedType in assemblysource.ExportedTypes)
                    {
                        if (exportedType.IsSubclassOf(typeof(AbstractBrokeredMessageRouterConfigurationSource)))
                        {
                            container.Register(typeof(AbstractBrokeredMessageRouterConfigurationSource), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }
        }
    }
}
