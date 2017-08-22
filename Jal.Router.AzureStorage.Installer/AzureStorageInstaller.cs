using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureStorage.Installer
{
    public class AzureStorageInstaller : IWindsorInstaller
    {
        private readonly string _connectionstring;
        public AzureStorageInstaller(string connectionstring)
        {
            _connectionstring = connectionstring;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IStorage)).ImplementedBy(typeof(AzureTableStorage)).DependsOn(new { connectionstring =_connectionstring }).LifestyleSingleton());
        }
    }
}
