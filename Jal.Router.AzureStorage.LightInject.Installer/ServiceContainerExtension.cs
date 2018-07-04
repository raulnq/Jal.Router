using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.AzureStorage.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAzureSagaStorage(this IServiceContainer container, string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            container.Register<ISagaStorage>(x=> new AzureSagaStorage(connectionstring, x.GetInstance<IComponentFactory>(), x.GetInstance<IConfiguration>(), sagastoragename, messagestorgename, tablenamesufix), typeof(AzureSagaStorage).FullName, new PerContainerLifetime());

            container.Register<IStartupTask>(x=> new AzureSagaStorageStartupTask(x.GetInstance<ILogger>(), connectionstring, sagastoragename, messagestorgename, tablenamesufix) , typeof(AzureSagaStorageStartupTask).FullName, new PerContainerLifetime());
        }

        public static void RegisterAzureMessageStorage(this IServiceContainer container, string connectionstring, string containername)
        {
            container.Register<IMessageStorage>(x => new AzureMessageStorage(connectionstring, containername), typeof(AzureMessageStorage).FullName, new PerContainerLifetime());

            container.Register<IStartupTask>(x => new AzureMessageStorageStartupTask(x.GetInstance<ILogger>(), connectionstring, containername), typeof(AzureMessageStorageStartupTask).FullName, new PerContainerLifetime());
        }
    }
}
