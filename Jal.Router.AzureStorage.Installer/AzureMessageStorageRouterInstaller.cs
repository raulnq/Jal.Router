using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.AzureStorage.Installer
{
    public class AzureMessageStorageRouterInstaller : IWindsorInstaller
    {
        private readonly string _connectionstring;

        private readonly string _container;

        public AzureMessageStorageRouterInstaller(string connectionstring, string container)
        {
            _connectionstring = connectionstring;

            _container = container;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMessageStorage>().ImplementedBy<AzureMessageStorage>().Named(typeof(AzureMessageStorage).FullName).DependsOn(new { connectionstring = _connectionstring, path = _container }).LifestyleSingleton());

            container.Register(Component.For<IStartupTask>().ImplementedBy<AzureMessageStorageStartupTask>().Named(typeof(AzureMessageStorageStartupTask).FullName).DependsOn(new { connectionstring = _connectionstring, container = _container }).LifestyleSingleton());
        }
    }
}
