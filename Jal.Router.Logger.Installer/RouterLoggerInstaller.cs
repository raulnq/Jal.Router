using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.Interface;

namespace Jal.Router.Logger.Installer
{
    public class RouterLoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IBusLogger)).ImplementedBy(typeof(BusLogger)).LifestyleSingleton().IsDefault());
            container.Register(Component.For(typeof(IRouterLogger)).ImplementedBy(typeof(RouterLogger)).LifestyleSingleton().IsDefault());
        }
    }
}
