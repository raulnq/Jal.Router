using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.AzureStorage.LightInject.Installer
{
    public class AzureStorageCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IEntityStorage, Impl.AzureEntityStorage>(typeof(Impl.AzureEntityStorage).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IStartupTask, AzureEntityStorageStartupTask>(typeof(AzureEntityStorageStartupTask).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMessageStorage, AzureMessageStorage>(typeof(AzureMessageStorage).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IStartupTask, AzureMessageStorageStartupTask>(typeof(AzureMessageStorageStartupTask).FullName, new PerContainerLifetime());
        }
    }
}
