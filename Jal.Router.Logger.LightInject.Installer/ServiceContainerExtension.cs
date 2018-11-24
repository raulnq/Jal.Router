using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface;
using Jal.Router.Logger.Impl;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterRouterLogger(this IServiceContainer container)
        {
            container.Register<IMiddleware<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            container.Register<IMiddleware<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            container.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
