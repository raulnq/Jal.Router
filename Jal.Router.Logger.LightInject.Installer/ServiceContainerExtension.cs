using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;
using LightInject;
using IMiddleware = Jal.Router.Interface.Inbound.IMiddleware;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouterLogger(this IServiceContainer container)
        {
            container.Register<Jal.Router.Interface.Outbound.IMiddleware, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            container.Register<IMiddleware, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<HeartBeat>, HeartBeatLogger>(typeof(HeartBeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
