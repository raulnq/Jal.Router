using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class AzureServiceBusRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IMessageAdapter<BrokeredMessage>)).ImplementedBy(typeof(BrokeredMessageAdapter)).LifestyleSingleton());
            container.Register(Component.For(typeof(IRouter<BrokeredMessage>)).ImplementedBy(typeof(Router<BrokeredMessage>)).LifestyleSingleton());
            container.Register(Component.For(typeof(IQueue)).ImplementedBy(typeof(AzureServiceBusQueue)).LifestyleSingleton());
            container.Register(Component.For(typeof(IPublisher)).ImplementedBy(typeof(AzureServiceBusTopic)).LifestyleSingleton());
        }
    }
}
