using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class BrokeredMessageRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IBrokeredMessageContentAdapter)).ImplementedBy(typeof(BrokeredMessageContentAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageRouter)).ImplementedBy(typeof(BrokeredMessageRouter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageReplyToAdapter)).ImplementedBy(typeof(BrokeredMessageReplyToAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageFromAdapter)).ImplementedBy(typeof(BrokeredMessageFromAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageIdAdapter)).ImplementedBy(typeof(BrokeredMessageIdAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IQueue)).ImplementedBy(typeof(AzureServiceBusQueue)).LifestyleSingleton());
            container.Register(Component.For(typeof(IPublisher)).ImplementedBy(typeof(AzureServiceBusTopic)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBusInterceptor)).ImplementedBy(typeof(LoggerBusInterceptor)).LifestyleSingleton());
        }
    }
}
