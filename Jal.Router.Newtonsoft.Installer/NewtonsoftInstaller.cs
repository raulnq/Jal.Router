using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.Interface;
using Jal.Router.Newtonsoft.Impl;

namespace Jal.Router.Newtonsoft.Installer
{
    public class NewtonsoftInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMessageSerializer>().ImplementedBy<JsonMessageSerializer>().Named(typeof(JsonMessageSerializer).FullName).LifestyleSingleton());
        }
    }
}
