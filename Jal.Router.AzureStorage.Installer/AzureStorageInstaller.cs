using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureStorage.Installer
{
    public class AzureStorageInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMessageStorage>().ImplementedBy<AzureMessageStorage>().Named(typeof(AzureMessageStorage).FullName).LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<AzureMessageStorageStartupTask>().Named(typeof(AzureMessageStorageStartupTask).FullName).LifestyleSingleton());

            container.Register(Component.For<ISagaStorage>().ImplementedBy<AzureSagaStorage>().Named(typeof(AzureSagaStorage).FullName).LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<AzureSagaStorageStartupTask>().Named(typeof(AzureSagaStorageStartupTask).FullName).LifestyleSingleton());
        }
    }
}
