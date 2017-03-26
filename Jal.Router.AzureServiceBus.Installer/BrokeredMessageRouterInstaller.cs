using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class BrokeredMessageRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IBrokeredMessageAdapter)).ImplementedBy(typeof(BrokeredMessageAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageRouter)).ImplementedBy(typeof(BrokeredMessageRouter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBus)).ImplementedBy(typeof(BrokeredMessageBus)).LifestyleSingleton());
            container.Register(Component.For(typeof(IContextBuilder)).ImplementedBy(typeof(ContextBuilder)).LifestyleSingleton());
        }
    }
}
