using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureServiceBus.Standard.Installer
{
    public class AzureServiceBusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<AzureServiceBusQueue>().Named(typeof(AzureServiceBusQueue).FullName).LifestyleSingleton());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<AzureServiceBusTopic>().Named(typeof(AzureServiceBusTopic).FullName).LifestyleSingleton());

            container.Register(Component.For<IRequestReplyChannel>().ImplementedBy<AzureServiceBusRequestReply>().Named(typeof(AzureServiceBusRequestReply).FullName).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<AzureServiceBusManager>().Named(typeof(AzureServiceBusManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<MessageAdapter>().Named(typeof(MessageAdapter).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageSerializer>().ImplementedBy<JsonMessageSerializer>().Named(typeof(JsonMessageSerializer).FullName).LifestyleSingleton());
        }
    }
}
