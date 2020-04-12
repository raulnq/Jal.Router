using Castle.Windsor;

namespace Jal.Router.AzureStorage.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddAzureStorageForRouter(this IWindsorContainer container)
        {
            container.Install(new AzureStorageInstaller());
        }
    }
}
