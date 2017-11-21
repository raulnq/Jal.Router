using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class AzureServiceBusRouterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<AzureServiceBusQueue>().Named(typeof(AzureServiceBusQueue).FullName).LifestyleSingleton());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<AzureServiceBusTopic>().Named(typeof(AzureServiceBusTopic).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<AzureServiceBusManager>().Named(typeof(AzureServiceBusManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageBodyAdapter>().ImplementedBy<BrokeredMessageBodyAdapter>().Named(typeof(BrokeredMessageBodyAdapter).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageMetadataAdapter>().ImplementedBy<BrokeredMessageMetadataAdapter>().Named(typeof(BrokeredMessageMetadataAdapter).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageBodySerializer>().ImplementedBy<JsonMessageBodySerializer>().Named(typeof(JsonMessageBodySerializer).FullName).LifestyleSingleton());
        }
    }
}
