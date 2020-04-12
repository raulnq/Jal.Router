using LightInject;

namespace Jal.Router.Newtonsoft.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddNewtonsoftForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<NewtonsoftCompositionRoot>();
        }
    }
}
