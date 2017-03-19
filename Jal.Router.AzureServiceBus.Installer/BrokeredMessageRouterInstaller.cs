using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Installer
{
    public class BrokeredMessageRouterInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IBrokeredMessageReader)).ImplementedBy(typeof(BrokeredMessageReader)).LifestyleSingleton());
            container.Register(Component.For(typeof(IBrokeredMessageRouter)).ImplementedBy(typeof(BrokeredMessageRouter)).LifestyleSingleton());
        }
    }
}
