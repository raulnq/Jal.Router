﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard.Installer
{
    public class AzureServiceBusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<AzureServiceBusQueue>().Named(typeof(AzureServiceBusQueue).FullName).LifestyleTransient());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<AzureServiceBusTopic>().Named(typeof(AzureServiceBusTopic).FullName).LifestyleTransient());

            container.Register(Component.For<IRequestReplyChannelFromPointToPointChannel>().ImplementedBy<AzureServiceBusRequestReplyFromPointToPointChannel>().Named(typeof(AzureServiceBusRequestReplyFromPointToPointChannel).FullName).LifestyleTransient());

            container.Register(Component.For<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>().ImplementedBy<AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel>().Named(typeof(AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel).FullName).LifestyleTransient());

            container.Register(Component.For<IChannelResource>().ImplementedBy<AzureServiceBusChannelResource>().Named(typeof(AzureServiceBusChannelResource).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<AzureServiceBusMessageAdapter>().Named(typeof(AzureServiceBusMessageAdapter).FullName).LifestyleSingleton());
        }
    }
}
