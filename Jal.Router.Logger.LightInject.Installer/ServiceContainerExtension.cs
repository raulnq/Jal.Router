using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouterLogger(this IServiceContainer container)
        {
            container.Register<IBusLogger, BusLogger>(new PerContainerLifetime());

            container.Register<IRouterLogger, RouterLogger>(new PerContainerLifetime());
        }
    }
}
