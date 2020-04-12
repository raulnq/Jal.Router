using Castle.Windsor;

namespace Jal.Router.AzureServiceBus.Standard.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddAzureServiceBusForRouter(this IWindsorContainer container)
        {
            container.Install(new AzureServiceBusInstaller());
        }
    }
}
