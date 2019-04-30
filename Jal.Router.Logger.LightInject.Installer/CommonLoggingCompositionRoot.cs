using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface;
using Jal.Router.Logger.Impl;
using Jal.Router.Model;
using Jal.Router.Model.Management;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public class CommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMiddlewareAsync<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddlewareAsync<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
