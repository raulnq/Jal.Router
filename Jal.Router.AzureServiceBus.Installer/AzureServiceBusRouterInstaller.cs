using System;
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
        private readonly int _maxconcurrentcalls;

        private readonly TimeSpan? _autorenewtimeout;

        public AzureServiceBusRouterInstaller(int maxconcurrentcalls=0, TimeSpan? autorenewtimeout = null)
        {
            _autorenewtimeout = autorenewtimeout;
            _maxconcurrentcalls = maxconcurrentcalls;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPointToPointChannel>().ImplementedBy<AzureServiceBusQueue>().Named(typeof(AzureServiceBusQueue).FullName).DependsOn(new { maxconcurrentcalls = _maxconcurrentcalls, autorenewtimeout = _autorenewtimeout }).LifestyleSingleton());

            container.Register(Component.For<IPublishSubscribeChannel>().ImplementedBy<AzureServiceBusTopic>().Named(typeof(AzureServiceBusTopic).FullName).DependsOn(new { maxconcurrentcalls = _maxconcurrentcalls, autorenewtimeout = _autorenewtimeout }).LifestyleSingleton());

            container.Register(Component.For<IChannelManager>().ImplementedBy<AzureServiceBusManager>().Named(typeof(AzureServiceBusManager).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageAdapter>().ImplementedBy<BrokeredMessageAdapter>().Named(typeof(BrokeredMessageAdapter).FullName).LifestyleSingleton());

            container.Register(Component.For<IMessageBodySerializer>().ImplementedBy<JsonMessageBodySerializer>().Named(typeof(JsonMessageBodySerializer).FullName).LifestyleSingleton());
        }
    }
}
