using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.AzureStorage.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAzureStorage(this IServiceContainer container, string connectionstring, string sagastoragename = "sagas", string messagestorgename = "messages", string tablenamesufix = "")
        {
            container.Register<IStorage>(x=> new AzureTableStorage(connectionstring, sagastoragename, messagestorgename, tablenamesufix), typeof(AzureTableStorage).FullName, new PerContainerLifetime());

            container.Register<IStartupTask>(x=> new AzureStorageStartupTask(connectionstring, sagastoragename, messagestorgename, tablenamesufix) , typeof(AzureStorageStartupTask).FullName, new PerContainerLifetime());
        }
    }
}
