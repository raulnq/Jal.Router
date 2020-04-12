using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Logger.Impl;
using Jal.Router.Model;
using LightInject;

namespace Jal.Router.Logger.LightInject.Installer
{
    public class CommonLoggingCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IAsyncMiddleware<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IAsyncMiddleware<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
