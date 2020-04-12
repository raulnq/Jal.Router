using Castle.Windsor;

namespace Jal.Router.Logger.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddCommonLoggingForRouter(this IWindsorContainer container)
        {
            container.Install(new CommonLoggingInstaller());
        }
    }
}
