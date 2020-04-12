using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddCommonLoggingForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<CommonLoggingCompositionRoot>();
        }
    }
}
