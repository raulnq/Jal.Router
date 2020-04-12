using Castle.Windsor;

namespace Jal.Router.Newtonsoft.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddNewtonsoftForRouter(this IWindsorContainer container)
        {
            container.Install(new NewtonsoftInstaller());
        }
    }
}
