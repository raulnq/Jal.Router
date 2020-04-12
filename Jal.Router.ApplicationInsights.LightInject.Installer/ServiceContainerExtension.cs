using LightInject;

namespace Jal.Router.ApplicationInsights.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddApplicationInsightsForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<ApplicationInsightsCompositionRoot>();
        }
    }
}
