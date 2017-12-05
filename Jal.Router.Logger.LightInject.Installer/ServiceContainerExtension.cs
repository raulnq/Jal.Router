using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouterLogger(this IServiceContainer container)
        {
            container.Register<IMiddleware, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            container.Register<Interface.Inbound.IMiddleware, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, HeartBeatLogger>(typeof(HeartBeatLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<StartupBeat>, StartupBeatLogger>(typeof(StartupBeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
