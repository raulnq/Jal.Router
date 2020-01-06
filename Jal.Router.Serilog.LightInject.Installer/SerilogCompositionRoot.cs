using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface;
using Jal.Router.Serilog.Impl;
using Jal.Router.Model;
using LightInject;

namespace Jal.Router.Serilog.LightInject.Installer
{
    public class SerilogCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMiddlewareAsync<MessageContext>, BusLogger>(typeof(BusLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IMiddlewareAsync<MessageContext>, RouterLogger>(typeof(RouterLogger).FullName, new PerContainerLifetime());

            serviceRegistry.Register<ILogger<Beat>, BeatLogger>(typeof(BeatLogger).FullName, new PerContainerLifetime());
        }
    }
}
