using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class BrokeredMessageRouterInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _sourceassemblies;

        public BrokeredMessageRouterInstaller(Assembly[] sourceassemblies)
        {
            _sourceassemblies = sourceassemblies;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IBrokeredMessageAdapter)).ImplementedBy(typeof(BrokeredMessageAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageRouter)).ImplementedBy(typeof(BrokeredMessageRouter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageContextBuilder)).ImplementedBy(typeof(BrokeredMessageContextBuilder)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageEndPointProvider)).ImplementedBy(typeof(BrokeredMessageEndPointProvider)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageSettingsExtractorFactory)).ImplementedBy(typeof(BrokeredMessageSettingsExtractorFactory)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageSettingsExtractor)).ImplementedBy(typeof(AppBrokeredMessageSettingsExtractor)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageRouterConfigurationSource)).ImplementedBy(typeof(EmptyBrokeredMessageRouterConfigurationSource)).Named(typeof(EmptyBrokeredMessageRouterConfigurationSource).FullName).LifestyleSingleton());
            
            if (_sourceassemblies != null)
            {
                foreach (var assembly in _sourceassemblies)
                {
                    var assemblyDescriptor = Classes.FromAssembly(assembly);
                    container.Register(assemblyDescriptor.BasedOn<AbstractBrokeredMessageRouterConfigurationSource>().WithServiceAllInterfaces());
                }
            }
        }
    }
}
