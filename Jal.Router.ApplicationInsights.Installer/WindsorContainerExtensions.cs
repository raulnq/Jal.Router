using Castle.Windsor;

namespace Jal.Router.ApplicationInsights.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddApplicationInsightsForRouter(this IWindsorContainer container)
        {
            container.Install(new ApplicationInsightsInstaller());
        }
    }
}
