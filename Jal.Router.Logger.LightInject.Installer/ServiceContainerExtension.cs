using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Impl;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouterLogger(this IServiceContainer container)
        {
            container.Register<IBusLogger, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            container.Register<IRouterLogger, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());
        }
    }
}
