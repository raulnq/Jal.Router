using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface;
using Jal.Router.Logger.Impl;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Installer
{
    public class CommonLoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMiddlewareAsync<MessageContext>>().ImplementedBy<BusLogger>().Named(typeof(BusLogger).FullName).LifestyleSingleton());
            container.Register(Component.For<IMiddlewareAsync<MessageContext>>().ImplementedBy<RouterLogger>().Named(typeof(RouterLogger).FullName).LifestyleSingleton());
            container.Register(Component.For<ILogger<Beat>>().ImplementedBy<BeatLogger>().Named(typeof(BeatLogger).FullName).LifestyleSingleton());
        }
    }
}
