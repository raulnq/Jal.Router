using LightInject;

namespace Jal.Router.Serilog.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddSerilogForRouter(this IServiceContainer container)
        {
            container.RegisterFrom<SerilogCompositionRoot>();
        }
    }
}
