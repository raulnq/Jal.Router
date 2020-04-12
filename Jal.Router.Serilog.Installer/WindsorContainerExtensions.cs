using Castle.Windsor;

namespace Jal.Router.Logger.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddSerilogForRouter(this IWindsorContainer container)
        {
            container.Install(new SerilogInstaller());
        }
    }
}
