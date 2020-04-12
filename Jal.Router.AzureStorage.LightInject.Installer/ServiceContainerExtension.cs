using LightInject;

namespace Jal.Router.AzureStorage.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddAzureStorageForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<AzureStorageCompositionRoot>();
        }
    }
}
