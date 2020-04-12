using LightInject;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddAzureServiceBusForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<AzureServiceBusCompositionRoot>();
        }
    }
}
